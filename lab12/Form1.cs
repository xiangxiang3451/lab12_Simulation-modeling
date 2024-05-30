using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab12
{
    public partial class Form1 : Form
    {
        private double[,] transitionMatrix = new double[,]
        {
            { -0.4, 0.3, 0.1 },  // Transition rates from sunny to other states
            { 0.4, -0.8, 0.4 },  // Transition rates from rainy to other states
            { 0.1, 0.4, -0.5 }   // Transition rates from cloudy to other states
        };

        private int state = 0;
        private double t = 0;
        private double totalT = 0;
        private Random random = new Random();
        private double[] stationaryDistribution = new double[3];
        private bool isTransitioning = false;

        //  labels to display stationary distribution
        private Label[] distributionLabels;

        public Form1()
        {
            InitializeComponent();

            // Initialize labels for displaying stationary distribution
            distributionLabels = new Label[3];
            for (int i = 0; i < distributionLabels.Length; i++)
            {
                distributionLabels[i] = new Label
                {
                    Location = new Point(10, 10 + i * 30),
                    Size = new Size(200, 25)
                };
                Controls.Add(distributionLabels[i]);
            }

            // Set initial label text
            UpdateDistributionLabels();
        }

        private void ShowPicture()
        {
            switch (state)
            {
                case 0:
                    pictureBoxWeather.Image = Image.FromFile("1.png");
                    break;
                case 1:
                    pictureBoxWeather.Image = Image.FromFile("2.png");
                    break;
                case 2:
                    pictureBoxWeather.Image = Image.FromFile("3.png");
                    break;
                default:
                    pictureBoxWeather.Image = Image.FromFile("1.png");
                    break;
            }
        }

        private int GetNextState(double[,] Q, int currentState)
        {
            double sum = 0;
            double a = random.NextDouble();

            for (int i = 0; i < Q.GetLength(1); i++)
            {
                sum += Math.Abs(Q[currentState, i]);
                if (a < sum) return i;
            }

            return Q.GetLength(1) - 1; // Return the last state if no transition is made
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (isTransitioning)
            {
                timer1.Stop();
                isTransitioning = false;
            }
            else
            {
                timer1.Start();
                isTransitioning = true;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            t += timer1.Interval;
            int nextState = GetNextState(transitionMatrix, state);
            if (nextState != state)
            {
                stationaryDistribution[state] += t;
                totalT += t;
                t = 0;
                state = nextState;

                ShowPicture();
                UpdateDistributionLabels(); // Update the distribution labels
            }
        }

        private void UpdateDistributionLabels()
        {
            for (int i = 0; i < stationaryDistribution.Length; i++)
            {
                double distribution = totalT > 0 ? stationaryDistribution[i] / totalT : 0;
                distributionLabels[i].Text = $"State {i}: {distribution:F4}";
            }
        }
    }
}
