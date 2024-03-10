using System.Windows;

namespace AlarmClock.Managers
{
	public static class MessageBoxManager
    {
        public static void DisplayInformation(string message)
            => InternalDisplayMessageBox(message, ResourceManager.GetResource(Resource.Global_ProgramTitle), MessageBoxImage.Asterisk);

        public static void DisplayError(string message)
            => InternalDisplayMessageBox(message, ResourceManager.GetResource(Resource.Global_ProgramTitle), MessageBoxImage.Error);

        public static bool DisplayConfirmation(string message, MessageBoxResult defaultResult = MessageBoxResult.Yes)
            => InternalDisplayMessageBox(message, ResourceManager.GetResource(Resource.Global_ProgramTitle), MessageBoxImage.Asterisk, MessageBoxButton.YesNo, defaultResult) == MessageBoxResult.Yes;

        private static MessageBoxResult InternalDisplayMessageBox(string message, string title, MessageBoxImage image, MessageBoxButton button = MessageBoxButton.OK, MessageBoxResult defaultResult = MessageBoxResult.OK)
            => MessageBox.Show(message, title, button, image, defaultResult, LanguageManager.FlowDirection == FlowDirection.LeftToRight ? MessageBoxOptions.None : MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
    }
}
