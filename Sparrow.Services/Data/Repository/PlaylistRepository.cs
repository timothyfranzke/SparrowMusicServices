using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sparrow.Services.Models;

namespace Sparrow.Services.Data.Repository
{
    public class PlaylistRepository
    {
        public IEnumerable<PlaylistTrack> GetPlaylistByHackerNews()
        {
            var today = DateTime.Now;
            var playlistModel = new List<PlaylistTrack>();
            using (var context = new sparrow_dbEntities())
            {
                var artists = context.SPRW_ARTIST.OrderByDescending(i => i.POP_INDEX);
                foreach (var artist in artists)
                {
                    var playlistItem = new PlaylistTrack
                    {
                        ArtistId = artist.ARTIST_ID,
                        ArtistName = artist.NAME
                    };
                    var sprwArtistSettings = artist.SPRW_ARTIST_SETTINGS.FirstOrDefault();
                    if (sprwArtistSettings != null)
                        playlistItem.Setting = sprwArtistSettings.SETTING;

                    playlistItem.Settings = artist.SPRW_ARTIST_SETTING.ToDictionary(i => i.KEY, i => i.VALUE);
                    var maxPopIndex = artist.SPRW_TRACK.Max(j => j.POP_INDEX);
                    var popTrack =
                        artist.SPRW_TRACK.FirstOrDefault(i => i.POP_INDEX == artist.SPRW_TRACK.Max(j => j.POP_INDEX));

                    if (popTrack != null)
                    {
                        playlistItem.AlbumId = popTrack.ALBUM_ID;
                        playlistItem.AlbumName = popTrack.SPRW_ALBUM.NAME;
                        playlistItem.Genres = artist.SPRW_GENRE.Select(i => i.GENRE_ID).ToList();
                        playlistItem.PopIndex = (double)artist.POP_INDEX;
                        playlistItem.TrackId = popTrack.TRACK_ID;
                        playlistItem.TrackName = popTrack.NAME;
                        if (artist.SPRW_MARKET_LOCATIONS != null)
                        {
                            playlistItem.Market = new MarketModel
                            {
                                Latitude = (double)artist.SPRW_MARKET_LOCATIONS.LAT_COORDS,
                                Longitude = (double)artist.SPRW_MARKET_LOCATIONS.LONG_COORDS,
                                Zip = artist.SPRW_MARKET_LOCATIONS.ZIP
                            };
                        }
                        playlistItem.ReleaseDate = popTrack.RELEASE_DATE;
                        playlistModel.Add(playlistItem);
                    }
                }
            }
            return playlistModel;
        }

        public int CreatePlaylist()
        {
            var id = -1;
            using (var context = new sparrow_dbEntities())
            {
                var playlist = new sprw_playlist { IS_ACTIVE = true, CREATED = DateTime.Now };
                context.sprw_playlist.Add(playlist);
                context.SaveChanges();
                id = (int)playlist.PLAYLIST_ID;
            }
            return id;
        }

        public void CreatePlaylistPage(int playlistId, int pageNum, string playlistItem)
        {
            using (var context = new sparrow_dbEntities())
            {
                var page = new SPRW_PLAYLIST_PAGES
                {
                    PLAYLIST_ID = playlistId,
                    PAGE_NUM = pageNum,
                    PLAYLIST = playlistItem
                };
                context.SPRW_PLAYLIST_PAGES.Add(page);
                context.SaveChanges();
            }
        }
    }
}