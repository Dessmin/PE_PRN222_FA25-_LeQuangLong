using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;

namespace SportsLendDB_LeQuangLong.Pages.EquipmentPage
{
    [Authorize(Roles = "Manager")]
    public class EditModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IEquipmentTypeService _equipmentTypeService;

        public EditModel(IEquipmentService equipmentService, IEquipmentTypeService equipmentTypeService)
        {
            _equipmentService = equipmentService;
            _equipmentTypeService = equipmentTypeService;
        }

        [BindProperty]
        public Equipment Equipment { get; set; } = new Equipment();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);
            if (equipment == null) return NotFound();
            Equipment = equipment;
            ViewData["TypeId"] = new SelectList(await _equipmentTypeService.GetEquipmentTypesAsync(), "TypeId", "TypeName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["TypeId"] = new SelectList(await _equipmentTypeService.GetEquipmentTypesAsync(), "TypeId", "TypeName");
                return Page();
            }

            if (User.IsInRole("Staff"))
            {
                var existing = await _equipmentService.GetEquipmentByIdAsync(Equipment.Id);
                if (existing != null)
                {
                    Equipment.DailyFeeUsd = existing.DailyFeeUsd;
                }
            }

            var success = await _equipmentService.UpdateEquipmentAsync(Equipment);
            if (success)
            {
                TempData["Message"] = "Equipment updated successfully.";
                return RedirectToPage("/EquipmentPage/Index");
            }
            ModelState.AddModelError(string.Empty, "Update failed.");
            ViewData["TypeId"] = new SelectList(await _equipmentTypeService.GetEquipmentTypesAsync(), "TypeId", "TypeName");
            return Page();
        }
    }
}
