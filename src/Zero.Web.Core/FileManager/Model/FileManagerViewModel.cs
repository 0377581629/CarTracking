using System;

namespace Zero.Web.FileManager.Model
{
    public class FileManagerViewModel
    {
        public string Name { get; set; }

        public long Size { get; set; }

        public string Path { get; set; }
        
        public string ActualPath { get; set; }

        public string Extension { get; set; }

        public bool IsDirectory { get; set; }

        public bool HasDirectories { get; set; }

        public DateTime Modified { get; set; }

        public DateTime ModifiedUtc { get; set; }
    }
}