using System.Collections.Generic;
using System.Linq;
using Sparrow.Services.Data.Repository;
using Sparrow.Services.Models;

namespace Sparrow.Services.API
{
    public static class Playlist
    {
        private static readonly PlayerRepository Repository = new PlayerRepository();

        public static PlaylistPageModel GetPlaylistMetaData()
        {
            return Repository.GetPlaylistMetaData();
        }
        public static PlaylistCacheModel GetPlaylistPages()
        {
            var playlist = (List<PlaylistTrack>)Repository.GetPlaylistByHackerNews();
            var model = new PlaylistCacheModel
            {
                Total = playlist.Count(),
                Pages = new List<PlaylistCachePageModel>()
            };
            var pageModel = new PlaylistCachePageModel
            {
                Page = 1,
                Tracks = new List<PlaylistTrack>()
            };

            for (int i = 0, j = 0; i < playlist.Count(); i++, j++)
            {
                if (j == 100)
                {
                    j = 0;
                    model.Pages.Add((PlaylistCachePageModel)pageModel.Clone());
                    pageModel.Page++;
                    pageModel.Tracks.Clear();
                }
                pageModel.Tracks.Add(playlist[i]);
            }

            return model;
        }
       
        public static string GetPlaylist(int page, int playlistId)
        {
            return Repository.GetPlaylist(page, playlistId);
        }

        public static SearchModel SearchByName(string name)
        {
            var model = new SearchModel();

            model.Artists = Repository.SearchArtists(name);
            //model.Albums = Repository.SearchAlbums(name);
            //model.Tracks = Repository.SearchTracks(name);

            return model;
        }

    }
}
