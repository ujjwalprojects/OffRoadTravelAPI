using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;
using System.Data.Entity;
using System.Data;
using TourTravels.Entities.Utility;
using TourTravels.Entities.ViewModels;

namespace TourTravels.Entities.Dal
{
    public class dalTourPackage
    {
        EFDBContext db = new EFDBContext();

        #region Admin section

        public async Task<string> SavePackageAsync(utblTourPackage model)
        {
            try
            {
                var parPackID = new SqlParameter("@PackageID", model.PackageID);
                var parPackName = new SqlParameter("@PackageName", model.PackageName);
                var parPackType = new SqlParameter("@PackageTypeID", model.PackageTypeID);
                var parPackRouting = new SqlParameter("@PackageRouting", model.PackageRouting ?? "");
                var parPickup = new SqlParameter("@PickupPoint", model.PickupPoint ?? "");
                var parDrop = new SqlParameter("@DropPoint", model.DropPoint ?? "");
                var parDays = new SqlParameter("@TotalDays", model.TotalDays);
                var parBaseFare = new SqlParameter("@BaseFare", model.BaseFare);
                var parPackDesc = new SqlParameter("@PackageDesc", model.PackageDesc ?? "");
                var parPackLink = new SqlParameter("@LinkText", model.LinkText);
                var parMetaText = new SqlParameter("@MetaText", model.MetaText ?? "");
                var parMetaDesc = new SqlParameter("@MetaDesc", model.MetaDesc ?? "");
                var parShowPack = new SqlParameter("@ShowPackage", model.ShowPackage);
                var parFarePer = new SqlParameter("@FarePer", model.FarePer);
                var parShowPrice = new SqlParameter("@ShowPrice", model.IsPriceVisible);

                return await db.Database.SqlQuery<string>("udspTourPackageSave @PackageID, @PackageName, @PackageTypeID, @PackageRouting, @PickupPoint, @DropPoint"
                    + ",@TotalDays, @BaseFare, @PackageDesc, @LinkText, @MetaText, @MetaDesc,@ShowPackage, @FarePer,@ShowPrice",
                    parPackID, parPackName, parPackType, parPackRouting, parPickup, parDrop,
                    parDays, parBaseFare, parPackDesc, parPackLink, parMetaText, parMetaDesc, parShowPack, parFarePer, parShowPrice).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<IEnumerable<utblTourPackageItinerary>> GetItinerariesByIDAsync(long id)
        {
            return await db.utblTourPackageItineraries.Where(x => x.PackageID == id).OrderBy(x => x.DayNo).ToListAsync();
        }
        public async Task<utblTourPackage> GetPackageByIDAsync(long id)
        {
            return await db.utblTourPackages.Where(x => x.PackageID == id).FirstOrDefaultAsync();
        }
        public async Task<PackageBriefInfo> GetPackageBriefInfoAsync(long id)
        {
            return await db.utblTourPackages.Where(x => x.PackageID == id)
                .Select(x => new PackageBriefInfo() { PackageID = x.PackageID, PackageName = x.PackageName, TotalDays = x.TotalDays })
                .FirstOrDefaultAsync();
        }
        public async Task<string> SaveItinerariesAsync(IEnumerable<utblTourPackageItinerary> model)
        {
            try
            {
                var parPackID = new SqlParameter("@PackageID", model.ElementAt(0).PackageID);

                ConvertListToDT objDT = new ConvertListToDT();
                DataTable dt = objDT.ConvertIEnumerableToDataTable(model);

                var parSubDT = new SqlParameter("@ItineraryTable", dt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.PackageItineraryType";

                return await db.Database.SqlQuery<string>("udspTourPackageItinerarySave @PackageID, @ItineraryTable", parPackID, parSubDT).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        
        public async Task<TourPackageImageVM> GetPackageImagesAsync(long packageid, int pageno, int pagesize)
        {
            TourPackageImageVM model = new TourPackageImageVM();
            var parPackID = new SqlParameter("@PackageID", packageid);
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.PackageImages = await db.Database.SqlQuery<utblTourPackageImage>("udspTourPackageImagePaged @PackageID, @Start, @PageSize, @TotalCount out",
                parPackID, parStart, parEnd, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SavePackageImageAsync(utblTourPackageImage model)
        {
            try
            {
                var parImageID = new SqlParameter("@PackageImageID", model.PackageImageID);
                var parPackID = new SqlParameter("@PackageID", model.PackageID);
                var parIsCover = new SqlParameter("@IsPackageCover", model.IsPackageCover);
                var parCaption = new SqlParameter("@PhotoCaption", model.PhotoCaption);
                var parThumb = new SqlParameter("@PhotoThumbPath", model.PhotoThumbPath);
                var parNormal = new SqlParameter("@PhotoNormalPath", model.PhotoNormalPath);

                return await db.Database.SqlQuery<string>("udspTourPackageImageSave @PackageImageID, @PackageID, @IsPackageCover, @PhotoCaption, @PhotoThumbPath, @PhotoNormalPath",
                    parImageID, parPackID, parIsCover, parCaption, parThumb, parNormal).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblTourPackageImage> GetPackageImageByIDAsync(long id)
        {
            return await db.utblTourPackageImages.Where(x => x.PackageImageID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeletePackageImageByIDAsync(long id)
        {
            try
            {
                var parImageID = new SqlParameter("@PackageImageID", id);

                return await db.Database.SqlQuery<string>("udspTourPackageImageDelete @PackageImageID", parImageID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> MakeCoverImageAsync(long packid, long imageid)
        {

            var parImgID = new SqlParameter("@PackageImageID", imageid);
            var parPackID = new SqlParameter("@PackageID", packid);

            return await db.Database.SqlQuery<string>("udspTourPackageMakeCover @PackageImageID, @PackageID", parImgID, parPackID).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<PackageActivities>> GetPackageActivitiesAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            return await db.Database.SqlQuery<PackageActivities>("select * from dbo.udfGetTourPackageActivities(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<IEnumerable<PackageInclusions>> GetPackageInclusionsAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            return await db.Database.SqlQuery<PackageInclusions>("select * from dbo.udfGetTourPackageInclusions(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<string> SavePackageActIncAsync(PackageActIncSaveModel model)
        {
            try
            {
                var parPackID = new SqlParameter("@PackageID", model.PackageID);
                ConvertListToDT objList = new ConvertListToDT();
                DataTable actdt = new DataTable();
                DataTable incdt = new DataTable();

                //Converting subject list to datatable if record is present else send empty datatable
                if (model.Activities != null)
                {
                    actdt = objList.ConvertIEnumerableToDataTable(model.Activities);
                }
                else
                {
                    if (actdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "PackageActivityID";
                        actdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "PackageID";
                        actdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "ActivityID";
                        actdt.Columns.Add(col2);
                        DataColumn col3 = new DataColumn();
                        col3.ColumnName = "ActivityFare";
                        actdt.Columns.Add(col3);
                    }
                }

                var parActDT = new SqlParameter("@ActivityTable", actdt);
                parActDT.SqlDbType = SqlDbType.Structured;
                parActDT.TypeName = "dbo.ActivityType";

                if (model.Inclusions != null)
                {
                    incdt = objList.ConvertIEnumerableToDataTable(model.Inclusions);
                }
                else
                {
                    if (incdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "PackageInclusionID";
                        incdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "PackageID";
                        incdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "InclusionID";
                        incdt.Columns.Add(col2);
                        DataColumn col3 = new DataColumn();
                        col3.ColumnName = "InclusionFare";
                        incdt.Columns.Add(col3);
                    }
                }

                var parIncDT = new SqlParameter("@InclusionTable", incdt);
                parIncDT.SqlDbType = SqlDbType.Structured;
                parIncDT.TypeName = "dbo.InclusionType";

                return await db.Database.SqlQuery<string>("udspTourPackageActivityInclusionSave @PackageID, @ActivityTable, @InclusionTable",
                    parPackID, parActDT, parIncDT).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<IEnumerable<PackageExclusions>> GetPackageExclusionsAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            return await db.Database.SqlQuery<PackageExclusions>("select * from dbo.udfGetTourPackageExclusions(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<IEnumerable<PackageTerms>> GetPackageTermsAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            return await db.Database.SqlQuery<PackageTerms>("select * from dbo.udfGetTourPackageTerms(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<IEnumerable<PackageCancellations>> GetPackageCancellationsAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            return await db.Database.SqlQuery<PackageCancellations>("select * from dbo.udfGetTourPackageCancellations(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<string> SavePackageExcTermsAsync(PackageExcTermSaveModel model)
        {
            try
            {
                var parPackID = new SqlParameter("@PackageID", model.PackageID);
                ConvertListToDT objList = new ConvertListToDT();
                DataTable excdt = new DataTable();
                DataTable termdt = new DataTable();
                DataTable candt = new DataTable();

                //Converting subject list to datatable if record is present else send empty datatable
                if (model.Exclusions != null)
                {
                    excdt = objList.ConvertIEnumerableToDataTable(model.Exclusions);
                }
                else
                {
                    if (excdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "PackageExclusionID";
                        excdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "PackageID";
                        excdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "ExclusionID";
                        excdt.Columns.Add(col2);
                    }
                }

                var parExDT = new SqlParameter("@ExclusionTable", excdt);
                parExDT.SqlDbType = SqlDbType.Structured;
                parExDT.TypeName = "dbo.ExclusionType";

                if (model.Terms != null)
                {
                    termdt = objList.ConvertIEnumerableToDataTable(model.Terms);
                }
                else
                {
                    if (termdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "PackageTermsID";
                        termdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "PackageID";
                        termdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "TermID";
                        termdt.Columns.Add(col2);
                    }
                }

                var parTermDT = new SqlParameter("@TermTable", termdt);
                parTermDT.SqlDbType = SqlDbType.Structured;
                parTermDT.TypeName = "dbo.TermType";


                if (model.Cancellations != null)
                {
                    candt = objList.ConvertIEnumerableToDataTable(model.Cancellations);
                }
                else
                {
                    if (candt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "PackageCancID";
                        candt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "PackageID";
                        candt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "CancellationID";
                        candt.Columns.Add(col2);
                    }
                }

                var parCanDT = new SqlParameter("@CancellationTable", candt);
                parCanDT.SqlDbType = SqlDbType.Structured;
                parCanDT.TypeName = "dbo.CancellationType";

                return await db.Database.SqlQuery<string>("udspTourPackageExclusionTermSave @PackageID, @ExclusionTable, @TermTable, @CancellationTable",
                    parPackID, parExDT, parTermDT, parCanDT).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<TourPackageVM> GetTourPackagePagedAsync(int pageno, int pagesize, string searchterm)
        {
            TourPackageVM model = new TourPackageVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(searchterm == null || searchterm == ""))
                parSearchTerm.Value = searchterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.Packages = await db.Database.SqlQuery<TourPackageView>("udspTourPackagePaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> DeletePackageAsync(long id)
        {
            try
            {
                var parPackID = new SqlParameter("@PackageID", id);

                return await db.Database.SqlQuery<string>("udspTourPackageDelete @PackageID", parPackID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<TourPackageView> GetPackageViewAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);

            return await db.Database.SqlQuery<TourPackageView>("select * from dbo.udfGetTourPackageView(@PackageID)", parPackID).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<PackageItineraryView>> GetPackageItinerariesAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);
            try
            {
                return await db.Database.SqlQuery<PackageItineraryView>("select * from dbo.udfGetTourPackageItineraries(@PackageID)", parPackID).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Gen TourPackage
        public async Task<GenTourPackageVM> GetGenTourPackageListAsync(int pageno, int pagesize, string sterm)
        {
            GenTourPackageVM model = new GenTourPackageVM();
            try
            {
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

                model.PackageList = await db.Database.SqlQuery<GenTourPackageView>("udspGenTourPackageList @Start, @PageSize,@SearchTerm, @TotalCount out",
                    parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<GenTourPackageVM> GetGenTourPackageDispListAsync(int pageno, int pagesize, string sterm)
        {
            GenTourPackageVM model = new GenTourPackageVM();
            try
            {
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

                model.PackageList = await db.Database.SqlQuery<GenTourPackageView>("udspGenTourPackageTypeDispList @Start, @PageSize,@SearchTerm, @TotalCount out",
                    parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        

        public async Task<GenTourPackageDtlView> GetGenTourPackageDtlView(string linktext)
        {
            var parlinktext = new SqlParameter("@linktext", linktext);
            return await db.Database.SqlQuery<GenTourPackageDtlView>("udspGenTourPackageDtlSelectByLinkText @linktext", parlinktext).FirstOrDefaultAsync();
        }
        public async Task<GenTourPackageImage> GenTourPackageCoverImgViewAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);

            return await db.Database.SqlQuery<GenTourPackageImage>("select * from dbo.udfGetTourPackageCoverImage(@PackageID)", parPackID).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<GenTourPackageImage>> GenTourPackageGalImgViewAsync(long id)
        {
            var parPackID = new SqlParameter("@PackageID", id);

            return await db.Database.SqlQuery<GenTourPackageImage>("select * from dbo.udfGetTourPackageGalleryImage(@PackageID)", parPackID).ToListAsync();
        }
        public async Task<IEnumerable<string>> GetStateDestinationNamesAsync()
        {
            List<string> names = new List<string>();
            names = await db.utblMstDestinations.Select(x => x.DestinationName).Distinct().ToListAsync();
            names.AddRange(await db.utblMstStates.Select(x => x.StateName).Distinct().ToListAsync());
            return names;
        }
        public async Task<IEnumerable<DestinationView>> GetAllDestinationsAsync()
        {
            return await db.utblMstDestinations.Join(db.utblMstStates, x => x.StateID, y => y.StateID, (x, y) => new DestinationView()
                {
                    DestinationID = x.DestinationID,
                    DestinationName = x.DestinationName,
                    StateID = x.StateID,
                    StateName = y.StateName,
                }).ToListAsync();
        }
        public async Task<GenTourPackageVM> SearchGenTourPackageListAsync(GenSearchModel search)
        {
            GenTourPackageVM model = new GenTourPackageVM();
            ConvertListToDT objList = new ConvertListToDT();
            DataTable subdt = new DataTable();
            try
            {
                //Converting subject list to datatable if record is present else send empty datatable
                if (search.TourType != null)
                {
                    List<IDModel> villList = search.TourType.Select(x => new IDModel()
                    {
                        ID = Convert.ToInt64(x)
                    }).ToList();
                    subdt = objList.ConvertIEnumerableToDataTable(villList);
                }
                else
                {
                    if (subdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "ID";
                        subdt.Columns.Add(col);
                    }
                }
                var parSubDT = new SqlParameter("@TourTypeTable", subdt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.IDType";

                var parStart = new SqlParameter("@Start", (search.PageNo - 1) * search.PageSize);
                var parEnd = new SqlParameter("@PageSize", search.PageSize);

                var parWhere = new SqlParameter("@Where", DBNull.Value);
                if (!(search.Where == null || search.Where == ""))
                    parWhere.Value = search.Where;
                // setting stored procedure OUTPUT value
                // This return total number of rows, and avoid two database call for data and total number of rows 
                var spOutput = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                model.PackageList = await db.Database.SqlQuery<GenTourPackageView>("udspGenTourPackageSearch @Start, @PageSize,@Where, @TourTypeTable, @TotalCount out",
                    parStart, parEnd, parWhere, parSubDT, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Bookings/enquiries
        public async Task<TourPackageBookEnquiryVM> GetTourPackageEnquiriesPagedAsync(int pageno, int pagesize, string searchterm)
        {
            TourPackageBookEnquiryVM model = new TourPackageBookEnquiryVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(searchterm == null || searchterm == ""))
                parSearchTerm.Value = searchterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.Enquiries = await db.Database.SqlQuery<TourPackageBookEnquiry>("udspTourPackageEnquiriesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<TourPackageBookEnquiry> GetEnquiryByCodeAsync(string code)
        {
            var parPackID = new SqlParameter("@EnquiryCode", code);
            try
            {
                return await db.Database.SqlQuery<TourPackageBookEnquiry>("select * from dbo.udfGetTourPackageEnquiryDtls(@EnquiryCode)", parPackID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<EnquiryItineraryView>> GetEnquiryItinerariesAsync(string code)
        {
            var parPackID = new SqlParameter("@EnquiryCode", code);
            try
            {
                return await db.Database.SqlQuery<EnquiryItineraryView>("select * from dbo.udfGetTourPackageEnquiryItineraries(@EnquiryCode)", parPackID).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<EnquiryActivitiesView>> GetEnquiryActivitiesAsync(string code)
        {
            var parPackID = new SqlParameter("@EnquiryCode", code);
            try
            {
                return await db.Database.SqlQuery<EnquiryActivitiesView>("select * from dbo.udfGetTourPackageEnquiryActivities(@EnquiryCode)", parPackID).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region package offer
        public async Task<IEnumerable<GenPackageOfferView>> GenGetOfferPackageListAsync()
        {
            try
            {
                List<GenPackageOfferView> model = new List<GenPackageOfferView>();
                model = await db.Database.SqlQuery<GenPackageOfferView>("udspGenOfferPackageList").ToListAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<GenTourPackageVM> GetGenOfferPackageListAsync(long POID, int pageno, int pagesize)
        {
            GenTourPackageVM model = new GenTourPackageVM();
            try
            {
                var parPOID = new SqlParameter("PackageOfferID", POID);
                var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
                var parEnd = new SqlParameter("@PageSize", pagesize);

                //var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
                //if (!(sterm == null || sterm == ""))
                //    parSearchTerm.Value = sterm;
                // setting stored procedure OUTPUT value
                // This return total number of rows, and avoid two database call for data and total number of rows 
                var spOutput = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                model.OfferPackageList = await db.Database.SqlQuery<GenOfferTourPackageView>("udspGenOfferPackagebyID @PackageOfferID, @Start, @PageSize, @TotalCount out",
                    parPOID, parStart, parEnd, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<PackageOfferVM> GetPackageOfferListAsync(int pageno, int pagesize, string sterm)
        {
            PackageOfferVM model = new PackageOfferVM();
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

            model.PackageOfferList = await db.Database.SqlQuery<PackageOfferView>("udspofferpackagepaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }

        public List<PackageDD> GetPackageListAsync()
        {
            List<PackageDD> obj = new List<PackageDD>();
            obj = db.Database.SqlQuery<PackageDD>("select PackageID,PackageName from utblTourPackages").ToList();
            return obj;
        }
        public async Task<IEnumerable<long>> GetOfferPackageAsync(long id)
        {
            string query = "select PackageID from utblPackageOfferMappings where PackageOfferID=" + id;
            return await db.Database.SqlQuery<long>(query).ToListAsync();
        }
        public async Task<string> SavePackageOfferAsync(SavePackageOffer model)
        {
            try
            {
                ConvertListToDT objDT = new ConvertListToDT();
                DataTable typeDt = new DataTable();

                var parPOffID = new SqlParameter("@PackageOfferID", model.PackageOffer.PackageOfferID);
                //var parPacID = new SqlParameter("@PackageID", DBNull.Value);
                //if (model.PackageID != null)
                //    parPacID = new SqlParameter("@PackageID", model.PackageID);
                var parOfferDiscount = new SqlParameter("@OfferDiscount", model.PackageOffer.OfferDiscount);
                var parOfferDesc = new SqlParameter("@OfferDesc", model.PackageOffer.OfferDesc);
                //var parPOffer = new SqlParameter("@OfferPercent", model.PackageOffer.OfferPercent);
                var parOfferImgPath = new SqlParameter("@OfferImagePath", model.PackageOffer.OfferImagePath);
                var parStart = new SqlParameter("@StartDate", model.PackageOffer.StartDate);
                var parEnd = new SqlParameter("@EndDate", model.PackageOffer.EndDate);

                List<IDModel> tyepList = model.PackageID.Select(x => new IDModel()
                {
                    ID = Convert.ToInt64(x)
                }).ToList();
                typeDt = objDT.ConvertIEnumerableToDataTable(tyepList);

                var parSubDT = new SqlParameter("@PackageOfferTable", typeDt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.IDType";

                return await db.Database.SqlQuery<string>("udspPackageOfferSave @PackageOfferID,@OfferDiscount, @OfferDesc, @OfferImagePath, @StartDate, @EndDate,@PackageOfferTable",
                     parPOffID, parOfferDiscount, parOfferDesc, parOfferImgPath, parStart, parEnd, parSubDT).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<utblPackageOffer> GetPackageOfferByIDAsync(long id)
        {
            return await db.utblPackageOffers.Where(x => x.PackageOfferID == id).FirstOrDefaultAsync();
        }
        //public async Task<string> EditPackageOfferAsync(EditPackageOffer model)
        //{
        //    try
        //    {
        //        var parPOffID = new SqlParameter("@PackageOfferID", model.PackageOffer.PackageOfferID);
        //        var parPacID = new SqlParameter("@PackageID", model.PackageOffer.PackageID);
        //        var parOfferDesc = new SqlParameter("@OfferDesc", model.PackageOffer.OfferDesc);
        //        var parPOffer = new SqlParameter("@OfferPercent", model.PackageOffer.OfferPercent);
        //        var parOfferImgPath = new SqlParameter("@OfferImagePath", model.PackageOffer.OfferImagePath);
        //        var parStart = new SqlParameter("@StartDate", model.PackageOffer.StartDate);
        //        var parEnd = new SqlParameter("@EndDate", model.PackageOffer.EndDate);
        //        return await db.Database.SqlQuery<string>("udspPackageOfferEdit @PackageOfferID,@PackageID,@OfferDesc, @OfferPercent,@OfferImagePath, @StartDate, @EndDate",
        //            parPOffID, parPacID, parOfferDesc, parPOffer, parOfferImgPath, parStart, parEnd).FirstOrDefaultAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Error: " + ex.Message;
        //    }
        //}

        public async Task<string> DeletePackageOfferAsync(long POID)
        {
            try
            {
                utblPackageOffer curObj = await db.utblPackageOffers.FindAsync(POID);
                db.utblPackageOffers.Remove(curObj);
                await db.SaveChangesAsync();
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

        #endregion

        #region General Destination
        public async Task<utblMstDestination> getDestinationSelectAsync(long id)
        {
            return await db.utblMstDestinations.Where(x => x.DestinationID == id).FirstOrDefaultAsync();
        }
        public async Task<GenTourPackageVM> GetDestinationPackageListAsync(string Destination,int pageno, int pagesize, string sterm)
        {
            GenTourPackageVM model = new GenTourPackageVM();
            try
            {
                var parDest = new SqlParameter("@DestinationName", Destination);
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

                model.PackageList = await db.Database.SqlQuery<GenTourPackageView>("udspGenDestinationPackageList @DestinationName,@Start, @PageSize,@SearchTerm, @TotalCount out",
                    parDest,parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion

    }
}
