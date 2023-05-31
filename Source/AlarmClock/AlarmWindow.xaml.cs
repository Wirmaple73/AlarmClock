using System;
using System.Media;
using System.Windows;

namespace AlarmClock
{
    public partial class AlarmWindow : Window
    {
        private static SoundPlayer player;

        public static string AlarmTitle { get; set; }
        public static string SoundLocation { get; set; }
        public static bool IsDefaultLocation { get; set; }

        public AlarmWindow()
        {
            InitializeComponent();

            Label_CurrentTime.Content = DateTime.Now.ToString("HH:mm");
            TextBox_AlarmTitle.Text = AlarmTitle;

            InitializePlayer();
        }

        private void InitializePlayer()
        {
            // Use the provided alarm sound in resources if user hasn't chosen a custom one
            if (IsDefaultLocation)
                player = new SoundPlayer(Properties.Resources.sound_alarm);
            else
                player = new SoundPlayer(SoundLocation);

            using (player)
            {
                // Revert to the default alarm sound if the selected one is invalid
                try
                {
                    player.PlayLooping();
                }
                catch /* (Exception ex) */
                {
                    // MessageBox.Show(ex.Message);
                    IsDefaultLocation = true;

                    player.Stream = Properties.Resources.sound_alarm;
                    player.PlayLooping();
                }
            }
        }

        private void StopPlayer()
        {
            using (player)
            {
                player.Stop();
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            StopPlayer();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopPlayer();
        }
    }
}
