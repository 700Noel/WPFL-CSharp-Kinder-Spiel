﻿using System.Drawing;
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

        private Brush[] buttonColors = [Brushes.Green, Brushes.Yellow, Brushes.Red];

        private int previousRandomColor = -1;

        private int greenClickedCounter = 0;

        public MainWindow()
        {
            InitializeComponent();

            buttonStyleTimer.Interval = TimeSpan.FromSeconds(4);
            buttonStyleTimer.Tick += OnButtonStyleTimerTick;
            buttonStyleTimer.Start();


            hideButtonTimer.Interval = TimeSpan.FromSeconds(2); // Trigger every second
            hideButtonTimer.Tick += OnHideButtonTimerTick;
            hideButtonTimer.Start();
        }

        private void OnHideButtonTimerTick(object sender, EventArgs e)
        {
            movingButton.Visibility = movingButton.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnButtonStyleTimerTick(object sender, EventArgs e)
        {
            // Generate random position and size for the button
            Random random = new Random();

            // Get random width and height
            double newWidth = random.Next(25, 60); // Between 50 and 200
            double newHeight = random.Next(25, 60); // Between 30 and 150


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

        private void changeLights(Brush color)
        {
            if (color != null)
            {
                if (color == Brushes.Red)
                {
                    this.topLight.Fill = Brushes.Red;
                    this.midLight.Fill = Brushes.DarkOrange;
                    this.botLight.Fill = Brushes.DarkGreen;

                }
                else if (color == Brushes.Yellow)
                {
                    this.topLight.Fill = Brushes.DarkRed;
                    this.midLight.Fill = Brushes.Orange;
                    this.botLight.Fill = Brushes.DarkGreen;
                }
                else if (color == Brushes.Green)
                {
                    this.topLight.Fill = Brushes.DarkRed;
                    this.midLight.Fill = Brushes.DarkOrange;
                    this.botLight.Fill = Brushes.Green;
                }
                else
                {
                    return;
                }
            }
        }
    }
}