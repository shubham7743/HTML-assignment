using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // DBContext object to access database
        private ProductDBContext db = new ProductDBContext();

        // Log4Net object to log exceptions in file
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProductController));

        // List objects for categories and quantities
        List<string> categories = new List<string> { "Commodity", "Common", "Consumer", "Digital", "Luxury" };
        List<int> quantities = new List<int> { 1, 2, 3, 4 , 5 ,6, 7, 8, 9, 10 };

        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.categories = categories;
            ViewBag.quantities = quantities;
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Category,Price,Quantity,ShortDescription,LongDescription,SmallImage,LargeImage")] Product product)
        {
            try
            {
                // Folder where images will be stored
                string imagePath = "~/Images";
                
                // Path to store small image
                string smallImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_small_image_" + Path.GetFileName(product.SmallImage.FileName);
                product.SmallImagePath = Path.Combine(imagePath, smallImageName);

                // Path to store large image
                if (product.LargeImage != null)
                {
                    string largeImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_large_image_" + Path.GetFileName(product.LargeImage.FileName);
                    product.LargeImagePath = Path.Combine(imagePath, largeImageName);
                }
                else
                {
                    product.LargeImagePath = "N/A";
                }

                // Add record in database and upload images
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    product.SmallImage.SaveAs(Server.MapPath(product.SmallImagePath));
                    if (product.LargeImage != null)
                    {
                        product.LargeImage.SaveAs(Server.MapPath(product.LargeImagePath));
                    }
                    db.SaveChanges();
                    TempData["message"] = "created";
                    TempData["data"] = product.Name;
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }

            ViewBag.categories = categories;
            ViewBag.quantities = quantities;
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            Session["SmallImage"] = product.SmallImagePath;
            Session["LargeImage"] = product.LargeImagePath;
            ViewBag.categories = categories;
            ViewBag.quantities = quantities;
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Category,Price,Quantity,ShortDescription,LongDescription,SmallImage,LargeImage")] Product product)
        {
            try
            {
                // Folder where images are stored
                string imagePath = "~/Images";

                // Modify and record in database and images in folder
                if (ModelState.IsValid)
                {
                    // Both small and large images are edited
                    if (product.SmallImage != null && product.LargeImage != null)
                    {
                        // Path to store large and small images
                        string smallImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_small_image_" + Path.GetFileName(product.SmallImage.FileName);
                        string largeImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_large_image_" + Path.GetFileName(product.LargeImage.FileName);
                        product.SmallImagePath = Path.Combine(imagePath, smallImageName);
                        product.LargeImagePath = Path.Combine(imagePath, largeImageName);

                        // Upload new images
                        product.SmallImage.SaveAs(Server.MapPath(product.SmallImagePath));
                        product.LargeImage.SaveAs(Server.MapPath(product.LargeImagePath));

                        //Delete old images
                        if (System.IO.File.Exists(Server.MapPath(Session["SmallImage"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath(Session["SmallImage"].ToString()));
                        }
                        if (System.IO.File.Exists(Server.MapPath(Session["LargeImage"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath(Session["LargeImage"].ToString()));
                        }
                    }

                    // Only large image is edited
                    else if (product.SmallImage == null && product.LargeImage != null)
                    {
                        // Path to store large and small image
                        product.SmallImagePath = Session["SmallImage"].ToString();
                        string largeImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_large_image_" + Path.GetFileName(product.LargeImage.FileName);
                        product.LargeImagePath = Path.Combine(imagePath, largeImageName);

                        // Upload new large image
                        product.LargeImage.SaveAs(Server.MapPath(product.LargeImagePath));

                        // Delete old large image
                        if (System.IO.File.Exists(Server.MapPath(Session["LargeImage"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath(Session["LargeImage"].ToString()));
                        }
                    }

                    // Only small image is edited
                    else if (product.SmallImage != null && product.LargeImage == null)
                    {
                        // Path to store small and large image
                        string smallImageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_small_image_" + Path.GetFileName(product.SmallImage.FileName);
                        product.SmallImagePath = Path.Combine(imagePath, smallImageName);
                        product.LargeImagePath = Session["LargeImage"].ToString();

                        //Upload new small image
                        product.SmallImage.SaveAs(Server.MapPath(product.SmallImagePath));

                        //Delete old small image
                        if (System.IO.File.Exists(Server.MapPath(Session["SmallImage"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath(Session["SmallImage"].ToString()));
                        }
                    }

                    // No images are edited
                    else
                    {
                        // Path to store small and large image
                        product.SmallImagePath = Session["SmallImage"].ToString();
                        product.LargeImagePath = Session["LargeImage"].ToString();
                    }

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "edited";
                    TempData["data"] = product.Name;
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
            ViewBag.categories = categories;
            ViewBag.quantities = quantities;
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Delete record and images from folder
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                if (System.IO.File.Exists(Server.MapPath(product.SmallImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath(product.SmallImagePath));
                }
                if (System.IO.File.Exists(Server.MapPath(product.LargeImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath(product.LargeImagePath));
                }
                TempData["message"] = "deleted";
                TempData["data"] = product.Name;
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(IEnumerable<int> allId)
        {
            try
            {
                // Get all products to be deleted
                IEnumerable<Product> products = db.Products.Where(x => allId.Contains(x.ID)).ToList();

                // Variable to keep count of products deleted
                int count = 0;

                // Delete record of each product and respective images from folder
                foreach (Product product in products)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    if (System.IO.File.Exists(Server.MapPath(product.SmallImagePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(product.SmallImagePath));
                    }
                    if (System.IO.File.Exists(Server.MapPath(product.LargeImagePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(product.LargeImagePath));
                    }
                    count++;
                }
                TempData["message"] = "deleted_multiple";
                TempData["data"] = count;
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return RedirectToAction("Index");
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
