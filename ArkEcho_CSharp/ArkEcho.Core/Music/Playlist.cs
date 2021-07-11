using System;
using System.Collections.Generic;

namespace ArkEcho.Core
{
    public class Playlist
    {
        public string Name { get; private set; } = string.Empty;

        public Guid GUID { get; private set; } = new Guid();

        public SortedSet<Guid> MusicFiles { get; private set; } = new SortedSet<Guid>();

        public Playlist(string Name)
        {
            this.Name = Name;
        }
    }
}
