using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Sparrow.Services.API;
using Sparrow.Services.Models;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Sparrow.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ArtistController : ApiController
    {
        private readonly Artist _artist;
        private readonly Security _security;
        public ArtistController()
        {
            _artist = new Artist();
            _security = new Security();
        }

        public ArtistController(Artist artist)
        {
            _artist = artist;
        }

        [HttpGet]
        [ActionName("Artist")]
        public HttpResponseMessage GetArtistById(int id)
        {
            try
            {

                var artists = _artist.GetArtistById(id);
                var response = Request.CreateResponse(HttpStatusCode.OK, artists);
                return response;
                
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
            }
        }

        [HttpGet]
        [ActionName("Artist")]
        public HttpResponseMessage GetAssociatedArtists(string email, string token)
        {
            try
            {
                if ( _security.Verify(token, email))
                {
                    var artists = _artist.GetArtists(email);
                    var response = Request.CreateResponse(HttpStatusCode.OK, artists);
                    return response;
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
            }
        }

        [HttpPost]
        [ActionName("Artist")]
        public HttpResponseMessage CreateArtist([FromBody]CreateArtistModel model)
        {
            if ( _security.Verify(model.Token, model.UserEmail))
            {
                try
                {
                    return Request.CreateResponse(HttpStatusCode.Created, _artist.CreateArtist(model));
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        [ActionName("Artist")]
        public HttpResponseMessage UpdateArtist([FromBody]ModifyArtistModel model)
        {
            if ( _security.Verify(model.Token, model.UserEmail))
            {
                try
                {
                    _artist.UpdateArtist(model);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ActionName("Album")]
        public HttpResponseMessage CreateAlbum([FromBody]CreateAlbumModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, _artist.CreateAlbum(model));
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ActionName("Album")]
        public HttpResponseMessage UpdateAlbum([FromBody]ModifyAlbumModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    _artist.UpdateAlbum(model);
                    Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ActionName("Album")]
        public HttpResponseMessage RemoveAlbum(int albumId, int artistId, string userEmail, string token)
        {
            try
            {
                if ( _security.Verify(token, userEmail, artistId))
                {
                    _artist.RemoveAlbum(albumId);
                    Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("AlbumImage")]
        public HttpResponseMessage CreateAlbumImage([FromBody]ImageBase64Model model)
        {
            try
            {
                model.Image64 = model.Image64.Substring(model.Image64.IndexOf(",") + 1);
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, _artist.CreateAlbumImg(model));
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
            }
        }

        [HttpPost]
        [ActionName("Associate")]
        public HttpResponseMessage AssociateUserWithArtist([FromBody]CreateArtistAssociation model)
        {
            if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
            {
                try
                {
                    _artist.CreateAssociation(model);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        [ActionName("Bulliten")]
        public HttpResponseMessage CreateArtistBulliten([FromBody]BullitenModel model)
        {
            if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
            {
                try
                {
                    _artist.CreateBulliten(model);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.StackTrace);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateTrack(int albumid, int artistId, string email, string token, string trackName, string fileType)
        {
            try
            {
                if (_security.Verify(token, email, artistId))
                {
                    var model = new CreateTrackModel
                    {
                        TrackName = trackName,
                        AlbumId = albumid,
                        ArtistId = artistId,
                        UserEmail = email,
                        FileType = fileType
                    };
                    model.Track = await Request.Content.ReadAsByteArrayAsync();
                    var id = _artist.CreateSingleTrack(model);
                    return Request.CreateResponse(HttpStatusCode.Created, id);
                }
                
                //_artist.CreateSingleTrack();
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [ActionName("Track")]
        public HttpResponseMessage RemoveTrack(int trackId, string userEmail, string token, int artistId)
        {
            try
            {
                if ( _security.Verify(token, userEmail, artistId))
                {
                    _artist.RemoveTrack(trackId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("Image")]
        public HttpResponseMessage CreateImage([FromBody]ImageBase64Model model)
        {
            try
            {
                model.Image64 = model.Image64.Substring(model.Image64.IndexOf(",", StringComparison.Ordinal) + 1);
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, _artist.CreateArtistImg(model));
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("Event")]
        public HttpResponseMessage CreateEvent([FromBody]CreateEventModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, _artist.CreateEvent(model));
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ActionName("Event")]
        public HttpResponseMessage RemoveEvent(int eventId, string userEmail, string token, int artistId)
        {
            try
            {
                if ( _security.Verify(token, userEmail, artistId))
                {
                    _artist.RemoveEvent(eventId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Common")]
        public HttpResponseMessage GetCommonData()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _artist.GetCommonData());
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Markets")]
        public HttpResponseMessage SearchMarket(string criteria, string value)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _artist.SearchMarkets(criteria, value));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Genre")]
        public HttpResponseMessage GetGenres()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _artist.GetGenres());
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("Genre")]
        public HttpResponseMessage AddGenre([FromBody]CreateGenreModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    _artist.AddGenre(model.ArtistId, model.GenreId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ActionName("Genre")]
        public HttpResponseMessage DeleteGenre(int artistId, int genreId, string userEmail, string token)
        {
            try
            {
                if ( _security.Verify(token, userEmail, artistId))
                {
                    _artist.RemoveGenre(artistId, genreId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("Setting")]
        public HttpResponseMessage CreateArtistSetting([FromBody] SettingModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    _artist.CreateSetting(model);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPut]
        [ActionName("Setting")]
        public HttpResponseMessage UpdateArtistSetting([FromBody] SettingModel model)
        {
            try
            {
                if ( _security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    _artist.UpdateSetting(model);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
