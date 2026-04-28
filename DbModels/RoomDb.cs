using System;

namespace DbModels
{
    public class RoomDb
    {
        public Guid Id { get; set; }
        public string FullNumber { get; set; }
        public int Floor { get; set; }
        public Guid BuildingId { get; set; }
        public BuildingDb Building { get; set; } = null!;
        public RoomDb(Guid id, string fullNumber, int floor, Guid buildingId)
        {
            Id = id;
            FullNumber = fullNumber;
            Floor = floor;
            BuildingId = buildingId;
        }
    }
}