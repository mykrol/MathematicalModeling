using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace MathLib
{
    public class Solver
    {
        public static List<List<Point>> solve(
                                        List<Point> initialValues,
                                        List<Point> boundaryValues,
                                        double T,
                                        double x0,
                                        double W,
                                        double y0,
                                        double H, 
                                        double timeStep,
                                        double spaceStep
                                        )
        {
            List<Point> S = GeneratePointsS(T, x0, W, y0, H, timeStep, spaceStep);
            List<Point> S0 = GeneratePointsS0(x0, W, y0, H, timeStep, spaceStep);
            List<Point> SG = GeneratePointsSG(T, x0, W, y0, H, timeStep, spaceStep);
            Func<Point, double> Yinf = CalculateYinf(S);
            Matrix<double> A = CalculateMatrix(initialValues, boundaryValues, S0, SG);
            Vector<double> Y = CalculateVector(initialValues, boundaryValues, S0, SG, Yinf);
            Matrix<double> Ainv = A.PseudoInverse();
            Vector<double> u = Ainv.Multiply(Y);
            Vector<double> u0 = u.SubVector(0, S0.Count);
            Vector<double> uG = u.SubVector(S0.Count, SG.Count);

            Func<Point, double> Y0 = CalculateYpart(S0, u0);
            Func<Point, double> YG = CalculateYpart(SG, uG);
            Func<Point, double> y = s => Yinf(s) + Y0(s) + YG(s);

            List<List<Point>> res = new List<List<Point>>();
            for (double t = 0; t <= T; t += timeStep)
            {
                res.Add(new List<Point>());
                List<Point> ltime = res.Last();
                for (double x1 = x0; x1 <= x0 + W; x1 += spaceStep)
                {
                    for (double x2 = y0; x2 <= y0 + H; x2 += spaceStep)
                    {
                        Point p = new Point(x1, x2, t, 0);
                        double val = y(p);
                        ltime.Add(new Point(x1, x2, t, val));
                    }
                }
            }

            return res;
        }

        private static Func<Point, double> CalculateYinf(List<Point> S)
        {
            return s1 =>
                {
                    double term = 0;
                    foreach (Point s2 in S)
                    {
                        term += Model.GreenFunction(s1, s2) * Model.rhsU(s2.x1_, s2.x2_, s2.t_);
                    }
                    return term;
                };
        }

        private static Func<Point, double> CalculateYpart(List<Point> Spart, Vector<double> upart)
        {
            return s1 =>
            {
                double term = 0;
                for (int i = 0; i < Spart.Count; ++i)
                {
                    term += Model.GreenFunction(s1, Spart[i]) * upart[i];
                }
                return term;
            };
        }

        private static Matrix<double> CalculateMatrix(
                                      List<Point> initialValues,
                                      List<Point> boundaryValues,
                                      List<Point> S0,
                                      List<Point> SG
                                      )
        {
            var M = Matrix<double>.Build;
            Func<int, int, double> A11func = (i, j) => Model.GreenFunction(initialValues[i], S0[j]);
            Func<int, int, double> A12func = (i, j) => Model.GreenFunction(initialValues[i], SG[j]);
            Func<int, int, double> A21func = (i, j) => Model.GreenFunction(boundaryValues[i], S0[j]);
            Func<int, int, double> A22func = (i, j) => Model.GreenFunction(boundaryValues[i], SG[j]);
            Matrix<double> A11 = M.Dense(initialValues.Count, S0.Count, A11func);
            Matrix<double> A12 = M.Dense(initialValues.Count, SG.Count, A12func);
            Matrix<double> A21 = M.Dense(boundaryValues.Count, S0.Count, A21func);
            Matrix<double> A22 = M.Dense(boundaryValues.Count, SG.Count, A22func);
            Matrix<double> A = M.Dense(initialValues.Count + boundaryValues.Count, S0.Count + SG.Count);
            A.SetSubMatrix(0, 0, A11);
            A.SetSubMatrix(0, S0.Count, A12);
            A.SetSubMatrix(initialValues.Count, 0, A21);
            A.SetSubMatrix(initialValues.Count, S0.Count, A22);
            return A;
        }

        private static Vector<double> CalculateVector(
                                      List<Point> initialValues,
                                      List<Point> boundaryValues,
                                      List<Point> S0,
                                      List<Point> SG,
                                      Func<Point, double> Yinf
                                      )
        {
            var V = Vector<double>.Build;
            Func<int, double> Y0func = i => initialValues[i].y_ - Yinf(initialValues[i]);
            Func<int, double> YGfunc = i => boundaryValues[i].y_ - Yinf(boundaryValues[i]);
            Vector<double> Y0 = V.Dense(initialValues.Count, Y0func);
            Vector<double> YG = V.Dense(boundaryValues.Count, YGfunc);
            Vector<double> Y = V.Dense(initialValues.Count + boundaryValues.Count);
            Y.SetSubVector(0, initialValues.Count, Y0);
            Y.SetSubVector(initialValues.Count, boundaryValues.Count, YG);
            return Y;
        }

        private static List<Point> GeneratePointsS(
                                   double T,
                                   double x0,
                                   double W,
                                   double y0,
                                   double H,
                                   double timeStep,
                                   double spaceStep
                                   )
        {
            List<Point> res = new List<Point>();
            for (double t = 0; t <= T; t += timeStep)
            {
                for (double x1 = x0; x1 <= x0 + W; x1 += spaceStep)
                {
                    for (double x2 = y0; x2 <= y0 + H; x2 += spaceStep)
                    {
                        res.Add(new Point(x1, x2, t, 0));
                    }
                }
            }
            return res;
        }

        private static List<Point> GeneratePointsS0(
                                   double x0,
                                   double W,
                                   double y0,
                                   double H,
                                   double timeStep,
                                   double spaceStep
                                   )
        {
            List<Point> res = new List<Point>();
            for (double t = -inf; t <= 0; t += timeStep)
            {
                for (double x1 = x0; x1 <= x0 + W; x1 += spaceStep)
                {
                    for (double x2 = y0; x2 <= y0 + H; x2 += spaceStep)
                    {
                        res.Add(new Point(x1, x2, t, 0));
                    }
                }
            }
            return res;
        }

        private static List<Point> GeneratePointsSG(
                                   double T,
                                   double x0,
                                   double W,
                                   double y0,
                                   double H,
                                   double timeStep,
                                   double spaceStep
                                   )
        {
            List<Point> res = new List<Point>();
            for (double t = 0; t <= T; t += timeStep)
            {
                for (double x1 = x0 - inf; x1 < x0; x1 += spaceStep)
                {
                    for (double x2 = x1 - inf; x2 < x0; x2 += spaceStep)
                    {
                        res.Add(new Point(x1, x2, t, 0));
                    }
                }

                for (double x1 = W; x1 <= W + inf; x1 += spaceStep)
                {
                    for (double x2 = H; x2 <= H + inf; x2 += spaceStep)
                    {
                        res.Add(new Point(x1, x2, t, 0));
                    }
                }
            }
            return res;
        }

        private const double inf = 1.5;
    }
}
