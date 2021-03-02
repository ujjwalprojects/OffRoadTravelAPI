using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;

namespace TourTravels.Entities.ViewModels
{
    public class TourGuideVM
    {
        public IEnumerable<TourGuideListView> TourGuides { get; set; }
        public int TotalRecords { get; set; }
    }
    public class TourGuideListView
    {
        public long TourGuideID { get; set; }
        public string TourGuideName { get; set; }
        public string TourGuideLink { get; set; }
        public string TourGuideShortDesc { get; set; }
    }
}
