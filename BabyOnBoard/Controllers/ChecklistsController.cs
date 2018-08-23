using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyOnBoard.Models;

namespace BabyOnBoard.Controllers
{
    public class ChecklistsController : Controller
    {
        private BabyOnBoardDataEntities db = new BabyOnBoardDataEntities();

        //GET: Index action method and associated action methods
        public ActionResult Index(string productType, string searchName, string sortOrder)
        {
            //get data for drop-down list of products in Index view
            List<string> productList = new List<string>();

            //get types from db
            var productQuery = from p in db.Checklists
                             orderby p.Type
                             select p.Type;

            //put unique types in a list and put the list in ViewBag
            productList.AddRange(productQuery.Distinct());
            ViewBag.ProductType = new SelectList(productList);

            //sorting parameters
            //put opposite sort value to the current parameter in the ViewBag, so that
            //the opposite kind of sort is made available on return to the Index view
            //the default sort type is names ascending, so this doesn't need to be specified
            ViewBag.NameSortParameter = String.IsNullOrEmpty(sortOrder) ? "nameDescending" : "";

            //make sure that the first sort on likes is descending order
            if ((sortOrder == null) || (sortOrder == "nameDescending") || (sortOrder == "nameAscending"))
            {
                ViewBag.LikesSortParameter = "likesDescending";
            }
            else if (sortOrder == "likesAscending")
            {
                ViewBag.LikesSortParameter = "likesDescending";
            }
            else
            {
                ViewBag.LikesSortParameter = "likesAscending";
            }

            //get all records from db
            var product = from p in db.Checklists
                            select p;

            //filter list of records according to search criteria
            if (!String.IsNullOrEmpty(searchName))
            {
                product = product.Where(n => n.Name.ToUpper().Contains(searchName.ToUpper()));
            }
            if (!String.IsNullOrEmpty(productType))
            {
                product = product.Where(t => t.Type == productType);
            }

            //sort records according to sort criterion
            switch (sortOrder)
            {
                case "nameDescending":
                    product = product.OrderByDescending(p => p.Name);
                    break;
                case "likesAscending":
                    product = product.OrderBy(p => p.Likes);
                    break;
                case "likesDescending":
                    product = product.OrderByDescending(p => p.Likes);
                    break;
                default:
                    product = product.OrderBy(p => p.Name);
                    break;
            }

            return View(product);
        }

        // GET: Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist checklist = db.Checklists.Find(id);

            if (checklist == null)
            {
                return HttpNotFound();
            }

            return View(checklist);

        }

        // GET: Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChecklistId,Picture,Name,Type,Brand,Condition,Price,Description,Likes,Dislikes")] Checklist checklist)
        {
            //set a default value for picture if it's null
            if (checklist.Picture == null)
            {
                checklist.Picture = "\\Content\\Images\\baby.jpg";
            }

            if (ModelState.IsValid)
            {
                db.Checklists.Add(checklist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checklist);

        }

        // GET: Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist checklist = db.Checklists.Find(id);

            if (checklist == null)
            {
                return HttpNotFound();
            }

            return View(checklist);

        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChecklistId,Picture,Name,Type,Brand,Condition,Price,Description,Likes,Dislikes")] Checklist checklist)
        {
            //set a default value for picture if it's null
            if (checklist.Picture == null)
            {
                checklist.Picture = "\\Content\\Images\\baby.jpg";
            }

            if (ModelState.IsValid)
            {
                db.Entry(checklist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checklist);

        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist checklist = db.Checklists.Find(id);

            if (checklist == null)
            {
                return HttpNotFound();
            }

            return View(checklist);

        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Checklist checklist = db.Checklists.Find(id);
            db.Checklists.Remove(checklist);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //for releasing resources that are finished with
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //used by Index view to refresh rows with AJAX when likes or dislikes updated
        public ActionResult _TableRow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist product = db.Checklists.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return PartialView(product);

        }

        //used by Index view to update likes when likes button is clicked
        [HttpPost]
        public ActionResult _Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist product = db.Checklists.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.Likes++;

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }

            product = db.Checklists.Find(id);

            return PartialView("_TableRow", product);

        }

        //used by Index view to update dislikes when dislikes button is clicked
        [HttpPost]
        public ActionResult _Dislike(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checklist product = db.Checklists.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.Dislikes++;

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }

            product = db.Checklists.Find(id);

            return PartialView("_TableRow", product);

        }

    }
}
