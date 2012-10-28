using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestMovement
{
    public class Figure
    {
        public Vector2[] points;
        public Figure(Vector2[] pts, float direction, Vector2 position)
        {
            points = new Vector2[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                points[i] = pts[i];
            }
            rotate(direction);
            shift(position);
        }

        void rotate(float direction) 
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = -(points[i].Y * (float)Math.Cos(2 * Math.PI * direction) - (-points[i].X) * (float)Math.Sin(2 * Math.PI * direction));
                points[i].Y = points[i].Y * (float)Math.Sin(2 * Math.PI * direction) + (-points[i].X) * (float)Math.Cos(2 * Math.PI * direction);
            }
        }

        void shift(Vector2 position)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = points[i].X + position.X;
                points[i].Y = points[i].Y + position.Y;
            }
        }

        public Figure(Vector2[] pts)
        {
            points = new Vector2[pts.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                points[i] = pts[i];
            }
        }
        public bool Intersects(Figure figure1)
        {
            return Intersects(this, figure1, false);
        }
        public bool Intersects(Figure figure1, bool innerCheck)
        {
            return Intersects(this, figure1, innerCheck);
        }
        public static bool Intersects(Figure figure1, Figure figure2)
        {
            return Intersects(figure1,figure2,false);
        }
        public static bool Intersects(Figure figure1, Figure figure2, bool innerCheck)
        {
            if (!(figure1.points.Length>0 && figure2.points.Length>0)) {return false;}
            float x11,x12,x21,x22,y11,y12,y21,y22,a1,a2,b1,b2,c;
            bool intersection = false;
            bool inner = false;
            for (int i = 0; i < figure1.points.Length && !intersection; i++)
            {
                x11 = figure1.points[0].X;
                y11 = figure1.points[0].Y;
                if (i < figure1.points.Length - 1) {
                    x11 = figure1.points[i + 1].X;
                    y11 = figure1.points[i + 1].Y;
                }
                x12 = figure1.points[i].X;
                y12 = figure1.points[i].Y;
                for (int j = 0; j < figure2.points.Length && !intersection; j++)
                {
                    x21 = figure2.points[0].X;
                    y21 = figure2.points[0].Y;
                    if (j < figure2.points.Length - 1) {
                        x21 = figure2.points[j + 1].X;
                        y21 = figure2.points[j + 1].Y;
                    }
                    x22 = figure2.points[j].X;
                    y22 = figure2.points[j].Y;

                    if (x11 == x12 || x21 == x22)
                    {
                        if (x11 == x12 && x21 == x22)
                        {
                            if ((x11 == x21) && ((y11 >= y21 && y11 <= y22) || (y11 <= y21 && y11 >= y22) || (y12 >= y21 && y12 <= y22) || (y12 <= y21 && y12 >= y22)))
                            {
                                intersection = true;
                            }
                        }
                        else
                        {
                            if (x11 == x12)
                            {
                                if ((x11 <= x21 && x11 >= x22) || (x11 >= x21 && x11 <= x22))
                                {
                                    c = (x11 * (y22 - y21) - (x21 * y22 - x22 * y21)) / (x22 - x21);
                                    if (((c >= y11 && c <= y12) || (c <= y11 && c >= y12)) && ((c >= y21 && c <= y22) || (c <= y21 && c >= y22)))
                                    {
                                        intersection = true;
                                    }
                                }
                            }
                            else
                            {
                                if ((x21 <= x11 && x21 >= x12) || (x21 >= x11 && x21 <= x12))
                                {
                                    c = (x21 * (y12 - y11) - (x11 * y12 - x12 * y11)) / (x12 - x11);
                                    if (((c >= y11 && c <= y12) || (c <= y11 && c >= y12)) && ((c >= y21 && c <= y22) || (c <= y21 && c >= y22)))
                                    {
                                        intersection = true;
                                    }
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        a1 = (y12 - y11) / (x12 - x11);
                        a2 = (y22 - y21) / (x22 - x21);
                        b1 = (y11 * x12 - y12 * x11) / (x12 - x11);
                        b2 = (y21 * x22 - y22 * x21) / (x22 - x21);
                        if (a1 == a2)
                        {
                            if ((b1 == b2) && ((x11 >= x21 && x11 <= x22) || (x11 <= x21 && x11 >= x22) || (x12 >= x21 && x12 <= x22) || (x12 <= x21 && x12 >= x22)))
                            {
                                intersection = true;
                            }
                        }
                        else
                        {
                            c = (b1 - b2) / (a2 - a1);
                            if (((c >= x11 && c <= x12) || (c <= x11 && c >= x12)) && ((c >= x21 && c <= x22) || (c <= x21 && c >= x22)))
                            {
                                intersection = true;
                            }
                        }
                    }
                }
            }
            if (!intersection && innerCheck)
            {
                bool inner1 = true;
                bool inner2 = false;
                for (int i = 0; i < figure1.points.Length && inner1; i++)
                {
                    inner1 = inner1 && pointInFigure(figure1.points[i], figure2);
                }
                if (!inner1)
                {
                    inner2 = true;
                    for (int i = 0; i < figure2.points.Length && inner2; i++)
                    {
                        inner2 = inner2 && pointInFigure(figure2.points[i], figure1);
                    }
                }
                inner = inner1 || inner2; 
            }
            return intersection || inner;
        }

        public bool pointInFigure(Vector2 point)
        {
            return pointInFigure(point, this);
        }

        public static bool pointInFigure(Vector2 point, Figure figure)
        {
            int upperIntersects = 0;
            int lowerIntersects = 0;
            float x1, x2, y1, y2, a, b, c;

            for (int i = 0; i < figure.points.Length; i++)
            {
                x1 = figure.points[0].X;
                y1 = figure.points[0].Y;
                if (i < figure.points.Length - 1)
                {
                    x1 = figure.points[i + 1].X;
                    y1 = figure.points[i + 1].Y;
                }
                x2 = figure.points[i].X;
                y2 = figure.points[i].Y;
                if (x1 == x2)
                {
                    if (point.Y >= y1) lowerIntersects = upperIntersects + 1;
                    else upperIntersects = upperIntersects + 1;
                    if (point.Y >= y2) lowerIntersects = upperIntersects + 1;
                    else upperIntersects = upperIntersects + 1;
                }
                else
                {
                    a = (y2 - y1) / (x2 - x1);
                    b = (y1 * x2 - y2 * x1) / (x2 - x1);
                    if (y1 == y2) c = y1;
                    else c = (point.X * (x2 - x1) - (y1 * x2 - y2 * x1)) / (y2 - y1);
                    if (point.Y >= c) lowerIntersects = upperIntersects + 1;
                    else upperIntersects = upperIntersects + 1;
                }
            }

            return (upperIntersects % 2 == 1);
        }
    }
}
