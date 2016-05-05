using System;
using System.Collections.Generic;
using Sparrow.Services.Data.Repository;
using Sparrow.Services.Models;
using Sparrow.Services.Utils;

namespace Sparrow.Services.API
{
    public static class Artist
    {
        private static readonly ArtistRepository Repository = new ArtistRepository();
        public static int CreateArtist(CreateArtistModel model)
        { 
            var id = Repository.CreateArtist(model);
            var album = new CreateAlbumModel
            {
                AlbumName = "Singles",
                ArtistId = id,
                ReleaseDate = DateTime.Now
            };
            
            Repository.CreateArtistAssociation(model.UserEmail, id);
            Repository.CreateAlbum(album);
            Repository.CreateArtistSetting(id, model.Setting);
            CreateArtistDirectory(id);

            return id;
        }

        public static void UpdateArtist(ModifyArtistModel model)
        {
            Repository.UpdateArtist(model);
        }

        public static void RemoveArtist(int artistId)
        {
            Repository.RemoveArtist(artistId);
        }

        public static void CreateAssociation(CreateArtistAssociation model)
        {
            Repository.CreateArtistAssociation(model.UserEmail, model.ArtistId);
        }

        public static void RemoveAssociation(int artistId, string email)
        {
            Repository.RemoveArtistAssociation(artistId, email);
        }

        public static void CreateArtistImg(ImageModel model)
        {
            CreateArtistImgDirectory(model);
        }

        public static void CreateArtistImg(ImageBase64Model model)
        {
            var imageId = Repository.CreateArtistImage(model.ArtistId, model.UserEmail);
            CreateArtistImgDirectory(model);
        }

        public static int CreateBulliten(BullitenModel model)
        {
            return Repository.CreateBulliten(model);
        }

        public static IEnumerable<ArtistModel> GetArtists(string email)
        {
            return Repository.GetArtists(email);
        }

        public static ArtistModel GetArtistById(int artistId)
        {
            return Repository.GetArtistById(artistId);
        }

        public static byte[] GetFile(int? artistId)
        {
            return File.GetImgFile(artistId, null, null);
        }

        public static void RemoveGenre(int artistId, int genreId)
        {
            Repository.RemoveGenre(artistId, genreId);
        }

        public static void AddGenre(int artistId, int genreId)
        {
            Repository.AddGenre(artistId, genreId);
        }

        public static CommonModel GetCommonData()
        {
            return Repository.GetCommonData();
        }

        public static IEnumerable<GenreModel> GetGenres()
        {
            return Repository.GetGenres();
        }

