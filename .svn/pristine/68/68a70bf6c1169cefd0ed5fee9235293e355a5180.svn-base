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
    [RoutePrefix("api/Admin/cabconfig")]
    public class CabConfigController : ApiController
    {
        dalCabConfig objCab = new dalCabConfig();

        #region Cab Types
        [HttpGet]
        [Route("CabTypes")]
        public async Task<IEnumerable<utblMstCabType>> CabTypes()
        {
            return await objCab.GetCabTypesAsync();
        }
        [HttpPost]
        [Route("SaveCabType")]
        public async Task<string> SaveCabType(utblMstCabType model)
        {
            if (ModelState.IsValid)
            {
                return await objCab.SaveCabTypeAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CabTypeByID")]
        public async Task<utblMstCabType> CabTypeByID(long id)
        {
            return await objCab.GetCabTypeByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCabType")]
        public async Task<string> DeleteCabType(long id)
        {
            return await objCab.DeleteCabTypeAsync(id);
        }
        #endregion

        #region Cabs
        [HttpGet]
        [Route("Cabs")]
        public async Task<CabVM> Cabs(int PageNo, int PageSize, string SearchTerm)
        {
            return await objCab.GetCabsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveCab")]
        public async Task<string> SaveCab(utblMstCab model)
        {
            if (ModelState.IsValid)
            {
                return await objCab.SaveCabAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CabByID")]
        public async Task<utblMstCab> CabByID(long id)
        {
            return await objCab.GetCabByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCab")]
        public async Task<string> DeleteCab(long id)
        {
            return await objCab.DeleteCabAsync(id);
        }
        #endregion

        #region Driver
        [HttpGet]
        [Route("Drivers")]
        public async Task<DriverVM> Drivers(int PageNo, int PageSize, string SearchTerm)
        {
            return await objCab.GetDriversAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveDriver")]
        public async Task<string> SaveDriver(utblMstCabDriver model)
        {
            if (ModelState.IsValid)
            {
                return await objCab.SaveDriverAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("DriverByID")]
        public async Task<utblMstCabDriver> DriverByID(long id)
        {
            return await objCab.GetDriverByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteDriver")]
        public async Task<string> DeleteDriver(long id)
        {
            return await objCab.DeleteDriverAsync(id);
        }
        #endregion

        #region Cab Heads
        [HttpGet]
        [Route("CabHeads")]
        public async Task<CabHeadVM> CabHeads(int PageNo, int PageSize, string SearchTerm)
        {
            return await objCab.GetCabHeadsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveCabHead")]
        public async Task<string> SaveCabHead(utblMstCabHead model)
        {
            if (ModelState.IsValid)
            {
                return await objCab.SaveCabHeadAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CabHeadByID")]
        public async Task<utblMstCabHead> CabHeadByID(long id)
        {
            return await objCab.GetCabHeadByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCabHead")]
        public async Task<string> DeleteCabHead(long id)
        {
            return await objCab.DeleteCabHeadAsync(id);
        }
        [HttpGet]
        [Route("AllCabHeads")]
        public async Task<IEnumerable<utblMstCabHead>> AllCabHeads()
        {
            return await objCab.GetAllCabHeadsAysnc();
        }
        #endregion
    }
}
