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

namespace TankApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static int N = 1000;
        public int Diameter, Height, Lenght, Radius;

        public static double Quotient(double coordinateX, double coordinateY)
        {
            return coordinateX * coordinateX / (coordinateY * coordinateY);
        }

        public static double CalculateCylinderSegmentVolume(int R, int H, int L)
        {
            double x, y;
            int n = 0, hN, S;
            hN = N * H / R;
            S = 2 * R * H;
            for (int i = 0; i <= N; i++)
            {
                x = 2 * i * R / N - R;
                {
                    for (int j = 0; j <= hN; j++)
                    {
                        y = j * H / hN - R;
                        if (Quotient(x, R) + Quotient(y, R) <= 1.0)
                            n++;
                    }
                }
            }
            return (double)S * n * L / (N * hN);
        }

        public static double CalculateEllipsoidSegmentVolume(int semiaxesA, int semiaxesB, int semiaxesC, int H)
        {
            double x, y, z, V;
            int n = 0, bN, hN;
            V = 4 * semiaxesA * semiaxesB * H;
            bN = N * semiaxesB / semiaxesA;
            hN = N * H / semiaxesA;
            for (int i = 0; i <= N; i++)
            {
                x = 2 * i * semiaxesA / N - semiaxesA;
                for (int j = 0; j <= bN; j++)
                {
                    y = 2 * j * semiaxesB / bN - semiaxesB;
                    for (int k = 0; k <= hN; k++)
                    {
                        z = k * H / hN - semiaxesC;
                        if (Quotient(x, semiaxesA) + Quotient(y, semiaxesB) + Quotient(z, semiaxesC) <= 1.0)
                            n++;
                    }
                }
            }
            return (double)V * n / (N * bN * hN);
        }

        public static double CalculateFilledTankVolume(int D, int L, int R, int H)
        {
            return (CalculateCylinderSegmentVolume(D / 2, H, L) + CalculateEllipsoidSegmentVolume(D / 2, R, D / 2, H)) / 1e9;
        }

        public static double CalculateCylinderVolume(int D, int L)
        {
            return Math.PI * (D / 2) * (D / 2) * L;
        }

        public static double CalculateEllipsoidVolume(int semiaxesA, int semiaxesB, int semiaxesC)
        {
            return Math.PI * semiaxesA * semiaxesB * semiaxesC * 4 / 3;
        }

        public static double CalculateTankVolume(int D, int L, int R)
        {
            return (double)(CalculateCylinderVolume(D, L) + CalculateEllipsoidVolume(D / 2, D / 2, R)) / 1e9;
        }

        public static string FormatVolume(double x)
        {
            return string.Format("{0:0.###}м3", x);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        public static int ConvertData(TextBox textBox)
        {
            return Convert.ToInt32(textBox.Text);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Diameter = ConvertData(textBoxDiameter);
            Height = ConvertData(textBoxHeight);
            Lenght = ConvertData(textBoxLenght);
            Radius = ConvertData(textBoxRadius);
            textBoxTankVolume.Text = FormatVolume(CalculateTankVolume(Diameter, Lenght, Radius));
            textBoxLiquidVolume.Text = FormatVolume(CalculateFilledTankVolume(Diameter, Lenght, Radius, Height));
        }
    }
}
