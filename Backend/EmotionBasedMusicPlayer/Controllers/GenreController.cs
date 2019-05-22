using EmotionBasedMusicPlayer.Core;
using EmotionBasedMusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmotionBasedMusicPlayer.Controllers
{
    //[AuthenticationFilter]
    [RoutePrefix("genres")]
    public class GenreController : MainApiController
    {
        #region Methods
        [HttpPost]
        [Route("")]
        public void Insert([FromBody]Genre genre)
        {
            BusinessContext.GenreBusiness.Insert(genre);
        }

        [HttpPut]
        [Route("")]
        public void Update([FromBody]Genre genre)
        {
            BusinessContext.GenreBusiness.Update(genre);
        }

        [HttpDelete]
        [Route("id/{genreID:Guid}")]
        public void Delete(Guid genreID)
        {
            BusinessContext.GenreBusiness.Delete(genreID);
        }

        [HttpDelete]
        [Route("name/{name}")]
        public void DeleteByName(string name)
        {
            BusinessContext.GenreBusiness.DeleteByName(name);
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Genre> ReadAll()
        {
            return BusinessContext.GenreBusiness.ReadAll();
        }

        [HttpGet]
        [Route("id/{genreID:Guid}")]
        public Genre ReadByID(Guid genreID)
        {
            return BusinessContext.GenreBusiness.ReadByID(genreID);
        }

        [HttpGet]
        [Route("name/{name}")]
        public Genre ReadByName(string name)
        {
            return BusinessContext.GenreBusiness.ReadByName(name);

        }
        #endregion
    }
}
