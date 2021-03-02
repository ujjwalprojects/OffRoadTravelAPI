using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;

namespace TourTravels.Entities.ViewModels
{
   
    public class ClientEnquiryManageModel
    {
        public utblClientEnquirie utblClientEnquirie { get; set; }
        public utblClientEnquiryItinerarie utblClientItinerarie { get; set; }
    }
    public class ClientEnqActivities
    {
        public long ClientActivityID { get; set; }
        public string EnquiryCode { get; set; }
        public long RefPackageID { get; set; }
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDesc { get; set; }
        public decimal ActivityFare { get; set; }
        public bool IsSelected { get; set; }
    }
    //model for final view of client custom package summary
    public class ClientEnquiryView
    {
        public string EnquiryCode { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNo { get; set; }
        public long RefPackageID { get; set; }
        public int NoOfDays { get; set; }
        public DateTime DateOfArrival { get; set; }
        public string NoOfAdult { get; set; }
        public string NoOfChildren { get; set; }
        public string HotelTypeName { get; set; }
        public string CabTypeName { get; set; }
    }
    public class ClientEnquiryItineraryView
    {
        public long ClientItineraryID { get; set; }
        public string EnquiryCode { get; set; }
        public int DayNo { get; set; }
        public long ItineraryID { get; set; }
        public string ItineraryName { get; set; }
        public string ItineraryRemarks { get; set; }
        public long OvernightDestinationID { get; set; }
        public string DestinationName { get; set; }
        public string OvernightDestination { get; set; }
        public bool SightSeeing { get; set; }
        public bool Breakfast { get; set; }
        public bool lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Stay { get; set; }
    }
    public class ClientEnquiryActivityView
    {
        public long ClientItineraryID { get; set; }
        public string EnquiryCode { get; set; }
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
    }
}
