using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGC.MonoGame.Vigilantes9.Debug.Samples;

namespace TGC.MonoGame.Vigilantes9.Debug
{
    public class DebugtViewer
    {

        public DebugtViewer(TGCDebug game)
        {
            Game = game;
            ActiveSampleTreeNode = -1;
        }

        /// <summary>
        ///     The viewer where the samples are going to be shown.
        /// </summary>
        private TGCDebug Game { get; }

        /// <summary>
        ///     The active sample in the tree.
        /// </summary>
        private int ActiveSampleTreeNode { get; set; }

        /// <summary>
        ///     Samples sorted by category.
        /// </summary>
        public SortedList<string, List<TGCSample>> SamplesByCategory { get; set; }

        /// <summary>
        ///     Samples available to view.
        /// </summary>
        public Dictionary<string, TGCSample> SamplesByName { get; set; }

        /// <summary>
        ///     Samples that have already been loaded.
        /// </summary>
        public Dictionary<string, TGCSample> AlreadyLoadedSamples { get; set; }

        /// <summary>
        ///     The active sample.
        /// </summary>
        private TGCSample ActiveSample { get; set; }

        /// <summary>
        ///     Loads the sample tree dynamically and groups them by category.
        /// </summary>
        public void LoadTreeSamples()
        {
            SamplesByCategory = new SortedList<string, List<TGCSample>>();
            SamplesByName = new Dictionary<string, TGCSample>();
            AlreadyLoadedSamples = new Dictionary<string, TGCSample>();

            var baseType = typeof(TGCSample);

            foreach (var type in baseType.Assembly.GetTypes())
            {
                if (type.BaseType != baseType || !type.IsClass || !type.IsPublic || type.IsAbstract) continue;
                try
                {
                    var sample = (TGCSample) Activator.CreateInstance(type, Game);
                    sample.Visible = false;
                    sample.Enabled = false;

                    if (!string.IsNullOrEmpty(sample.Category))
                    {
                        if (SamplesByCategory.ContainsKey(sample.Category))
                            SamplesByCategory[sample.Category].Add(sample);
                        else
                            SamplesByCategory.Add(sample.Category, new List<TGCSample> {sample});
                    }

                    SamplesByName.Add(sample.Name, sample);
                }
                catch
                {
                    Console.WriteLine("Could not load sample type: " + type);
                }
            }
        }
        
        /// <summary>
        ///     Load the welcome sample
        /// </summary>
        public void LoadWelcomeSample()
        {
            LoadSample(typeof(TGCLogoSample).Name);
        }
        
        
        public void LoadNextSample()
        {
            bool flag = false;
            foreach (KeyValuePair<string, TGCSample> pair in SamplesByName)
            {
                if(flag) {
                    LoadSample(pair.Key);
                    break;
                }

                if(pair.Key == ActiveSample.Name)
                    flag = true;
            }
        }
        
        public void LoadPreviousSample()
        {
            string previousSample = SamplesByName.FirstOrDefault().Key;
            foreach (KeyValuePair<string, TGCSample> pair in SamplesByName)
            {
                if(pair.Key == ActiveSample.Name) {
                    LoadSample(previousSample);
                    break;
                }

                previousSample = pair.Key;
            }
        }

        /// <summary>
        ///     Enable the selected sample and disables the others.
        /// </summary>
        /// <param name="sampleName">The name of the sample to load.</param>
        public void LoadSample(string sampleName)
        {
            if (ActiveSample != null)
            {
                // Unbind any Texture modifiers from ImGUI
                ActiveSample.UnloadSampleContent();
                ActiveSample.Enabled = false;
                ActiveSample.Visible = false;
            }

            var newSample = SamplesByName[sampleName];
            newSample.Visible = true;
            newSample.Enabled = true;
            ActiveSample = newSample;


            newSample.Prepare();

            if (!Game.Components.Contains(newSample))
            {
                Game.Components.Add(newSample);
            }
            else
            {
                newSample.Initialize();
                newSample.ReloadContent();
            }
        }
    }
}