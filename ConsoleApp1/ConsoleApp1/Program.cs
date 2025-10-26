class Program
{
    static readonly HashSet<string> excludedFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "obj", ".vs", ".git"
    };

    static void Main(string[] args)
    {
        Console.Write("Enter folder path: ");
        string rootPath = "D:\\Work\\projects\\QuestHero\\BackEnd";

        if (Directory.Exists(rootPath))
        {
            TraverseDirectory(rootPath);
        }
        else
        {
            Console.WriteLine("Invalid path.");
        }
    }

    static void TraverseDirectory(string path)
    {
        try
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                string folderName = Path.GetFileName(dir);
                if (!excludedFolders.Contains(folderName))
                {
                    Console.WriteLine($"[Folder] {dir}");
                    TraverseDirectory(dir); // Recursive call
                }
            }

            foreach (var file in Directory.GetFiles(path))
            {
                Console.WriteLine($"  [File] {file}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing {path}: {ex.Message}");
        }
    }
}