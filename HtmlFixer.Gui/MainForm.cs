namespace HtmlFixer.Gui
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// When the MainForm gets created by Program.cs, it will use
        /// the built-in InitializeComponent function to set up the UI.
        /// The contents of this function get re-generated every time you save 
        /// the form in the Form Designer.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function that fires when you click the browse button
        /// </summary>
        /// <param name="sender">The sender that fired the Click event (browser button)</param>
        /// <param name="e">Event arguments</param>
        private void browseButton_Click(object sender, EventArgs e)
        {
            // Create and open a folder dialog
            using var dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();

            // Show a message and abort if no valid folder was chosen
            if (dialogResult != DialogResult.OK || string.IsNullOrEmpty(dialog.SelectedPath))
            {
                MessageBox.Show("Choose a valid folder");
                return;
            }

            // Use the Fixer to fix the files
            var result = Fixer.Fix(dialog.SelectedPath);

            // Show a result message
            MessageBox.Show($"Fixed {result} HTML-files in {dialog.SelectedPath}");
        }
    }
}
