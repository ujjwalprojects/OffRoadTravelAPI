using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TourTravels.Entities.Dal;
using TourTravels.Entities.Models;
using TourTravels.Entities.ViewModels;

namespace TourTravels.API.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/admin/tourguide")]
    public class TourGuideController : ApiController
    {
        dalTourGuide objGuide = new dalTourGuide();

        [HttpGet]
        [Route("GuidePaged")]
        public async Task<TourGuideVM> GuidePaged(int PageNo, int PageSize, string SearchTerm)
        {
            return await objGuide.GetGuidesPagedAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveGuide")]
        public async Task<string> SaveGuide(utblTourGuide model)
        {
            if (ModelState.IsValid)
            {
                return await objGuide.SaveGuideAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("GuideByID")]
        public async Task<utblTourGuide> GuideByID(long id)
        {
            return await objGuide.GetGuideByIDAsync(id);
        }
        [HttpGet]
        [Route("AllGuides")]
        public async Task<IEnumerable<TourGuideListView>> AllGuides()
        {
            return await objGuide.GetAllGuidesAsync();
        }
        [HttpDelete]
        [Route("DeleteGuide")]
        public async Task<string> DeleteGuide(long id)
        {
            return await objGuide.DeleteGuideAsync(id);
        }
    }
}
