using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsLendDB.BLL.DTOs;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;
using SportsLendDB.DAL.Repositories;
using SportsLendDB.DAL.Repositories.Interfaces;

namespace SportsLendDB.BLL.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<Equipment> _equipmentRepository;

        public EquipmentService(ILogger<EquipmentService> logger)
        {
            _logger = logger;
            _equipmentRepository ??= new GenericRepository<Equipment>();
        }

        public async Task<(List<Equipment> List, int TotalCount)> GetEquipmentsAsync(int pageIndex, int pageSize, string? searchTerm)
        {
            try
            {
                _logger.LogInformation($"Fetching equipments with search term: {searchTerm}");
                var equipments = _equipmentRepository.GetAllAsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    equipments = equipments.Where(e => e.Name.ToLower().Contains(searchTerm.ToLower()) || e.Brand.ToLower().Contains(searchTerm.ToLower()));
                }
                var items = await equipments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalCount = await equipments.CountAsync();

                _logger.LogInformation($"Fetched {items.Count} equipments.");
                return (items, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching equipments.");
                throw;
            }
        }

        public async Task<Equipment?> GetEquipmentByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching equipment with ID: {id}");
                var equipment = await _equipmentRepository.GetByIdAsync(id);
                if (equipment == null)
                {
                    _logger.LogWarning($"Equipment with ID: {id} not found.");
                }
                return equipment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching equipment with ID: {id}");
                throw;
            }
        }

        public async Task<Equipment?> AddEquipmentAsync(CreateEquipmentDto equipmentDto)
        {
            try
            {
                _logger.LogInformation($"Adding new equipment: {equipmentDto.Name}");

                var newSeq = GenerateNewId();

                var equipment = new Equipment
                {
                    EquipmentId = $"EQ{newSeq:D3}",
                    Name = equipmentDto.Name,
                    Brand = equipmentDto.Brand,
                    TypeId = equipmentDto.Type,
                    Condition = equipmentDto.Condition.ToString(),
                    DailyFeeUsd = equipmentDto.DailyFeeUsd,
                    InStock = equipmentDto.InStock
                };

                _logger.LogInformation($"Generated EquipmentId: {equipment.EquipmentId}");

                await _equipmentRepository.CreateAsync(equipment);

                _logger.LogInformation("Equipment added successfully.");
                return equipment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new equipment.");
                return null;
            }
        }

        public async Task<bool> UpdateEquipmentAsync(Equipment equipment)
        {
            try
            {
                _logger.LogInformation($"Updating equipment with ID: {equipment.Id}");
                await _equipmentRepository.UpdateAsync(equipment);
                _logger.LogInformation("Equipment updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating equipment with ID: {equipment.Id}");
                return false;
            }
        }

        public async Task<Equipment?> CheckLoanAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Checking loan status for equipment with ID: {id}");
                var equipment = await _equipmentRepository.GetAllAsQueryable()
                    .Where(i => i.Id == id)
                    .Include(e => e.Loans)
                    .FirstOrDefaultAsync();

                if (equipment!.Loans != null && equipment.Loans.Any())
                {
                    _logger.LogInformation($"Equipment with ID: {id} is currently on loan.");
                    return null;
                }

                return equipment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking loan status for equipment with ID: {id}");
                return null;
            }
        }

        public async Task<bool> DeleteEquipmentAsync(Equipment equipment)
        {
            try
            {
                _logger.LogInformation($"Deleting equipment with ID: {equipment.Id}");
                var item = await _equipmentRepository.GetByIdAsync(equipment.Id);

                await _equipmentRepository.RemoveAsync(item);
                _logger.LogInformation("Equipment deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting equipment with ID: {equipment.Id}");
                return false;
            }
        }

        public int GenerateNewId()
        {
            try
            {
                _logger.LogInformation("Generating new equipment ID.");
                var equipments = _equipmentRepository.GetAllAsQueryable();
                if (!equipments.Any())
                {
                    return 1;
                }
                var maxId = equipments.Max(e => e.Id);
                var newId = maxId + 1;
                _logger.LogInformation($"Generated new equipment ID: {newId}");
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating new equipment ID.");
                throw;
            }
        }
    }
}
