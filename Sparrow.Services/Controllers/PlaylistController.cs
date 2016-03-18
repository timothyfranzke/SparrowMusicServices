﻿using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Http.Cors;
using Sparrow.Services.API;
using Sparrow.Services.Models;

namespace Sparrow.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlaylistController : ApiController
    {
        [HttpPost]
        [ActionName("Build")]
        public HttpResponseMessage BuildPlaylist()
        {
            var memory = MemoryCache.Default;
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        [ActionName("Discover")]
        public HttpResponseMessage GetPlaylist(int page)
        {
            var memory = MemoryCache.Default;
            try
            {
                if (!memory.Contains("playlist"))
                {
                    var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
                    var playlist = API.Playlist.GetPlaylist(page);
                    
                    memory.Add("playlist", playlist, expiration);
                    return Request.CreateResponse(HttpStatusCode.OK, playlist);
                }
                return Request.CreateResponse(HttpStatusCode.OK, memory.Get("playlist"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Search")]
        public HttpResponseMessage Search(string name)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, API.Playlist.SearchByName(name));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Audio")]
        public HttpResponseMessage AudioFile(int artistId, int? albumId, int trackId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Artist.GetFile("audio", artistId, albumId, trackId));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Image")]
        public HttpResponseMessage ImageFile(int artistId, int albumId, int trackId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Artist.GetFile("image", artistId, albumId, trackId));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("Popular")]
        public HttpResponseMessage TrackPopularity([FromBody]PopularityModel model)
        {
            try
            {
                API.Artist.ModifyTrackPopularity(model);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}