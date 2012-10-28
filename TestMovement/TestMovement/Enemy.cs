using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestMovement
{
    public class Enemy : FlyingObject
    {
        public int bullet_DelayMS = 400;
        public int bullet_LastMS = 0;
        public bool dead = false;
        public bool visible = true;
        public int animationPos = 15;
        public int deadtimeMS = 0;
        public int deadAnimateInterval = 70;
        public Figure figure;

        public Enemy(Vector2 position, float velocity, float direction, float basic, Figure figure)
            : base(position, velocity, direction, basic)
        {
            this.figure = figure;
        }

        public override void Update(int gameTime, Vector2 globalShift)
        {
            if (position.X < 0 || position.Y < 0 || position.X > 480 || position.Y > 800)
            {
                bullet_LastMS = 0;
            }
            else
            {
                bullet_LastMS = bullet_LastMS + gameTime;
            }
            if (!alive)
            {
                deadtimeMS = deadtimeMS + gameTime;
                animationPos = deadtimeMS / deadAnimateInterval;
                if (animationPos < 3)
                {
                    animationPos = animationPos * 5;
                    animationPos = 15 - animationPos;
                }
                else
                {
                    animationPos = animationPos - 3;
                    visible = false;
                    if (animationPos > 15) dead = true;
                }
            }
            base.Update(gameTime, globalShift);
        }

        public override Figure getFigure()
        {
            return new Figure(figure.points,direction,position);
        }
    }
}
