using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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

namespace WPFChess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double dragOffsetX = 0;
        private double dragOffsetY = 0;
        private object dragItem = null;
        public delegate void MouseMoveEventHandler(object sender, MouseEventArgs e);
        private Board board;

        public MainWindow()
        {
            InitializeComponent();
            Variables.autoRotation = autoRotation;
            board = new Board(100, 100, 8, 0, boardCanvas, new MouseMoveEventHandler(MouseMove), new MouseMoveEventHandler(MouseClick));
        }
        
        
        private void MouseMove(object sender, MouseEventArgs e)
        {         

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (board.duringPromotion == null)
                {
                    dragItem = sender;
                    Image image = sender as Image;
                    if (board.rightTurn(image))
                    {
                        dragOffsetX = e.GetPosition(boardCanvas).X - Canvas.GetLeft(image);
                        dragOffsetY = e.GetPosition(boardCanvas).Y - Canvas.GetTop(image);
                        board.showMoves(image);
                        DragDrop.DoDragDrop(image, image, DragDropEffects.Move);
                    }
                }
            }
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (board.duringPromotion != null)
            {
                board.dropPiece(sender as Image, new Point());
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(boardCanvas);
            Image image = dragItem as Image;
            board.dropPiece(image, dropPosition);
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(boardCanvas);
            Image image = dragItem as Image;
            Canvas.SetLeft(image, dropPosition.X - dragOffsetX);
            Canvas.SetTop(image, dropPosition.Y - dragOffsetY);
        }

        private void rotate_Click(object sender, RoutedEventArgs e)
        {
            Variables.board.rotateBoard();
        }

        private void prev_button_Click(object sender, RoutedEventArgs e)
        {
            Variables.board.history.previousMove();
        }

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            Variables.board.history.nextMove();
        }
    }
}
