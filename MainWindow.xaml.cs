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
        private static Delegate handler { get; set; }
        private Board board;

        public MainWindow()
        {
            InitializeComponent();
            handler = MouseMove;
            board = new Board(100, 100, 0, boardCanvas, new MouseMoveEventHandler(MouseMove));
        }
        
        
        private void MouseMove(object sender, MouseEventArgs e)
        {         

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                dragItem = sender;
                Image image = sender as Image;
                dragOffsetX = e.GetPosition(boardCanvas).X - Canvas.GetLeft(image);
                dragOffsetY = e.GetPosition(boardCanvas).Y - Canvas.GetTop(image);
                //Piece piece = board.findPieceById(image.Name);
                DragDrop.DoDragDrop(image, image, DragDropEffects.Move);
                   

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
    }
}
