using MarinaData;
using Microsoft.AspNetCore.Mvc;
namespace MoviesMVC.Components
{
    public class UnleasedSlipsByDockViewComponent : ViewComponent
    {
        private InlandMarinaContext _context { get; set; }

        // constructor with parameter - context object injected into the controller
        public UnleasedSlipsByDockViewComponent(InlandMarinaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id) //dock id
        {
            List<Slip> slips = null;
            if (id == 0) // code for all
            {
                slips = MarinaManager.GetUnleasedSlips(_context);
            }
            else
            {
                slips = MarinaManager.GetUnleasedSlipsByDock(_context, id); // filter accordingly
            }
            return View(slips); // the view for the view component is called Default.cshtml
        }
    }
}
