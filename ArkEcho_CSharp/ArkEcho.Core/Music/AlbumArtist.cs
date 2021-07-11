using System;
using System.Collections.Generic;

namespace ArkEcho.Core
{
    public class AlbumArtist
    {
        public AlbumArtist() { }

        public Guid GUID { get; } = Guid.NewGuid();

        public List<Guid> AlbumID { get; private set; } = new List<Guid>();

        public List<Guid> MusicFileIDs { get; private set; } = new List<Guid>();

        public string Name { get; set; } = string.Empty;
    }
}
