using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Linq;

namespace BestSalesman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static DispatcherTimer dT;
        static int Radius = 30;
        static int PointCount = 5;
        static int Population = 20;
        static double ChanceMutation = 0.5;
        static int NIteration = 100;
        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List<Ellipse>();
        static PointCollection pC = new PointCollection();
        static int[,] popolation;
        int counter = 0;

        public MainWindow()
        {
            dT = new DispatcherTimer();
            InitializeComponent();
            InitPoints();
            InitPolygon();
            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }
        private void InitPoints()
        {
            Random rnd = new Random();
            pC.Clear();
            EllipseArray.Clear();
            for (int i = 0; i < PointCount; i++)
            {
                Point p = new Point();
                p.X = rnd.Next(Radius, (int)(0.75 * MainWin.Width) - 3 * Radius);
                p.Y = rnd.Next(Radius, (int)(0.90 * MainWin.Height - 3 * Radius));
                pC.Add(p);
            }
            for (int i = 0; i < PointCount; i++)
            {
                Ellipse el = new Ellipse();
                el.StrokeThickness = 2;
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.Black;
                el.Fill = Brushes.DeepPink;
                EllipseArray.Add(el);
            }
        }
        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.StrokeThickness = 2;
        }
        private void PlotPoints()
        {
            for (int i = 0; i < PointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius / 2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius / 2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }
        private void PlotWay(int[] BestWayIndex)
        {
            PointCollection Points = new PointCollection();
            for (int i = 0; i < BestWayIndex.Length; i++)
                Points.Add(pC[BestWayIndex[i]]);
            myPolygon.Points = Points;
            MyCanvas.Children.Add(myPolygon);
        }
        private void VelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            dT.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(item.Content));

        }
        private void StopStart_Click(object sender, RoutedEventArgs e)
        {
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                counter = 0;
            }
            else
            {
                NumElemCB.IsEnabled = false;
                dT.Start();
                FirstGeneration();
            }
        }
        private void NumElemCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            PointCount = Convert.ToInt32(item.Content);
            InitPoints();
            InitPolygon();
        }

        private void OneStep(object sender, EventArgs e)
        {
            MyCanvas.Children.Clear();
            //InitPoints();
            PlotPoints();
            PlotWay(GetBestWay());
            counter++;
            Ni.Content = "Номер ітерації: " + counter.ToString();
            if (counter.ToString() == TextBox3.Text)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                counter = 0;
            }
        }

        private int[] GetBestWay()
        {
            Random rnd = new Random();
            //Sort();
            int[] way = new int[PointCount];
            for (int i = Population / 2; i < Population; i++)
            {

                if (rnd.NextDouble() < 1 - ChanceMutation)
                {
                    way = NextGeneration();
                    for (int j = 0; j < PointCount; j++)
                    {
                        popolation[i, j] = way[j];
                    }
                }
                else
                {
                    Mutation(i);
                }
            }
            Sort();
            for (int i = 0; i < PointCount; i++)
            {
                way[i] = popolation[0, i];
            }
            return way;
        }

        static void FirstGeneration()
        {
            popolation = new int[Population, PointCount];
            for (int i = 0; i < popolation.GetLength(0); i++)
            {
                for (int j = 0; j < popolation.GetLength(1); j++)
                {
                    popolation[i, j] = j;
                }
            }
            Random rnd = new Random();
            int a, b, tmp;
            for (int i = 0; i < popolation.GetLength(0); i++)
            {
                for (int j = 0; j < popolation.GetLength(1); j++)
                {
                    a = rnd.Next(popolation.GetLength(1));
                    b = rnd.Next(popolation.GetLength(1));
                    tmp = popolation[i, a];
                    popolation[i, a] = popolation[i, b];
                    popolation[i, b] = tmp;
                }
            }
        }

        static void Sort()
        {
            double length1, length2;
            for (int i = 0; i < Population - 1; i++)
            {
                for (int j = 0; j < Population - i - 1; j++)
                {
                    length1 = FindLength(j);
                    length2 = FindLength(j + 1);
                    if (length1 > length2)
                    {
                        int tmp;
                        for (int k = 0; k < PointCount; k++)
                        {
                            tmp = popolation[j, k];
                            popolation[j, k] = popolation[j + 1, k];
                            popolation[j + 1, k] = tmp;
                        }
                    }
                }
            }
        }

        static double FindLength(int row)
        {
            double len1 = 0;
            int[] way = new int[PointCount];
            for (int i = 0; i < PointCount; i++)
            {
                way[i] = popolation[row, i];
            }
            for (int i = 0; i < PointCount - 1; i++)
            {
                len1 += Math.Sqrt(Math.Pow((pC[way[i]].X - pC[way[i + 1]].X), 2) + Math.Pow((pC[way[i]].Y - pC[way[i + 1]].Y), 2));
            }
            len1 = len1 + Math.Sqrt(Math.Pow((pC[way[0]].X - pC[way[PointCount - 1]].X), 2) + Math.Pow((pC[way[0]].Y - pC[way[PointCount - 1]].Y), 2));
            return len1;
        }

        static int[] NextGeneration()
        {
            Random rnd = new Random();
            int krosover;
            int i1, i2;
            int[] temp1 = new int[PointCount];
            int[] temp2 = new int[PointCount];
            List<int> ch1 = new List<int>();
            bool check = true;
            i1 = rnd.Next(Population);
            i2 = rnd.Next(Population);
            krosover = rnd.Next(2, PointCount - 1);
            for (int k = 0; k < krosover; k++)
            {
                temp1[k] = popolation[i1, k];
                temp2[k] = popolation[i2, k];
            }
            for (int k = krosover; k < PointCount; k++)
            {
                temp1[k] = popolation[i2, k];
                temp2[k] = popolation[i1, k];
            }
            if (rnd.NextDouble() <= 0.5)
            {
                for (int u = 0; u < PointCount; u++)
                {
                    check = true;
                    for (int k = 0; k < ch1.Count; k++)
                    {
                        if (temp1[u] == ch1[k])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                        ch1.Add(temp1[u]);
                }
                for (int u = 0; u < PointCount; u++)
                {
                    check = true;
                    for (int k = 0; k < ch1.Count; k++)
                    {
                        if (temp2[u] == ch1[k])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                        ch1.Add(temp2[u]);
                }
            }
            else
            {
                for (int u = 0; u < PointCount; u++)
                {
                    check = true;
                    for (int k = 0; k < ch1.Count; k++)
                    {
                        if (temp2[u] == ch1[k])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                        ch1.Add(temp2[u]);
                }
                for (int u = 0; u < PointCount; u++)
                {
                    check = true;
                    for (int k = 0; k < ch1.Count; k++)
                    {
                        if (temp1[u] == ch1[k])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                        ch1.Add(temp1[u]);
                }
            }
            return ch1.ToArray();
        }

        static void Mutation(int i)
        {
            Random rnd = new Random();
            int j1 = rnd.Next(PointCount);
            int j2 = rnd.Next(PointCount);
            int tmp;
            if (j1 > j2)
            {
                tmp = j1;
                j1 = j2;
                j2 = tmp;
            }
            for (int j = 0; j <= (j2 - j1) / 2; j++)
            {
                tmp = popolation[i, j1 + j];
                popolation[i, j1 + j] = popolation[i, j2 - j];
                popolation[i, j2 - j] = tmp;
            }
        }

        private void TextBox1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Population = int.Parse(TextBox1.Text);
        }
        private void TextBox2_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChanceMutation = double.Parse(TextBox2.Text);
        }
        private void TextBox3_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NIteration = int.Parse(TextBox3.Text);
        }
    }
}