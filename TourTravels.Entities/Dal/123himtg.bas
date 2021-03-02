using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using TourTravels.Entities.Models;
using TourTravels.Entities.ViewModels;

namespace TourTravels.Entities.Dal
{
    public class dalConfigurations
    {
        EFDBContext objDB = new EFDBContext();


        #region Countries
        public async Task<IEnumerable<utblMstCountries>> getCountriesAsync()
        {
            return await objDB.utblMstCountries.ToListAsync();
        }
        public async Task<string> addCountriesAsync(utblMstCountries model)
        {
            try
            {
                if (model.CountryID == 0)
                {
                    objDB.utblMstCountries.Add(model);
                    await objDB.SaveChangesAsync();
                    return "New Country Details Added";
                }
                else
                {
                    utblMstCountries curObj = await objDB.utblMstCountries.FindAsync(model.CountryID);
                    curObj.CountryName = model.CountryName;
                    await objDB.SaveChangesAsync();
                    return "Country Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<utblMstCountries> getCountryByIDAsync(long id)
        {
            return await objDB.utblMstCountries.Where(x => x.CountryID == id).FirstOrDefaultAsync();
        }

        public async Task<string> deleteCountriesAsync(long id)
        {
            try
            {
                utblMstCountries curObj = await objDB.utblMstCountries.FindAsync(id);
                objDB.utblMstCountries.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Country Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        #endregion Countries

        #region States
        public async Task<IEnumerable<StateView>> GetStatesAsync()
        {
            return await objDB.utblMstStates
                .Join(objDB.utblMstCountries, x => x.CountryID, y => y.CountryID, (x, y) => new StateView()
                {
                    StateID = x.StateID,
                    CountryID = x.CountryID,
                    CountryName = y.CountryName,
                    StateName = x.StateName
                }).ToListAsync();
        }
        public async Task<string> SaveStatesAsync(utblMstState model)
        {
            try
            {
                if (model.StateID == 0)
                {
                    objDB.utblMstStates.Add(model);
                    await objDB.SaveChangesAsync();
                    return "New State Added";
                }
                else
                {
                    utblMstState curState = await objDB.utblMstStates.FindAsync(model.StateID);
                    curState.CountryID = model.CountryID;
                    curState.StateName = model.StateName;
                    await objDB.SaveChangesAsync();
                    return "State Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstState> GetStateByIDAsync(long id)
        {
            return await objDB.utblMstStates.Where(x => x.StateID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteStateAsync(long id)
        {
            try
            {
                utblMstState curObj = await objDB.utblMstStates.FindAsync(id);
                objDB.utblMstStates.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "State Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<IEnumerable<utblMstState>> GetStateByCountryAsync(long id)
        {
            return await objDB.utblMstStates.Where(x => x.CountryID == id).ToListAsync();
        }
        #endregion

        #region Destinations
        public async Task<DestinationVM> GetDestinationsAsync(int pageno, int pagesize, string sterm)
        {
            DestinationVM model = new DestinationVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.Destinations = await objDB.Database.SqlQuery<DestinationView>("udspMstDestinationPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveDestinationAsync(utblMstDestination model)
        {
            try
            {
                var parDestID = new SqlParameter("@DestinationID", model.DestinationID);
                var parCountryID = new SqlParameter("@CountryID", model.CountryID);
                var parStateID = new SqlParameter("@StateID", model.StateID);
                var parDestName = new SqlParameter("@DestinationName", model.DestinationName);
                var parDestDesc = new SqlParameter("@DestinationDesc", model.DestinationDesc ?? "");

                return await objDB.Database.SqlQuery<string>("udspMstDestinationSave @DestinationID, @CountryID, @StateID, @DestinationName, @DestinationDesc",
                    parDestID, parCountryID, parStateID, parDestName, parDestDesc).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstDestination> GetDestinationByIDAsync(long id)
        {
            return await objDB.utblMstDestinations.Where(x => x.DestinationID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblMstDestination>> GetAllDestinationsAsync()
        {
            return await objDB.utblMstDestinations.OrderBy(x => x.DestinationName).ToListAsync();
        }
        public async Task<string> DeleteDestinationAsync(long id)
        {
            try
            {
                utblMstDestination curObj = await objDB.utblMstDestinations.FindAsync(id);
                objDB.utblMstDestinations.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Destination Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region Package Type
        public async Task<PackageTypeVM> GetPackageTypeListAsync(int pageno, int pagesize, string sterm)
        {
            PackageTypeVM model = new PackageTypeVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.PackageList = await objDB.Database.SqlQuery<PackageTypeView>("udspMstPackageTypeList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SavePackageTypeAsync(utblMstPackageType model)
        {
            try
            {
                var parPID = new SqlParameter("@PackageTypeID", model.PackageTypeID);
                var parPName = new SqlParameter("@PackageTypeName", model.PackageTypeName ?? "");

                return await objDB.Database.SqlQuery<string>("udspMstPackageTypeAddEdit @PackageTypeID, @PackageTypeName",
                    parPID, parPName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstPackageType> GetPackageTypeByIDAsync(long id)
        {
            utblMstPackageType obj = new utblMstPackageType();
            obj = await objDB.utblMstPackageTypes.Where(x => x.PackageTypeID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeletePackageTypeAsync(long PackageTypeID)
        {
            try
            {
                utblMstPackageType curObj = await objDB.utblMstPackageTypes.FindAsync(PackageTypeID);
                objDB.utblMstPackageTypes.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Package Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<IEnumerable<utblMstPackageType>> GetAllPackageTypesAsync()
        {
            return await objDB.utblMstPackageTypes.ToListAsync();
        }
        #endregion

        #region Itineraries
        public async Task<ItineraryVM> GetItineraryListAsync(int pageno, int pagesize, string sterm)
        {
            ItineraryVM model = new ItineraryVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.ItinenaryList = await objDB.Database.SqlQuery<ItineraryView>("udspMstItineraryList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }

        public async Task<IEnumerable<utblMstDestination>> GetDestinationListAsync()
        {
            return await objDB.utblMstDestinations.ToListAsync();
        }
        public async Task<string> SaveItineraryAsync(utblMstItinerarie model)
        {
            try
            {
                var parPID = new SqlParameter("@ItineraryID", model.ItineraryID);
                var parItName = new SqlParameter("@ItineraryName", model.ItineraryName);
                var parItDesc = new SqlParameter("@ItineraryDesc", model.ItineraryDesc);
                var parOvDest = new SqlParameter("@OvernightDestinationID", DBNull.Value);
                if (model.OvernightDestinationID != null)
                    parOvDest = new SqlParameter("@OvernightDestinationID", model.OvernightDestinationID);

                return await objDB.Database.SqlQuery<string>("udspMstItineraryAddEdit @ItineraryID, @ItineraryName,@ItineraryDesc,@OvernightDestinationID",
                    parPID, parItName, parItDesc, parOvDest).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstItinerarie> GetItineraryByIDAsync(long id)
        {
            utblMstItinerarie obj = new utblMstItinerarie();
            obj = await objDB.utblMstItineraries.Where(x => x.ItineraryID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteItineraryAsync(long ItineraryID)
        {
            try
            {
                utblMstItinerarie curObj = await objDB.utblMstItineraries.FindAsync(ItineraryID);
                objDB.utblMstItineraries.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Itinerary Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        #endregion

        #region Inclusions
        public async Task<InclusionVM> GetInclusionListAsync(int pageno, int pagesize, string sterm)
        {
            InclusionVM model = new InclusionVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.InclusionList = await objDB.Database.SqlQuery<InclusionView>("udspMstInclusionList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveInclusionAsync(utblMstInclusion model)
        {
            try
            {
                var parPID = new SqlParameter("@InclusionID", model.InclusionID);
                var parIName = new SqlParameter("@InclusionName", model.InclusionName);
                var parIDesc = new SqlParameter("@InclusionType", model.InclusionType);

                return await objDB.Database.SqlQuery<string>("udspMstInclusionAddEdit @InclusionID, @InclusionName,@InclusionType",
                    parPID, parIName, parIDesc).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstInclusion> GetInclusionByIDAsync(long id)
        {
            utblMstInclusion obj = new utblMstInclusion();
            obj = await objDB.utblMstInclusions.Where(x => x.InclusionID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteInclusionAsync(long InclustionID)
        {
            try
            {
                utblMstInclusion curObj = await objDB.utblMstInclusions.FindAsync(InclustionID);
                objDB.utblMstInclusions.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Inclusion Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        #endregion

        #region Exclusions
        public async Task<ExclusionVM> GetExclusionListAsync(int pageno, int pagesize, string sterm)
        {
            ExclusionVM model = new ExclusionVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.ExclusionList = await objDB.Database.SqlQuery<ExclusionView>("udspMstExclusionList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveExclusionAsync(utblMstExclusion model)
        {
            try
            {
                var parPID = new SqlParameter("@ExclusionID", model.ExclusionID);
                var parIName = new SqlParameter("@ExclusionName", model.ExclusionName);

                return await objDB.Database.SqlQuery<string>("udspMstExclusionAddEdit @ExclusionID, @ExclusionName",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstExclusion> GetExclusionByIDAsync(long id)
        {
            utblMstExclusion obj = new utblMstExclusion();
            obj = await objDB.utblMstExclusions.Where(x => x.ExclusionID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteExclusionAsync(long ExclusionID)
        {
            try
            {
                utblMstExclusion curObj = await objDB.utblMstExclusions.FindAsync(ExclusionID);
                objDB.utblMstExclusions.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Exclusion Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        #endregion

        #region Terms

        public async Task<TermVM> GetTermListAsync(int pageno, int pagesize, string sterm)
        {
            TermVM model = new TermVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.TermList = await objDB.Database.SqlQuery<TermView>("udspMstTermList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveTermAsync(utblMstTerm model)
        {
            try
            {
                var parPID = new SqlParameter("@TermID", model.TermID);
                var parIName = new SqlParameter("@TermName", model.TermName);

                return await objDB.Database.SqlQuery<string>("udspMstTermAddEdit @TermID, @TermName",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstTerm> GetTermByIDAsync(long id)
        {
            utblMstTerm obj = new utblMstTerm();
            obj = await objDB.utblMstTerms.Where(x => x.TermID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteTermAsync(long TermID)
        {
            try
            {
                utblMstTerm curObj = await objDB.utblMstTerms.FindAsync(TermID);
                objDB.utblMstTerms.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Term Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region Banner

        public async Task<BannerVM> GetBannerListAsync(int pageno, int pagesize, string sterm)
        {
            BannerVM model = new BannerVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.BannerList = await objDB.Database.SqlQuery<BannerView>("udspMstBannerList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveBannerAsync(utblMstBanner model)
        {
            try
            {
                var parPID = new SqlParameter("@BannerID", model.BannerID);
                var parIName = new SqlParameter("@BannerPath", model.BannerPath);

                return await objDB.Database.SqlQuery<string>("udspMstBannerAddEdit @BannerID, @BannerPath",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstBanner> GetBannerByIDAsync(long id)
        {
            utblMstBanner obj = new utblMstBanner();
            obj = await objDB.utblMstBanners.Where(x => x.BannerID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteBannerAsync(long BannerID)
        {
            try
            {
                utblMstBanner curObj = await objDB.utblMstBanners.FindAsync(BannerID);
                objDB.utblMstBanners.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Banner Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region Activities
        public async Task<ActivityVM> GetActivityListAsync(int pageno, int pagesize, string sterm)
        {
            try
            {
                ActivityVM model = new ActivityVM();
                var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
                var parEnd = new SqlParameter("@PageSize", pagesize);

                var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
                if (!(sterm == null || sterm == ""))
                    parSearchTerm.Value = sterm;
                // setting stored procedure OUTPUT value
                // This return total number of rows, and avoid two database call for data and total number of rows 
                var spOutput = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                model.ActivityList = await objDB.Database.SqlQuery<ActivityView>("udspMstActivitiesList @Start, @PageSize,@SearchTerm, @TotalCount out",
                    parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {
                return "Error: " + ex;
            }
        }

        public async Task<string> SaveActivityAsync(utblMstActivitie model)
        {
            try
            {
                var parAID = new SqlParameter("@ActivityID", model.ActivityID);
                var parAName = new SqlParameter("@ActivityName", model.ActivityName);
                var parADesc = new SqlParameter("@ActivityDesc", model.ActivityDesc);
                var parDestinationID = new SqlParameter("@DestinationID", model.DestinationID);
                var parBaseFare = new SqlParameter("@BaseFare", model.BaseFare);

                return await objDB.Database.SqlQuery<string>("udspMstActivitiesAddEdit @ActivityID, @ActivityName,@ActivityDesc,@DestinationID,@BaseFare",
                    parAID, parAName, parADesc, parDestinationID, parBaseFare).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstActivitie> GetActivityByIDAsync(long id)
        {
            utblMstActivitie obj = new utblMstActivitie();
            obj = await objDB.utblMstActivities.Where(x => x.ActivityID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteActivityAsync(long ActivityID)
        {
            try
            {
                utblMstActivitie curObj = await objDB.utblMstActivities.FindAsync(ActivityID);
                objDB.utblMstActivities.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Activity Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}
