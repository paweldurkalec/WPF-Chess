using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFChess
{
    internal class Timer
    {

        Stopwatch stopwatch;
        DispatcherTimer dispatcherTimer;
        Label name;
        Point position;
        Label content;
        Ellipse circleAround;
        TimeSpan time;
        public bool ended;

        public Timer(int seconds, Point position, string name)
        {
            this.position = position;
            ended = false;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Tick);
            dispatcherTimer.Interval += new TimeSpan(0, 0, 0, 0, 1);
            time = new TimeSpan(0,0,0,seconds);
            stopwatch = new Stopwatch();
            content = new Label()
            {
                FontSize=30,
            };
            content.Content = String.Format("{0:00}:{1:00}",
                    time.Minutes, time.Seconds);
            Variables.boardCanvas.Children.Add(content);
            Canvas.SetTop(content, position.Y);
            Canvas.SetLeft(content, position.X);
            this.name = new Label()
            {
                FontSize = 30,
                FontWeight = System.Windows.FontWeights.Medium,
                Content=name
            };
            Variables.boardCanvas.Children.Add(this.name);
            circleAround = new Ellipse()
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black
            };
            Variables.boardCanvas.Children.Add(circleAround);
            Canvas.SetTop(circleAround, position.Y + 48 / 2 - circleAround.Height / 2);
            Canvas.SetLeft(circleAround, position.X + 81.2 / 2 - circleAround.Width / 2);
            Canvas.SetTop(this.name, position.Y + content.ActualHeight / 2 - this.name.ActualHeight / 2 - 65);
            Canvas.SetLeft(this.name, position.X + content.ActualWidth / 2 - this.name.ActualWidth / 2);
        }

        public void start()
        {
            stopwatch.Start();
            dispatcherTimer.Start();
        }

        public void stop()
        {
            dispatcherTimer.Stop();
            stopwatch.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                TimeSpan ts = stopwatch.Elapsed;
                stopwatch.Restart();
                time = time - ts;              
            }
            if (time <= new TimeSpan(0, 0, 0, 0, 0))
            {
                time = new TimeSpan(0, 0, 0, 0, 0);
                ended = true;
            }
            content.Content = String.Format("{0:00}:{1:00}",
                    time.Minutes, time.Seconds);
            Canvas.SetTop(circleAround, position.Y + content.ActualHeight / 2 - circleAround.Height / 2);
            Canvas.SetLeft(circleAround, position.X + content.ActualWidth / 2 - circleAround.Width / 2);
            Canvas.SetTop(name, position.Y + content.ActualHeight / 2 - name.ActualHeight / 2 - 65);
            Canvas.SetLeft(name, position.X + content.ActualWidth / 2 - name.ActualWidth / 2);
        }
    }
}
