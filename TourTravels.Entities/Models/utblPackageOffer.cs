using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblPackageOffer
    {
        [Key]
        public long PackageOfferID { get; set; }
        public Int16 OfferDiscount { get; set; }
        [Required]
        public string OfferDesc { get; set; }
        [Required]
        public string OfferImagePath { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
