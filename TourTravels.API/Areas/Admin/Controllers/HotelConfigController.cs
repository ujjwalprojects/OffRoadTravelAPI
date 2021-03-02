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
    [RoutePrefix("api/Admin/hotelconfig")]
    public class HotelConfigController : ApiController
    {
        dalHotelConfig objHotel = new dalHotelConfig();

        #region Hotel Type
        [HttpGet]
        [Route("HotelTypes")]
        public async Task<IEnumerable<utblMstHotelType>> HotelTypes()
        {
            return await objHotel.GetHotelTypesAsync();
        }
        [HttpPost]
        [Route("SaveHotelType")]
        public async Task<string> SaveHotelType(utblMstHotelType model)
        {
            if (ModelState.IsValid)
            {
                return await objHotel.SaveHotelTypeAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("HotelTypeByID")]
        public async Task<utblMstHotelType> HotelTypeByID(long id)
        {
            return await objHotel.GetHotelTypeByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteHotelType")]
        public async Task<string> DeleteHotelType(long id)
        {
            return await objHotel.DeleteHotelTypeAsync(id);
        }
        #endregion

        #region Hotels
        [HttpGet]
        [Route("Hotels")]
        public async Task<HotelVM> Hotels(int PageNo, int PageSize, string SearchTerm)
        {
            return await objHotel.GetHotelsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveHotel")]
        public async Task<string> SaveHotel(HotelSaveModel model)
        {
            if (ModelState.IsValid)
            {
                return await objHotel.SaveHotelAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("HotelByID")]
        public async Task<utblMstHotel> HotelByID(long id)
        {
            return await objHotel.GetHotelByIDAsync(id);
        }
        [HttpGet]
        [Route("HotelTypesOfHotel")]
        public async Task<IEnumerable<long>> HotelTypesOfHotel(long id)
        {
            return await objHotel.GetHotelHotelTypesAsync(id);
        }
        [HttpDelete]
        [Route("DeleteHotel")]
        public async Task<string> DeleteHotel(long id)
        {
            return await objHotel.DeleteHotelAsync(id);
        }
        #endregion
    }
}
