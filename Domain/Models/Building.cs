namespace Domain.Models;

public class Building
{
    public Guid Id { get; }
    public string Letter { get; }

    public Building(Guid id, string letter)
    {
        Id = id;
        Letter = letter;
    }
}