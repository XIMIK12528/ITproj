using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<Building?> GetBuildingByLetterAsync(string letter);
        Task AddBuildingAsync(Building building);
        Task<bool> RoomExistsAsync(string fullNumber);
        Task AddRoomAsync(Room room);
        Task SaveChangesAsync();
    }
}