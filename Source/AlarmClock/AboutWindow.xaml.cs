using System.Windows;
using System.Windows.Input;

namespace AlarmClock
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void TextBlock_GithubLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(TextBlock_GithubLink.Text);
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
