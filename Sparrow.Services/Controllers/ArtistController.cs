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
        [HttpGet]
        [ActionName("Artist")]
        public HttpResponseMessage GetArtistById(int id)
        {
            try
            {
                var artists = API.Artist.GetArtistById(id);
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
                if (Security.Verify(token, email))
                {
                    var artists = API.Artist.GetArtists(email);
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
            if (Security.Verify(model.Token, model.UserEmail))
            {
                try
                {
                    return Request.CreateResponse(HttpStatusCode.Created, API.Artist.CreateArtist(model));
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
            if (Security.Verify(model.Token, model.UserEmail))
            {
                try
                {
                    API.Artist.UpdateArtist(model);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, API.Artist.CreateAlbum(model));
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.UpdateAlbum(model);
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
                if (Security.Verify(token, userEmail, artistId))
                {
                    API.Artist.RemoveAlbum(albumId);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.CreateAlbumImg(model);
                    return Request.CreateResponse(HttpStatusCode.Created);
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
            if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
            {
                try
                {
                    API.Artist.CreateAssociation(model);
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
        public async Task<HttpResponseMessage> CreateTrack(int albumid, int artistId, string email, string token, string trackName)
        {
            try
            {
                if (API.Security.Verify(token, email, artistId))
                {
                    var model = new CreateTrackModel
                    {
                        TrackName = trackName,
                        AlbumId = albumid,
                        ArtistId = artistId,
                        UserEmail = email
                    };
                    model.Track = await Request.Content.ReadAsByteArrayAsync();
                    var id = API.Artist.CreateSingleTrack(model);
                    return Request.CreateResponse(HttpStatusCode.Created, id);
                }
                
                //API.Artist.CreateSingleTrack();
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
                if (Security.Verify(token, userEmail, artistId))
                {
                    API.Artist.RemoveTrack(trackId);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.CreateArtistImg(model);
                    return Request.CreateResponse(HttpStatusCode.Created);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    return Request.CreateResponse(HttpStatusCode.Created, API.Artist.CreateEvent(model));
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
                if (Security.Verify(token, userEmail, artistId))
                {
                    API.Artist.RemoveEvent(eventId);
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
                return Request.CreateResponse(HttpStatusCode.OK, API.Artist.GetCommonData());
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
                return Request.CreateResponse(HttpStatusCode.OK, API.Artist.SearchMarkets(criteria, value));
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
                return Request.CreateResponse(HttpStatusCode.OK, API.Artist.GetGenres());
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.AddGenre(model.ArtistId, model.GenreId);
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
                if (Security.Verify(token, userEmail, artistId))
                {
                    API.Artist.RemoveGenre(artistId, genreId);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.CreateSetting(model);
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
                if (Security.Verify(model.Token, model.UserEmail, model.ArtistId))
                {
                    API.Artist.UpdateSetting(model);
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
