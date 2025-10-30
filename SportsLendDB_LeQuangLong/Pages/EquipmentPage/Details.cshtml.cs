using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;

namespace SportsLendDB_LeQuangLong.Pages.EquipmentPage
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;

        public DetailsModel(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public Equipment Equipment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);
            if (equipment == null) return NotFound();
            Equipment = equipment;
            return Page();
        }
    }
}
