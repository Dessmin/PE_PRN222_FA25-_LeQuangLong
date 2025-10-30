using Microsoft.Extensions.Logging;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;
using SportsLendDB.DAL.Repositories;
using SportsLendDB.DAL.Repositories.Interfaces;

namespace SportsLendDB.BLL.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<EquipmentType> _equipmentTypeRepository;

        public EquipmentTypeService(ILogger<EquipmentTypeService> logger)
        {
            _logger = logger;
            _equipmentTypeRepository ??= new GenericRepository<EquipmentType>();
        }

        public async Task<List<EquipmentType>> GetEquipmentTypesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all equipment types.");
                var equipmentTypes = await _equipmentTypeRepository.GetAllAsync();
                _logger.LogInformation($"Fetched {equipmentTypes.Count} equipment types.");
                return equipmentTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching equipment types.");
                throw;
            }
        }
    }
}
