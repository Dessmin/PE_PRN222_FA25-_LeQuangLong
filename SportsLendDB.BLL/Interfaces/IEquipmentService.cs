using SportsLendDB.BLL.DTOs;
using SportsLendDB.BO.Models;

namespace SportsLendDB.BLL.Interfaces
{
    public interface IEquipmentService
    {
        Task<(List<Equipment> List, int TotalCount)> GetEquipmentsAsync(int pageIndex, int pageSize, string? searchTerm);
        Task<Equipment?> GetEquipmentByIdAsync(int id);
        Task<bool> AddEquipmentAsync(CreateEquipmentDto equipmentDto);
        Task<bool> UpdateEquipmentAsync(Equipment equipment);
        Task<Equipment?> CheckLoanAsync(int id);
        Task<bool> DeleteEquipmentAsync(Equipment equipment);
    }
}
