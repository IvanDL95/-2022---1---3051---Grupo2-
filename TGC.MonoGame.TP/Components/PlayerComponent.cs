#region Using Statements

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TGC.MonoGame.TP.Models.Boxes;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Geometries.Textures;

#endregion Using Statements
namespace TGC.MonoGame.TP.Components
{
    public class PlayerComponent : GameComponent
    {
        #region Constants

        protected const float CameraDistance = 1000f;

        #endregion Constants
        public List<Player> Players { get; }


        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="game">The game.</param>
        public PlayerComponent(TGCGame game) : base(game)
        {
            Players = new List<Player>();
        }

        public void CreatePlayer(CombatVehicle vehicle)
        {
            Players.Add(new Player(vehicle));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Players.Count > 0)
            {
                foreach (Player player in Players)
                {
                    player.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }
    }
}