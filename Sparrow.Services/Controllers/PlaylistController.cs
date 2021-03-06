﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Sparrow.Services.API;
using Sparrow.Services.Models;

namespace Sparrow.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlaylistController : ApiController
    {
        private readonly Artist _artist;
        private readonly Playlist _playlist;

        public PlaylistController()
        {
            _artist = new Artist();
            _playlist = new Playlist();
        }
        public PlaylistController(Artist artist, Playlist playlist)
        {
            _artist = artist;
            _playlist = playlist;
        }

        [HttpPost]
        [ActionName("Build")]
        public HttpResponseMessage BuildPlaylist()
        {
            var memory = MemoryCache.Default;
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        [ActionName("Discover")]
        public HttpResponseMessage GetPlaylist(int? page, int? playlistId)
        {
            var memory = MemoryCache.Default;
            try
            {
                if (page == null || playlistId == null)
                {
                    var playlistMetaData = _playlist.GetPlaylistMetaData();
                    return Request.CreateResponse(HttpStatusCode.OK, playlistMetaData);
                }
                else
                {
                    var playlist = _playlist.GetPlaylist((int)page, (int)playlistId);
                    var playlistModel = JsonConvert.DeserializeObject<List<PlaylistTrack>>(playlist);
                    return Request.CreateResponse(HttpStatusCode.OK, playlistModel);
                }
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
                return Request.CreateResponse(HttpStatusCode.OK, _playlist.SearchByName(name));
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
                return Request.CreateResponse(HttpStatusCode.OK, _artist.GetFile("audio", artistId, albumId, trackId));
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
                return Request.CreateResponse(HttpStatusCode.OK, _artist.GetFile("image", artistId, albumId, trackId));
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
                _artist.ModifyTrackPopularity(model);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ActionName("Generate")]
        public HttpResponseMessage GeneratePlaylist(string id)
        {
            try
            {
                if (id == "Yy9Fe19RulARt2vIHuMR")
                {
                    _playlist.CreatePlaylist();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
