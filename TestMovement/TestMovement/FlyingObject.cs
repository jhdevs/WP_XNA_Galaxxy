using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestMovement
{
    public class FlyingObject
    {
        public Vector2 position;
        public float velocity;
        public float direction;
        public bool alive = true;

        public FlyingObject(Vector2 position, float velocity, float direction, float basic)
        {
            this.position = position;
            if (basic != 0.0f)
            {
                this.position.X = position.X + basic * (float)Math.Sin(-2 * Math.PI * direction);
                this.position.Y = position.Y + basic * (float)Math.Cos(2 * Math.PI * direction);
            }
            this.velocity = velocity;
            this.direction = direction;
        }

        public void Update(int gameTime)
        {
            Update(gameTime, Vector2.Zero);
        }

        public virtual void Update(int gameTime, Vector2 globalShift)
        {
            float distance = velocity * gameTime;
            position.X = position.X + distance * (float)Math.Sin(-2 * Math.PI * direction) - globalShift.X;
            position.Y = position.Y + distance * (float)Math.Cos(2 * Math.PI * direction) - globalShift.Y;
        }

        public virtual Figure getFigure()
        {
            Vector2[] points = new Vector2[1];
            points[0] = position;
            return new Figure(points);
        }
    }
}
