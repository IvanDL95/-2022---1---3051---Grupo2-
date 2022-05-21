#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion Using Statements

namespace TGC.MonoGame.Vigilantes9.Gameplay
{
    public abstract class Level : DrawableGameComponent
    {
        protected ContentManager Content;
        protected Vector3 LevelScale { get; set; }

        protected const float DefaultLevelScale = 1000f;
        protected const float LevelCeiling = 2000f;
        protected const float LevelFloor = 0f;
        protected const float LevelHeight = LevelCeiling - LevelFloor;
        protected const float WallsThickness = 1f;

        /// <summary>
        ///     Create Bounding Boxes to enclose the level. 6 total, 4 walls, floor and ceiling.
        /// </summary>
        /// <param name="player">The current player object</param>
        public Level(Game game, Player player, Vector2 scale) : base(game)
        {
            Player = player;
            Content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
            LevelScale = new Vector3(scale.X, LevelHeight, scale.Y);
        }

        public override void Initialize()
        {
            BoundingBox[] levelBoundaries = GetBoundaries(Vector3.One * WallsThickness);
            LevelColliders = new List<BoundingBox>(levelBoundaries);

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        #region GamePlay

        protected Player Player { get; set; }
        protected List<BoundingBox> LevelColliders;

        public List<BoundingBox> Colliders
        {
            get { return LevelColliders; }
        }

        /// <summary>
        ///     Create Bounding Boxes to enclose the level. 6 total, 4 walls, floor and ceiling.
        /// </summary>
        /// <param name="wallThickness">Wall Thickness</param>
        protected BoundingBox[] GetBoundaries(Vector3 wallThickness)
        {
            return new BoundingBox[]
            {
                // Walss Boundary
                new BoundingBox(new Vector3(-LevelScale.X, LevelFloor, -LevelScale.Z) - wallThickness, new Vector3( LevelScale.X, LevelCeiling,-LevelScale.Z)),
                new BoundingBox(new Vector3(-LevelScale.X, LevelFloor,  LevelScale.Z) - wallThickness, new Vector3( LevelScale.X, LevelCeiling, LevelScale.Z)),
                new BoundingBox(new Vector3( LevelScale.X, LevelFloor, -LevelScale.Z) - wallThickness, new Vector3( LevelScale.X, LevelCeiling, LevelScale.Z)),
                new BoundingBox(new Vector3(-LevelScale.X, LevelFloor, -LevelScale.Z) - wallThickness, new Vector3(-LevelScale.X, LevelCeiling, LevelScale.Z)),

                // Floor/Ceiling Boundary
                new BoundingBox(new Vector3(-LevelScale.X, LevelFloor, -LevelScale.Z) - wallThickness, new Vector3(LevelScale.X, LevelFloor, LevelScale.Z)),
                new BoundingBox(new Vector3(-LevelScale.X, LevelCeiling, -LevelScale.Z) - wallThickness, new Vector3(LevelScale.X, LevelCeiling, LevelScale.Z)),
            };
        }

        #endregion GamePlay
    }
}