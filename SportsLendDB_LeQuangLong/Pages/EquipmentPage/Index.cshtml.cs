using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;

namespace SportsLendDB_LeQuangLong.Pages.EquipmentPage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IEquipmentService _equipmentService;

        public IndexModel(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public IList<Equipment> Equipment { get; set; } = new List<Equipment>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        private const int PageSize = 4;
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            PageIndex = pageIndex ?? 1;

            var (items, totalCount) = await _equipmentService.GetEquipmentsAsync(PageIndex, PageSize, SearchString);

            Equipment = items;
            TotalCount = totalCount;
            return Page();
        }
    }
}
