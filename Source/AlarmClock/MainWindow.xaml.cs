using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Threading;
// TODO: SORT USINGSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
// TOOLTIPS

namespace AlarmClock
{
    public partial class MainWindow : Window
    {
        private static readonly string[] days = { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
        private static readonly ScheduleChecker checker = new ScheduleChecker();

        public MainWindow()
        {
            InitializeComponent();

            // Load the schedule file contents into the ListView
            UpdateScheduleList();

            // Set the alarm window' sound location to default
            AlarmWindow.IsDefaultLocation = true;

            // Start checking for defined alarms
            checker.AlarmSounded += ScheduleChecker_AlarmSounded;
            checker.Start();
        }

        private void Button_Alarm_New_Click(object sender, RoutedEventArgs e)
        {
            new AlarmSettingsWindow().ShowDialog();
            UpdateScheduleList();
        }

        private void Button_Alarm_Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedItem();
        }

        private void Button_Alarm_ToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (ListView_ScheduleList.SelectedItem == null)
                return;

            try
            {
                var selItem = GetSelectedItem();
                string oldValue = GetItemData(selItem);

                selItem.Status = selItem.Status == "فعال" ? "غیرفعال" : "فعال";

                ScheduleFileManager.ReplaceFileData(oldValue, GetItemData(selItem));
                ListView_ScheduleList.Items.Refresh();
            }
            catch (Exception ex)
            {
                ScheduleFileManager.DisplayErrorMessage(ex.Message);
            }
        }

        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Menu_Help_About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void Button_SetDefaultSoundLocation_Click(object sender, RoutedEventArgs e)
        {
            SetMenuCheckboxes(true, false);
            AlarmWindow.IsDefaultLocation = true;
        }

