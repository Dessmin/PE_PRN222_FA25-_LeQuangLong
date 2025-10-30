using Microsoft.AspNetCore.SignalR;
using SportsLendDB.BLL.DTOs;
using SportsLendDB.BLL.Interfaces;
namespace SportsLendDB_LeQuangLong.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly IEquipmentService _equipmentService;

        public SignalRHub(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }
        public async Task HubCreate(CreateEquipmentDto equipmentDto)
        {
            await Clients.All.SendAsync("ReceiveAdd", equipmentDto);
            await _equipmentService.AddEquipmentAsync(equipmentDto);
        }
    }
}
