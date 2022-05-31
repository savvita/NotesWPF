using System.IO;

namespace NotesWPF
{
    public static class Settings
    {
        public static int MaxCellphonesCount { get; set; }
        private static string filePath = @"d:/source";
        public static string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }
        }
    }
}
