﻿using System;
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
    [RoutePrefix("api/Admin/configuration")]
    public class ConfigurationController : ApiController
    {
        //
        // GET: /Admin/Configuration/
        dalConfigurations objDal = new dalConfigurations();

        #region Countries
        [Route("CountriesList")]
        [HttpGet]
        public async Task<IEnumerable<utblMstCountries>> CountriesList()
        {
            return await objDal.getCountriesAsync();
        }

        [HttpPost]
        [Route("addEditCountries")]
        public async Task<string> AddCountries(utblMstCountries model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.addCountriesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CountriesByID")]
        public async Task<utblMstCountries> CountriesByID(long id)
        {
            return await objDal.getCountryByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCountries")]
        public async Task<string> DeleteCountries(long id)
        {
            return await objDal.deleteCountriesAsync(id);
        }
        #endregion

        #region States
        [Route("StateList")]
        [HttpGet]
        public async Task<IEnumerable<StateView>> StateList()
        {
            return await objDal.GetStatesAsync();
        }

        [HttpPost]
        [Route("SaveState")]
        public async Task<string> SaveState(utblMstState model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveStatesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("StateByID")]
        public async Task<utblMstState> StateByID(long id)
        {   
            return await objDal.GetStateByIDAsync(id);
        }
        [HttpGet]
        [Route("StateByCountry")]
        public async Task<IEnumerable<utblMstState>> StateByCountry(long id)
        {
            return await objDal.GetStateByCountryAsync(id);
        }
        [HttpDelete]
        [Route("DeleteState")]
        public async Task<string> DeleteState(long id)
        {
            return await objDal.DeleteStateAsync(id);
        }
        #endregion




        #region Destination
        [HttpGet]
        [Route("Destinations")]
        public async Task<DestinationVM> Destinations(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetDestinationsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveDestination")]
        public async Task<string> SaveDestination(utblMstDestination model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveDestinationAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("DestinationByID")]
        public async Task<utblMstDestination> DestinationByID(long id)
        {
            return await objDal.GetDestinationByIDAsync(id);
        }
        [HttpGet]
        [Route("AllDestinations")]
        public async Task<IEnumerable<utblMstDestination>> AllDestinations()
        {
            return await objDal.GetAllDestinationsAsync();
        }
        [HttpDelete]
        [Route("DeleteDestination")]
        public async Task<string> DeleteDestination(long id)
        {
            return await objDal.DeleteDestinationAsync(id);
        }
        #endregion

        #region Package Type

        [HttpGet]
        [Route("packagelist")]
        public async Task<PackageTypeVM> PackageList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetPackageTypeListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savepackagetype")]
        public async Task<string> SavePackage(utblMstPackageType model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SavePackageTypeAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("packagetypebyid")]
        public async Task<utblMstPackageType> PackageTypeByID(long PackageTypeID)
        {
            return await objDal.GetPackageTypeByIDAsync(PackageTypeID);
        }
        [HttpDelete]
        [Route("deletepackagetype")]
        public async Task<string> DeletePackage(long PackageTypeID)
        {
            return await objDal.DeletePackageTypeAsync(PackageTypeID);
        }
        [HttpGet]
        [Route("AllPackageTypes")]
        public async Task<IEnumerable<utblMstPackageType>> AllPackageTypes()
        {
            return await objDal.GetAllPackageTypesAsync();
        }
        #endregion

        #region Itinerary


        [HttpGet]
        [Route("destionationDD")]
        public async Task<IEnumerable<utblMstDestination>> DestionationList()
        {
            return await objDal.GetDestinationListAsync();
        }

        [HttpGet]
        [Route("itinerarylist")]
        public async Task<ItineraryVM> ItineraryList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetItineraryListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveitinerary")]
        public async Task<string> SaveItinerary(utblMstItinerarie model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveItineraryAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("itinerarybyid")]
        public async Task<utblMstItinerarie> ItineraryByID(long ItineraryID)
        {
            return await objDal.GetItineraryByIDAsync(ItineraryID);
        }
        [HttpDelete]
        [Route("deleteitinerary")]
        public async Task<string> Deleteitinerary(long ItineraryID)
        {
            return await objDal.DeleteItineraryAsync(ItineraryID);
        }
        [HttpGet]
        [Route("AllItineraries")]
        public async Task<IEnumerable<utblMstItinerarie>> AllItineraries()
        {
            return await objDal.GetAllItinerariesAsync();
        }
        #endregion

        #region Inclusions

        [HttpGet]
        [Route("inclusionlist")]
        public async Task<InclusionVM> InclusionsList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetInclusionListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveinclusion")]
        public async Task<string> SaveInclusions(utblMstInclusion model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveInclusionAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("inclusionbyid")]
        public async Task<utblMstInclusion> InclusionsByID(long InclusionID)
        {
            return await objDal.GetInclusionByIDAsync(InclusionID);
        }
        [HttpDelete]
        [Route("deleteinclusion")]
        public async Task<string> Deleteinclusions(long InclusionID)
        {
            return await objDal.DeleteInclusionAsync(InclusionID);
        }
        #endregion

        #region Exclusions

        [HttpGet]
        [Route("exclusionlist")]
        public async Task<ExclusionVM> ExclusionList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetExclusionListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveexclusion")]
        public async Task<string> SaveExclusions(utblMstExclusion model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveExclusionAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("exclusionbyid")]
        public async Task<utblMstExclusion> ExclusionByID(long ExclusionID)
        {
            return await objDal.GetExclusionByIDAsync(ExclusionID);
        }
        [HttpDelete]
        [Route("deleteexclusion")]
        public async Task<string> DeleteExclusion(long ExclusionID)
        {
            return await objDal.DeleteExclusionAsync(ExclusionID);
        }
        #endregion

        #region Terms
        [HttpGet]
        [Route("termlist")]
        public async Task<TermVM> TermList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetTermListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveterm")]
        public async Task<string> SaveExclusions(utblMstTerm model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveTermAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("termbyid")]
        public async Task<utblMstTerm> TermByID(long TermID)
        {
            return await objDal.GetTermByIDAsync(TermID);
        }
        [HttpDelete]
        [Route("deleteterm")]
        public async Task<string> DeleteTerm(long TermID)
        {
            return await objDal.DeleteTermAsync(TermID);
        }

        #endregion

        #region Banner

        [HttpGet]
        [Route("bannerlist")]
        public async Task<BannerVM> BannerList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetBannerListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savebanner")]
        public async Task<string> SaveBanner(utblMstBanner model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveBannerAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("bannerbyid")]
        public async Task<utblMstBanner> BannerByID(long BannerID)
        {
            return await objDal.GetBannerByIDAsync(BannerID);
        }
        [HttpDelete]
        [Route("deletebanner")]
        public async Task<string> DeleteBanner(long BannerID)
        {
            return await objDal.DeleteBannerAsync(BannerID);
        }

       

        #endregion

        #region Activity
        [HttpGet]
        [Route("activitylist")]
        public async Task<ActivityVM> ActivityList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetActivityListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveactivity")]
        public async Task<string> SaveActivity(utblMstActivitie model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveActivityAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("activitybyid")]
        public async Task<utblMstActivitie> ActivityByID(long id)
        {
            return await objDal.GetActivityByIDAsync(id);
        }
        [HttpDelete]
        [Route("deleteactivity")]
        public async Task<string> DeleteActivity(long ActicityID)
        {
            return await objDal.DeleteActivityAsync(ActicityID);
        }
        #endregion

        #region Tour Cancellation
        [HttpGet]
        [Route("tourcancellist")]
        public async Task<TourCancelVM> TourCancelList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetTourCancelListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savetourcancel")]
        public async Task<string> SaveTourCancel(utblMstTourCancellation model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveTourCancelAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("tourcancelbyid")]
        public async Task<utblMstTourCancellation> TourCancelByID(long CancellationID)
        {
            return await objDal.GetTourCancelByIDAsync(CancellationID);
        }
        [HttpDelete]
        [Route("deletetourcancel")]
        public async Task<string> DeleteTourCancel(long CancellationID)
        {
            return await objDal.DeleteTourCancelAsync(CancellationID);
        }


        #endregion
    }
}