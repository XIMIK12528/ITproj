namespace Domain.Models;

public class Room
{
    public Guid Id { get; }
    public string FullNumber { get; }
    public int Floor { get; }
    public Guid BuildingId { get; }

    public Room(Guid id, string fullNumber, int floor, Guid buildingId)
    {
        Id = id;
        FullNumber = fullNumber;
        Floor = floor;
        BuildingId = buildingId;
    }
}