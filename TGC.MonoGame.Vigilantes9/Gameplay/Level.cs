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
        protected const ushort LevelsSidesCount = (ushort)6;
        protected const float WallsThickness = 2f;

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
            BoundingBox[] levelBoundaries = SetBoundaries(Vector3.One * WallsThickness);
            LevelColliders = new List<BoundingBox>(levelBoundaries);

            base.Initialize();
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
        protected BoundingBox[] SetBoundaries(Vector3 wallThickness)
        {
            var levelScale = LevelScale / 2;
            var minVector = wallThickness / 2;

            var levelBoundaries = new BoundingBox[LevelsSidesCount];
            // Walss Boundary
            levelBoundaries[0] = new BoundingBox(new Vector3(-LevelScale.X, LevelFloor, -LevelScale.Z) - minVector, new Vector3( LevelScale.X, LevelCeiling,-LevelScale.Z) + minVector);
            levelBoundaries[1] = new BoundingBox(new Vector3(-LevelScale.X, LevelFloor,  LevelScale.Z) - minVector, new Vector3( LevelScale.X, LevelCeiling, LevelScale.Z) + minVector);
            levelBoundaries[2] = new BoundingBox(new Vector3( LevelScale.X, LevelFloor, -LevelScale.Z) - minVector, new Vector3( LevelScale.X, LevelCeiling, LevelScale.Z) + minVector);
            levelBoundaries[3] = new BoundingBox(new Vector3(-LevelScale.X, LevelFloor, -LevelScale.Z) - minVector, new Vector3(-LevelScale.X, LevelCeiling, LevelScale.Z) + minVector);

            var levelFloorBoundary = LevelFloor - WallsThickness;
            var levelCeilingBoundary = LevelCeiling + WallsThickness;
            // Floor/Ceiling Boundary
            levelBoundaries[4] = new BoundingBox(new Vector3(-LevelScale.X, levelFloorBoundary, -LevelScale.Z) - minVector, new Vector3(LevelScale.X, levelFloorBoundary, LevelScale.Z) + minVector);
            levelBoundaries[5] = new BoundingBox(new Vector3(-LevelScale.X, levelCeilingBoundary, -LevelScale.Z) - minVector, new Vector3(LevelScale.X, levelCeilingBoundary, LevelScale.Z) + minVector);

            return levelBoundaries;
        }

        #endregion GamePlay
    }
}