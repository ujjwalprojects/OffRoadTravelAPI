using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.ViewModels
{
    public class StateView
    {
        public long StateID { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
    }
}
