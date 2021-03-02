using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;

namespace TourTravels.Entities.ViewModels
{
    public class CabView
    {
        public long CabID { get; set; }
        public string CabName { get; set; }
        public string CabNo { get; set; }
        public long CabTypeID { get; set; }
        public string CabTypeName { get; set; }
        public long CabHeadID { get; set; }
        public string CabHeadName { get; set; }
    }
    public class CabVM
    {
        public IEnumerable<CabView> Cabs { get; set; }
        public int TotalRecords { get; set; }
    }
    public class DriverVM
    {
        public IEnumerable<utblMstCabDriver> Drivers { get; set; }
        public int TotalRecords { get; set; }
    }
    public class CabHeadVM
    {
        public IEnumerable<utblMstCabHead> CabHeads { get; set; }
        public int TotalRecords { get; set; }
    }
}
