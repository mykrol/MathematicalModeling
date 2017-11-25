using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLib;

namespace Testing
{
    class Program
    {
        static String MeshToString(List<List<Point>> mesh)
        {
            String res = "";
            foreach (List<Point> timeslice in mesh)
            {
                foreach (Point p in timeslice)
                {
                    res += p.ToString();  
                }
                res += "\r\n";
            }
            return res;
        }
        static void Main(string[] args)
        {
            //Console.WriteLine(MeshToString(Model.GetPoints(0, 1, 0, 1, 0, 1, 2)));
            //Console.ReadKey();
            List<Point> initialValues = new List<Point>(new Point[] { new Point(0.5, 0, 0, 0), new Point(1, 0.5, 0, 1) });
            List<Point> boundaryValues = new List<Point>(new Point[] { new Point(0, 0, 0.5, 0.2), new Point(1, 1, 0.2, 0.3) });
            List<List<Point>> mesh = MathLib.Solver.solve(initialValues, boundaryValues, 1, 0, 1, 0, 1, 0.1, 0.1);
            Console.WriteLine(MeshToString(mesh));
            Console.ReadKey(true);
        }
    }
}
