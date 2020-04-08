using AzureBlobStorageApp.BlobHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureBlobStorageApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {  
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase httpPostedFileBase)
        {
            foreach (string file in Request.Files)
            {
                httpPostedFileBase = Request.Files[file];
            }
            BlobManager blobManager = new BlobManager("picture");
            string absoluteUri = blobManager.UploadFile(httpPostedFileBase);
            return View();
        }

        public ActionResult Get()
        {
            BlobManager blobManager = new BlobManager("picture");
            List<string> _blobList= blobManager.BlobList();

            return View(_blobList);
        }

        public ActionResult Delete(string absoluteUri)
        {
            BlobManager blobManager = new BlobManager("picture");
            blobManager.DeleteBlob(absoluteUri);

            return RedirectToAction("Get");
        }
    }
}