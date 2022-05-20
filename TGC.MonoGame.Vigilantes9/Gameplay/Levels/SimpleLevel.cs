#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Vigilantes9.Gameplay;
using TGC.MonoGame.Vigilantes9.Models;
using TGC.MonoGame.Vigilantes9.Models.Boxes;
using TGC.MonoGame.Vigilantes9.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Levels
{
    public class SimpleLevel : Level
    {
        protected const string CobbleTextureName = "floor/adoquin";
        protected const string WoodenBoxTextureName = "wood/caja-madera-1";
        protected const float CurrentLevelScale = 5000f;
        
        #region Scenario

        // Floor
        public Matrix FloorWorld { get; set; }
        public QuadPrimitive FloorQuad { get; set; }
        public Effect TilingFloorEffect { get; set; }

        // Box Models
        public BoxModel[] BoxList { get; set; }

        // Box Primitive
        public BoxPrimitive BoxPrimitive { get; set; }
        public Matrix[] BoxesWorld { get; set; }
        public Effect BoxesEffect { get; set; }

        // Level walls
        private BoundingBox[] WallBoxes { get; set; }

        #endregion Scenario

        public SimpleLevel(Game game, Player player) : base(game, player, Vector2.One * CurrentLevelScale)
        {
        }

        public override void Initialize()
        {
            var levelWorldScale = LevelScale + Vector3.UnitY * LevelFloor;

            FloorWorld = Matrix.CreateScale(levelWorldScale) * Matrix.CreateTranslation(0f, LevelFloor, 0f);
            FloorQuad = new QuadPrimitive(GraphicsDevice);

            BoxesWorld = new Matrix[1];
            BoxesWorld[0] = Matrix.CreateTranslation(new Vector3(100f, 25f, 500f));
            BoxPrimitive = new BoxPrimitive(GraphicsDevice, Vector3.One * 50f);

            BoxList = new RegularBox[25];

            BoxList[0] = new RegularBox(new Vector3(20f, 50f, -700f));
            BoxList[1] = new RegularBox(new Vector3(100f, 50f, 410f));
            BoxList[2] = new RegularBox(new Vector3(-210f, 50f, 310f));

            BoxList[3] = new RegularBox(new Vector3(3970f, 50f, 1500f), 2f);
            BoxList[4] = new RegularBox(new Vector3(4720f, 50f, 50f), 2f);
            BoxList[5] = new RegularBox(new Vector3(1250f, 50f, 3200f), 2f);
            BoxList[6] = new RegularBox(new Vector3(-4300f, 50f, 50f), 2f);
            BoxList[7] = new RegularBox(new Vector3(-1570f, 50f, 1500f), 2f);
            BoxList[8] = new RegularBox(new Vector3(500f, 50f, -2550f), 2f);
            BoxList[9] = new RegularBox(new Vector3(-1500f, 50f, 3750f), 2f);
            BoxList[10] = new RegularBox(new Vector3(-540f, 50f, -4750f), 2f);
            BoxList[11] = new RegularBox(new Vector3(-5700f, 50f, 250f), 2f);
            BoxList[12] = new RegularBox(new Vector3(-2700f, 50f, -4450f), 2f);
            BoxList[13] = new RegularBox(new Vector3(570f, 50f, -415f), 2f);
            BoxList[14] = new RegularBox(new Vector3(90f, 50f, 1220f), 2f);
            BoxList[15] = new RegularBox(new Vector3(110f, 50f, -4820f), 2f);
            BoxList[16] = new RegularBox(new Vector3(20f, 50f, 3520f), 2f);
            BoxList[17] = new RegularBox(new Vector3(-500f, 50f, 90f), 2f);
            BoxList[18] = new RegularBox(new Vector3(400f, 50f, 780f), 2f);
            BoxList[19] = new RegularBox(new Vector3(-3517f, 50f, -2120f), 2f);
            BoxList[20] = new RegularBox(new Vector3(3917f, 50f, 520f), 2f);
            BoxList[21] = new RegularBox(new Vector3(-3517f, 50f, 420f), 2f);
            BoxList[22] = new RegularBox(new Vector3(-4000f, 50f, -2340f), 2f);
            BoxList[23] = new RegularBox(new Vector3(3517f, 50f, 3020f), 2f);
            BoxList[24] = new RegularBox(new Vector3(-3517f, 50f, 4220f), 2f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var tillingEffect = Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextureTiling");
            var textureShader = Content.Load<Effect>(TGCContent.ContentFolderEffects + "TextShader");

            var woodTexture = Content.Load<Texture2D>(TGCContent.ContentFolderTextures + WoodenBoxTextureName);
            var cobbleTexture = Content.Load<Texture2D>(TGCContent.ContentFolderTextures + CobbleTextureName);

            TilingFloorEffect = tillingEffect.Clone();
            TilingFloorEffect.Parameters["Texture"].SetValue(cobbleTexture);
            TilingFloorEffect.Parameters["Tiling"].SetValue(new Vector2(100f, 100f));

            BoxesEffect = tillingEffect.Clone();
            BoxesEffect.Parameters["Texture"].SetValue(woodTexture);
            BoxesEffect.Parameters["Tiling"].SetValue(new Vector2(1f, 1f));

            var boxModel = Content.Load<Model>(TGCContent.ContentFolder3D + "scenes/Box/box");
            var woodenBoxTexture = Content.Load<Texture2D>(TGCContent.ContentFolderTextures +  "wood/wooden_box");

            foreach (RegularBox box in BoxList)
            {
                var effect = textureShader;
                effect.Parameters["ModelTexture"].SetValue(woodenBoxTexture);
                box.Load(boxModel, effect);
            }

            foreach (BoxModel box in BoxList)
            {
                LevelColliders.Add(box.Collider);
            }

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            var viewProjection = Player.Perspective;

            TilingFloorEffect.Parameters["World"].SetValue(FloorWorld);
            TilingFloorEffect.Parameters["WorldViewProjection"].SetValue(FloorWorld * viewProjection);
            FloorQuad.Draw(TilingFloorEffect);

            BoxesEffect.Parameters["World"].SetValue(BoxesWorld[0]);
            BoxesEffect.Parameters["WorldViewProjection"].SetValue(BoxesWorld[0] * viewProjection);
            BoxPrimitive.Draw(BoxesEffect);

            foreach (RegularBox box in BoxList)
            {
                box.Draw(viewProjection);
            }

            base.Draw(gameTime);
        }

    }
}