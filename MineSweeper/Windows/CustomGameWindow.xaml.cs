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
using System.Windows.Shapes;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for CustomGameWindow.xaml
    /// </summary>
    public partial class CustomGameWindow : Window
    {
        private int length;
        private int height;
        private int bombs;
        public CustomGameWindow()
        {
            InitializeComponent();
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                length = int.Parse(txtWidth.Text);
                height = int.Parse(txtHeight.Text);
                bombs = int.Parse(txtBombs.Text);

                if (length < 3 || height < 3 || bombs < 1 || bombs >= length * height || height > 50 || length > 50)
                {
                    throw new Exception();
                }

                GameWindow gw = new GameWindow(length, height, bombs);
                gw.Show();
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Make sure all fields are numbers between 3 and 50", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
