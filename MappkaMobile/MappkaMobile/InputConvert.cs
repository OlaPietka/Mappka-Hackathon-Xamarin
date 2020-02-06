using MappkaMobile.Enum;
using System.Collections.Generic;

namespace MappkaMobile
{
    public class InputConvert
    {
        private readonly List<string> emergencyRoom = new List<string> { "Nazwa plcówki:", "Rodzaj opieki:", "Stomatologia", "Ortopedia", "Neurologia", "Laryngologia", "Pediatria", "Kardiologia" };
        private readonly List<string> clinic = new List<string> { "Nazwa przychodni:", "Rodzaj opieki:", "Stomatologia", "Ortopedia", "Neurologia", "Laryngologia", "Pediatria", "Kardiologia" };
        private readonly List<string> food = new List<string> { "Nazwa restauracji:", "Rodzaj jedzenia:", "Włoska", "Polska", "Meksykańska", "Azjatycka", "Europejska", "Amerykańska" };
        private readonly List<string> club = new List<string> { "Nazwa klubu:", "Rodzaj muzyki:", "Jazz", "Techno", "Rock", "Pop", "Hiphop", "Blues" };
        private readonly List<string> office = new List<string> { "Nazwa urzędu", "Rodzaj urzędu:", "Pracy", "Skarbowy", "Miasta", "Statystyczny", " ", " " };

        public void SetServiceType(string type)
        {
            switch (type)
            {
                case "Restauracja":
                    Db.DatabaseStrings.DbName = ServiceType.Food;
                    break;
                case "Przychodnia":
                    Db.DatabaseStrings.DbName = ServiceType.Clinic;
                    break;
                case "Klub":
                    Db.DatabaseStrings.DbName = ServiceType.Club;
                    break;
                case "Urząd":
                    Db.DatabaseStrings.DbName = ServiceType.Office;
                    break;
                case "SOR":
                    Db.DatabaseStrings.DbName = ServiceType.EmergencyRoom;
                    break;
            }
        }

        public List<string> GetServiceList(ServiceType type)
        {
            switch (type)
            {
                case ServiceType.Food:
                    return food;
                case ServiceType.Clinic:
                    return clinic;
                case ServiceType.Club:
                    return club;
                case ServiceType.Office:
                    return office;
                case ServiceType.EmergencyRoom:
                    return emergencyRoom;
                default:
                    return null;
            }
        }
    }
}
