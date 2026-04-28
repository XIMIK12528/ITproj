using DataAccsess.Context;
using DbModels;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using DbModels.Converters;  

namespace Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly Context _context;
        public ScheduleRepository(Context context) => _context = context;

        public async Task<Building?> GetBuildingByLetterAsync(string letter)
        {
            var db = await _context.Buildings.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Letter == letter);
            return db?.ToDomain();
        }

        public async Task AddBuildingAsync(Building building)
        {
            await _context.Buildings.AddAsync(building.ToDb());
        }
        public async Task<bool> RoomExistsAsync(string fullNumber) =>
            await _context.Set<RoomDb>().AnyAsync(r => r.FullNumber == fullNumber);
        public async Task AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room.ToDb());
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}