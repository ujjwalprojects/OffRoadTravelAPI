using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.ViewModels
{
    public class ItineraryView
    {
        public long ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string ItineraryDesc { get; set; }
        public long OvernightDestinationID { get; set; }
    }

    public class ItineraryVM
    {
        public IEnumerable<ItineraryView> ItinenaryList { get; set; }
        public int TotalRecords { get; set; }
    }
}
