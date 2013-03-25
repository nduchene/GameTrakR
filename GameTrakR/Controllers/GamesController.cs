using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameTrakR.Code;

namespace GameTrakR.Controllers
{
    public class GamesController : Controller
    {
        public ActionResult Index()
        {
			GamesViewModel _viewModel = new GamesViewModel();
            return View(_viewModel);
        }

		public ActionResult Admin()
		{
			GamesViewModel _viewModel = new GamesViewModel();
			return View(_viewModel);
		}
		
		public ActionResult Groups()
		{
			GamesViewModel _viewModel = new GamesViewModel();
			return View(_viewModel);
		}		
    }
}
