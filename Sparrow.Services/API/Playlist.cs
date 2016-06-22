using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sparrow.Services.Data.Repository;
using Sparrow.Services.Models;

namespace Sparrow.Services.API
{
    public class Playlist
    {
        private readonly PlayerRepository Repository = new PlayerRepository();
        private readonly PlaylistRepository PlaylistRepo = new PlaylistRepository();

        public Playlist()
        {
            
        }

        public Playlist(PlayerRepository playerRepo)
        {
            
        }

        public Playlist(PlaylistRepository playlistRepo)
        {
            
        }

        public Playlist(PlayerRepository playerRepo, PlaylistRepository playlistRepo)
        {
            
        }

        public PlaylistPageModel GetPlaylistMetaData()
        {
            return Repository.GetPlaylistMetaData();
        }
        public PlaylistCacheModel GetPlaylistPages()
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
       
        public string GetPlaylist(int page, int playlistId)
        {
            return Repository.GetPlaylist(page, playlistId);
        }

        public SearchModel SearchByName(string name)
        {
            var model = new SearchModel();

            model.Artists = Repository.SearchArtists(name);
            //model.Albums = Repository.SearchAlbums(name);
            //model.Tracks = Repository.SearchTracks(name);

            return model;
        }

        public void CreatePlaylist()
        {
            int page = 1;
            int playlistId = PlaylistRepo.CreatePlaylist();
            var playlist = PlaylistRepo.GetPlaylistByHackerNews().ToList();
            var playlistList = new List<PlaylistTrack>();

            for (var i = 1; i <= playlist.Count(); i++)
            {

                if (i % 10 == 0 || i == playlist.Count())
                {
                    var playlistString = JsonConvert.SerializeObject(playlistList);
                    PlaylistRepo.CreatePlaylistPage(playlistId, page, playlistString);
                    playlistList.Clear();
                    page++;
                }
                else
                {
                    playlistList.Add(playlist[i - 1]);
                }
            }
        }

    }
}
