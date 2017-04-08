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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Threading;

namespace XO3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string FName = "qbase.dat";

            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(FName))
            {
                using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
                {
                    QLearning = (TQLearning)formatter.Deserialize(fs);
                }

            }
            else
            {
                QLearning = new TQLearning(textBox);

                using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, QLearning);
                }
            }

            Base = new TBase();

            Pole = new TPole(gPole, textBox, textBox1, Base, QLearning);
        }

        TQLearning QLearning;

        private void cmClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        TPole Pole;

        TBase Base;

        private void cmRun(object sender, RoutedEventArgs e)
        {
            Pole = new TPole(gPole, textBox, textBox1, Base, QLearning);
        }

        private void cmCheck(object sender, MouseButtonEventArgs e)
        {
            if (Pole == null)
            {
                return;
            }

            if(!Pole.Can)
            {
                return;
            }

            Point p = e.GetPosition(gPole);

            int i, j;

            Pole.Get_ij(p.X, p.Y, out i, out j);

            if ((i < 0) || (j < 0))
            {
                return;
            }

            Pole.Move(i, j);
        }

        DispatcherTimer Timer;

        Random rnd;

        private void cmAutoRun(object sender, RoutedEventArgs e)
        {
            Pole = new TPole(gPole, textBox, textBox1, Base, QLearning);

            rnd = new Random();

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(onTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Timer.Start();
        }

        void onTick(object sender, EventArgs e)
        {
            int i, j;

            Pole.Pos.GetRand(rnd, out i, out j);

            Pole.Move(i, j);
            
        }
    }
}
