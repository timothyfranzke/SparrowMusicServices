using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using Sparrow.Services.Data.Repository;
using Sparrow.Services.File;
using Sparrow.Services.File.Interface;
using Sparrow.Services.Models;
using Sparrow.Services.Utils;
using Sparrow.Services.Utils.Interface;

namespace Sparrow.Services.API
{
    public class Artist
    {
        private  readonly ArtistRepository Repository = new ArtistRepository();
        private readonly IImageFileManager imageFileManager;
        private readonly IAudioFileManager audioFileManager;

        public Artist()
        {
            audioFileManager = new FileSystemAudioFileManager();
            imageFileManager = new FileSystemImageFileManager();
        }

        public Artist(IImageFileManager imageFileManager)
        {
            this.imageFileManager = imageFileManager;
        }

        public Artist(IAudioFileManager fileManager, IImageFileManager imageFileManager)
        {
            this.imageFileManager = imageFileManager;
        }

        public int CreateArtist(CreateArtistModel model)
        { 
            var id = Repository.CreateArtist(model);
            var album = new CreateAlbumModel
            {
                AlbumName = "Singles",
                ArtistId = id,
                ReleaseDate = DateTime.Now
            };
            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Setting);
            Repository.CreateArtistAssociation(model.UserEmail, id);
            Repository.CreateAlbum(album);
            Repository.CreateArtistSetting(id, settings);
            CreateArtistDirectory(id);

            return id;
        }

        public void UpdateArtist(ModifyArtistModel model)
        {
            Repository.UpdateArtist(model);
        }

        public void RemoveArtist(int artistId)
        {
            Repository.RemoveArtist(artistId);
        }

        public void CreateAssociation(CreateArtistAssociation model)
        {
            Repository.CreateArtistAssociation(model.UserEmail, model.ArtistId);
        }

        public void RemoveAssociation(int artistId, string email)
        {
            Repository.RemoveArtistAssociation(artistId, email);
        }

        public void CreateArtistImg(ImageModel model)
        {
            //imageFileManager.Create(model.ArtistId, );
            //CreateArtistImgDirectory(model);
        }

        public int CreateArtistImg(ImageBase64Model model)
        {
            var imageId = Repository.CreateArtistImage(model.ArtistId, model.UserEmail);
            imageFileManager.Create(model.TrackingId.ToString(), model.Image64, imageId);

            return imageId;
        }

        public  int CreateBulliten(BullitenModel model)
        {
            return Repository.CreateBulliten(model);
        }

        public  IEnumerable<ArtistModel> GetArtists(string email)
        {
            return Repository.GetArtists(email);
        }

        public  ArtistModel GetArtistById(int artistId)
        {
            return Repository.GetArtistById(artistId);
        }

        public  byte[] GetFile(int? artistId)
        {
            return null;
            //return File.GetImgFile(artistId, null, null);
        }

        public  void RemoveGenre(int artistId, int genreId)
        {
            Repository.RemoveGenre(artistId, genreId);
        }

        public  void AddGenre(int artistId, int genreId)
        {
            Repository.AddGenre(artistId, genreId);
        }

        public  CommonModel GetCommonData()
        {
            return Repository.GetCommonData();
        }

        public  IEnumerable<GenreModel> GetGenres()
        {
            return Repository.GetGenres();
        }

        public  IEnumerable<MarketSearchModel> SearchMarkets(string criteria, string value)
        {
            switch (criteria)
            {
                case "name":
                    return Repository.SearchMarkbetByName(value);
            }
            return null;
        }

        private  void CreateArtistDirectory(int id)
        {
            var imageDir = "tracks/" + id;
            var trackDir = "~/~/tracks/" + id;
/*            var dirAlbum = dir + "/albums";
            var dirImage = dir + "/imgs";
            var dirSingle = dirAlbum + "/singles";*/
            FileUtils.CreateDirectory(imageDir);
/*/*            try
            {
                File.CreateDirectory(dir);
                File.CreateDirectory(dirAlbum);
                File.CreateDirectory(dirImage);
                File.CreateDirectory(dirSingle);#1#
            }
            catch (Exception e)
            {
                throw e;
            }*/
        }
        #region Album
        public  int CreateAlbum(CreateAlbumModel model)
        {
            if (model.ReleaseDate < DateTime.Now)
                model.ReleaseDate = DateTime.Now;

            var id = Repository.CreateAlbum(model);
            //CreateAlbumDirectory(model.ArtistId, id);

            return id;
        }

        public  void UpdateAlbum(ModifyAlbumModel model)
        {
            Repository.UpdateAlbum(model);
        }

        public  void RemoveAlbum(int albumId)
        {
            Repository.RemoveAlbum(albumId);
        }

