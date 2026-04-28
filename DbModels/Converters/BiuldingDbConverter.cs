using Domain.Models;
using DbModels;

namespace DbModels.Converters
{

    public static class ScheduleConverter
    {
        public static Building ToDomain(this BuildingDb db)
        {
            return new Building(db.Id, db.Letter);
        }

        public static BuildingDb ToDb(this Building domain)
        {
            return new BuildingDb(domain.Id, domain.Letter);
        }

    }
}