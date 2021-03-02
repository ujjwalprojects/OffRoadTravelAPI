using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblTourPackageImage
    {
        [Key]
        public long PackageImageID { get; set; }
        [Required]
        public long PackageID { get; set; }
        [Required]
        public string PhotoThumbPath { get; set; }
        [Required]
        public string PhotoNormalPath { get; set; }
        public string PhotoCaption { get; set; }
        public bool IsPackageCover { get; set; }
    }
}
