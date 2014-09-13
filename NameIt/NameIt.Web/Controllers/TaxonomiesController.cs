using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NameIt.Domain.Services;

namespace NameIt.Web.Controllers
{
    public class TaxonomiesController : ApiController
    {
        public IHttpActionResult Get()
        {
            var service = new TaxonomyService();
            return Ok(service.GetAll());
        }
    }
}
