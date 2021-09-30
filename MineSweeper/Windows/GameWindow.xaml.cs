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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Random r;
        private readonly int height;
        private readonly int length;
        private int bombs;
        public GameWindow(int l, int h, int b)
        {
            InitializeComponent();
            
            r = new Random();
            height = h;
            length = l;
            bombs = b;

            GenerateTiles();
        }

        private void GenerateTiles()
        {
            StackPanel mineField = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            for (int row = 0; row < height; row++)
            {
                StackPanel RowStack = new StackPanel();
                RowStack.Orientation = Orientation.Horizontal;
                // generate row here
                for (int col = 0; col < length; col++)
                {
                    // Generate Column here
                    Button GridBtn = new Button
                    {
                        Width = 20,
                        Height = 20
                    };
                    RowStack.Children.Add(GridBtn);
                }

                mineField.Children.Add(RowStack);
            }

            MinefieldGrid.Children.Add(mineField);
        }
    }
}
