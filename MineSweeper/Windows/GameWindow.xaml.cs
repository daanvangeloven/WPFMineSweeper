using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MineSweeper.Classes;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, INotifyPropertyChanged
    {
        private Random r;
        private readonly int _height;
        private readonly int _length;
        private int _bombs;
        private int _start_bombs;
        private DispatcherTimer t;
        private int _time;
        private Tile[,] _tileGrid;
        readonly ImageSource mineSource;
        readonly ImageSource flagSource;
        private int _tileCounter;

        public GameWindow(int l, int h, int b)
        {
            InitializeComponent();
            
            r = new Random();
            _height = h;
            _length = l;
            _bombs = b;
            _start_bombs = b;

            // Initialize new dispatchertimer with 1 second interval
            t = new DispatcherTimer{Interval = TimeSpan.FromMilliseconds(1000)};
            t.Tick += TOnTick;
            t.Start();
            DataContext = this;
            _tileCounter = -2; //this has to be -2 because both the lenght and height parameter start at 1
            GenerateTiles();
            
            // Load needed images into memory
            flagSource = new BitmapImage(new Uri(@"Resources/flag.png", UriKind.Relative));
            mineSource = new BitmapImage(new Uri(@"Resources/mine.png", UriKind.Relative));
        }

        // Every tick this method is called
        private void TOnTick(object? sender, EventArgs e)
        {
            Time++; 
        }

        /// <summary>
        /// Generate the starting grid into a multidimensional array
        /// </summary>
        private void GenerateTiles()
        {
             // Generate empty grid of tile objects
           _tileGrid = new Tile[_height, _length];
           for (int row = 0; row < _height; row++)
           {
               // generate row here
               for (int col = 0; col < _length; col++)
               {
                   // Generate column here
                   _tileGrid[row, col] = new Tile()
                   {
                       IsCleared = false,
                       IsFlagged = false,
                       IsMine = false,
                       Row = row,
                       Column = col
                   };
                    

                }
            }

           // Place mines in empty grid
           for(int i = 0; i < _bombs; i++)
           {
               int row = r.Next(0, _height);
               int col = r.Next(_length);

               if (_tileGrid[row, col].IsMine) --i; // Check if the field is already a mine
               else _tileGrid[row, col].IsMine = true;
                Debug.WriteLine(_tileGrid[row, col]);
            }
           GenerateGrid();
        }

        /// <summary>
        /// Generate the starting playing field UI elements
        /// </summary>
        private void GenerateGrid()
        {
            StackPanel mineField = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            for (int row = 0; row < _tileGrid.GetLength(0); row++)
            {

                StackPanel RowStack = new StackPanel();
                RowStack.Orientation = Orientation.Horizontal;
                // generate row here
                for (int col = 0; col < _tileGrid.GetLength(1); col++)
                {
                    // Generate Column here
                    Button gridBtn = new Button
                    {
                        Width = 20,
                        Height = 20,
                        Background = Brushes.White,
                        Style = (Style)Application.Current.Resources["TileButton"],
                        Name = "btn"+row+col
                    };

                    var row1 = row;
                    var col1 = col;
                    gridBtn.Click += (s, a) =>  GridBtn_Click(s, a,row1, col1);
                    gridBtn.MouseRightButtonDown += (s, a) => FlagTile(s, a, row1, col1);
                    RowStack.Children.Add(gridBtn);
                }
                mineField.Children.Add(RowStack);
            }
            MinefieldGrid.Children.Add(mineField);
        }

        /// <summary>
        /// This method gets called when a tile gets left-clicked
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="a">Event Arguments</param>
        /// <param name="row">Row of the tile</param>
        /// <param name="col">Column of the tile</param>
        private void GridBtn_Click(object sender, RoutedEventArgs a ,int row, int col)
        {
            if (!_tileGrid[row, col].IsFlagged)
            {
                // Stap 1: Als er een mijn ligt is het game over
                if (_tileGrid[row, col].IsMine)
                {
                    ((Button) sender).Background = new ImageBrush(mineSource);
                    if ((MessageBox.Show("You have hit a mine! \n Restart?", "Game Over", MessageBoxButton.YesNo) ==
                         MessageBoxResult.Yes))
                    {
                        new GameWindow(_length, _height, _start_bombs).Show();
                        this.Close();
                    }
                    else new MainWindow().Show();

                    this.Close();

                }
                else
                {
                    ((Button) sender).IsEnabled = false;
                    ((Button) sender).Background = Brushes.Black;
                    ((Button) sender).Foreground = Brushes.White;
                    _tileGrid[row, col].IsCleared = true;

                    _tileCounter++;

                    // Maak een list van elke button om de aangeklikte heen
                    int mineCounter = 0;
                    List<Tile> surroundingTiles = GetNeighbouringCells(row, col);
                    foreach (Tile t in surroundingTiles)
                    {
                        // Loop door die lijst en tel de mines eromheen
                        if (t.IsMine) mineCounter++;
                    }

                    // Stap 3: Als dit 0 is dan clear je alle tiles eromheen en tel je bij de geclearde tiles weer opnieuw het aantal mijnen
                    if (mineCounter != 0) ((Button) sender).Content = mineCounter.ToString();
                    else
                    {
                        ClearSurroundingCells(row, col);
                    }
                }

                CheckWin();
            }
        }

        // CHeck if all tiles have been cleared
        private void CheckWin()
        {
            bool isCleared = true;
            for (int i = 0; i < _tileGrid.GetLength(0); i++)
            {
                if (!isCleared) break; 

                for (int j = 0; j < _tileGrid.GetLength(1); j++)
                {
                    if (!_tileGrid[i, j].IsFlagged && !_tileGrid[i, j].IsCleared)
                    {
                        isCleared = false;
                        break;
                    }
                }
            }

            if (isCleared)
            {
                t.Stop();
                if (MessageBox.Show("You have cleared all mines! \n Restart?", "Congratulations!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    new GameWindow(_length, _height, _start_bombs).Show();
                    this.Close();
                }
                else new MainWindow().Show();
            }
        }

        /// <summary>
        /// Clear surrounding empty cells
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        private void ClearSurroundingCells(int row, int col)
        {
            List<Tile> surroundingCells = GetNeighbouringCells(row, col);
            foreach (Tile tile in surroundingCells)
            {
                List<Tile> tileList = GetNeighbouringCells(tile.Row, tile.Column);
                int counter = 0;
                foreach (Tile tile1 in tileList)
                {
                    if (tile1.IsMine)
                    {
                        counter++;
                    }
                }
                if (counter == 0 && (tile.Row != row || tile.Column != col) && !tile.IsCleared)
                {
                    tile.IsCleared = true;
                    ClearSurroundingCells(tile.Row, tile.Column);
                }
                else
                {

                    ((Button)FindDescendant(MinefieldGrid, $"btn{tile.Row}{tile.Column}")).IsEnabled = false;
                    ((Button)FindDescendant(MinefieldGrid, $"btn{tile.Row}{tile.Column}")).Background = Brushes.Black;
                    ((Button) FindDescendant(MinefieldGrid, $"btn{tile.Row}{tile.Column}")).Foreground = Brushes.White;
                    _tileGrid[tile.Row, tile.Column].IsCleared = true;
                    if (counter>0) ((Button)FindDescendant(MinefieldGrid, $"btn{tile.Row}{tile.Column}")).Content = counter;
                }
            }
        }

        /// <summary>
        /// Get a list of neighbouring cells
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <returns>List of surrounding cells</returns>
        private List<Tile> GetNeighbouringCells(int row, int col)
        {
            List<Tile> surroundingTiles = new List<Tile>();

            for (int i = -1; i <= 1; i++)
            {
                //check if row exists (in case of corners or edges)
                if (row + i > -1 && row + i < _tileGrid.GetLength(0))
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        // check if column exists (in case of corners or edges)
                        if (col + j > -1 && col + j < _tileGrid.GetLength(1) && (col + j != 0 || row + i != 0))
                        {
                            surroundingTiles.Add(_tileGrid[row+i, col+j]);
                        }
                    }
                }
            }
            return surroundingTiles;
        }

        /// <summary>
        /// This method gets called when a tile gets right-clicked
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="a">Event Arguments</param>
        /// <param name="row">Row of the tile</param>
        /// <param name="col">Column of the tile</param>
        private void FlagTile(object sender, RoutedEventArgs a, int row, int col)
        {
            if (_tileGrid[row, col].IsFlagged)
            {
                ((Button)sender).Foreground = Brushes.White;
                ((Button)sender).Background = Brushes.White;
                _tileGrid[row, col].IsFlagged = false;
                Bombs++;
                _tileCounter--;
            }
            else
            {
                ((Button)sender).Background = new ImageBrush(flagSource);
                ((Button)sender).Foreground = Brushes.Transparent;
                _tileGrid[row, col].IsFlagged = true;
                --Bombs;
                _tileCounter++;
            }

            CheckWin();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string elementName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(elementName));
        }

        public int Bombs
        {
            get => _bombs;
            set { _bombs = value; OnPropertyChanged("Bombs"); }

        }

        public int Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged("Time"); }
        }

        // Bron: http://csharphelper.com/blog/2020/09/find-controls-by-name-in-wpf-with-c/
        // Find a descendant control by name.
        private static DependencyObject FindDescendant(
            DependencyObject parent, string name)
        {
            // See if this object has the target name.
            FrameworkElement element = parent as FrameworkElement;
            if ((element != null) && (element.Name == name)) return parent;

            // Recursively check the children.
            int num_children = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < num_children; i++)
            {
                // See if this child has the target name.
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                DependencyObject descendant = FindDescendant(child, name);
                if (descendant != null) return descendant;
            }

            // We didn't find a descendant with the target name.
            return null;
        }
    }
}
