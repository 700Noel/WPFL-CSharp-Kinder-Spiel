﻿using System.Diagnostics;
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
        private DispatcherTimer gameTimer = new DispatcherTimer();

        private int gameTick = 0;

        private Brush[] buttonColors = [Brushes.LightGreen, Brushes.Goldenrod, Brushes.Red];

        private int previousRandomColor = -1;

        private int clickedCounter = 0;

        private String gameMode = "TrafficLight";

        private Brush currentColor;

        private Difficulty Difficulty = Difficulty.Hard;

        private Dictionary<Difficulty, int[]> sizeDifficulties = new Dictionary<Difficulty, int[]>()
        {
            [Difficulty.Easy] = [120, 180],
            [Difficulty.Medium] = [60, 120],
            [Difficulty.Hard] = [30, 60]
        };

        private Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Interval = TimeSpan.FromSeconds(2);
            gameTimer.Tick += OnGameTimerTick;

            buttonStyleTimer.Interval = TimeSpan.FromSeconds(2);
            buttonStyleTimer.Tick += OnButtonStyleTimerTick;
            //buttonStyleTimer.Start();


            hideButtonTimer.Interval = TimeSpan.FromSeconds(1); // Trigger every second
            hideButtonTimer.Tick += OnHideButtonTimerTick;
            //hideButtonTimer.Start();
        }

        private void OnGameTimerTick(object sender, EventArgs e)
        {
            dispatcherTimer_Tick();
        }

        private void dispatcherTimer_Tick() //object sender, EventArgs e
        {
            gameTick++;

            // Hiding of Buttons
            movingButton.Visibility = movingButton.Visibility == Visibility.Hidden ? Visibility.Collapsed : Visibility.Collapsed;
            movingButton_Red.Visibility = movingButton_Red.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            movingButton_Orange.Visibility = movingButton_Orange.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            movingButton_Green.Visibility = movingButton_Green.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

            //Change the Appearence
            Random random = new Random();

            int[] sizeValues = sizeDifficulties[Difficulty];

            double newWidth = random.Next(sizeValues[0], sizeValues[1]);
            double newHeight = random.Next(sizeValues[0], sizeValues[1]);

            // Apply the new size and position
            movingButton_Red.Width = newWidth;
            movingButton_Red.Height = newHeight;

            movingButton_Orange.Width = newWidth;
            movingButton_Orange.Height = newHeight;

            movingButton_Green.Width = newWidth;
            movingButton_Green.Height = newHeight;

            double newLeft1 = random.Next(100, 350);
            double newTop1 = random.Next(50, 130);

            double newLeft2 = random.Next(100, 350);
            double newTop2 = random.Next(50, 130);

            double newLeft3 = random.Next(100, 350);
            double newTop3 = random.Next(50, 130);

            double[] xPositions = [newLeft1, newLeft2, newLeft3];
            double[] yPositions = [newTop1, newTop2, newTop3];

            // Assign new Button Positions
            assign_New_Positions(xPositions, yPositions, random);

            int randomColorIndex = random.Next(0, buttonColors.Length);

            if (randomColorIndex == previousRandomColor)
            {
                if (randomColorIndex == buttonColors.Length - 1)
                {
                    randomColorIndex--;
                }
                else
                {
                    randomColorIndex++;
                }
            }
            changeLights(buttonColors[randomColorIndex]);
            this.currentColor = buttonColors[randomColorIndex];
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

            double newLeft = random.Next(150, 400);
            double newTop = random.Next(30, 180);

            movingButton.Margin = new Thickness(newLeft, newTop, 0, 0);


            int randomColorIndex = random.Next(0, buttonColors.Length);

            if (randomColorIndex == previousRandomColor)
            {
                if (randomColorIndex == buttonColors.Length - 1)
                {
                    randomColorIndex--;
                }
                else
                {
                    randomColorIndex++;
                }
            }

            previousRandomColor = randomColorIndex;

            movingButton.Background = buttonColors[randomColorIndex];
            movingButton.Tag = randomColorIndex;
        }

        private void assign_New_Positions(double[] x_Axis, double[] y_Axis, Random random)
        {
            int xIndex;
            int yIndex;

            xIndex = random.Next(0, x_Axis.Length - 1);
            yIndex = random.Next(0, y_Axis.Length - 1);

            movingButton_Red.Margin = new Thickness(x_Axis[xIndex], y_Axis[yIndex], 0, 0);
            x_Axis = x_Axis.Where((val, idx) => idx != xIndex).ToArray();
            y_Axis = y_Axis.Where((val, idx) => idx != yIndex).ToArray();

            xIndex = random.Next(0, x_Axis.Length - 1);
            yIndex = random.Next(0, y_Axis.Length - 1);

            movingButton_Orange.Margin = new Thickness(x_Axis[xIndex], y_Axis[yIndex], 0, 0);
            x_Axis = x_Axis.Where((val, idx) => idx != xIndex).ToArray();
            y_Axis = y_Axis.Where((val, idx) => idx != yIndex).ToArray();

            xIndex = random.Next(0, x_Axis.Length - 1);
            yIndex = random.Next(0, y_Axis.Length - 1);

            movingButton_Green.Margin = new Thickness(x_Axis[xIndex], y_Axis[yIndex], 0, 0);
            x_Axis = x_Axis.Where((val, idx) => idx != xIndex).ToArray();
            y_Axis = y_Axis.Where((val, idx) => idx != yIndex).ToArray();
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

                /*
                // Get the ID from the Tag property
                int id = (int)colorButton.Tag;
                if(id == 0)
                {
                    greenClickedCounter++;
                }
                */
                changeLights(colorButton.Background);
            }
        }

        private void Button_Clicked_Red(object sender, RoutedEventArgs e)
        {
            if (sender is Button colorButton)
            {
                if (this.currentColor == colorButton.Background)
                {
                    TestBox.Text = "ROOOOOT";
                    correct_Button_Pressed();

                }
                else
                {
                    TestBox.Text = "FALSCH";
                }
            }
        }

        private void Button_Clicked_Orange(object sender, RoutedEventArgs e)
        {
			if (sender is Button colorButton)
			{
                if (this.currentColor == colorButton.Background)
                {
                    TestBox.Text = "OLOOONGE";
                    correct_Button_Pressed();
                }
                else
                {
                    TestBox.Text = "FALSCH";
                }
			}
		}

        private void Button_Clicked_Green(object sender, RoutedEventArgs e)
        {
			if (sender is Button colorButton)
			{
				if (this.currentColor == colorButton.Background)
				{
					TestBox.Text = "GREEEN";
                    correct_Button_Pressed();
                }
                else
                {
                    TestBox.Text = "FALSCH";
                }
            }
		}

        private void correct_Button_Pressed()
        {
            if (clickedCounter == 0)
            {
                stopwatch.Start();
            }

            clickedCounter++;
            pbStatus.Value = clickedCounter;
            dispatcherTimer_Tick();

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

        private void ChangeGame_Traffic(object sender, RoutedEventArgs e)
        {
            this.gameMode = "TrafficLight";
            this.movingButton_Red.Visibility = Visibility.Visible;
            this.movingButton_Orange.Visibility = Visibility.Visible;
            this.movingButton_Green.Visibility = Visibility.Visible;
            this.movingButton.Visibility = Visibility.Collapsed;

            gameTimer.Start();

            buttonStyleTimer.Stop();
            hideButtonTimer.Stop();

            e.Handled = true;


        }

        private void ChangeGame_Other(object sender, RoutedEventArgs e)
        {
            this.gameMode = "Other";
            this.movingButton_Red.Visibility = Visibility.Collapsed;
            this.movingButton_Orange.Visibility = Visibility.Collapsed;
            this.movingButton_Green.Visibility = Visibility.Collapsed;
            this.movingButton.Visibility = Visibility.Visible;
            buttonStyleTimer.Start();
            hideButtonTimer.Start();

            gameTimer.Stop();

            e.Handled = true;
        }

        private void changeLights(Brush color)
        {
            // Only change the Lights after every second gameTick
            if (color != null && gameTick % 2 == 0)
            {
                if (color == Brushes.Red)
                {
                    this.topLight.Fill = Brushes.Red;
                    this.midLight.Fill = Brushes.DarkGoldenrod;
                    this.botLight.Fill = Brushes.DarkGreen;

                }
                else if (color == Brushes.Goldenrod)
                {
                    this.topLight.Fill = Brushes.DarkRed;
                    this.midLight.Fill = Brushes.Goldenrod;
                    this.botLight.Fill = Brushes.DarkGreen;
                }
                else if (color == Brushes.LightGreen)
                {
                    this.topLight.Fill = Brushes.DarkRed;
                    this.midLight.Fill = Brushes.DarkGoldenrod;
                    this.botLight.Fill = Brushes.LightGreen;
                }
                else
                {
                    return;
                }
            }
        }
    }
}