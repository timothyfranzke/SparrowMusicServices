using System;
using System.Collections.Generic;
using System.Linq;
using Sparrow.Services.Models;

namespace Sparrow.Services.Data.Repository
{
    public class ArtistRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ArtistAssociation
        public void CreateArtistAssociation(string email, int artistId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var userId = GetUserId(email);
                    var artistMember = new SPRW_ARTIST_MEMBER
                    {
                        ROLE_ID = 1,
                        ACT_IND = true,
                        ARTIST_ID = artistId,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = userId,
                        USER_ID = userId
                    };
                    context.SPRW_ARTIST_MEMBER.Add(artistMember);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateArtistAssociation(int artistId, int userId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artistMember = new SPRW_ARTIST_MEMBER
                    {
                        ROLE_ID = 1,
                        ACT_IND = true,
                        ARTIST_ID = artistId,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = userId,
                        USER_ID = userId
                    };
                    context.SPRW_ARTIST_MEMBER.Add(artistMember);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveArtistAssociation(int artistId, string email)
        {
            try
            {
                var userId = GetUserId(email);
                using (var context = new sparrow_dbEntities())
                {
                    var artistMember =
                        context.SPRW_ARTIST_MEMBER.FirstOrDefault(i => i.ARTIST_ID == artistId && i.USER_ID == userId);

                    if (artistMember != null)
                    {
                        artistMember.ACT_IND = false;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveArtistAssociation(int artistId, int userId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artistMember =
                        context.SPRW_ARTIST_MEMBER.FirstOrDefault(i => i.ARTIST_ID == artistId && i.USER_ID == userId);

                    if (artistMember != null)
                    {
                        artistMember.ACT_IND = false;
                    }
                    
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Event
        public int CreateEvent(CreateEventModel model)
        {
            var userId = GetUserId(model.UserEmail);
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var eventModel = new SPRW_ARTIST_EVENT()
                    {
                        ACT_IND = true,
                        ADDRESS = model.Address,
                        ARTIST_ID = model.ArtistId,
                        CITY = model.City,
                        DESCRP = model.Description,
                        EVENT_DATE = model.EventDate,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = userId,
                        NAME = model.Name,
                        STATE = model.State,
                        URL = model.Url
                    };
                    context.SPRW_ARTIST_EVENT.Add(eventModel);
                    context.SaveChanges();
                    return eventModel.EVENT_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateEvent(ModifyEventModel model)
        {
            var userId = GetUserId(model.UserEmail);
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var eventModel = context.SPRW_ARTIST_EVENT.FirstOrDefault(i => i.EVENT_ID == model.EventId);
                    if (eventModel != null)
                    {
                        eventModel.ADDRESS = model.Address;
                        eventModel.ARTIST_ID = model.ArtistId;
                        eventModel.CITY = model.City;
                        eventModel.DESCRP = model.Description;
                        eventModel.EVENT_DATE = model.EventDate;
                        eventModel.LAST_MAINT_TIME = DateTime.Now;
                        eventModel.LAST_MAINT_USER_ID = userId;
                        eventModel.NAME = model.Name;
                        eventModel.STATE = model.State;
                        eventModel.URL = model.Url;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveEvent(int eventId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var eventModel = context.SPRW_ARTIST_EVENT.FirstOrDefault(i => i.EVENT_ID == eventId);
                    if (eventModel != null)
                    {
                        eventModel.ACT_IND = false;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public int CreateBulliten(BullitenModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var bulliten = new ARTIST_BLOG
                    {
                        ACT_IND = true,
                        BLOG = model.Bulliten,
                        SPRW_ARTIST = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == model.ArtistId),
                        LAST_MAINT_TIME = DateTime.Now
                    };
                    context.ARTIST_BLOG.Add(bulliten);
                    context.SaveChanges();

                    return bulliten.BLOG_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public IEnumerable<MarketSearchModel> SearchMarkbetByName(string name)
        {
            var marketList = new List<MarketSearchModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var markets = context.SPRW_MARKET_LOCATIONS.Where(i => i.CITY.Contains(name));
                    foreach (var market in markets)
                    {
                        var marketModel = new MarketSearchModel
                        {
                            Longitude = (double) market.LONG_COORDS,
                            Latitude = (double) market.LAT_COORDS,
                            Name = market.CITY,
                            Zip = market.ZIP
                        };
                        marketList.Add(marketModel);
                    }
                }
            }
            catch (Exception e)
            {
                
            }
            return marketList;
        }

        #region Artist
        public int CreateArtist(CreateArtistModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var userId = GetUserId(model.UserEmail);
                    var artist = new SPRW_ARTIST()
                    {
                        ACT_IND = true,
                        NAME = model.Name,
                        DESCRP = model.Description,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = model.UserEmail
                    };
                    var sprwMarketLocations = context.SPRW_MARKET_LOCATIONS.FirstOrDefault(i => i.ZIP == model.Zip);
                    if (sprwMarketLocations != null)
                    {
                        artist.SPRW_MARKET_LOCATIONS = sprwMarketLocations;
                    }

                    context.SPRW_ARTIST.Add(artist);
                    context.SaveChanges();

                    return artist.ARTIST_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateArtist(ModifyArtistModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var userId = GetUserId(model.UserEmail);
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == model.ArtistId);
                    if (artist != null)
                    {
                        artist.ACT_IND = model.Active;
                        artist.NAME = model.Name;
                        artist.DESCRP = model.Description;
                        artist.LAST_MAINT_TIME = DateTime.Now;
                        artist.LAST_MAINT_USER_ID = model.UserEmail;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveArtist(int artistId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    if (artist != null)
                    {
                        artist.ACT_IND = false;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateArtistImage(int artistId, string userEmail)
        {
            var date = DateTime.Now;
            var imageId = -1;
            using (var context = new sparrow_dbEntities())
            {
                var image = new SPRW_ARTIST_IMG
                {
                    ACT_IND = true,
                    ARTIST_ID = artistId,
                    IMG_PRIMARY = true,
                    IMG_PATH = "",
                    LAST_MAINT_TIME = date,
                    LAST_MAINT_USER_ID = userEmail
                };
                context.SPRW_ARTIST_IMG.Add(image);
                context.SaveChanges();
                imageId = image.IMG_ID;
            }
            return imageId;
        }

        public void CreateArtistSetting(int artistId, Dictionary<string, string> artistSettings)
        {
            using (var context = new sparrow_dbEntities())
            {

                var settings =
                    artistSettings.Select(
                        i => new SPRW_ARTIST_SETTING {ARTIST_ID = artistId, KEY = i.Key, VALUE = i.Value});
                context.SPRW_ARTIST_SETTING.AddRange(settings);
                context.SaveChanges();
            }
        }

        public ArtistModel GetArtistById(int artistId)
        {
            ArtistModel artistModel;
            using (var context = new sparrow_dbEntities())
            {
                var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                artistModel = new ArtistModel()
                {
                    ArtistId = artist.ARTIST_ID,
                    ArtistName = artist.NAME,
                    Description = artist.DESCRP,
                    HasImage = artist.SPRW_ARTIST_IMG.Any(),
                    ImgId = artist.SPRW_ARTIST_IMG.Any()?artist.SPRW_ARTIST_IMG.Max(i=>i.IMG_ID):-1
                };
                
                var bulliten = context.ARTIST_BLOG.FirstOrDefault(i => i.ARTIST_ID == artistId);
                if (bulliten != null && bulliten.LAST_MAINT_TIME.AddHours(24) > DateTime.Now)
                {
                    artistModel.Bulliten = bulliten.BLOG;
                }
                /*var sprwArtistSettings = artist.SPRW_ARTIST_SETTINGS.FirstOrDefault();
                if (sprwArtistSettings != null)
                    artistModel.Settings = sprwArtistSettings.SETTING;*/
                artistModel.Settings = artist.SPRW_ARTIST_SETTING.ToDictionary(i => i.KEY, i => i.VALUE);
                var albums = new List<AlbumModel>();
                foreach (var album in artist.SPRW_ALBUM)
                {
                    var albumModel = new AlbumModel()
                    {
                        AlbumId = album.ALBUM_ID,
                        AlbumName = album.NAME,
                        Description = album.DESCRP,
                        ReleaseDate = album.RELEASE_DATE,
                        HasImage = album.SPRW_ALBUM_IMG.Any(),
                        ImgId = album.SPRW_ALBUM_IMG.Any()?album.SPRW_ALBUM_IMG.Max(i=>i.IMG_ID):-1
                    };
                    var tracks = new List<TrackModel>();
                    foreach (var track in album.SPRW_TRACK)
                    {
                        if (track.ACT_IND)
                        {
                            var trackModel = new TrackModel()
                            {
                                ArtistId = artist.ARTIST_ID,
                                AlbumId = album.ALBUM_ID,
                                TrackId = track.TRACK_ID,
                                TrackName = track.NAME
                            };
                            tracks.Add(trackModel);
                        }
                        
                    }
                    
                    albumModel.Tracks = tracks;
                    albums.Add(albumModel);
                }
                artistModel.Albums = albums;

                var events = new List<EventModel>();
                foreach (var e in artist.SPRW_ARTIST_EVENT.Where(i=>i.EVENT_DATE >= DateTime.Now))
                {
                    var model = new EventModel
                    {
                        EventId = e.EVENT_ID,
                        Address = e.ADDRESS,
                        City = e.CITY,
                        State = e.STATE,
                        Description = e.DESCRP,
                        EventDate = e.EVENT_DATE
                    };
                    events.Add(model);
                }
                artistModel.Events = events;
                artistModel.Genres = new List<GenreModel>();
                foreach (var genre in artist.SPRW_GENRE)
                {
                    var genreModel = new GenreModel
                    {
                        Genre = genre.GENRE,
                        GenreId = genre.GENRE_ID
                    };
                    artistModel.Genres.Add(genreModel);

                }
                /*var artistSettings = artist.SPRW_ARTIST_SETTINGS.FirstOrDefault();
                if (artistSettings != null)
                    artistModel.Settings = artistSettings.SETTING;*/
                artistModel.Settings = artist.SPRW_ARTIST_SETTING.ToDictionary(i => i.KEY, i => i.VALUE);
            }
            return artistModel;
        }

        public IEnumerable<ArtistModel> GetArtists(string email)
        {
            var id = GetUserId(email);
            var artistList = new List<ArtistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artists = context.SPRW_ARTIST_MEMBER.Where(i => i.USER_ID == id).Select(i => i.SPRW_ARTIST).ToList();
                    foreach (var artist in artists)
                    {
                        var artistItem = new ArtistModel();
                        artistItem.ArtistId = artist.ARTIST_ID;
                        artistItem.ArtistName = artist.NAME;
                        artistList.Add(artistItem);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return artistList;
        }

        public IEnumerable<ArtistModel> GetArtistsByName(string name)
        {
            var artistList = new List<ArtistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artists = context.SPRW_ARTIST.Where(i => i.NAME.StartsWith(name)).ToList();
                    foreach (var artist in artists)
                    {
                        var artistModel = new ArtistModel
                        {
                            ArtistId = artist.ARTIST_ID,
                            ArtistName = artist.NAME,
                            Description = artist.DESCRP,
                        };
                        var bulliten = context.ARTIST_BLOG.FirstOrDefault(i => i.ARTIST_ID == artist.ARTIST_ID);
                        if (bulliten != null && bulliten.LAST_MAINT_TIME.AddHours(24) > DateTime.Now)
                        {
                            artistModel.Bulliten = bulliten.BLOG;
                        }

                        var albums = new List<AlbumModel>();
                        foreach (var album in artist.SPRW_ALBUM)
                        {
                            var albumModel = new AlbumModel()
                            {
                                AlbumId = album.ALBUM_ID,
                                AlbumName = album.NAME,

                            };
                            var tracks = new List<TrackModel>();
                            foreach (var track in album.SPRW_TRACK)
                            {
                                var trackModel = new TrackModel()
                                {
                                    TrackId = track.TRACK_ID,
                                    TrackName = track.NAME
                                };
                                tracks.Add(trackModel);
                            }
                            albumModel.Tracks = tracks;
                            albums.Add(albumModel);
                        }
                        artistModel.Albums = albums;

                        var events = new List<EventModel>();
                        foreach (var e in artist.SPRW_ARTIST_EVENT)
                        {
                            var model = new EventModel
                            {
                                Address = e.ADDRESS,
                                City = e.CITY,
                                State = e.STATE,
                                Description = e.DESCRP,
                                EventDate = e.EVENT_DATE
                            };
                            events.Add(model);
                        }
                        artistModel.Events = events;
                        artistList.Add(artistModel);
                    }
                }

                return artistList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FollowArtist(string email, int artistId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    var user = context.SPRW_USER.FirstOrDefault(i => i.EMAIL == email);
                    if (artist != null) artist.SPRW_USER.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UnFollowArtist(string email, int artistId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    var user = context.SPRW_USER.FirstOrDefault(i => i.EMAIL == email);
                    artist.SPRW_USER.Remove(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region Genre

        public CommonModel GetCommonData()
        {
            var commonData = new CommonModel();
            commonData.Genres = (List<GenreModel>)GetGenreData();
            using (var context = new sparrow_dbEntities())
            {
                commonData.Max = (double) context.SPRW_ARTIST.Max(i => i.POP_INDEX);
                commonData.Min = (double) context.SPRW_ARTIST.Min(i => i.POP_INDEX);
                commonData.StartDate = context.SPRW_ALBUM.Min(i => i.RELEASE_DATE);
                commonData.EndDate = context.SPRW_ALBUM.Max(i => i.RELEASE_DATE);
            }

            return commonData;
        }

        public IEnumerable<GenreModel> GetGenres()
        {
            return GetGenreData();
        }
        public void AddGenre(int artistId, int genreId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var genre = context.SPRW_GENRE.FirstOrDefault(i => i.GENRE_ID == genreId);
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    if (artist != null)
                        artist.SPRW_GENRE.Add(genre);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveGenre(int artistId, int genreId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var genre = context.SPRW_GENRE.FirstOrDefault(i => i.GENRE_ID == genreId);
                    var artist = context.SPRW_ARTIST.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    if (artist != null)
                        artist.SPRW_GENRE.Remove(genre);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Album
        public AlbumModel GetAlbum(int albumId)
        {
            try
            {
                var albumModel = new AlbumModel();
                using (var context = new sparrow_dbEntities())
                {
                    var album = context.SPRW_ALBUM.FirstOrDefault(i => i.ALBUM_ID == albumId);
                    if (album != null)
                    {
                        albumModel.AlbumId = album.ALBUM_ID;
                        albumModel.AlbumName = album.NAME;
                        var tracks = new List<TrackModel>();
                        foreach (var track in album.SPRW_TRACK)
                        {
                            var trackModel = new TrackModel
                            {
                                TrackId = track.TRACK_ID,
                                TrackName = track.NAME
                            };
                            tracks.Add(trackModel);
                        }
                        albumModel.Tracks = tracks;
                    }
                }

                return albumModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AlbumModel> GetAlbums(int artistId)
        {
            try
            {
                var model = new List<AlbumModel>();
                using (var context = new sparrow_dbEntities())
                {
                    var albums = context.SPRW_ALBUM.Where(i => i.SPRW_ARTIST.ARTIST_ID == artistId).ToList();

                    foreach (var album in albums)
                    {
                        var albumModel = new AlbumModel
                        {
                            AlbumId = album.ALBUM_ID,
                            AlbumName = album.NAME,
                            Tracks = new List<TrackModel>()
                        };

                        foreach (var track in album.SPRW_TRACK)
                        {
                            var trackModel = new TrackModel
                            {
                                TrackId = track.TRACK_ID,
                                TrackName = track.NAME
                            };
                            albumModel.Tracks.Add(trackModel);
                        }
                        model.Add(albumModel);
                    }
                }

                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateAlbum(CreateAlbumModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var album = new SPRW_ALBUM()
                    {
                        ARTIST_ID = model.ArtistId,
                        RELEASE_DATE = model.ReleaseDate,
                        NAME = model.AlbumName,
                        DESCRP = model.Description,
                        LAST_MAINT_USER_ID = "",
                        LAST_MAINT_TIME = DateTime.Now
                    };
                    context.SPRW_ALBUM.Add(album);
                    context.SaveChanges();

                    return album.ALBUM_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateAlbum(ModifyAlbumModel model)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var album = context.SPRW_ALBUM.FirstOrDefault(i => i.ALBUM_ID == model.AlbumId);
                    if (album != null)
                    {
                        album.ARTIST_ID = model.ArtistId;
                        album.RELEASE_DATE = model.ReleaseDate;
                        album.NAME = model.AlbumName;
                        album.DESCRP = model.Description;
                        album.LAST_MAINT_USER_ID = model.UserEmail;
                        album.LAST_MAINT_TIME = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveAlbum(int albumId)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var album = context.SPRW_ALBUM.FirstOrDefault(i => i.ALBUM_ID == albumId);
                    if (album != null)
                    {
                        album.ACT_IND = false;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateAlbumImage(int albumId, string userEmail)
        {
            var date = DateTime.Now;
            var imageId = -1;
            using (var context = new sparrow_dbEntities())
            {
                var image = new SPRW_ALBUM_IMG
                {
                    ACT_IND = true,
                    ALBUM_ID = albumId,
                    IMG_PATH = "",
                    LAST_MAINT_TIME = date,
                    LAST_MAINT_USER_ID = userEmail
                };
                context.SPRW_ALBUM_IMG.Add(image);
                context.SaveChanges();
                imageId = image.IMG_ID;
            }
            return imageId;
        }
        #endregion

        #region Track

        public TrackModel GetTrack(int id)
        {
            var model = new TrackModel();
            try
            {
                log.Info("method : CreateTrack | action : starting db connections");
                using (var context = new sparrow_dbEntities())
                {
                    var track = context.SPRW_TRACK.FirstOrDefault(i => i.TRACK_ID == id);
                    if (track != null)
                    {
                        model.TrackName = track.NAME;
                        model.TrackId = track.TRACK_ID;
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<TrackModel> GetTracks(int albumId)
        {
            throw new NotImplementedException();
        }
        public int CreateTrack(CreateTrackModel model)
        {
            var trackId = -1;
            DateTime relaseDate;
            if (model.ReleaseDate == null)
            {
                relaseDate = DateTime.Now;
            }
            else
            {
                relaseDate = (DateTime)model.ReleaseDate;
            }
            try
            {
                log.Info("method : CreateTrack | action : starting db connections");
                using (var context = new sparrow_dbEntities())
                {
                    var album = new SPRW_TRACK()
                    {
                        ARTIST_ID = model.ArtistId,
                        ALBUM_ID = model.AlbumId,
                        ACT_IND = false,
                        IS_REJECTED = false,
                        RELEASE_DATE = relaseDate,
                        NAME = model.TrackName,
                        DESCRP = model.Description,
                        LAST_MAINT_TIME = DateTime.Now,
                        LAST_MAINT_USER_ID = model.UserEmail
                    };

                    context.SPRW_TRACK.Add(album);
                    context.SaveChanges();

                    trackId = album.TRACK_ID;
                    var trackQueue = new SPRW_TRACK_QUEUE
                    {
                        TRACK_ID = trackId,
                        DATE_QUEUED = DateTime.Now,
                        IS_REVIEWING = false,
                        TRACK_PATH = ""
                    };

                    context.SPRW_TRACK_QUEUE.Add(trackQueue);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return trackId;
        }

        public void UpdateTrack(ModifyTrackModel model)
        {
            try
            {
                log.Info("method : CreateTrack | action : starting db connections");
                using (var context = new sparrow_dbEntities())
                {
                    var track = context.SPRW_TRACK.FirstOrDefault(i => i.TRACK_ID == model.TrackId);
                    if (track != null)
                    {
                        track.ARTIST_ID = model.ArtistId;
                        track.ALBUM_ID = model.AlbumId;
                        track.ACT_IND = true;
                        if (model.ReleaseDate != null)
                            track.RELEASE_DATE = (DateTime)model.ReleaseDate;
                        track.NAME = model.TrackName;
                        track.DESCRP = model.Description;
                        track.LAST_MAINT_TIME = DateTime.Now;
                        track.LAST_MAINT_USER_ID = model.UserEmail;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteTrack(int id)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var track = context.SPRW_TRACK.FirstOrDefault(i => i.TRACK_ID == id);
                    if (track != null) track.ACT_IND = false;

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TrackPathModel GetTrackPath(int trackId)
        {
            var model = new TrackPathModel();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var track = context.SPRW_TRACK.FirstOrDefault(i => i.TRACK_ID == trackId);
                    if (track != null)
                    {
                        model.AlbumId = track.ALBUM_ID;
                        model.ArtistId = track.ARTIST_ID;
                        model.TrackId = trackId;
                    }
                }
            }
            catch (Exception e)
            {

            }

            return model;
        }
        public void AddTrackPopularityLike(PopularityModel model)
        {
            var success = true;
            try
            {
                var userID = GetUserId(model.UserEmail);
                log.Info("method : AddTrackPopularityLike | action : starting db connection | message : liking track " + model.TrackId);
                using (var context = new sparrow_dbEntities())
                {

                    var like = new SPRW_TRACK_POPULAR_LIKE()
                    {
                        DISLIKE_DATE = DateTime.Now,
                        TRACK_ID = model.TrackId,
                        USER_ID = userID
                    };
                    context.SPRW_TRACK_POPULAR_LIKE.Add(like);
                    context.SaveChanges();
                }
                UpdatePopIndex(model.TrackId);
            }
            catch (Exception e)
            {
                log.Error("method : AddTrackPopularityLike | exception : " + e.Message);
            }
        }

        //public void AddTrackPopularityDislike(PopularityModel model)
        //{
        //    try
        //    {
        //        var userId = GetUserId(model.UserEmail);
        //        using (var context = new sparrow_dbEntities())
        //        {

        //            var dislike = new SPRW_TRACK_POPULAR_DISLIKES()
        //            {
        //                DISLIKE_DATE = DateTime.Now,
        //                TRACK_ID = model.TrackId,
        //                USER_ID = userId
        //            };
        //            context.SPRW_TRACK_POPULAR_DISLIKES.Add(dislike);
        //            context.SaveChanges();
        //        }
        //        UpdatePopIndex(model.TrackId);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error("method : AddTrackPopularityDislike | exception : " + e.Message);
        //    }
        //}

        public void AddTrackPopularityPlayThrough(PopularityModel model)
        {
            var success = true;
            try
            {
                var userId = GetUserId(model.UserEmail);
                using (var context = new sparrow_dbEntities())
                {
                    var playThrough = new SPRW_TRACK_POPULAR_PLAY_THROUGH
                    {
                        DISLIKE_DATE = DateTime.Now,
                        TRACK_ID = model.TrackId,
                        USER_ID = userId 
                    };
                    context.SPRW_TRACK_POPULAR_PLAY_THROUGH.Add(playThrough);
                    context.SaveChanges();
                }
                UpdatePopIndex(model.TrackId);

            }
            catch (Exception e)
            {
                log.Error("method : AddTrackPopularityPlayThrough | exception : " + e.Message);
            }
        }

        public void AddTrackPopularitySelect(PopularityModel model)
        {
            var success = true;
            try
            {
                var userId = GetUserId(model.UserEmail);
                using (var context = new sparrow_dbEntities())
                {
                    var select = new SPRW_TRACK_POPULAR_SELECT()
                    {
                        DISLIKE_DATE = DateTime.Now,
                        TRACK_ID = model.TrackId,
                        USER_ID = userId
                    };
                    context.SPRW_TRACK_POPULAR_SELECT.Add(select);
                    context.SaveChanges();
                }
                UpdatePopIndex(model.TrackId);

            }
            catch (Exception e)
            {
                log.Error("method : AddTrackPopularityPlayThrough | exception : " + e.Message);
            }
        }



        #endregion

        #region PlayList
        public List<PlaylistModel> GetPlaylist(int page)
        {
            var playlist = new List<PlaylistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var tracks =
                        context.SPRW_TRACK.OrderByDescending(i => i.SPRW_TRACK_POPULAR_LIKE.Count).ThenBy(i => i.LAST_MAINT_TIME).Take(100);
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
                return playlist;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Search
        public IEnumerable<ArtistModel> SearchArtists(string name)
        {
            var artists = new List<ArtistModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var artistList = context.SPRW_ARTIST.Where(i => i.NAME.StartsWith(name)).Take(3);
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
                throw e;
            }

            return tracks;
        }

        public IEnumerable<UserModel> SearchUsers(string email)
        {
            var userList = new List<UserModel>();
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var users = context.SPRW_USER.Where(i => i.EMAIL.Contains(email));
                    foreach (var user in users)
                    {
                        var model = new UserModel()
                        {
                            UserEmail = user.EMAIL,
                            UserId = user.USER_ID
                        };
                        userList.Add(model);
                    }
                }
                return userList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region User
        
        #endregion

        public void CreateSetting(int artistId, string setting)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var model = new SPRW_ARTIST_SETTINGS
                    {
                        ARTIST_ID = artistId,
                        SETTING = setting
                    };
                    context.SPRW_ARTIST_SETTINGS.Add(model);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void UpdateSetting(int artistId, string setting)
        {
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var sprwArtistSettings = context.SPRW_ARTIST_SETTINGS.FirstOrDefault(i => i.ARTIST_ID == artistId);
                    if (sprwArtistSettings != null)
                        sprwArtistSettings.SETTING = setting;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #region Private Methods
        private void UpdatePopIndex(int trackId)
        {
            using (var context = new sparrow_dbEntities())
            {
                var track = context.SPRW_TRACK.FirstOrDefault(i => i.TRACK_ID == trackId);
                if (track != null)
                {
                    var artist = track.SPRW_ARTIST;
                    var diffDays = (DateTime.Now - track.LAST_MAINT_TIME).Days;
                    var popCount = (track.SPRW_TRACK_POPULAR_LIKE.Count * 2) + track.SPRW_TRACK_POPULAR_PLAY_THROUGH.Count +
                                   track.SPRW_TRACK_POPULAR_SELECT.Count;
                    popCount -= track.SPRW_TRACK_POPULAR_SKIPS.Count;

                    var popIndex = popCount / Math.Pow(diffDays, 1.8);
                    track.POP_INDEX = (decimal)popIndex;
                    var maxPopIndex = artist.SPRW_TRACK.Max(i => i.POP_INDEX);
                    if (maxPopIndex != null)
                        artist.POP_INDEX = (decimal)maxPopIndex;
                }

                context.SaveChanges();
            }
        }
        private int GetUserId(string email)
        {
            var id = -1;
            try
            {
                using (var context = new sparrow_dbEntities())
                {
                    var firstOrDefault = context.SPRW_USER.FirstOrDefault(i => i.EMAIL.ToLower().Equals(email.ToLower()));
                    if (firstOrDefault != null)
                        id = firstOrDefault.USER_ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return id;
        }

        private IEnumerable<GenreModel> GetGenreData()
        {
            var genreList = new List<GenreModel>();
            using (var context = new sparrow_dbEntities())
            {
                var genres = context.SPRW_GENRE;
                foreach (var genre in genres)
                {
                    genreList.Add(new GenreModel
                    {
                        Genre = genre.GENRE,
                        GenreId = genre.GENRE_ID
                    });
                }
            }
            return genreList;
        } 
        #endregion
    }
}
