using System.Windows.Input;
using OctopusNotify.App.Commands;

namespace OctopusNotify.App
{
    public static class AppCommands
    {
        public static RoutedUICommand Test { get; } = new RoutedUICommand("Test", "Test", typeof(AppCommands));

        public static ExitCommand Exit { get; } = new ExitCommand();

        public static LaunchOctopusCommand LaunchOctopus { get; } = new LaunchOctopusCommand();

        public static ShowSettingsCommand ShowSettings { get; } = new ShowSettingsCommand();

        public static ShowMainWindowCommand ShowMainWindow { get; } = new ShowMainWindowCommand();

        public static ShowStubWindowCommand ShowStub { get; } = new ShowStubWindowCommand();

        public static ShowAboutWindowCommand ShowAbout { get; } = new ShowAboutWindowCommand();
    }
}
