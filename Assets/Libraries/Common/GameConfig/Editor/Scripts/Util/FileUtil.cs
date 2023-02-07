using System.IO;

namespace Game.Config.Model
{

    public class DirectoryUtil
    {
        public static void CleanDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }
    }
}