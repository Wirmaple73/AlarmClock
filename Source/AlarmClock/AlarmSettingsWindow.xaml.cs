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
using System.Windows.Shapes;
using System.IO;
// SORT USINGSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
// TOOLTIPS

namespace AlarmClock
{
    public partial class AlarmSettingsWindow : Window
    {
        private const string noTitleCaption = "[بدون عنوان]";

        public AlarmSettingsWindow()
        {
            InitializeComponent();

            FillAndSetInitialValues();
        }

        private void FillAndSetInitialValues()
        {
            // Fill the comboboxes (hours & minutes)
            for (byte i = 0; i < 24; i++)
                AddToComboBox(ComboBox_Hour, i);

            for (byte i = 0; i < 60; i++)
                AddToComboBox(ComboBox_Minute, i);

            // Set the initial combobox values
            ComboBox_Hour.SelectedIndex   = DateTime.Now.Hour;
            ComboBox_Minute.SelectedIndex = DateTime.Now.Minute;
            ComboBox_Day.SelectedIndex    = (int)DateTime.Now.DayOfWeek;
        }

        private void AddToComboBox(ComboBox c, byte i)
        {
            // Used for adding an additional zero to the beginning of the item value
            c.Items.Add((i < 10) ? ("0" + i) : (i.ToString()));
        }

        private void Button_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var time = TimeSpan.Parse(ComboBox_Hour.SelectedValue + ":" + ComboBox_Minute.SelectedValue);

                // Format:
                // Status,Day,Time,Title
                string contents = string.Format
                (
                    "{0},{1},{2},{3}",
                    RadioButton_Enabled.IsChecked == true ? "1" : "0",
                    ComboBox_Day.SelectedIndex,
                    time,
                    string.IsNullOrWhiteSpace(TextBox_Title.Text) ? noTitleCaption : TextBox_Title.Text
                );

                // Check for duplicates
                string dayTime = string.Format("{0},{1}", ComboBox_Day.SelectedIndex, time);

                if (ScheduleFileManager.ReadText().Contains(dayTime))
                {
                    MessageBox.Show(".هشدار موردنظر از قبل وجود دارد", "زنگ هشدار", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                    return;
                }
                else { ScheduleFileManager.Write(contents, false); }
            }
            catch (Exception ex)
            {
                ScheduleFileManager.DisplayErrorMessage(ex.Message);
            }

            this.Close();
        }

        private void Button_DiscardSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
