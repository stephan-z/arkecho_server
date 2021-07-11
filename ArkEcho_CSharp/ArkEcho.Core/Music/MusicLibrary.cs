using System.Collections.Generic;

namespace ArkEcho.Core
{
    // TODO: Alles Serializable
    public class MusicLibrary
    {
        public MusicLibrary() { }

        public List<AlbumArtist> AlbumArtists { get; private set; } = new List<AlbumArtist>();

        public List<Album> Album { get; private set; } = new List<Album>();

        public List<MusicFile> MusicFiles { get; private set; } = new List<MusicFile>();

        public List<Playlist> Playlists { get; private set; } = new List<Playlist>();
    }
}