        private void Button_ChangeSoundLocation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "(WAV) انتخاب فایل صوتی",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Filter = "WAV فایل‌ (*.wav)|*.wav",
                DefaultExt = ".wav"
            };

            if (dialog.ShowDialog() == true)
            {
                SetMenuCheckboxes(false, true);
                AlarmWindow.IsDefaultLocation = false;
                AlarmWindow.SoundLocation = dialog.FileName;
            }
        }

        private void ListView_ScheduleList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedItem();
        }

        private void ScheduleChecker_AlarmSounded(object sender, EventArgs e)
        {
            // Update the menu item checkboxes if an error occured in playing the custom sound file last time
            if (AlarmWindow.IsDefaultLocation)
                SetMenuCheckboxes(true, false);

            UpdateScheduleList();
            new AlarmWindow().ShowDialog();
        }

        private void SetMenuCheckboxes(bool defaultSound, bool changeSound)
        {
            MenuItem_SetDefaultSoundLocation.IsChecked = defaultSound;
            MenuItem_ChangeSoundLocation.IsChecked = changeSound;
        }

        private void DeleteSelectedItem()
        {
            if (ListView_ScheduleList.SelectedItem == null)
                return;

            try
            {
                var selItem = GetSelectedItem();

                ScheduleFileManager.ReplaceFileData(GetItemData(selItem), "");
                ListView_ScheduleList.Items.RemoveAt(ListView_ScheduleList.SelectedIndex);
            }
            catch (Exception ex)
            {
                ScheduleFileManager.DisplayErrorMessage(ex.Message);
            }
        }

        private string GetItemData(AlarmSchedule selItem)
        {
            return string.Format
            (
                "{0},{1},{2},{3}",
                selItem.Status == "فعال" ? "1" : "0",
                Array.IndexOf(days, selItem.Day),
                selItem.Time,
                selItem.Title
            );
        }

        private AlarmSchedule GetSelectedItem()
        {
            return (ListView_ScheduleList.SelectedItem as List<AlarmSchedule>)[0];
        }

        private void UpdateScheduleList()
        {
            string[] lineArray;

            try
            {
                lineArray = ScheduleFileManager.Read();
            }
            catch { return; }


            ListView_ScheduleList.Items.Clear();

            foreach (string line in lineArray)
            {
                // Skip the current line if it's empty or a comment
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                    continue;

                var lineSplit = line.Split(',');

                try
                {
                    // Format:
                    // Status,Day,Time,Title
                    ListView_ScheduleList.Items.Add
                    (
                        new List<AlarmSchedule>()
                        {
                            new AlarmSchedule()
                            {
                                Status = lineSplit[0] == "1" ? "فعال" : "غیرفعال",
                                Day    = days[int.Parse(lineSplit[1])],
                                Time   = TimeSpan.Parse(lineSplit[2]),
                                Title  = lineSplit[3]
                            }
                        }
                    );
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    class AlarmSchedule
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
    }

    static class ScheduleFileManager
    {
        private static readonly string scheduleFileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AlarmClock"),
                                            scheduleFilePath = Path.Combine(ScheduleFileDirectory, "schedule.acs");

        private static void EnsurePathExists()
        {
            try
            {
                if (!Directory.Exists(ScheduleFileDirectory))
                    Directory.CreateDirectory(ScheduleFileDirectory);

                if (!File.Exists(scheduleFilePath))
                    File.Create(scheduleFilePath);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex.Message);
            }
        }

        public static string ScheduleFileDirectory
        {
            get { return scheduleFileDirectory; }
        }

        public static string ScheduleFilePath
        {
            get { return scheduleFilePath; }
        }

        public static string[] Read()
        {
            EnsurePathExists();
            return File.ReadAllLines(ScheduleFilePath);
        }

        public static string ReadText()
        {
            EnsurePathExists();
            return File.ReadAllText(ScheduleFilePath);
        }

        public static void Write(string contents, bool overwrite)
        {
            try
            {
                if (!Directory.Exists(ScheduleFileDirectory))
                    Directory.CreateDirectory(ScheduleFileDirectory);

                if (overwrite)
                    File.WriteAllText(ScheduleFilePath, contents + Environment.NewLine);
                else
                    File.AppendAllText(ScheduleFilePath, contents + Environment.NewLine);
            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex.Message);
            }
        }

        public static void ReplaceFileData(string oldValue, string newValue)
        {
            string s = ScheduleFileManager.ReadText().Replace(oldValue, newValue).Trim();
            ScheduleFileManager.Write(s, true);
        }

        public static void DisplayErrorMessage(string message)
        {
            string errorMessage = string.Format(
                @".خطایی در ذخیره فایل زمان‌بندی‌ها رخ داد

                :جزئیات خطا
                {0}


                .نباشد و دوباره تلاش کنید Read-only مطمئن شوید پوشه‌ و فایل بالا"
                , message);

            MessageBox.Show(errorMessage, "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
        }
    }

    class ScheduleChecker
    {
        private readonly DispatcherTimer timer;

        public delegate void AlarmHandler(object sender, EventArgs e);
        public event AlarmHandler AlarmSounded;

        public ScheduleChecker()
        {
            timer = new DispatcherTimer();

            timer.Interval = new TimeSpan(0, 0, 0, 1);  // Delay: 1s
            timer.Tick += timer_Tick;
        }

        public void Start()
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            string[] lineArray;

            try
            {
                lineArray = ScheduleFileManager.Read();
            }
            catch { return; }


            foreach (string line in lineArray)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                    continue;

                try
                {
                    var lineSplit = line.Split(',');

                    bool isAlarmEnabled = lineSplit[0] == "1";
                    int day = int.Parse(lineSplit[1]);
                    var ts = TimeSpan.Parse(lineSplit[2]);

                    if (isAlarmEnabled)
                    {
                        // Check if the day of the current alarm is today
                        if (day == (int)DateTime.Now.DayOfWeek)
                        {
                            if (ts.Hours == DateTime.Now.Hour && ts.Minutes == DateTime.Now.Minute)
                            {
                                DisableAlarmStatus(line);

                                // Pass the current alarm title/description to the alarm window
                                AlarmWindow.AlarmTitle = lineSplit[3];

                                // Fire the event handler to update the ListView and open the alarm window
                                if (AlarmSounded != null)
                                    AlarmSounded(this, EventArgs.Empty);
                            }
                        }
                    }
                }
                catch { continue; }
            }
        }

        private void DisableAlarmStatus(string line)
        {
            // Replace the first character in the line with '0'
            var chars = line.ToCharArray();
            chars[0] = '0';

            ScheduleFileManager.ReplaceFileData(line, new string(chars));
        }
    }
}
