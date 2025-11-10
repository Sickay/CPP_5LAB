using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double f(double x, ref int k1) // ліві частини рівнянь
        {
            switch (k1)
            {
                case 0: return x * x - 4;
                case 1: return 3 * x - 4 * Math.Log(x) - 5;
                case 2: return Math.Pow(x, 3) - 4 * x + 1;
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        label7.Visible = false; // робимо невидимим вікно для введення Kmax
                        textBox4.Visible = false;
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    break;
                case 1:
                    {
                        label7.Visible = true;
                        textBox4.Visible = true;
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int L, k = -1, Kmax = 0, m = -1;
            double D, Eps = 0, a, b;
            L = 0;

            // ======= Вибір МЕТОДУ =======
            // 1. Через RadioButton
            if (radioButton5.Checked) m = 0;
            else if (radioButton4.Checked) m = 1;
            // 2. Через ComboBox (якщо RadioButton не вибрано)
            else
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: m = 0; break;
                    case 1: m = 1; break;
                }
            }

            if (m == -1)
            {
                MessageBox.Show("Оберіть метод!");
                return;
            }

            // ======= Вибір РІВНЯННЯ =======
            // 1. Через RadioButton
            if (radioButton1.Checked) k = 0;
            else if (radioButton2.Checked) k = 1;
            else if (radioButton3.Checked) k = 2;
            // 2. Через ComboBox (якщо RadioButton не вибрано)
            else
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0: k = 0; break;
                    case 1: k = 1; break;
                    case 2: k = 2; break;
                }
            }

            if (k == -1)
            {
                MessageBox.Show("Оберіть рівняння!");
                return;
            }

            // ======= Перевірка введення =======
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Введіть усі необхідні дані!");
                return;
            }

            a = Convert.ToDouble(textBox1.Text);
            b = Convert.ToDouble(textBox2.Text);
            if (a > b) { D = a; a = b; b = D; }
            Eps = Convert.ToDouble(textBox3.Text);
            if ((Eps > 1e-1) || (Eps <= 0))
            {
                Eps = 1e-4;
                textBox3.Text = Convert.ToString(Eps);
            }

            if (m == 0)
                if ((f(a, ref k)) * (f(b, ref k)) > 0)
                {
                    MessageBox.Show("Введіть правильний інтервал [a,b]!");
                    return;
                }
            if ((a <= 0 || b <= 0) && ((k == 1)))
            {
                MessageBox.Show("Для рівняння 2 інтервал [a,b] повинен бути більшим за 0!\n" +
                                "Логарифм визначений тільки для додатних x.");
                return;
            }

            if (Math.Abs(f(a, ref k)) < Eps)
            {
                textBox5.Text = a.ToString();
                textBox6.Text = L.ToString();
                return;
            }
            if (Math.Abs(f(b, ref k)) < Eps)
            {
                textBox5.Text = b.ToString();
                textBox6.Text = L.ToString();
                return;
            }

            // ======= Виклик обраного методу =======
            switch (m)
            {
                case 0:
                    textBox5.Text = Convert.ToString(MDP(a, b, Eps, ref k, ref L));
                    textBox6.Text = L.ToString();
                    label10.Text = "К-ть поділів =";
                    break;

                case 1:
                    if (textBox4.Text == "")
                    {
                        MessageBox.Show("Введіть Kmax!");
                        return;
                    }
                    Kmax = Convert.ToInt32(textBox4.Text);
                    textBox5.Text = Convert.ToString(MN(a, b, Eps, ref k, Kmax, ref L));
                    textBox6.Text = L.ToString();
                    label10.Text = "К-ть ітерацій =";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        label7.Visible = false; // робимо невидимим вікно для введення Kmax
                        textBox4.Visible = false;
                    }
                    break;
                case 1:
                    {
                        label7.Visible = true; // робимо видимим вікно для введення Kmax
                        textBox4.Visible = true;
                    }
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
    

