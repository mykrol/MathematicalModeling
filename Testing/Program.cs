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
            Console.WriteLine(MeshToString(Model.GetPoints(0, 1, 0, 1, 0, 1, 2)));
            Console.ReadKey();
        }
    }
}
