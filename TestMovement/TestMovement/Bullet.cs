using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestMovement
{
    public class Bullet : FlyingObject
    {
        Vector2 prevPosition;

        public Bullet(Vector2 position, float velocity, float direction, float basic)
            : base (position, velocity, direction, basic)
        {
        }

        public override void Update(int gameTime, Vector2 globalShift)
        {
            prevPosition = position;
            base.Update(gameTime, globalShift);
        }

        public override Figure getFigure()
        {
            float shiftW = 4;
            float shiftH = 7;
            Vector2[] points = new Vector2[4];
            Vector2 pos = position;
            pos.X = pos.X + shiftH * (float)Math.Sin(2 * Math.PI * direction);
            pos.Y = pos.Y + shiftH * (float)Math.Cos(2 * Math.PI * direction);
            points[0] = pos;
            points[0].X = points[0].X - shiftW * (float)Math.Cos(2 * Math.PI * direction);
            points[0].Y = points[0].Y - shiftW * (float)Math.Sin(2 * Math.PI * direction);
            points[1] = pos;
            points[1].X = points[1].X + shiftW * (float)Math.Cos(2 * Math.PI * direction);
            points[1].Y = points[1].Y + shiftW * (float)Math.Sin(2 * Math.PI * direction);
            pos = prevPosition;
            pos.X = pos.X - shiftH * (float)Math.Sin(2 * Math.PI * direction);
            pos.Y = pos.Y - shiftH * (float)Math.Cos(2 * Math.PI * direction);
            points[2] = pos;
            points[2].X = points[2].X + shiftW * (float)Math.Cos(2 * Math.PI * direction);
            points[2].Y = points[2].Y + shiftW * (float)Math.Sin(2 * Math.PI * direction);
            points[3] = pos;
            points[3].X = points[3].X - shiftW * (float)Math.Cos(2 * Math.PI * direction);
            points[3].Y = points[3].Y - shiftW * (float)Math.Sin(2 * Math.PI * direction);

            return new Figure(points);
        }
    }
}
