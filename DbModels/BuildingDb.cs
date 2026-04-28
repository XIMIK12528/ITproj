using System;
using System.Collections.Generic;

namespace DbModels
{
    public class BuildingDb
    {
        public Guid Id { get; set; }
        public string Letter { get; set; }
        public List<RoomDb> Rooms { get; set; } = new();
        public BuildingDb(Guid id, string letter)
        {
            Id = id;
            Letter = letter;
        }
    }
}