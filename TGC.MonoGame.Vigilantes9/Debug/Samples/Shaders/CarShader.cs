using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Vigilantes9.Cameras;
using TGC.MonoGame.Vigilantes9.Debug;

namespace TGC.MonoGame.Vigilantes9.Debug.Samples.Shaders
{
    /// <summary>
    ///     Basic Shader:
    ///     Units Involved:
    ///     # Unit 8 - Video Adapters - Shaders.
    ///     It's the hello world of shaders.
    ///     Author: Mariano Banquiero
    /// </summary>
    public class CarShader : TGCSample
    {
        private float time;

        /// <inheritdoc />
        public CarShader(TGCDebug game) : base(game)
        {
            Category = TGCSampleCategory.Shaders;
            Name = "Car Shader";
            Description = "Car Shader Sample. Animation by vertex shader and coloring by pixel shader.";
        }

        private Camera Camera { get; set; }
        private Effect Effect { get; set; }
        private Model Model { get; set; }
        private Texture2D Texture { get; set; }

        /// <inheritdoc />
        public override void Initialize()
        {
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0f, 50f, 400f));
            time = 0;
            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>(ContentFolder3D + "vehicles/Car/car");
            // From the effect of the model I keep the texture.
            Texture = ((BasicEffect) Model.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;

            // Load a shader in runtime, outside the Content pipeline.
            // First you must run "mgfxc <SourceFile> <OutputFile> [/Debug] [/Profile:<DirectX_11,OpenGL>]"
            // https://docs.monogame.net/articles/tools/mgfxc.html
            //byte[] byteCode = File.ReadAllBytes(Game.Content.RootDirectory + "/" + ContentFolderEffects + "BasicShader.fx");
            //Effect = new Effect(GraphicsDevice, byteCode);

            // Load a shader using Content pipeline.
            Effect = Game.Content.Load<Effect>(ContentFolderEffects + "TextShader");

            base.LoadContent();
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            // Game.Gizmos.UpdateViewProjection(Camera.View, Camera.Projection);

            base.Update(gameTime);
        }

        /// <inheritdoc />
        public override void Draw(GameTime gameTime)
        {
            Game.Background = Color.CornflowerBlue;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;          

            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var mesh = Model.Meshes.FirstOrDefault();

            if (mesh != null)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                    Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform);
                    Effect.Parameters["ViewProjection"].SetValue(Camera.View * Camera.Projection);
                    Effect.Parameters["ModelTexture"].SetValue(Texture);
                    // Effect.Parameters["View"].SetValue(Camera.View);
                    // Effect.Parameters["Projection"].SetValue(Camera.Projection);
                    //Effect.Parameters["WorldViewProjection"].SetValue(Camera.WorldMatrix * Camera.View * Camera.Projection);
                    //Effect.Parameters["ModelTexture"].SetValue(Texture);
                    //Effect.Parameters["Time"].SetValue(time);
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}