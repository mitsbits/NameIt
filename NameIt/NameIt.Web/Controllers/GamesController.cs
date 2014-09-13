using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NameIt.Domain.Services;

namespace NameIt.Web.Controllers
{
    public class GamesController : ApiController
    {
        public IHttpActionResult Get(int id)
        {
            var service = new GameService(new BlockService(new TaxonomyService()));
            var game = service.GetGame(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

    }
}
