using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using productlist.Models;

namespace productlist.Controllers
{
    public class CategoriesController : Controller
    {
        private machinetestEntities db = new machinetestEntities();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var categories = await db.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category category = await db.Categories.FindAsync(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);

                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while creating the category: {ex.Message}");
                }
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Fetch the category, not the product
            Category category = await db.Categories.FindAsync(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }



        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while updating the category: {ex.Message}");
                }
            }

            return View(category);
        }


        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Category category = await db.Categories.FindAsync(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);

            if (category == null)
                return HttpNotFound();

            try
            {
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the category: {ex.Message}");
                return View(category);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
