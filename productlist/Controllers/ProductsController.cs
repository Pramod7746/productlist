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
    public class ProductsController : Controller
    {
        private machinetestEntities db = new machinetestEntities();

        // GET: Products
        public async Task<ActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of products per page
            var products = await db.Products
                                    .Include(p => p.Category)
                                    .OrderBy(p => p.ProductId)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = Math.Ceiling((double)await db.Products.CountAsync() / pageSize);

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = await db.Products.FindAsync(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // GET: Products/Create
        // GET: Products/Create
        // GET: Products/Create
        public ActionResult Create()
        {
            // Get all categories from the database
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductName,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If validation fails, repopulate the categories for the dropdown
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }




        // GET: Products/Edit/5
        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = await db.Products.FindAsync(id);

            if (product == null)
                return HttpNotFound();

            // Populate ViewBag.CategoryId with a SelectList
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }


        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ProductName,CategoryId")] Product product)
        {
            if (!db.Categories.Any(c => c.CategoryId == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Invalid category selected.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            // Repopulate ViewBag.CategoryId
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = await db.Products.FindAsync(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);

            if (product == null)
                return HttpNotFound();

            try
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(product);
            }
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
