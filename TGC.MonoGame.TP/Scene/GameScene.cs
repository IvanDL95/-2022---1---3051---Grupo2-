using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Models.Boxes;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries.Textures;
using TGC.MonoGame.Collisions;

namespace TGC.MonoGame.TP.Scene
{
    public class GameScene
    {
        protected const float FloorScale = 5000f;
        protected const string CobbleTextureName = "floor/adoquin";
        private const string BoxTexture = "wood/caja-madera-1";

        // Textures
        private Texture2D StonesTexture { get; set; }
        private Texture2D WoodenTexture { get; set; }
        private Texture2D CobbleTexture { get; set; }

        private Floor AFuckingFloor { get; set; }
        private Effect TilingEffect { get; set; }
        private Effect TextureEffect { get; set; }
        private RegularBox[] SceneBoxList { get; set; }
        //private RegularBox ASingleBox { get; set; }

        public BoundingBox[] Colliders { get; set; }

        public void Load(Game game)
        {
            // Load Textures
            // StonesTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + "stones");
            WoodenTexture = game.Content.Load<Texture2D>(TGCContent.ContentFolderTextures + BoxTexture);
            CobbleTexture = game.Content.Load<Texture2D>(TGCContent.ContentFolderTextures + CobbleTextureName);

            TilingEffect = game.Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextureTiling");
            TextureEffect = game.Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextureBasic");

            TilingEffect.Parameters["Texture"].SetValue(CobbleTexture);
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(100f, 100f));
            
            AFuckingFloor = new Floor(new QuadPrimitive(game.GraphicsDevice), FloorScale);
            AFuckingFloor.Load(CobbleTexture, TilingEffect);

            //var boxPosition = new Vector3(500f, 50f, 20f);
            //var boxWorld = Matrix.CreateTranslation(boxPosition); //Matrix.CreateWorld(boxPosition, Vector3.Forward, Vector3.Up);

            //ASingleBox = new RegularBox(game, TextureEffect, boxWorld);
            //ASingleBox.Effect.Parameters["Texture"].SetValue(WoodenTexture);

            Colliders = new BoundingBox[1];
            //Colliders[1] = BoundingVolumesExtensions.FromMatrix(AFuckingFloor.World);
            //Colliders[0] = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(10f) * Matrix.CreateTranslation(boxPosition));
        }

        public void Draw(GameTime gameTime, InGameCamera camera)
        {
            Matrix viewProjection = camera.View * camera.Projection;

            //ASingleBox.Effect.Parameters["WorldViewProjection"].SetValue(ASingleBox.World * viewProjection);
            //ASingleBox.Draw(gameTime);

            AFuckingFloor.Draw(viewProjection);
        }
    }
}