using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NameIt.Domain;
using NameIt.Domain.Services;

namespace NameIt.Web.Controllers
{
    public class NameItController : Controller
    {
        public ActionResult Root(int? taxonomy, int? part)
        {
            if (TempData["game"] != null) TempData["game"] = TempData["game"] as Game;

            if ((!taxonomy.HasValue && !part.HasValue))
                return View("Index", Index());
            if (!part.HasValue) part = 0;

            var game = Part(taxonomy.Value, part.Value);

            if (game == null) 
                return RedirectToAction("Root", new{taxonomy = "", part = ""});

            if (part.Value == 0) return RedirectToAction("Root", new {taxonomy, part = 1});

            TempData["game"] = game;

            return View("Part", game);
        }

        private  Game Part(int taxonomy, int part)
        {
            Game game = null;
            if (part == 0)
            {
                var service = new GameService(new BlockService(new TaxonomyService()));
                game = service.GetGame(taxonomy);
                TempData["game"] = game;
            }
            game = TempData["game"] as Game;
            if (game == null || game.SetBucket.Count < part)
               return null;

            return game;

        }

        private IList<Taxonomy> Index()
        {
            var service = new TaxonomyService();
            var model = service.GetAll();
            return model;
        }

    }
}