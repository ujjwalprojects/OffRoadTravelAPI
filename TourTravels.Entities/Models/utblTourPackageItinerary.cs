using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblTourPackageItinerary
    {
        [Key]
        public long PackageItineraryID { get; set; }
        [Required]
        public long PackageID { get; set; }
        [Required]
        public long ItineraryID { get; set; }
        [Required]
        public int DayNo { get; set; }
        [Required]
        public string ItineraryRemarks { get; set; }
        public long? OvernightDestinationID { get; set; }
        public string OvernightDestination { get; set; }
        public bool SightSeeing { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Stay { get; set; }
    }
}
