using MarinaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InlandMarinaApp.Controllers
{
    public class SlipsController : Controller
    {
        private InlandMarinaContext _context { get; set; }

        public SlipsController(InlandMarinaContext context)
        {
            _context = context;
        }

        // GET: SlipsController
        public ActionResult Index()
        {
            try
            {
                // prepare the docks list for the selected element (dropdown)
                List<Dock> docks = MarinaManager.GetDocks(_context);
                var list = new SelectList(docks, "ID", "Name").ToList(); // to add "All" option. otherwise we'd have SelectList
                list.Insert(0, new SelectListItem("All Docks", "0")); // 0 as the value for All.
                ViewBag.Docks = list; //define viewbag

                return View(); // no data passed to the view because this will call the view component to get the data (AJAX)
            } catch
            {
                TempData["Message"] = $"Something went wrong with populating Slips. Try again later.";
                TempData["IsError"] = "True";
                return RedirectToAction("Index", "Home");
            }
        }

        // method to be called from AJAX function
        public ActionResult GetUnleasedSlips(int id) // dock id
        {
            // invoke default view component
            return ViewComponent("UnleasedSlipsByDock", id);
        }


        [Authorize]
        public ActionResult Lease()
        {
            try
            {
                List<Slip> slips = MarinaManager.GetUnleasedSlips(_context);
                var list = new SelectList(slips, "ID", "ID");
                ViewBag.Slips = list;
                int? customerID = HttpContext.Session.GetInt32("CurrentCustomer")!;
                if (customerID != null)
                {
                    ViewBag.CustomerID = customerID;
                }
                else
                {
                    ViewBag.CustomerID = 0; // dummy
                }
                return View(new Lease());
            }
            catch
            {
                TempData["Message"] = $"Something went wrong with populating Lease page. Try later.";
                TempData["IsError"] = "True";
                return RedirectToAction("Index", "Home");
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lease(int id, Lease newLeaseData)
        {
                if (ModelState.IsValid) // check the validation attributes in the Lease class
                {
                        MarinaManager.AddLease(_context, newLeaseData);
                        TempData["Message"] = $"Successfully added Lease for Slip #{newLeaseData.SlipID}";
                    // no need to set up IsError
                    return RedirectToAction(nameof(MySlips));
                }
                else
                {
                    TempData["Message"] = $"Something went wrong with adding lease. Try later.";
                    TempData["IsError"] = "True";
                    return View("Lease", newLeaseData); // if errors, stay on the same page with content
                }   
        }
        [Authorize]
        public ActionResult MySlips()
        {
            // get the cookie from the logged in user. 
            int? customerId = HttpContext.Session.GetInt32("CurrentCustomer");
            // prepare the list of leases by customer id with the cookie
            List<Slip> slips = MarinaManager.GetLeasedSlipsByCustomerId(_context, customerId);

            return View(slips);
        }
    }
}
