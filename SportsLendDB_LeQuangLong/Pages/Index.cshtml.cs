using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SportsLendDB_LeQuangLong.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Authentication/Login");
            }
            // Redirect to actual Equipment list page
            return RedirectToPage("/EquipmentPage/Index");
        }
    }
}
