using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblClientEnquiryActivitie
    {
        [Key]
        public long ClientActivityID { get; set; }
        public string EnquiryCode { get; set; }
        public long RefPackageID { get; set; }
        public long ActivityID { get; set; }
    }
}
