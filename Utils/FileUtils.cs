namespace ImgToPdfConverter.Utils;

public static class FileUtils
{
    public static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void EnsureDirectoriesExist(string[] pathes)
    {
        foreach (var path in pathes)
        {
            EnsureDirectoryExists(path);
        }
    }
}
