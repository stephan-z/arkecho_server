using System;
using System.IO;

namespace ArkEcho.Core
{
    public class MusicFile
    {
        public Guid GUID { get; } = Guid.NewGuid();

        public Guid Album { get; set; } = Guid.Empty;

        public Guid AlbumArtist { get; set; } = Guid.Empty;

        public string Title { get; set; } = string.Empty;

        public string Performer { get; set; } = string.Empty;

        public uint Disc { get; set; } = 0;

        public uint Track { get; set; } = 0;

        public uint Year { get; set; } = 0;

        public string Folder { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FileFormat { get; set; }

        public MusicFile(string FilePath)
        {
            FileInfo info = new FileInfo(FilePath);

            this.Folder = info.DirectoryName;
            this.FileName = info.Name;

            string extensionCleared = info.Extension.Substring(1);
            if (Resources.SupportedFileFormats.Contains(extensionCleared))
                FileFormat = extensionCleared;
            else
                FileFormat = "ERROR";
        }

        public string GetFullFilePath()
        {
            return $"{Folder}\\{FileName}";
        }
    }
}
