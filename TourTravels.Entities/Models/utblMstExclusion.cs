using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblMstExclusion
    {
        [Key]
        public long ExclusionID { get; set; }
        [Required]
        public string ExclusionName { get; set; }
    }
}
