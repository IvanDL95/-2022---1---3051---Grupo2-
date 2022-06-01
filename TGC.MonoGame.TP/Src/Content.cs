using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Drawers;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP
{
    internal class Content
    {
        private const string EffectsFolder = "Effects/";
        private const string ModelsFolder = "Models/";
        private const string TexturesFolder = "Textures/";

        private readonly ContentManager ContentManager;

        internal readonly Model M_Vehicle, M_Box;
        internal readonly Effect E_BasicShader, E_TilingShader;
        internal readonly Texture2D T_WoodenBox, T_Vehicle, T_Palette;
        internal readonly Drawer D_Vehicle, D_Box, D_Floor;
        internal readonly TypedIndex SH_Vehicle, SH_Box, SH_Floor;

        internal Content(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.ContentManager = contentManager;

            // Efects
            E_BasicShader = LoadEffect("BasicShader");
            E_TilingShader = LoadEffect("TilingShader"); 

            // Models
            M_Vehicle = LoadModel("vehicles/car/car");
            M_Box = LoadModel("scenes/Box/box");

            // Texture
            T_Vehicle = M_Vehicle.getTexture();
            T_WoodenBox = LoadTexture("wood/wooden_box");
            T_Palette = LoadTexture("floor/palette");

            // Shapes
            SH_Vehicle = LoadShape(M_Vehicle.CreateBoxShape(1.5f));
            SH_Box = LoadShape(M_Box.CreateBoxShape(2f));
            SH_Floor = LoadShape(new Box(5000f, 10, 5000f));

            //Drawers
            D_Vehicle = new BasicDrawer(M_Vehicle, T_Vehicle);
            D_Box = new BasicDrawer(M_Box, T_WoodenBox);
            D_Floor = new GeometryDrawer(new Quad(graphicsDevice), E_BasicShader, T_Palette);

            Console.WriteLine("Cargue el Contenido!");

        }
        private Model LoadModel(string name)
        {
            return ContentManager.Load<Model>(ModelsFolder + name);
        }

        private Effect LoadEffect(string name) => ContentManager.Load<Effect>(EffectsFolder + name);
        private Texture2D LoadTexture(string name) => ContentManager.Load<Texture2D>(TexturesFolder + name);
        private TypedIndex LoadShape<S>(S shape) where S : unmanaged, IShape => TGCGame.PhysicsSimulation.LoadShape(shape);

    }
}