using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourTravels.Entities.ViewModels
{
    public class HotelView
    {
        public long HotelID { get; set; }
        public long DestinationID { get; set; }
        public string DestinationName { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelContact { get; set; }
        public string HotelEmail { get; set; }
        public string HotelTypes { get; set; }
    }
    public class HotelVM
    {
        public IEnumerable<HotelView> Hotels { get; set; }
        public int TotalRecords { get; set; }
    }
    public class HotelSaveModel
    {
        public long HotelID { get; set; }
        [Required]
        public long DestinationID { get; set; }
        [Required]
        public string HotelName { get; set; }
        [Required]
        public string HotelAddress { get; set; }
        [Required]
        public string HotelContact { get; set; }
        public string HotelEmail { get; set; }
        [Required]
        public List<long> HotelTypes { get; set; }
    }
    public class IDModel
    {
        public long ID { get; set; }
    }
}
