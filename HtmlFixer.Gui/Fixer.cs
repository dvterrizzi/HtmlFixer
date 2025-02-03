using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace HtmlFixer.Gui;

public static class Fixer
{
    /// <summary>
    /// Public function that can be called to fix HTML files starting from the specified master folder
    /// </summary>
    /// <param name="masterFolder">The folder to start looking in</param>
    /// <returns>Amount of files that were modified</returns>
    public static int Fix(string masterFolder)
    {
        var filesFixed = 0;

        foreach (var htmlFile in GetFilesIncludingSubfolders(masterFolder, "message*.html"))
        {
            // Read the input HTML file and fix all the img src's
            FixHtmlDocument(htmlFile);

            // Increment the total amount of fixed files with 1
            filesFixed++;
        }

        return filesFixed;
    }

    /// <summary>
    /// Function to fix a single HTML file
    /// </summary>
    /// <param name="inputFile">Full path to the single HTML file</param>
    private static void FixHtmlDocument(string inputFile)
    {
        // Store the folder where the HTML file is residing
        var htmlFileFolder = Path.GetDirectoryName(inputFile);

        // Every HTML file has its own photos folder next to it
        var photosFolder = Path.Combine(htmlFileFolder, "photos");

        // Create a list of all image files in the photos folder
        var photoFiles = Directory.GetFiles(photosFolder).Select(p => Path.GetFileName(p)).ToList();

        // Load the HTML file into an HtmlDocument object
        var doc = new HtmlDocument();
        doc.Load(inputFile);

        // Get all <img class="_2yuc"> tags (XPath format)
        const string MEDIA_XPATH = @"//img[contains(@class, '_2yuc')]";
        var imgs = doc.DocumentNode.SelectNodes(MEDIA_XPATH);

        foreach (var img in imgs)
        {
            // Get the src of the <img>
            var src = img.Attributes.Single(a => a.Name == "src");

            try
            {
                // Load the img src into a Uri object for easier manipulation
                var url = new Uri(src.Value);

                // Get the only the filename of the img src
                var filename = Path.GetFileNameWithoutExtension(url.Segments.Last());

                // Find a photo from the photos folder that starts with the filename
                var matchedPhoto = photoFiles.FirstOrDefault(p => p.StartsWith(filename));

                // Skip if no match was found
                if (matchedPhoto == null)
                    continue;

                // Update the img src to the photos folder + matched photo filename
                src.Value = Path.Combine(photosFolder, matchedPhoto);
            }
            catch (Exception)
            {
                // Silently skip this img in case anything goes wrong
            }
        }

        // Overwrite the contents of the HTML file with the modified one from memory
        doc.Save(inputFile);
    }

    /// <summary>
    /// Function to recursively find files using a search pattern
    /// </summary>
    /// <param name="path">Path to start looking</param>
    /// <param name="searchPattern">The search pattern to use</param>
    /// <returns>Flat list of all files matching the search pattern</returns>
    private static List<string> GetFilesIncludingSubfolders(string path, string searchPattern)
    {
        // List to hold all found paths
        List<string> paths = [];

        // Get every folder that's in the current folder
        var directories = Directory.GetDirectories(path);

        // Recursively call this function on every folder in this folder
        foreach (var directory in directories)
            paths.AddRange(GetFilesIncludingSubfolders(directory, searchPattern));
        

        // Add all files to the master list of paths that match the search pattern
        paths.AddRange(Directory.GetFiles(path, searchPattern).ToList());

        return paths;
    }
}