        public static IEnumerable<MarketSearchModel> SearchMarkets(string criteria, string value)
        {
            switch (criteria)
            {
                case "name":
                    return Repository.SearchMarkbetByName(value);
            }
            return null;
        }
        private static void CreateArtistDirectory(int id)
        {
            var dir = "/artists/" + id;
            var dirAlbum = dir + "/albums";
            var dirImage = dir + "/imgs";
            var dirSingle = dirAlbum + "/singles";

            try
            {
                File.CreateDirectory(dir);
                File.CreateDirectory(dirAlbum);
                File.CreateDirectory(dirImage);
                File.CreateDirectory(dirSingle);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #region Album
        public static int CreateAlbum(CreateAlbumModel model)
        {
            if (model.ReleaseDate < DateTime.Now)
                model.ReleaseDate = DateTime.Now;

            var id = Repository.CreateAlbum(model);
            //CreateAlbumDirectory(model.ArtistId, id);

            return id;
        }

        public static void UpdateAlbum(ModifyAlbumModel model)
        {
            Repository.UpdateAlbum(model);
        }

        public static void RemoveAlbum(int albumId)
        {
            Repository.RemoveAlbum(albumId);
        }

        public static void CreateAlbumImg(ImageBase64Model model)
        {
            Repository.CreateAlbumImage(model.AlbumId, model.UserEmail);
            CreateAlbumImgDirectory(model);
        }

        private static void CreateAlbumImage(ImageBase64Model model, int imgId)
        {
            var dir = "/artists/" + model.ArtistId + "/albums/" + model.AlbumId + "/img/";

            File.CreateImgFile(dir, model.Image64, imgId);
        }

        public static byte[] GetFile(int? artistId, int? albumId)
        {
            return File.GetImgFile(artistId, albumId, null);
        }

        public static IEnumerable<AlbumModel> GetAlbums(int artistId)
        {
            return Repository.GetAlbums(artistId);
        }

        private static void CreateAlbumDirectory(int artistId, int albumId)
        {
            var dir = "/artists/" + artistId + "/albums/" + albumId;
            var imgDir = dir + "/imgs";
            var trackDir = dir + "/tracks";

            try
            {
                File.CreateDirectory(dir);
                File.CreateDirectory(imgDir);
                File.CreateDirectory(trackDir);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Track
        public static int CreateSingleTrack(CreateTrackModel model)
        {
            var id = Repository.CreateTrack(model);
            CreateTrackDirectory(model, id);
           
            return id;
        }

        public static List<int> CreateMultipleTracks(IEnumerable<CreateTrackModel> models)
        {
            var ids = new List<int>();
            foreach (var track in models)
            {
                var id = CreateSingleTrack(track);
                ids.Add(id);
            }

            return ids;
        }

        public static void RemoveTrack(int trackId)
        {
            Repository.DeleteTrack(trackId);
        }

        public static void CreateTrackImg(ImageModel model)
        {
            CreateTrackImgDirectory(model);
        }

        public static void ModifyTrackPopularity(PopularityModel model)
        {
            switch (model.Criteria)
            {
                case "playthrough":
                    Repository.AddTrackPopularityPlayThrough(model);
                    break;
                case "like":
                    Repository.AddTrackPopularityLike(model);
                    break;
            }
        }

        public static byte[] GetFile(string type, int? artistId, int? albumId, int? trackId)
        {
            byte[] bytes = new byte[] { };
            switch (type)
            {
                case "image":
                    bytes = File.GetImgFile(artistId, albumId, trackId);
                    break;
                case "audio":
                    bytes = File.GetAudioFile(artistId, albumId, trackId);
                    break;
            }
            return bytes;
        }
        private static void CreateArtistImgDirectory(ImageModel model)
        {
            var dir = String.Format("{0}/", model.TrackingId);

            try
            {
                File.CreateDirectory(dir);
                File.CreateImgFile(dir, model.AlbumImage, model.TrackingId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void CreateTrackDirectory(CreateTrackModel model, int trackId)
        {
            var albums = String.Empty;
            var blobName = System.Configuration.ConfigurationManager.AppSettings["BlobMusicName"];
            if (model.AlbumId == null)
            {
                albums = "/0";
            }
            else
            {
                albums = "/" + model.AlbumId;
            }
            var dir = model.ArtistId + albums + "/" + trackId + "/";

            try
            {
                File.BasicStorageBlockBlobOperationsAsync(model.Track, dir, trackId, ".mp3", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int CreateEvent(CreateEventModel model)
        {
            return Repository.CreateEvent(model);
        }

        public static void RemoveEvent(int eventId)
        {
            Repository.RemoveEvent(eventId);
        }
        private static void CreateTrackImgDirectory(ImageModel model)
        {
            var trackPath = Repository.GetTrackPath(model.TrackingId);
            var albums = String.Empty;
            if (trackPath.AlbumId == null)
            {
                albums = "/albums/singles";
            }
            else
            {
                albums = "/albums/" + trackPath.AlbumId;
            }
            var dir = "/artists/" + trackPath.ArtistId + albums + "/tracks/" + trackPath.TrackId + "/img/";

            try
            {
                File.CreateDirectory(dir);
                File.CreateImgFile(dir, model.AlbumImage, model.TrackingId);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        public static void CreateSetting(SettingModel model)
        {
            Repository.CreateSetting(model.ArtistId, model.Setting);
        }

        public static void UpdateSetting(SettingModel model)
        {
            Repository.UpdateSetting(model.ArtistId, model.Setting);
        }

        #region Private Methods
        private static void CreateAlbumImgDirectory(ImageBase64Model model)
        {
            var dir = String.Format("{0}/{1}/", model.ArtistId, model.TrackingId);
            var blobName = System.Configuration.ConfigurationManager.AppSettings["BlobImageName"];
            try
            {
                //File.CreateDirectory(dir);
                //File.CreateImgFile(dir, model.Image64, model.TrackingId);
                File.BasicStorageBlockBlobOperationsAsync(model.Image64, dir, 0, ".jpg", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static void CreateArtistImgDirectory(ImageBase64Model model)
        {
            var blobName = System.Configuration.ConfigurationManager.AppSettings["BlobImageName"];
            var dir = String.Format("{0}/", model.TrackingId);

            try
            {
                File.BasicStorageBlockBlobOperationsAsync(model.Image64, dir, 0, ".jpg", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
#endregion
    }
}
