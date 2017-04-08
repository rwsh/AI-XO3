using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XO3
{
    class TPole
    {
        public Canvas g;
        TextBox tb;
        TextBox tbCount;
        public double H;

        public TQLearning QL;

        public TPosition Pos;
        char V;

        TBase Base;

        int Count = 0;

        public TPole(Canvas g, TextBox tb, TextBox tbCount, TBase Base, TQLearning QL)
        {
            this.Base = Base;
            this.QL = QL;
        
            this.g = g;
            this.tb = tb;
            this.tbCount = tbCount;

            H = g.Height;
            g.Width = H;

            Init();
        }

        void Init()
        {
            g.Children.Clear();

            DrawPole();

            Pos = new TPosition();
            V = 'X';

            Count++;

            tbCount.Text = Count.ToString();
        }

        public void SaveQLearning()
        {
            string FName = "qbase.dat";
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, QL);
            }

        }

        void DrawPole()
        {
            Brush br = Brushes.Blue;

            for (int i = 0; i < 4; i++)
            {
                Line L = new Line();
                L.Stroke = br;
                L.X1 = (H / 3) * i;
                L.X2 = L.X1;
                L.Y1 = 0;
                L.Y2 = H;
                L.StrokeThickness = 3;
                g.Children.Add(L);

                Line M = new Line();
                M.Stroke = br;
                M.Y1 = (H / 3) * i;
                M.Y2 = M.Y1;
                M.X1 = 0;
                M.X2 = H;
                M.StrokeThickness = 3;
                g.Children.Add(M);
            }
        }

        public bool Can
        {
            get
            {
                if(V == 'X')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Result Move(int i, int j)
        {
            int i1, j1, i2, j2;

            if (Pos.Who(out i1, out j1, out i2, out j2) != Result.Game)
            {
                return Pos.Who(out i1, out j1, out i2, out j2);
            }

            if(Pos.Mark(i, j, V))
            {
                DrawMark(i, j);

                if(V == 'X')
                {
                    V = 'O';
                }
                else
                {
                    V = 'X';
                }

                Result res = Pos.Who(out i1, out j1, out i2, out j2);

                if (res == Result.X)
                {
                    DrawLine(i1, j1, i2, j2);
                    //MessageBox.Show("Выиграли Крестики!");

                    SaveQLearning();

                    Init();
                }

                if (res == Result.O)
                {
                    DrawLine(i1, j1, i2, j2);
                    //MessageBox.Show("Выиграли Нолики!");
                    SaveQLearning();

                    Init();
                }

                if (res == Result.Non)
                {
                    //MessageBox.Show("Ничья!");
                    SaveQLearning();

                    Init();
                }

                if ((res == Result.Game)&&(V == 'O'))
                {
                    Run(); // Ход машины
                }

                return res;
            }
            else
            {
                return Result.Error;
            }
        }

        TPosition PreMove(int i, int j)
        {
            TPosition res = Pos.Copy();

            if(res.Mark(i, j, V))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        void Run()
        {
            //Base.Run(this); // Ход Машины

            string s = Pos.GetS();

            int[] As = QL.Get_As(s);

            if (As.Count() == 0)
            {
                return;
            }

            double q;

            int a = QL.Get_A(s, 0.1, out q);

            int[] ij = Pos.Get_ij(a);

            TPosition P = PreMove(ij[0], ij[1]);

            int[] ix = new int[4];

            double r = 0;

            TAnalysis Analysis = new TAnalysis();

            ActionType Res = Analysis.WhatResult2(P);

            if (Res == ActionType.Win)
            {
                r = 10;
            }

            if (Res == ActionType.Def)
            {
                r = -10;
            }

            tb.Text = r.ToString();

            /*
                        Result Res = P.Who(out ix[0], out ix[1], out ix[2], out ix[3]);


                        if (Res == Result.X)
                        {
                            r = -10;
                        }

                        if (Res == Result.O)
                        {
                            r = 10;
                        }
            */

            QL.Update_Q(s, a, r, P.GetS(), 0.1, 0.9);

            Move(ij[0], ij[1]);
        }

        void DrawLine(int i1, int j1, int i2, int j2)
        {
            double H3 = H / 3;

            Brush br = Brushes.Black;

            Line L;

            L = new Line();
            L.Stroke = br;
            L.StrokeThickness = 4;
            L.X1 = i1 * H3 + H3 / 2;
            L.Y1 = j1 * H3 + H3 / 2;
            L.X2 = i2 * H3 + H3 / 2;
            L.Y2 = j2 * H3 + H3 / 2;

            g.Children.Add(L);

        }

        void DrawMark(int i, int j)
        {
            double H3 = H / 3;

            if(V == 'X')
            {
                Brush br = Brushes.Red;

                Line L;
                double x0 = i * H3;
                double y0 = j * H3;

                L = new Line();
                L.Stroke = br;
                L.StrokeThickness = 2;
                L.X1 = x0 + 5;
                L.Y1 = y0 + 5;
                L.X2 = x0 + H3 - 5;
                L.Y2 = y0 + H3 - 5;
                g.Children.Add(L);

                L = new Line();
                L.Stroke = br;
                L.StrokeThickness = 2;
                L.X1 = x0 + H3 - 5;
                L.Y1 = y0 + 5;
                L.X2 = x0 + 5;
                L.Y2 = y0 + H3 - 5;
                g.Children.Add(L);
            }
            else
            {
                Ellipse O = new Ellipse();
                Brush br = Brushes.Green;
                O.Stroke = br;
                O.StrokeThickness = 2;
                O.Width = H3 - 10;
                O.Height = H3 - 10;
                O.Margin = new Thickness(i * H3 + 5, j * H3 + 5, 0, 0);
                g.Children.Add(O);

            }
        }

        public void Get_ij(double x, double y, out int i, out int j)
        {
            i = -1;
            j = -1;

            double H3 = H / 3.0;

            if (x < H3)
            {
                i = 0;
            }
            if ((x > H3) && (x < H3 * 2))
            {
                i = 1;
            }
            if (x > 2 * H3)
            {
                i = 2;
            }

            if (y < H3)
            {
                j = 0;
            }
            if ((y > H3) && (y < H3 * 2))
            {
                j = 1;
            }
            if (y > 2 * H3)
            {
                j = 2;
            }
        }

    }
}
