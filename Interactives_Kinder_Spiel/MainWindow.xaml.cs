﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private Brush[] buttonColors = [Brushes.LightGreen, Brushes.Yellow, Brushes.Red];

        private int previousRandomColor = -1;

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

            ChangeGameTraffic();
        }

        private void OnGameTimerTick(object sender, EventArgs e)
        {
            DispatcherTimerTick();
        }

        private void DispatcherTimerTick() //object sender, EventArgs e
        {
            gameTick++;

            // Hiding of Buttons
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
            AssignNewPositions(xPositions, yPositions, random);

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
            ChangeLights(buttonColors[randomColorIndex]);
            this.currentColor = buttonColors[randomColorIndex];
        }

        private void AssignNewPositions(double[] x_Axis, double[] y_Axis, Random random)
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

        private void ButtonClickedRed(object sender, RoutedEventArgs e)
        {
            if (sender is Button colorButton)
            {
                if (this.currentColor == colorButton.Background)
                {
                    CorrectButtonPressed();
                }
                else
                {
                    IncorrectButtonPressed();
                }
            }
        }

        private void ButtonClickedOrange(object sender, RoutedEventArgs e)
        {
			if (sender is Button colorButton)
			{
                if (this.currentColor == colorButton.Background)
                {
                    CorrectButtonPressed();
                }
                else
                {
                    IncorrectButtonPressed();
                }
            }
		}

        private void ButtonClickedGreen(object sender, RoutedEventArgs e)
        {
			if (sender is Button colorButton)
			{
				if (this.currentColor == colorButton.Background)
				{
                    CorrectButtonPressed();
                } 
                else
                {
                    IncorrectButtonPressed();
                }
            }
		}

        private void IncorrectButtonPressed()
        {
            if (pbStatus.Value > 0)
            {
                pbStatus.Value--;
            }
            DispatcherTimerTick();
        }

        private void CorrectButtonPressed()
        {
            if (pbStatus.Value == 0)
            {
                stopwatch.Start();
            }

            pbStatus.Value++;
            DispatcherTimerTick();

            if (pbStatus.Value == 10)
            {
                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;

                MessageBox.Show($"Maximum clicks reached!\nTime taken: {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds.");

                pbStatus.Value = 0;
                stopwatch.Reset();
            }
        }

        private void ChangeModeEasy(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Easy;
            EasyMode.IsChecked = true;
            MediumMode.IsChecked = false;
            HardMode.IsChecked = false;
            e.Handled = true;
        }

        private void ChangeModeMedium(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Medium;
            EasyMode.IsChecked = false;
            MediumMode.IsChecked = true;
            HardMode.IsChecked = false;
            e.Handled = true;
        }

        private void ChangeModeHard(object sender, RoutedEventArgs e)
        {
            Difficulty = Difficulty.Hard;
            EasyMode.IsChecked = false;
            MediumMode.IsChecked = false;
            HardMode.IsChecked = true;
            e.Handled = true;
        }

        private void ChangeGoal(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check != null)
            {
                if (double.TryParse(check.Content.ToString(), out double goal))
                {
                    pbStatus.Maximum = goal;
                    pbStatus.Value = 0;

                    switch (goal)
                    {
                        case 10:
                            Goal10.IsChecked = true;
                            Goal20.IsChecked = false;
                            Goal50.IsChecked = false;
                            Goal100.IsChecked = false;
                            break;
                        case 20:
                            Goal10.IsChecked = false;
                            Goal20.IsChecked = true;
                            Goal50.IsChecked = false;
                            Goal100.IsChecked = false;
                            break;
                        case 50:
                            Goal10.IsChecked = false;
                            Goal20.IsChecked = false;
                            Goal50.IsChecked = true;
                            Goal100.IsChecked = false;
                            break;
                        case 100:
                            Goal10.IsChecked = false;
                            Goal20.IsChecked = false;
                            Goal50.IsChecked = false;
                            Goal100.IsChecked = true;
                            break;
                    }
                }
            }
        }


        private void ChangeGameTraffic()
        {
            this.gameMode = "TrafficLight";
            this.movingButton_Red.Visibility = Visibility.Visible;
            this.movingButton_Orange.Visibility = Visibility.Visible;
            this.movingButton_Green.Visibility = Visibility.Visible;

            gameTimer.Start();

            buttonStyleTimer.Stop();
            hideButtonTimer.Stop();
        }

        private void ChangeLights(Brush color)
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
                else if (color == Brushes.Yellow)
                {
                    this.topLight.Fill = Brushes.DarkRed;
                    this.midLight.Fill = Brushes.Yellow;
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