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


        public ActionResult Details(int id)
        {
            var service = CreateService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateService();
            service.DeleteNote(id);
            TempData["SaveResult"] = "Your note was deleted";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var service = CreateService();
            var detail = service.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (ModelState.IsValid)
            {
                if (model.NoteId != id)
                {
                    ModelState.AddModelError("", "ID Mismatch");
                    return View(model);
                }

                var service = CreateService();

                if (service.UpdateNote(model))
                {
                    TempData["SaveResult"] = "Your note was updated";
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("","Your note could not be updated");
            return View(model);
        }
    }
}