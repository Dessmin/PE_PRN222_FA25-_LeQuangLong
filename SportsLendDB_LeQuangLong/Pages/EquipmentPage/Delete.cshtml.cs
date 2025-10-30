using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;

namespace SportsLendDB_LeQuangLong.Pages.EquipmentPage
{
    [Authorize(Roles = "Manager")]
    public class DeleteModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;

        public DeleteModel(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [BindProperty]
        public Equipment Equipment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);

            if (equipment == null) return NotFound();

            Equipment = equipment;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);

            if (equipment == null) return NotFound();

            var check = await _equipmentService.CheckLoanAsync(equipment.Id);
            if (check == null)
            {
                TempData["Message"] = "Cannot delete: equipment is currently on loan.";
                return Page();
            }

            var deleted = await _equipmentService.DeleteEquipmentAsync(equipment);
            if (!deleted)
            {
                TempData["Message"] = "Delete failed.";
            }
            return RedirectToPage("/EquipmentPage/Index");
        }
    }
}
