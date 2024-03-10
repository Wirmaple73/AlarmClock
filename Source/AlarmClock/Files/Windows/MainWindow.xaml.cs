using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AlarmClock.Properties;
using AlarmClock.Managers;

namespace AlarmClock
{
	public partial class MainWindow
	{
		// Used to avoid displaying the "Up to date" message once the program starts
		private bool displayUpToDateMessage = false;

		public MainWindow()
		{
			InitializeComponent();

			ImportSettings();
			ReloadAlarmListSource();

			AlarmManager.AlarmSounded += AlarmChecker_AlarmSounded;
			AlarmManager.IsEnabled = true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Sort the ListView by Time (ascending)
			var view = (CollectionView)CollectionViewSource.GetDefaultView(AlarmList.ItemsSource);

			view.SortDescriptions.Add(new(ResourceManager.GetResource(Resource.MW_ListViewTime), ListSortDirection.Ascending));
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = !MessageBoxManager.DisplayConfirmation(ResourceManager.GetResource(Resource.MW_ExitConfirmation), MessageBoxResult.No);

			AlarmManager.ExportAlarms();
			ExportSettings();
		}

		private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

		private void AddAlarm_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var window = new AlarmSettingsWindow();

			if (window.ShowDialog().Value)
				AlarmManager.Add(window.Alarm);
		}

		private void SwapAlarmStatus_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (AlarmList.SelectedItem is not null)
			{
				AlarmManager.SwapStatus((Alarm)AlarmList.SelectedItem);
				ReloadAlarmListSource();
			}
			else
			{
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_AlarmNotSelected));
			}
		}

		private void ModifyAlarm_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (AlarmList.SelectedItem is not null)
			{
				var window = new AlarmSettingsWindow()
				{
					Alarm		= (Alarm)AlarmList.SelectedItem,
					ModifyAlarm = true
				};

				if (window.ShowDialog().Value)
					AlarmManager.Replace((Alarm)AlarmList.SelectedItem, window.Alarm);
			}
			else
			{
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_AlarmNotSelected));
			}
		}

		private void DeleteAlarm_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (AlarmList.SelectedItem is not null)
				AlarmManager.Remove((Alarm)AlarmList.SelectedItem);
			else
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_AlarmNotSelected));
		}

		private void DisplayAboutWindow_Executed(object sender, ExecutedRoutedEventArgs e)
			=> new AboutWindow().ShowDialog();

		private void SelectLanguage_Click(object sender, RoutedEventArgs e)
		{
			SetProgramLanguage(
				Enum.TryParse((string)((MenuItem)sender).Tag, out Language result) ? result :
				throw new InvalidOperationException("The selected language could not be resolved.")
			);

			ReloadAlarmListSource();  // Update the alarm list language
		}

		private void StartWithWindows_Click(object sender, RoutedEventArgs e)
			=> SetStartWithWindowsEnabled(StartWithWindows.IsChecked);

		private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
		{
			switch (await VersionManager.GetVersionLookupStateAsync())
			{
				case VersionLookupState.UpToDate when displayUpToDateMessage:
					MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.MW_UpToDate));
					break;

				case VersionLookupState.UpdateAvailable when MessageBoxManager.DisplayConfirmation(ResourceManager.GetResource(Resource.MW_UpdateAvailable)):
					URLOpener.Open(VersionManager.RepositoryURL);
					break;

				case VersionLookupState.ConnectionError:
					MessageBoxManager.DisplayError(ResourceManager.GetResource(Resource.MW_ConnectionError));
					break;
			}

			displayUpToDateMessage = true;
		}

		private void AlarmChecker_AlarmSounded(object sender, AlarmSoundedEventArgs e)
		{
			ReloadAlarmListSource();
			new AlarmWindow() { Alarm = e.Alarm }.ShowDialog();
		}

		private void AlarmToolBar_Loaded(object sender, RoutedEventArgs e)
		{
			// Hide the toolbar overflow grid
			// Source: https://stackoverflow.com/a/1051264/18954775

			var toolBar = (ToolBar)sender;

			if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
				overflowGrid.Visibility = Visibility.Collapsed;

			if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
				mainPanelBorder.Margin = new();
		}

		private void ReloadAlarmListSource()
		{
			AlarmList.ItemsSource = null;
			AlarmList.ItemsSource = AlarmManager.Alarms;
		}

		private void SetProgramLanguage(Language language)
		{
			if (!Enum.IsDefined(language))
				throw new ArgumentOutOfRangeException(nameof(language), "The selected language could not be resolved.");

			LanguageManager.Language = language;
			SetMenuItemsChecked(language == AlarmClock.Language.English);

			void SetMenuItemsChecked(bool isEnglishItemChecked)
			{
				EnglishItem.IsChecked = isEnglishItemChecked;
				PersianItem.IsChecked = !isEnglishItemChecked;
			}
		}

		private void SetStartWithWindowsEnabled(bool isEnabled)
		{
			try
			{
				StartupManager.SetStartWithWindowsEnabled(isEnabled);
			}
			catch /* (RegistrySubkeyNotFoundException ex) */
			{
				StartWithWindows.IsChecked = !StartWithWindows.IsChecked;
				MessageBoxManager.DisplayError(ResourceManager.GetResource(Resource.MW_RegistryError));
			}
		}

		private void ImportSettings()
		{
			this.WindowState = Settings.Default.WindowState;

			this.Width	= Settings.Default.WindowWidth;
			this.Height = Settings.Default.WindowHeight;
			this.Left	= Settings.Default.WindowLeft;
			this.Top	= Settings.Default.WindowTop;

			AutoCheckForUpdates.IsChecked = Settings.Default.AutoCheckForUpdates;
			StartWithWindows.IsChecked	  = Settings.Default.StartWithWindows;

			if (Settings.Default.AutoCheckForUpdates)
				CheckForUpdates_Click(this, null);

			SetStartWithWindowsEnabled(Settings.Default.StartWithWindows);
			SetProgramLanguage(Settings.Default.Language);
		}

		private void ExportSettings()
		{
			Settings.Default.WindowState = this.WindowState;

			Settings.Default.WindowWidth  = this.Width;
			Settings.Default.WindowHeight = this.Height;
			Settings.Default.WindowLeft   = this.Left;
			Settings.Default.WindowTop	  = this.Top;

			Settings.Default.Language = LanguageManager.Language;

			Settings.Default.AutoCheckForUpdates = AutoCheckForUpdates.IsChecked;
			Settings.Default.StartWithWindows	 = StartWithWindows.IsChecked;

			Settings.Default.Save();
		}
    }
}
