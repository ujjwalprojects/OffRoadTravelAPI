using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TourTravels.Entities.Models;
using TourTravels.Entities.Utility;
using System.Data;
using TourTravels.Entities.ViewModels;

namespace TourTravels.Entities.Dal
{
    public class dalClientEnquiry
    {
        EFDBContext objDB = new EFDBContext();

        public async Task<string> SaveDirectClientEnqAsync(utblClientEnquirie model)
        {
            try
            {
                var parClientName = new SqlParameter("@ClientName", model.ClientName);
                var parClientEmail = new SqlParameter("@ClientEmail", model.ClientEmail);
                var parClientPhoneNo = new SqlParameter("@ClientPhoneNo", model.ClientPhoneNo);
                var parRefPackageID = new SqlParameter("@RefPackageID", model.RefPackageID);
                var parNoOfDays = new SqlParameter("@NoOfDays", model.NoOfDays);
                var parRemarks = new SqlParameter("@Remarks", model.Remarks ?? "");
                var parDateOfArrival = new SqlParameter("@DateOfArrival", model.DateOfArrival);
                var parNoOfAdult = new SqlParameter("@NoOfAdult", model.NoOfAdult);
                var parNoOfChildren = new SqlParameter("@NoOfChildren", model.NoOfChildren);
                var parHotelTypeID = new SqlParameter("@HotelTypeID", DBNull.Value);
                if (model.HotelTypeID != null || model.HotelTypeID != 0)
                {
                    parHotelTypeID = new SqlParameter("@HotelTypeID", model.HotelTypeID);
                }
                var parCabTypeID = new SqlParameter("@CabTypeID", DBNull.Value);
                if (model.CabTypeID != null || model.CabTypeID != 0)
                {
                    parCabTypeID = new SqlParameter("@CabTypeID", model.CabTypeID);
                }
                var parIsDirectBooking = new SqlParameter("@IsDirectBooking", model.IsDirectBooking);
                var parStatus = new SqlParameter("@Status", model.Status);
                var parTransDate = new SqlParameter("@TransDate", model.TransDate);

                if (model.EnquiryCode == "" || model.EnquiryCode == null)
                {
                    return await objDB.Database.SqlQuery<string>("udspDirectClientEnquiryInsert @ClientName, @ClientEmail,@ClientPhoneNo,@RefPackageID,@NoOfDays,@Remarks,@DateOfArrival,@NoOfAdult,@NoOfChildren,@HotelTypeID,@CabTypeID,@IsDirectBooking,@Status,@TransDate",
                        parClientName, parClientEmail, parClientPhoneNo, parRefPackageID, parNoOfDays, parRemarks, parDateOfArrival, parNoOfAdult, parNoOfChildren, parHotelTypeID, parCabTypeID, parIsDirectBooking, parStatus, parTransDate).FirstOrDefaultAsync();
                }
                else
                {
                    var parEnquiryCode = new SqlParameter("@EnquiryCode", model.EnquiryCode);
                    return await objDB.Database.SqlQuery<string>("udspCustomClientEnquiryUpdate @EnquiryCode, @ClientName, @ClientEmail,@ClientPhoneNo,@NoOfDays,@DateOfArrival,@NoOfAdult,@NoOfChildren,@HotelTypeID,@CabTypeID,@Status,@TransDate",
                      parEnquiryCode, parClientName, parClientEmail, parClientPhoneNo, parNoOfDays, parDateOfArrival, parNoOfAdult, parNoOfChildren, parHotelTypeID, parCabTypeID, parStatus, parTransDate).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> UpdateClientEnquiryStatusAsync(ClientEnquiryView model)
        {
            try
            {
                utblClientEnquirie cmodel = await objDB.utblClientEnquiries.Where(x=> x.EnquiryCode == model.EnquiryCode).FirstOrDefaultAsync();
                cmodel.Status = "Submitted";
                await objDB.SaveChangesAsync();
                return "Status Updated";
            }
            catch (SqlException ex)
            {
                return "Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> SaveItinerariesAsync(IEnumerable<utblClientEnquiryItinerarie> model)
        {
            try
            {
                var parEnquiryCode = new SqlParameter("@enqirycode", model.ElementAt(0).EnquiryCode);

                ConvertListToDT objDT = new ConvertListToDT();
                DataTable dt = objDT.ConvertIEnumerableToDataTable(model);

                var parSubDT = new SqlParameter("@ClientItineraryTable", dt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.ClientItineraryType";

                return await objDB.Database.SqlQuery<string>("udspClientEnquiryItinerarySave @enqirycode, @ClientItineraryTable", parEnquiryCode, parSubDT).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> SaveClientEnqActAsync(IEnumerable<utblClientEnquiryActivitie> model)
        {
            try
            {
                var parEnquiryCode = new SqlParameter("@enquirycode", model.ElementAt(0).EnquiryCode);
                ConvertListToDT objList = new ConvertListToDT();
                DataTable actdt = new DataTable();

                //Converting subject list to datatable if record is present else send empty datatable
                if (model != null)
                {
                    actdt = objList.ConvertIEnumerableToDataTable(model);
                }
                else
                {
                    if (actdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "ClientActivityID";
                        actdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "EnquiryCode";
                        actdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "RefPackageID";
                        actdt.Columns.Add(col2);
                        DataColumn col3 = new DataColumn();
                        col3.ColumnName = "ActivityID";
                        actdt.Columns.Add(col3);
                    }
                }

                var parActDT = new SqlParameter("@ClientActivityTable", actdt);
                parActDT.SqlDbType = SqlDbType.Structured;
                parActDT.TypeName = "dbo.ClientActivityType";


                return await objDB.Database.SqlQuery<string>("udspClientEnquiryActivitySave  @enquirycode, @CLientActivityTable",
                    parEnquiryCode, parActDT).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblClientEnquirie> GetClientEnquiryInfoAsync(string eqcode)
        {
            return await objDB.utblClientEnquiries.Where(x => x.EnquiryCode == eqcode).FirstOrDefaultAsync();
        }
        public async Task <List<utblClientEnquiryItinerarie>> GetClientEnqItinerarylistAsync(string eqcode)
        {
            return await objDB.utblClientEnquiryItineraries.Where(x=> x.EnquiryCode == eqcode).ToListAsync();
        }
        public async Task<List<utblClientEnquiryActivitie>> GetClientEnqActivitielistAsync(string eqcode)
        {
            return await objDB.utblClientEnquiryActivites.Where(x => x.EnquiryCode == eqcode).ToListAsync();
        }
        public async Task<IEnumerable<ClientEnqActivities>> GetClientEnqActivitiesAsync(string eqcode)
        {
            try
            {
                var pareqcode = new SqlParameter("@enquirycode", eqcode);
                return await objDB.Database.SqlQuery<ClientEnqActivities>("select * from dbo.udfGetClientEnquiryActivities(@enquirycode)", pareqcode).ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        //methods for retrieving details of client enquiry for custom package configuration summary
        public async Task<ClientEnquiryView> GetClientEnqView(string eqcode)
        {
            var pareqcode = new SqlParameter("@enquirycode", eqcode);
            return await objDB.Database.SqlQuery<ClientEnquiryView>("select * from dbo.udfGetClientEnquiryView(@enquirycode)", pareqcode).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ClientEnquiryItineraryView>> GetClientEnqItinerariesView(string eqcode)
        {
            var pareqcode = new SqlParameter("@enquirycode", eqcode);
            return await objDB.Database.SqlQuery<ClientEnquiryItineraryView>("select * from dbo.udfGetClientEnquiryItineraries(@enquirycode)", pareqcode).ToListAsync();
        }
    }
}
