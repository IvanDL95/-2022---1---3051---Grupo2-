#region Using Statements

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Models.Boxes;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.TP.Scene
{
    public class GameScene : DrawableGameComponent
    {
        #region Constants

        protected const float FloorScale = 5000f;
        protected const string CobbleTextureName = "floor/adoquin";
        private const string BoxTexture = "wood/caja-madera-1";

        #endregion Constants

        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D WoodenTexture { get; set; }
        private Texture2D CobbleTexture { get; set; }

        // Camera to draw the scene
        private Camera GameCamera { get; set; }

        private Floor AFuckingFloor { get; set; }
        private Effect TilingEffect { get; set; }
        private Effect TextureEffect { get; set; }
        private RegularBox[] SceneBoxList { get; set; }
        private RegularBox ASingleBox { get; set; }

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="game">The game.</param>
        public GameScene(Game game, Camera gameCamera) : base(game)
        {
            GameCamera = gameCamera;
        }

        public override void Initialize()
        {
            AFuckingFloor = new Floor(new QuadPrimitive(GraphicsDevice), FloorScale);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load Textures
            // StonesTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + "stones");
            WoodenTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + BoxTexture);
            CobbleTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + CobbleTextureName);

            TilingEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + "TextureTiling");
            TextureEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + "TextureBasic");

            TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(100f, 100f));
            AFuckingFloor.Load(CobbleTexture, TilingEffect);

            var boxWorld = Matrix.CreateWorld(new Vector3(500f, 50f, 20f), Vector3.Forward, Vector3.Up);

            ASingleBox = new RegularBox(Game, TextureEffect, boxWorld);
            ASingleBox.Effect.Parameters["Texture"].SetValue(WoodenTexture);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix viewProjection = GameCamera.View * GameCamera.Projection;

            ASingleBox.Effect.Parameters["WorldViewProjection"].SetValue(ASingleBox.World * viewProjection);
            ASingleBox.Draw(gameTime);

            AFuckingFloor.Draw(viewProjection);

            base.Draw(gameTime);
        }
    }
}