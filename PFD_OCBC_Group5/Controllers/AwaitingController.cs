using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFD_OCBC_Group5.Controllers
{
    public class AwaitingController : Controller
    {
        // GET: AwaitingController
        public ActionResult Index()
        {
            return View();
        }
    }
}
