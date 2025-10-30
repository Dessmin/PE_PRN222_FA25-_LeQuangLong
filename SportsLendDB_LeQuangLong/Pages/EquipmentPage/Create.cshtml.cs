using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using SportsLendDB.BLL.DTOs;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB_LeQuangLong.Hubs;

namespace SportsLendDB_LeQuangLong.Pages.EquipmentPage
{
    [Authorize(Roles = "Manager")]
    public class CreateModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IHubContext<SignalRHub> _hubContext;

        public CreateModel(IEquipmentService equipmentService, IEquipmentTypeService equipmentTypeService, IHubContext<SignalRHub> hubContext)
        {
            _equipmentService = equipmentService;
            _equipmentTypeService = equipmentTypeService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Types"] = new SelectList(await _equipmentTypeService.GetEquipmentTypesAsync(), "TypeId", "TypeName");
            return Page();
        }

        [BindProperty]
        public CreateEquipmentDto Input { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {

            if (string.IsNullOrWhiteSpace(Input.Name) || Input.Name.Length < 3 || Input.Name.Length > 50)
            {
                ModelState.AddModelError("Input.Name", "Name must be 3-50 characters.");
            }
            if (string.IsNullOrWhiteSpace(Input.Brand) || Input.Brand.Length < 2 || Input.Brand.Length > 30)
            {
                ModelState.AddModelError("Input.Brand", "Brand must be 2-30 characters.");
            }
            if (Input.DailyFeeUsd < 0.5m || Input.DailyFeeUsd > 200m)
            {
                ModelState.AddModelError("Input.DailyFeeUsd", "Daily fee must be between 0.5 and 200.");
            }
            var allowed = new[] { "New", "Good", "Used", "Damaged" };
            if (!allowed.Contains(Input.Condition.ToString()))
            {
                ModelState.AddModelError("Input.Condition", "Invalid condition.");
            }
            if (Input.InStock < 0 || Input.InStock > 999)
            {
                ModelState.AddModelError("Input.InStock", "InStock must be between 0 and 999.");
            }

            var success = await _equipmentService.AddEquipmentAsync(Input);

            // notify via SignalR if successful
            if (success)
            {
                await _hubContext.Clients.All.SendAsync("EquipmentCreated", Input);

                TempData["Message"] = "New equipment added successfully.";
                return RedirectToPage("/EquipmentPage/Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to add equipment.");
            ViewData["TypeId"] = new SelectList(await _equipmentTypeService.GetEquipmentTypesAsync(), "TypeId", "TypeName");
            return Page();
        }
    }
}