        public int CreateAlbumImg(ImageBase64Model model)
        {
            var dir = String.Format("{0}/{1}", model.ArtistId, model.TrackingId);
            var imageId = Repository.CreateArtistImage(model.ArtistId, model.UserEmail);

            imageFileManager.Create(dir, model.Image64, imageId);

            return imageId;
        }

        private  void CreateAlbumImage(ImageBase64Model model, int imgId)
        {
            var dir = "/artists/" + model.ArtistId + "/albums/" + model.AlbumId + "/img/";

            //File.CreateImgFile(dir, model.Image64, imgId);
        }

        public  byte[] GetFile(int? artistId, int? albumId)
        {
            return null;
            //return File.GetImgFile(artistId, albumId, null);
        }

        public  IEnumerable<AlbumModel> GetAlbums(int artistId)
        {
            return Repository.GetAlbums(artistId);
        }

        private  void CreateAlbumDirectory(int artistId, int albumId)
        {
            var dir = "/artists/" + artistId + "/albums/" + albumId;
            var imgDir = dir + "/imgs";
            var trackDir = dir + "/tracks";

            try
            {
/*                File.CreateDirectory(dir);
                File.CreateDirectory(imgDir);
                File.CreateDirectory(trackDir);*/
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Track
        public  int CreateSingleTrack(CreateTrackModel model)
        {
            var id = Repository.CreateTrack(model);
            var path = string.Format("{0}/{1}/{2}", model.ArtistId, model.AlbumId, id);
            //CreateTrackDirectory(model, id);
            
            audioFileManager.Create(path, model.Track, id, model.FileType);
           
            return id;
        }

        public  List<int> CreateMultipleTracks(IEnumerable<CreateTrackModel> models)
        {
            var ids = new List<int>();
            foreach (var track in models)
            {
                var id = CreateSingleTrack(track);
                ids.Add(id);
            }

            return ids;
        }

        public  void RemoveTrack(int trackId)
        {
            Repository.DeleteTrack(trackId);
        }

        public  void CreateTrackImg(ImageModel model)
        {
            CreateTrackImgDirectory(model);
        }

        public  void ModifyTrackPopularity(PopularityModel model)
        {
          switch (model.Criteria)
            {
                case "playthrough":
                    Repository.AddTrackPopularityPlayThrough(model);
                    break;
                case "like":
                    Repository.AddTrackPopularityLike(model);
                    break;
                case "select":
                    Repository.AddTrackPopularitySelect(model);
                    break;

            }
        }

        public  byte[] GetFile(string type, int? artistId, int? albumId, int? trackId)
        {
            byte[] bytes = new byte[] { };
            switch (type)
            {
/*                case "image":
                    bytes = File.GetImgFile(artistId, albumId, trackId);
                    break;
                case "audio":
                    bytes = File.GetAudioFile(artistId, albumId, trackId);
                    break;*/
            }
            return bytes;
        }
        private  void CreateArtistImgDirectory(ImageModel model)
        {
            var dir = String.Format("{0}/", model.TrackingId);

            try
            {
/*                File.CreateDirectory(dir);
                File.CreateImgFile(dir, model.AlbumImage, model.TrackingId);*/
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private  void CreateTrackDirectory(CreateTrackModel model, int trackId)
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
                //File.BasicStorageBlockBlobOperationsAsync(model.Track, dir, trackId, ".mp3", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public  int CreateEvent(CreateEventModel model)
        {
            return Repository.CreateEvent(model);
        }

        public  void RemoveEvent(int eventId)
        {
            Repository.RemoveEvent(eventId);
        }
        private  void CreateTrackImgDirectory(ImageModel model)
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
/*                File.CreateDirectory(dir);
                File.CreateImgFile(dir, model.AlbumImage, model.TrackingId);*/
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        public  void CreateSetting(SettingModel model)
        {
            Repository.CreateSetting(model.ArtistId, model.Setting);
        }

        public  void UpdateSetting(SettingModel model)
        {
            Repository.UpdateSetting(model.ArtistId, model.Setting);
        }

        #region Private Methods
        private  void CreateAlbumImgDirectory(ImageBase64Model model)
        {
            var dir = String.Format("{0}/{1}/", model.ArtistId, model.TrackingId);
            var blobName = System.Configuration.ConfigurationManager.AppSettings["BlobImageName"];
            try
            {
                //File.CreateDirectory(dir);
                //File.CreateImgFile(dir, model.Image64, model.TrackingId);
                //File.BasicStorageBlockBlobOperationsAsync(model.Image64, dir, 0, ".jpg", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private  void CreateArtistImgDirectory(ImageBase64Model model)
        {
            var blobName = System.Configuration.ConfigurationManager.AppSettings["BlobImageName"];
            var dir = String.Format("{0}/", model.TrackingId);

            try
            {
                //File.BasicStorageBlockBlobOperationsAsync(model.Image64, dir, 0, ".jpg", blobName).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
#endregion
    }
}
