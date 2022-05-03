#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Player;
using TGC.MonoGame.TP.Geometries.Textures;

#endregion Using Statements

namespace TGC.MonoGame.TP.Scene
{
    class Floor
    {
        // A Quad to draw the floor - TODO hacerlo polimorfico tal vez
        private QuadPrimitive Quad { get; set; }

        // The Floor World
        private Matrix World { get; set; }

        // The Floor Texture
        private Texture2D Texture { get; set; }

        public Floor(QuadPrimitive quad, float scale)
        {
            Quad = quad;
            World = Matrix.CreateScale(scale);
        }

        public void Load(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(float elapsedTime)
        {
            // ¯\_(ツ)_ /¯
        }

        public void Draw(Effect effect, Matrix viewProjection)
        {
            effect.Parameters["WorldViewProjection"].SetValue(World * viewProjection);
            effect.Parameters["Texture"].SetValue(Texture);
            Quad.Draw(effect);
        }
    }
}
