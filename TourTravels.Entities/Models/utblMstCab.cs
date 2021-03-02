using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblMstCab
    {
        [Key]
        public long CabID { get; set; }
        [Required]
        public string CabName { get; set; }
        [Required]
        public string CabNo { get; set; }
        [Required]
        public long CabTypeID { get; set; }
        [Required]
        public long CabHeadID { get; set; }
    }
}
