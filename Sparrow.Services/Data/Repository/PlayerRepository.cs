using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Sparrow.Services.Models;

namespace Sparrow.Services.Data.Repository
{
    public class PlayerRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IEnumerable<PlaylistModel> GetPlaylist(int page)
        {
            var playlist = new List<PlaylistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var tracks =
                    context.SPRW_TRACK.Where(i=>i.ACT_IND == true).OrderByDescending(i => i.SPRW_TRACK_POPULAR_LIKE.Count).ThenBy(i => i.LAST_MAINT_TIME).Take(100);
                    foreach (var track in tracks)
                    {
                        if (playlist.Exists(i => i.ArtistName == track.SPRW_ARTIST.NAME))
                        {
                            var playlistItem = new PlaylistTrack
                            {
                                AlbumId = track.ALBUM_ID,
                                AlbumName = track.SPRW_ALBUM != null ? track.SPRW_ALBUM.NAME : null,
                                TrackId = track.TRACK_ID,
                                PopIndex =
                                    track.SPRW_TRACK_POPULAR_LIKE.Count(i => i.DISLIKE_DATE > DateTime.Now.AddMonths(-6)) 
                            };
                            var firstOrDefault = playlist.FirstOrDefault(i => i.ArtistName == track.SPRW_ARTIST.NAME);
                            if (firstOrDefault != null)
                                firstOrDefault.Tracks.Add(playlistItem);
                        }
                        else
                        {
                            playlist.Add(new PlaylistModel
                            {
                                ArtistId = track.ARTIST_ID,
                                ArtistName = track.SPRW_ARTIST.NAME,
                                Genres = track.SPRW_ARTIST.SPRW_GENRE.Select(i => i.GENRE).ToList(),
                                Tracks = new List<PlaylistTrack>
                                {
                                    new PlaylistTrack
                                    {
                                        AlbumId = track.ALBUM_ID,
                                        AlbumName = track.SPRW_ALBUM != null ? track.SPRW_ALBUM.NAME : null,
                                        TrackId = track.TRACK_ID,
                                        PopIndex =
                                            track.SPRW_TRACK_POPULAR_LIKE.Count(i => i.DISLIKE_DATE > DateTime.Now.AddMonths(-6)) 
                                            
                                    }}
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return playlist;
        }

        public PlaylistPageModel GetPlaylistMetaData()
        {
            var pageModel = new PlaylistPageModel();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var playlistId = context.sprw_playlist.Max(i => i.PLAYLIST_ID);
                    var pages = context.SPRW_PLAYLIST_PAGES.Where(i => i.PLAYLIST_ID == playlistId).Max(i => i.PAGE_NUM);
                    pageModel.PlaylistID = playlistId;
                    pageModel.TotalPages = pages;
                }

            }
            catch (Exception e)
            {
                
            }
            return pageModel;
        }

        public string GetPlaylist(int page, int playlistId)
        {
            var playlist = string.Empty;
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var model = context.SPRW_PLAYLIST_PAGES.FirstOrDefault(
                        i => i.PLAYLIST_ID == playlistId && i.PAGE_NUM == page);
                    if (model != null)
                        playlist =
                            model.PLAYLIST;
                }
            }
            catch (Exception e)
            {
                
            }
            return playlist;
        }

        public IEnumerable<ArtistModel> SearchArtists(string name)
        {
            var artists = new List<ArtistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artistList = context.SPRW_ARTIST.Where(i => i.NAME.Contains(name)).Take(5);
                    foreach (var artist in artistList)
                    {
                        var selectedArtist = new ArtistModel
                        {
                            ArtistName = artist.NAME,
                            ArtistId = artist.ARTIST_ID
                        };
                        artists.Add(selectedArtist);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return artists;
        }

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
                                Latitude = (double) artist.SPRW_MARKET_LOCATIONS.LAT_COORDS,
                                Longitude = (double) artist.SPRW_MARKET_LOCATIONS.LONG_COORDS,
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

        public IEnumerable<AlbumModel> SearchAlbums(string name)
        {
            var albums = new List<AlbumModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var albumList = context.SPRW_ALBUM.Where(i => i.NAME.StartsWith(name)).Take(3);
                    foreach (var album in albumList)
                    {
                        var selectedAlbum = new AlbumModel
                        {
                            //                            Artist = new ArtistModel
                            //                            {
                            //                                AristName = album.SPRW_ARTIST.NAME,
                            //                                ArtistId = album.ARTIST_ID,
                            //                                Description = album.SPRW_ARTIST.DESCRP
                            //                            },
                            AlbumName = album.NAME,
                            AlbumId = album.ARTIST_ID
                        };

                        albums.Add(selectedAlbum);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return albums;
        }

        public IEnumerable<TrackModel> SearchTracks(string name)
        {
            var tracks = new List<TrackModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var trackList = context.SPRW_TRACK.Where(i => i.NAME.StartsWith(name)).Take(3);
                    foreach (var track in trackList)
                    {
                        var selectedTrack = new TrackModel
                        {
                            //                            Album = new AlbumModel
                            //                            {
                            //                                Artist = new ArtistModel
                            //                                {
                            //                                    AristName = track.SPRW_ARTIST.NAME,
                            //                                    ArtistId = track.ARTIST_ID,
                            //                                    Description = track.SPRW_ARTIST.DESCRP
                            //                                },
                            //                                AlbumName = track.SPRW_ALBUM.NAME,
                            //                                AlbumId = track.SPRW_ALBUM.ALBUM_ID
                            //                            },
                            TrackId = track.TRACK_ID,
                            TrackName = track.NAME
                        };
                        selectedTrack.TrackName = track.NAME;
                        selectedTrack.TrackId = track.TRACK_ID;
                        tracks.Add(selectedTrack);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return tracks;
        }

        
    }
}
