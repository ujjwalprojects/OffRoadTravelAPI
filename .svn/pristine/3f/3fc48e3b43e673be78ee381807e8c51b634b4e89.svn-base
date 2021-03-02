using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using TourTravels.Entities.ViewModels;
using System.Data;
using TourTravels.Entities.Utility;

namespace TourTravels.Entities.Dal
{
    public class dalHotelConfig
    {
        EFDBContext db = new EFDBContext();

        #region Hotel Types

        public async Task<IEnumerable<utblMstHotelType>> GetHotelTypesAsync()
        {
            return await db.utblMstHotelTypes.ToListAsync();
        }
        public async Task<string> SaveHotelTypeAsync(utblMstHotelType model)
        {
            try
            {
                if (model.HotelTypeID == 0)
                {
                    db.utblMstHotelTypes.Add(model);
                    await db.SaveChangesAsync();
                    return "New Hotel Type Added";
                }
                else
                {
                    utblMstHotelType curObj = await db.utblMstHotelTypes.FindAsync(model.HotelTypeID);
                    curObj.HotelTypeName = model.HotelTypeName;
                    curObj.BaseFare = model.BaseFare;
                    await db.SaveChangesAsync();
                    return "Hotel Type Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstHotelType> GetHotelTypeByIDAsync(long id)
        {
            return await db.utblMstHotelTypes.Where(x => x.HotelTypeID == id).FirstOrDefaultAsync();
        }

        public async Task<string> DeleteHotelTypeAsync(long id)
        {
            try
            {
                utblMstHotelType curObj = await db.utblMstHotelTypes.FindAsync(id);
                db.utblMstHotelTypes.Remove(curObj);
                await db.SaveChangesAsync();
                return "Hotel Type Details Removed";
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

        #region Hotels
        public async Task<HotelVM> GetHotelsAsync(int pageno, int pagesize, string sterm)
        {
            HotelVM model = new HotelVM();
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

            model.Hotels = await db.Database.SqlQuery<HotelView>("udspMstHotelPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveHotelAsync(HotelSaveModel model)
        {
            try
            {
                ConvertListToDT objDT = new ConvertListToDT();
                DataTable typeDt = new DataTable();

                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parDestID = new SqlParameter("@DestinationID", model.DestinationID);
                var parHotelName = new SqlParameter("@HotelName", model.HotelName);
                var parHotelAddress = new SqlParameter("@HotelAddress", model.HotelAddress);
                var parHotelContact = new SqlParameter("@HotelContact", model.HotelContact);
                var parHotelEmail = new SqlParameter("@HotelEmail", model.HotelEmail ?? "");

                List<IDModel> tyepList = model.HotelTypes.Select(x => new IDModel()
                {
                    ID = Convert.ToInt64(x)
                }).ToList();
                typeDt = objDT.ConvertIEnumerableToDataTable(tyepList);

                var parSubDT = new SqlParameter("@HotelTypeTable", typeDt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.IDType";

                return await db.Database.SqlQuery<string>("udspMstHotelSave @HotelID, @DestinationID, @HotelName, @HotelAddress, @HotelContact, @HotelEmail, @HotelTypeTable",
                    parHotelID, parDestID, parHotelName, parHotelAddress, parHotelContact, parHotelEmail, parSubDT).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstHotel> GetHotelByIDAsync(long id)
        {
            return await db.utblMstHotels.Where(x => x.HotelID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<long>> GetHotelHotelTypesAsync(long id)
        {
            string query = "select HotelTypeID from utblHotelTypeMappings where HotelID=" + id;
            return await db.Database.SqlQuery<long>(query).ToListAsync();
        }
        public async Task<string> DeleteHotelAsync(long id)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", id);

                return await db.Database.SqlQuery<string>("udspMstHotelDelete @HotelID", parHotelID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}
