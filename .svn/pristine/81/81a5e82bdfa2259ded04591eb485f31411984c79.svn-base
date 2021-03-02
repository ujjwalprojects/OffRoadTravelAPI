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

namespace TourTravels.API.Areas.General.Controllers
{
    [RoutePrefix("api/general/clientenquiry")]
    public class ClientEnquiryController : ApiController
    {
        dalClientEnquiry objDAL = new dalClientEnquiry();
        dalHotelConfig objHotel = new dalHotelConfig();
        dalCabConfig objCab = new dalCabConfig();
        dalConfigurations objConfig = new dalConfigurations();
        dalTourPackage objTourpkg = new dalTourPackage();

        [HttpPost]
        [Route("SaveDirectClientEnq")]
        public async Task<string> SaveDirectClientEnq(utblClientEnquirie model)
        {
            if (ModelState.IsValid)
            {
                return await objDAL.SaveDirectClientEnqAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpPost]
        [Route("UpdateClientEnqStatus")]
        public async Task<string> UpdateClientEnqStatus(ClientEnquiryView model)
        {
            return await objDAL.UpdateClientEnquiryStatusAsync(model);
        }
        [HttpPost]
        [Route("SaveClientEnqItinerary")]
        public async Task<string> SaveClientEnqItinerary(IEnumerable<utblClientEnquiryItinerarie> model)
        {
            if (ModelState.IsValid)
            {
                return await objDAL.SaveItinerariesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpPost]
        [Route("SaveClientEnqActivity")]
        public async Task<string> SaveClientEnqActivity(IEnumerable<utblClientEnquiryActivitie> model)
        {
            if (ModelState.IsValid)
            {
                return await objDAL.SaveClientEnqActAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("GenClientEnquiryInfo")]
        public async Task<utblClientEnquirie> GenClientEnquiryInfo(string eqcode)
        {
            return await objDAL.GetClientEnquiryInfoAsync(eqcode);
        }
        [HttpGet]
        [Route("GenClientEqItineraryList")]
        public async Task<IEnumerable<utblClientEnquiryItinerarie>> GenClientEqItineraryList(string eqcode)
        {
            return await objDAL.GetClientEnqItinerarylistAsync(eqcode);
        }
        [HttpGet]
        [Route("GenClientEqActivityList")]
        public async Task<IEnumerable<utblClientEnquiryActivitie>> GenClientEqActivityist(string eqcode)
        {
            return await objDAL.GetClientEnqActivitielistAsync(eqcode);
        }
        [HttpGet]
        [Route("HotelTypes")]
        public async Task<IEnumerable<utblMstHotelType>> HotelTypes()
        {
            return await objHotel.GetHotelTypesAsync();
        }
        [HttpGet]
        [Route("CabTypes")]
        public async Task<IEnumerable<utblMstCabType>> CabTypes()
        {
            return await objCab.GetCabTypesAsync();
        }
        [HttpGet]
        [Route("AllItineraries")]
        public async Task<IEnumerable<utblMstItinerarie>> AllItineraries()
        {
            return await objConfig.GetAllItinerariesAsync();
        }
        [HttpGet]
        [Route("AllDestinations")]
        public async Task<IEnumerable<utblMstDestination>> AllDestinations()
        {
            return await objConfig.GetAllDestinationsAsync();
        }
        [HttpGet]
        [Route("PackageActivities")]
        public async Task<IEnumerable<PackageActivities>> PackageActivites(long id)
        {
            return await objTourpkg.GetPackageActivitiesAsync(id);
        }
        [HttpGet]
        [Route("ClientEnqActivities")]
        public async Task<IEnumerable<ClientEnqActivities>> ClientEnqActivities(string eqcode)
        {
            return await objDAL.GetClientEnqActivitiesAsync(eqcode);
        }
        [HttpGet]
        [Route("itinerarybyid")]
        public async Task<utblMstItinerarie> ItineraryByID(long ItineraryID)
        {
            return await objConfig.GetItineraryByIDAsync(ItineraryID);
        }


        //methods for retrieving details of client enquiry for custom package configuration summary
        [HttpGet]
        [Route("ClientEnqInfoView")]
        public async Task<ClientEnquiryView> ClientEnqInfoView(string eqcode)
        {
            return await objDAL.GetClientEnqView(eqcode);
        }
        [HttpGet]
        [Route("ClientEnqItineraryView")]
        public async Task<IEnumerable<ClientEnquiryItineraryView>> ClientEnqItineraryView(string eqcode)
        {
            return await objDAL.GetClientEnqItinerariesView(eqcode);
        }
    }
}
