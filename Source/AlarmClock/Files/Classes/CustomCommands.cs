using System.Windows.Input;

namespace AlarmClock
{
	public static class CustomCommands
	{
		// Source:
		// https://wpf-tutorial.com/commands/implementing-custom-commands/

		public static readonly RoutedUICommand SwapAlarmStatus = new(
			"Swap alarm status",
			"Swap alarm status",
			typeof(CustomCommands),
			new InputGestureCollection() { new KeyGesture(Key.F5, ModifierKeys.None) }
		);
	}
}
