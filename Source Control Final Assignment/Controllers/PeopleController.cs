using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Source_Control_Final_Assignment.Models;

namespace Source_Control_Final_Assignment.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private PersonDBContext db = new PersonDBContext();
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PeopleController));
        // GET: People
        public ActionResult Index()
        {
            return View(db.Persons.ToList());
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Age,Phone,About,ProfilePhoto")] Person person)
        {
            try
            {
                int y = 0;
                int x = 5 / y; //This statement is to generate exception
                string imagePath = "~/Images";
                string imageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(person.ProfilePhoto.FileName);
                person.ProfileImagePath = Path.Combine(imagePath, imageName);
                if (ModelState.IsValid)
                {
                    db.Persons.Add(person);
                    person.ProfilePhoto.SaveAs(Server.MapPath(person.ProfileImagePath));
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return View(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
