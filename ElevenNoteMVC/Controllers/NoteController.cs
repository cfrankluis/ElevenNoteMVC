using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElevenNoteData;
using ElevenNoteModels;
using ElevenNoteServices;
using Microsoft.AspNet.Identity;

namespace ElevenNoteMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private NoteService CreateService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }

        // GET: Note
        public ActionResult Index()
        {
            var service = CreateService();
            var model = service.GetNotes();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if(ModelState.IsValid)
            {
                var service = CreateService();
                if (service.CreateNote(model))
                {
                    TempData["SaveResult"] = "Your note was created.";
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("","Note could not be created.");

            return View(model);
        }


    }
}