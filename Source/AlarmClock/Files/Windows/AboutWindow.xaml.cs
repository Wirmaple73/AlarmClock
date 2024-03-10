using System.Windows;
using System.Windows.Navigation;
using AlarmClock.Managers;

namespace AlarmClock
{
	public partial class AboutWindow : Window
	{
		private readonly string RepositoryURL;

		public AboutWindow()
		{
			InitializeComponent();

			// Apparently WPF doesn't support binding to 'static readonly' fields
			BuildInfo.Text = $"{VersionManager.CurrentVersion} ({VersionManager.BuildDate:yyyy/MM/dd})";
			RepositoryURL = GithubRepositoryURL.NavigateUri.ToString();
		}

		private void GithubProfile_RequestNavigate(object sender, RequestNavigateEventArgs e)
			=> URLOpener.Open(RepositoryURL);
	}
}
