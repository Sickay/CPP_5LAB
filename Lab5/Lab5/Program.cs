using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab5
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            double f(double x, ref int k1) // ліві частини рівнянь
            {
                switch (k1)
                {
                    case 0: return x * x - 4;
                    case 1: return 3 * x - 4 * Math.Log(x) - 5;
                }
                return 0;
            }
            double fp(double x, double d, ref int k1) // перша похідна
            {
                return (f(x + d, ref k1) - f(x, ref k1)) / d;
            }
            double f2p(double x, double d, ref int k1) // друга похідна
            {
                return (f(x + d, ref k1) + f(x - d, ref k1) - 2 * f(x, ref k1)) / (d * d);
            }
            double MDP(double a, double b, double Eps, ref int k1, ref int L)
            {
                double c = 0, Fc;
                while (b - a > Eps)
                {
                    c = 0.5 * (b - a) + a;
                    L++; // лічильник кількості поділів інтервалу [a, b]
                    Fc = f(c, ref k1);
                    if (Math.Abs(Fc) < Eps) // перевірка, чи точка с не є поблизу кореня x*
                        return c; // завершення роботи функції MDP
                    if (f(a, ref k1) * Fc > 0) a = c;
                    else b = c;
                }
                return c; // завершення роботи функції MDP
            }
            double MN(double a, double b, double Eps, ref int k1, int Kmax, ref int L)
            {
                double x, Dx, D;
                int i;
                Dx = 0.0;
                D = Eps / 100.0;
                x = b;
                if (f(x, ref k1) * f2p(x, D, ref k1) < 0)
                    x = a;
                if (f(x, ref k1) * f2p(x, D, ref k1) < 0)
                    MessageBox.Show("Для цього рівняння збіжність ітерацій не гарантована");
                for (i = 1; i <= Kmax; i++)
                {
                    Dx = f(x, ref k1) / fp(x, D, ref k1);
                    x = x - Dx;
                    if (Math.Abs(Dx) < Eps)
                    {
                        L = i;
                        return x; // завершення роботи функції MN
                    }
                }
                MessageBox.Show("За задану кількість ітерацій кореня не знайдено");
                return -1000.0; // -1000.0 Це наша ознака цієї нестандартної ситуації
            }
           
        
    }
    }
}
