using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;
namespace TourTravels.Entities.ViewModels
{
    public class PackageTypeView
    {
        public long PackageTypeID { get; set; }
        public string PackageTypeName { get; set; }
        public bool IsVisible { get; set; }
    }

    public class PackageTypeVM
    {
        public IEnumerable<PackageTypeView> PackageList { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PackageOfferView
    {
        public long PackageOfferID { get; set; }
        public string PackageName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class GenPackageOfferView
    {
        public long PackageOfferID { get; set; }
        public string OfferDesc { get; set; }
        public string OfferImagePath { get; set; }
    }
    public class PackageOfferVM
    {
        public IEnumerable<GenPackageOfferView> GenPackageOfferList { get; set; }
        public IEnumerable<PackageOfferView> PackageOfferList { get; set; }
        public IEnumerable<PackageDD> PackageList { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PackageOffer
    {
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


    public class SavePackageOffer
    {
        public List<PackageDD> PackageList { get; set; }
        [Required]
        public List<long> PackageID { get; set; }
        public PackageOffer PackageOffer { get; set; }
    }
    public class EditPackageOffer
    {
        public utblPackageOffer PackageOffer { get; set; }
        public List<PackageDD> PackageList { get; set; }
        public long PackageID { get; set; }
    }
    public class PackageDD
    {
        public long PackageID { get; set; }
        public string PackageName { get; set; }
    }
}
