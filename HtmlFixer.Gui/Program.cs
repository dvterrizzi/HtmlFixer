namespace HtmlFixer.Gui
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize the application ans show a new MainForm
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}