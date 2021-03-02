using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TourTravels.API.Models;
using TourTravels.Entities.Dal;
using TourTravels.Entities.Models;
using TourTravels.Entities.ViewModels;

namespace TourTravels.API.Areas.General.Controllers
{

    [RoutePrefix("api/general/tourpackage")]
    public class GenTourPackageController : ApiController
    {
        dalTourPackage objDAL = new dalTourPackage();
        dalConfigurations objDalBanner = new dalConfigurations();
        dalTourGuide objTG = new dalTourGuide();

        [HttpGet]
        [Route("GenTourPackageList")]
        public async Task<GenTourPackageVM> GenTourPackageList(int PageNo, int PageSize, string SearchTerm)
        {
            GenTourPackageVM objVM = new GenTourPackageVM();
            objVM = await objDAL.GetGenTourPackageListAsync(PageNo, PageSize, SearchTerm);
            return objVM;
        }

        [HttpGet]
        [Route("GenTourPackageDtl")]
        public async Task<GenTourPackageDtlView> GetGenTourPackageDtl(string linktext)
        {
            return await objDAL.GetGenTourPackageDtlView(linktext);
        }
        [HttpGet]
        [Route("ItinerariesView")]
        public async Task<IEnumerable<PackageItineraryView>> ItinerariesView(long id)
        {
            return await objDAL.GetPackageItinerariesAsync(id);

        }

        [HttpGet]
        [Route("PackageInclusions")]
        public async Task<IEnumerable<PackageInclusions>> PackageInclusions(long id)
        {
            return await objDAL.GetPackageInclusionsAsync(id);
        }
        [HttpGet]
        [Route("PackageExclusions")]
        public async Task<IEnumerable<PackageExclusions>> PackageExclusions(long id)
        {
            return await objDAL.GetPackageExclusionsAsync(id);
        }
        [HttpGet]
        [Route("PackageActivities")]
        public async Task<IEnumerable<PackageActivities>> PackageActivites(long id)
        {
            return await objDAL.GetPackageActivitiesAsync(id);
        }
        [HttpGet]
        [Route("PackageTerms")]
        public async Task<IEnumerable<PackageTerms>> PackageTerms(long id)
        {
            return await objDAL.GetPackageTermsAsync(id);
        }
        [HttpGet]
        [Route("PackageCancellations")]
        public async Task<IEnumerable<PackageCancellations>> PackageCancellations(long id)
        {
            return await objDAL.GetPackageCancellationsAsync(id);
        }
        [HttpGet]
        [Route("GenTourPackageCoverImgView")]
        public async Task<GenTourPackageImage> GenTourPackageCoverImgView(long id)
        {
            return await objDAL.GenTourPackageCoverImgViewAsync(id);
        }
        [HttpGet]
        [Route("PackageGalleryImage")]
        public async Task<IEnumerable<GenTourPackageImage>> GenTourPackageGalImgView(long id)
        {
            return await objDAL.GenTourPackageGalImgViewAsync(id);
        }
        //for Homepage

        [HttpGet]
        [Route("homebannerlist")]
        public List<utblMstBanner> GetBannerList()
        {
            return objDalBanner.getBannerList();
        }
        [HttpGet]
        [Route("WhereNames")]
        public async Task<IEnumerable<string>> WhereNames()
        {
            return await objDAL.GetStateDestinationNamesAsync();
        }
        [HttpGet]
        [Route("TourTypes")]
        public async Task<IEnumerable<utblMstPackageType>> TourTypes()
        {
            return await objDalBanner.GetAllPackageTypesAsync();
        }
        [HttpPost]
        [Route("GenTourPackageSearch")]
        public async Task<GenTourPackageVM> GenTourPackageSearch(GenSearchModel model)
        {
            return await objDAL.SearchGenTourPackageListAsync(model);
        }
        [HttpGet]
        [Route("Destinations")]
        public async Task<IEnumerable<DestinationView>> Destinations()
        {
            return await objDAL.GetAllDestinationsAsync();
        }
        //for choosing Package Type to Display
        [HttpGet]
        [Route("GenTourPackageDispList")]
        public async Task<GenTourPackageVM> GenTourPackageDispList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDAL.GetGenTourPackageDispListAsync(PageNo, PageSize, SearchTerm);
        }

        #region destinatton package
        [HttpGet]
        [Route("GenDestinationByID")]
        public async Task<GenDestinationVM> GenDestListByID(long id)
        {
            GenDestinationVM model = new GenDestinationVM();
            model.GenDestination = await objDAL.getDestinationSelectAsync(id);
            return model;
        }

        [HttpGet]
        [Route("GenDestinationPackageDispList")]
        public async Task<GenTourPackageVM> GenDestintaionPackageDispList(string Destination, int PageNo, int PageSize, string SearchTerm)
        {
            return await objDAL.GetDestinationPackageListAsync(Destination, PageNo, PageSize, SearchTerm);
        }

        #endregion

        [HttpGet]
        [Route("GenOfferPackagelist")]
        public async Task<IEnumerable<GenPackageOfferView>> GenOfferPackageList()
        {
            return await objDAL.GenGetOfferPackageListAsync();
        }
        [HttpGet]
        [Route("GenOfferPackageByID")]
        public async Task<GenTourPackageVM> GenTourPackageByID(long POID, int PageNo, int PageSize)
        {
            return await objDAL.GetGenOfferPackageListAsync(POID, PageNo, PageSize);
        }
        [HttpGet]
        [Route("TourGuides")]
        public async Task<IEnumerable<TourGuideListView>> TourGuides()
        {
            return await objTG.GetAllGuidesAsync();
        }
        [HttpGet]
        [Route("GuideDetails")]
        public async Task<utblTourGuide> GuideDetails(string link)
        {
            return await objTG.GetGuideByLinkAsync(link);
        }
    }
}
