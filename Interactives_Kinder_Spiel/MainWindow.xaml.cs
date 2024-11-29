using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Threading;

namespace Interactives_Kinder_Spiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer buttonStyleTimer = new DispatcherTimer();
        private DispatcherTimer hideButtonTimer = new DispatcherTimer();

        private Difficulty Difficulty = Difficulty.Hard;

        private Brush[] buttonColors = [Brushes.Green, Brushes.Yellow, Brushes.Red];

        private const int BRUSHGREEN = 0;
        private const int BRUSHYELLOW = 1;
        private const int BRUSHRED = 2;

        private int previousRandomColor = -1;

        private int clickedCounter = 0;

        private Stopwatch stopwatch = new Stopwatch();

        private Dictionary<Difficulty, int[]> sizeDifficulties = new Dictionary<Difficulty, int[]>() 
        {
            [Difficulty.Easy] = [150, 200],
            [Difficulty.Medium] = [70, 140],
            [Difficulty.Hard] = [20, 50]
        };

        public MainWindow()
        {
            InitializeComponent();

            buttonStyleTimer.Interval = TimeSpan.FromSeconds(2);
            buttonStyleTimer.Tick += OnButtonStyleTimerTick;
            buttonStyleTimer.Start();


            hideButtonTimer.Interval = TimeSpan.FromSeconds(1); // Trigger every second
            hideButtonTimer.Tick += OnHideButtonTimerTick;
            hideButtonTimer.Start();
        }

        private void OnHideButtonTimerTick(object sender, EventArgs e)
        {
            movingButton.Visibility = movingButton.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnButtonStyleTimerTick(object sender, EventArgs e)
        {
            Random random = new Random();

            int[] sizeValues = sizeDifficulties[Difficulty];

            double newWidth = random.Next(sizeValues[0], sizeValues[1]);
            double newHeight = random.Next(sizeValues[0], sizeValues[1]);

            //double newLeft = random.Next(50, 300);
            //double newTop = random.Next(30, 300);


            // Apply the new size and position
            movingButton.Width = newWidth;
            movingButton.Height = newHeight;

            double newLeft = random.Next(300, 620);
            double newTop = random.Next(50, 300);

            movingButton.Margin = new Thickness(newLeft, newTop, 0, 0);


            int randomColorIndex = random.Next(0, buttonColors.Length);

            if (randomColorIndex == previousRandomColor)
            {
                if(randomColorIndex == buttonColors.Length - 1)
                {
                    randomColorIndex--;
                } else
                {
                    randomColorIndex++;
                }
            }

            previousRandomColor = randomColorIndex;

            movingButton.Background = buttonColors[randomColorIndex];
            movingButton.Tag = randomColorIndex;
        }

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button colorButton)
            {
                if (clickedCounter == 0)
                {
                    stopwatch.Start();
                }

                clickedCounter++;
                pbStatus.Value = clickedCounter;

                if (clickedCounter == 10)
                {
                    stopwatch.Stop();
                    TimeSpan elapsed = stopwatch.Elapsed;

                    MessageBox.Show($"Maximum clicks reached!\nTime taken: {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds.");

                    clickedCounter = 0;
                    pbStatus.Value = 0;
                    stopwatch.Reset();
                }
            }
        }

        private void ChangeMode_Easy(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Easy;
            EasyMode.IsChecked = true;
            MediumMode.IsChecked = false;
            HardMode.IsChecked = false;
            e.Handled = true;
        }

        private void ChangeMode_Medium(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Medium;
            EasyMode.IsChecked = false;
            MediumMode.IsChecked = true;
            HardMode.IsChecked = false;
            e.Handled = true;
        }

        private void ChangeMode_Hard(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Hard;
            EasyMode.IsChecked = false;
            MediumMode.IsChecked = false;
            HardMode.IsChecked = true;
            e.Handled = true;
        }
    }
}