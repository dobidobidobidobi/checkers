using CheckersLogic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Math;


namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Ellipse[,] highlights = new Ellipse[8, 8];

        private Game game;

        private Ellipse highlight;

        private bool boardIsInverted;

        private Mode mode;

        private Piece? pieceSelected = null;

        private bool engineCalculationInProgress;

        private int engineDepth;
        public MainWindow()

        {
            InitializeComponent();
            mode = Mode.PvP;
            InitializeBoard();
            game = new Game();
            DrawBoard(game.Board);
            highlight = GetHighlight();
            InitializeHighlights();
            boardIsInverted = false;
            engineCalculationInProgress = false;
            engineDepth = 1;
        }



        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    pieceImages[i,j] = image;
                    PieceGrid.Children.Add(image);
                }
            }
        }
        private void InitializeHighlights()
        {
            if (highlight != null) 
            { 
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Ellipse ellipse = new Ellipse { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                        highlights[i, j] = ellipse;
                        MoveHighlights.Children.Add(ellipse);
                    }
                } 
            }
            
        }
        


        private void DrawBoard(Board board)
        { 
            if (!boardIsInverted)
                for (int i = 0; i < 8;i++)
                {
                    for(int j = 0;  j < 8;j++)
                    {
                        Piece? piece = board.Grid[i, j].OccupiedBy;
                        pieceImages[i,j].Source = Images.GetImageSource(piece);
                    }
                }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Piece? piece = board.Grid[i, j].OccupiedBy;
                        int offsetX = 7 - i;
                        int offsetY = 7 - j;
                        pieceImages[offsetX, offsetY].Source = Images.GetImageSource(piece);
                    }
                }
            }
        }

        private Ellipse GetHighlight()
        {   
            //kvuli spravnemu resizovani
            double cellSize = Math.Min((CheckerBoard.ActualHeight), CheckerBoard.ActualWidth)/8;

            Ellipse circle = new Ellipse
            {
                Width = cellSize / 3,
                Height = cellSize/ 3,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(160, 40, 40, 40)),
            }; 
            return circle;
        }

        private void ConvertToHighlight(Ellipse highlight,Ellipse child)
        {
            child.Height = highlight.Height;
            child.Width = highlight.Width;
            child.Fill = highlight.Fill;
        }

        private void HighlightResize(Ellipse highlight, Ellipse child)
        {
            child.Height = highlight.Height;
            child.Width = highlight.Width;
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            highlight = GetHighlight();
            foreach (Ellipse child in highlights)
            {
                if (child != null) { HighlightResize(highlight, child); }
            }
        }

        private void ShowHighlights(Piece piece)
        {
            if (!boardIsInverted)
            {
                foreach (Position position in piece.AvailableMoves)
                {
                    ConvertToHighlight(highlight, highlights[position.Row, position.Column]);
                }
            }
            else
            {
                foreach (Position position in piece.AvailableMoves)
                {
                    ConvertToHighlight(highlight, highlights[ 7 - position.Row, 7 - position.Column]);
                }
            }
        }

        private void HideHighlights()
        {
            foreach (Ellipse child in highlights) { child.Fill = null; }
        }

        private void CheckerBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(CheckerBoard);
            Position positionClicked = ToSquarePosition(point);
            if (game.Winner == null)
            {
                if (mode == Mode.PvP)
                {
                    PvpAction(positionClicked);
                }

                else if (mode == Mode.PvE)
                {
                    PveAction(positionClicked);
                }
            }
            if (game.Winner != null) { ShowWinner(game.Winner); }
        }

        private void PvpAction(Position positionClicked)
        {
            HideHighlights();
          
            if ((pieceSelected != null) && (pieceSelected.AvailableMoves.Contains(positionClicked)))
            {
                game.MakeMove(pieceSelected, positionClicked);
                DrawBoard(game.Board);
                pieceSelected = null;
            }

            else
            {

                pieceSelected = game.Board.Grid[positionClicked.Row, positionClicked.Column].OccupiedBy;

                if ((pieceSelected != null) && (game.MoveablePieces.Contains(pieceSelected)))
                {
                    ShowHighlights(pieceSelected);
                }
                else { pieceSelected = null; }
            }       


        }

        private void PveAction(Position positionClicked)
        {
            HideHighlights();
            if (!engineCalculationInProgress)
            {
                Player currentPlayer = game.CurrentPlayer;

                if ((pieceSelected != null) && (pieceSelected.AvailableMoves.Contains(positionClicked)))
                {
                    game.MakeMove(pieceSelected, positionClicked);
                    DrawBoard(game.Board);
                    pieceSelected = null;
                    if ((game.Winner == null) && (currentPlayer != game.CurrentPlayer))
                    {
                        game.MakeEngineMove(engineDepth);
                        DrawBoard(game.Board);
                    }

                }

                else
                {

                    pieceSelected = game.Board.Grid[positionClicked.Row, positionClicked.Column].OccupiedBy;

                    if ((pieceSelected != null) && (game.MoveablePieces.Contains(pieceSelected)))
                    {
                        ShowHighlights(pieceSelected);
                    }
                    else { pieceSelected = null; }
                }
            }
        }


        private Position ToSquarePosition(Point point)
        {
            double squaresize = CheckerBoard.Width / 8;
            int row = ((int)(point.Y / squaresize));
            int column = ((int)(point.X / squaresize));
            if (!boardIsInverted)
            {
                return new Position(row, column);
            }
            return new Position(7 - row,7 - column);
        }

        private void ShowWinner(Player player)
        {
            WinnerPopUp.Opacity = 0.8;
            string winner = "";
            if (player.Color == CheckersLogic.Color.White)
            {
                winner = "White wins";
            }
            else { winner = "Black wins"; }

            WinnerPopUpText.Text = winner;  
        }

        private void HideWinner()
        {
            WinnerPopUp.Opacity = 0;
            WinnerPopUpText.Text = "";
        }

        private void GameReset()
        {
            game = new Game();
            HideHighlights();
            HideWinner();
            DrawBoard(game.Board);
            pieceSelected = null;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GameReset();
        }

        private void InvertBoard_Click(object sender, RoutedEventArgs e)
        {
            boardIsInverted = !boardIsInverted;
            DrawBoard(game.Board);
            InvertHighlights();

        }

        private void InvertHighlights()
        { 
            List<Position> oldPositions = new List<Position>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (highlights[i, j].Fill == highlight.Fill)
                    {
                        oldPositions.Add(new Position(i, j));
                    }
                }
            }
            HideHighlights();

            foreach (Position position in oldPositions)
            {
                ConvertToHighlight(highlight, highlights[7 - position.Row, 7 -  position.Column]);
            }
            
        }

        private void PvpMode_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.PvP;
            PvpMode.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4CBB17"));
            PveMode.Background = Brushes.Red;
            EveMode.Background = Brushes.Red;

        }

        private void PveMode_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.PvE;
            PveMode.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4CBB17"));
            PvpMode.Background = Brushes.Red;
            EveMode.Background = Brushes.Red;
            game.MakeEngineMove(engineDepth);
            DrawBoard(game.Board);
        }

        private void EveMode_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.EvE;
            EveMode.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4CBB17"));
            PveMode.Background = Brushes.Red;
            PvpMode.Background = Brushes.Red;
        }
    }
}