using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    public struct Point
    {
        public Point(double x1,
                double x2,
                double t,
                double y)
        {
            x1_ = x1;
            x2_ = x2;
            t_ = t;
            y_ = y;
        }

        public override String ToString()
        {
            return "{" + x1_.ToString() + ", " + x2_.ToString() + ", " +
                    t_.ToString() + ", " + y_.ToString() + "} ";
        }

        public double x1_;
        public double x2_;
        public double t_;
        public double y_;
    }


    public class Model
    {
        public const double C = 2.0;
        public const int Mx1    = 1000;
        public const int Mx2    = 1000;
        public const int Mt     = 1000;

        static double solution(double x1, double x2, double t)
        {
            // y(x1, x2, t) = sin(x1 * t) * sin(x2*t)
            return Math.Sin(x1 * t) * Math.Sin(x2 * t);
        }

        public static double rhsU(double x1, double x2, double t)
        {
            return (2 * Math.Pow(C, 2) * Math.Pow(t, 2) - Math.Pow(x1, 2) - Math.Pow(x2, 2)) * solution(x1, x2, t) +
                    2 * x1 * x2 * Math.Cos(t * x1) * Math.Cos(t * x2);
        }

        //G(s1 - s2)
        public static double GreenFunction(Point s1, Point s2)
        {
            double r = Math.Sqrt(Math.Pow(s1.x1_ - s2.x1_, 2) + Math.Pow(s1.x2_ - s2.x2_, 2));
            double num = C * (s1.t_ - s2.t_) - r;
            if (num <= 0) return 0;
            return num / (2 * Math.PI * C * (C * C * Math.Pow(s1.t_ - s2.t_, 2) - r * r));
        }

        //
        // 
        //  
        //
        public static List<List<Point>> GetPoints(double t0,
                                           double T,
                                           double x0,
                                           double y0,
                                           double x1,
                                           double y1,
                                           int N
                                            ) // number of knots
        {

            double xh = (x1 - x0) / N;
            double yh = (y1 - y0) / N;
            double th = (T - t0) / N;

            List<List<Point>> mesh = new List<List<Point>>();


            for (int t = 0; t < N; ++t)
            {
                mesh.Add(new List<Point>());
                List<Point> ltime = mesh.Last();
                for (int x = 0; x < N; ++x)
                {
                    for (int y = 0; y < N; ++y)
                    {
                        ltime.Add(new Point(x0 + xh * x,
                                             y0 + yh * y,
                                             t0 + th * t,
                                             solution(
                                                    x0 + xh * x,
                                                    y0 + yh * y,
                                                    t0 + th * t)
                                           ));
                    }
                }
            }
            return mesh;
        }
    }
}
