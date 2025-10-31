using System.Windows.Input;

namespace StaffManager.UI
{
    public static class AppCommands
    {
        public static readonly RoutedUICommand FocusName =
            new RoutedUICommand("Focus Name", nameof(FocusName), typeof(AppCommands), new InputGestureCollection
                {
                    new KeyGesture(Key.N, ModifierKeys.Alt) // Alt+N
                });

        public static readonly RoutedUICommand FocusId =
            new RoutedUICommand("Focus ID", nameof(FocusId), typeof(AppCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.I, ModifierKeys.Alt) // Alt+I
                });
        public static readonly RoutedUICommand PopulateFromSelection =
            new RoutedUICommand("Populate From Selection", nameof(PopulateFromSelection), typeof(AppCommands),
                new InputGestureCollection
                {
                    new KeyGesture(Key.Tab)  // Tab key
                });
    }
}