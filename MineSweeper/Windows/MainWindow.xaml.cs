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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame(int lenght, int height, int bombs)
        {
            GameWindow gw = new GameWindow(lenght, height, bombs);
            gw.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartGame(9, 9, 10);

        }

        private void BtnIntermediate_Click(object sender, RoutedEventArgs e)
        {
            StartGame(16, 16, 40);
        }

        private void BtnExpert_Click(object sender, RoutedEventArgs e)
        {
            StartGame(30, 16, 99);
        }

        private void BtnCustom_Click(object sender, RoutedEventArgs e)
        {
            CustomGameWindow cgw = new CustomGameWindow();
            cgw.Show();
            this.Close();
        }
    }
}
