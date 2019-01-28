using System.IO;

namespace AntiDuplWPF.Helper
{
    public class IOHelper
    {
        public static bool IsDirectory(string filename)
        {
            if (!Directory.Exists(filename))
            {
                return false;
            }
            FileAttributes attr = File.GetAttributes(filename);
            return (attr & FileAttributes.Directory) != 0;
        }
    }
}
