using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sparrow.Services.Test
{
    [TestClass]
    public class APITests
    {
        [TestMethod]
        public void Playlist_GetPlaylistPages_Success()
        {
            //API.Playlist.GetPlaylist()
        }

        [TestMethod]
        public void ForgotPassword_Success()
        {
            API.User.ResetPassword("timothyfranzke@gmail.com");
        }
    }
}
