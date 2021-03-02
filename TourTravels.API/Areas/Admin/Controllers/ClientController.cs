using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TourTravels.Entities.Dal;
using TourTravels.Entities.ViewModels;

namespace TourTravels.API.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/admin/client")]
    public class ClientController : ApiController
    {
        dalTourPackage objPack = new dalTourPackage();

        [HttpGet]
        [Route("Enquiries")]
        public async Task<TourPackageBookEnquiryVM> ClientEnquiries(int PageNo, int PageSize, string SearchTerm)
        {
            return await objPack.GetTourPackageEnquiriesPagedAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpGet]
        [Route("EnquiryByCode")]
        public async Task<TourPackageBookEnquiry> EnquiryByCode(string code)
        {
            return await objPack.GetEnquiryByCodeAsync(code);
        }
        [HttpGet]
        [Route("EnquiryItineraries")]
        public async Task<IEnumerable<EnquiryItineraryView>> EnquiryItineraries(string code)
        {
            return await objPack.GetEnquiryItinerariesAsync(code);
        }
        [HttpGet]
        [Route("EnquiryActivities")]
        public async Task<IEnumerable<EnquiryActivitiesView>> EnquiryActivities(string code)
        {
            return await objPack.GetEnquiryActivitiesAsync(code);
        }
    }
}
