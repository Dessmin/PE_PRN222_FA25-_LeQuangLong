using SportsLendDB.BO.Models;

namespace SportsLendDB.BLL.Interfaces
{
    public interface IEquipmentTypeService
    {
        Task<List<EquipmentType>> GetEquipmentTypesAsync();
    }
}
