using System;
using System.Collections.Generic;
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
using System.Collections;

namespace XO3
{
    [Serializable]
    public class TQLearning
    {
        Random rnd;

        public TQsa Qsa;

        public TQLearning(TextBox tb)
        {
            rnd = new Random();
            Qsa = new TQsa();
        }

        public void Update_Q(string s, int a, double r, string s1, double alpha, double gamma)
        {
            double Qmax;
            Get_A(s1, -1, out Qmax);

            double Q = Qsa.Get_Q(s, a) + alpha * (r + gamma * Qmax - Qsa.Get_Q(s, a));
            Qsa.Set(new TQSAItem(s, a, Q));
        }

        public int Get_A(string s, double eps, out double Qmax)
        {
            Qmax = 0;

            int[] As = Get_As(s);

            if (As.Count() < 1)
            {
                return -1;
            }

            if (rnd.NextDouble() < eps)
            {
                return As[rnd.Next(As.Count())];
            }

            int i_max = As[0];
            double q_max = Qsa.Get_Q(s, As[0]);

            for (int i = 0; i < As.Count(); i++)
            {
                double q = Qsa.Get_Q(s, As[i]);
                if (q > q_max)
                {
                    q = q_max;
                    i_max = As[i];
                }
            }

            Qmax = q_max;

            return i_max;
        }

        public int[] Get_As(string s)
        {
            int N = 0;
            int[] ints = new int[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    ints[N] = i;
                    N++;
                }
            }

            if (N == 0)
            {
                return null;
            }
            else
            {
                int[] res = new int[N];
                for (int i = 0; i < N; i++)
                {
                    res[i] = ints[i];
                }

                return res;
            }
        }

    }


    [Serializable]
    public class TQsa
    {
        ArrayList arr;

        public TQsa()
        {
            arr = new ArrayList();
        }

        public void Set(TQSAItem Item)
        {
            int ind = IsIs(Item);
            if (ind < 0)
            {
                arr.Add(Item);
            }
            else
            {
                this[ind] = Item;
            }
        }

        public double Get_Q(string s, int a)
        {
            int ind = IsIs(new TQSAItem(s, a, 0));
            if (ind < 0)
            {
                return 0;
            }
            else
            {
                return this[ind].val;
            }
        }

        public int IsIs(TQSAItem Item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IsEq(Item))
                {
                    return i;
                }
            }

            return -1;
        }

        public int Count
        {
            get
            {
                return arr.Count;
            }
        }

        public TQSAItem this[int ind]
        {
            get
            {
                return (TQSAItem)arr[ind];
            }

            set
            {
                arr[ind] = value;
            }
        }

    }

    [Serializable]
    public class TQSAItem
    {
        public string s;
        public int a;
        public double val;

        public TQSAItem(string s, int a, double val)
        {
            this.s = s;
            this.a = a;
            this.val = val;
        }

        public bool IsEq(TQSAItem Item)
        {
            if ((s == Item.s) && (a == Item.a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
