#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Models.Boxes;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.TP.Scene
{
    public class GameScene
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

        private Floor AFuckingFloor { get; set; }
        private Effect TilingEffect { get; set; }
        private Effect TextureEffect { get; set; }
        private RegularBox[] SceneBoxList { get; set; }
        private RegularBox ASingleBox { get; set; }

        private TGCGame game;
        private GraphicsDevice graphicsDevice;

        public GameScene(TGCGame game, GraphicsDevice graphicsDevice) {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
        }


        public void Load()
        {
            // Load Textures
            // StonesTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + "stones");
            WoodenTexture = game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + BoxTexture);
            CobbleTexture = game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + CobbleTextureName);

            TilingEffect = game.Content.Load<Effect>(TGCGame.ContentFolderEffects + "TextureTiling");
            TextureEffect = game.Content.Load<Effect>(TGCGame.ContentFolderEffects + "TextureBasic");

            TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(100f, 100f));
            AFuckingFloor = new Floor(new QuadPrimitive(graphicsDevice), FloorScale);
            AFuckingFloor.Load(CobbleTexture, TilingEffect);

            var boxWorld = Matrix.CreateWorld(new Vector3(500f, 50f, 20f), Vector3.Forward, Vector3.Up);

            ASingleBox = new RegularBox(game, TextureEffect, boxWorld);
            ASingleBox.Effect.Parameters["Texture"].SetValue(WoodenTexture);
        }

        public void Draw(GameTime gameTime, InGameCamera camera)
        {
            Matrix viewProjection = camera.View * camera.Projection;

            ASingleBox.Effect.Parameters["WorldViewProjection"].SetValue(ASingleBox.World * viewProjection);
            ASingleBox.Draw(gameTime);

            AFuckingFloor.Draw(viewProjection);
        }
    }
}