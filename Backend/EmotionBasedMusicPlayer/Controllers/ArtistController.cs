using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Filters;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Controllers
{
    [AuthenticationFilter]
    [RoutePrefix("artists")]
    public class ArtistController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]Artist artist)
        {
            BusinessContext.ArtistBusiness.Insert(artist);
        }

        [HttpPut]
        [Route("")]
        public void Update([FromBody]Artist artist)
        {
            BusinessContext.ArtistBusiness.Update(artist);
        }

        [HttpDelete]
        [Route("id/{artistID}")]
        public void Delete(string artistID)
        {
            BusinessContext.ArtistBusiness.Delete(artistID);
        }

        [HttpDelete]
        [Route("name/{name}")]
        public void DeleteByName(string name)
        {
            BusinessContext.ArtistBusiness.DeleteByName(name);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Artist> ReadAll()
        {
            return BusinessContext.ArtistBusiness.ReadAll();
        }

        [HttpGet]
        [Route("id/{artistID}")]
        public Artist ReadByID(string artistID)
        {
            return BusinessContext.ArtistBusiness.ReadByID(artistID);
        }

        [HttpGet]
        [Route("name/{name}")]
        public Artist ReadByName(string name)
        {
            return BusinessContext.ArtistBusiness.ReadByName(name);
        }
        #endregion
    }
}
