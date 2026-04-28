using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbModels.Converters
{
    public static class RoomConverter
    {
        public static Room ToDomain(this RoomDb db)
        {
            return new Room(db.Id, db.FullNumber, db.Floor, db.BuildingId);
        }

        public static RoomDb ToDb(this Room domain)
        {
            return new RoomDb(domain.Id, domain.FullNumber, domain.Floor, domain.BuildingId);
        }
    }
}
