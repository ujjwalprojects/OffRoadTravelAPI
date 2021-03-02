using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourTravels.Entities.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using TourTravels.Entities.ViewModels;

namespace TourTravels.Entities.Dal
{
    public class dalCabConfig
    {
        EFDBContext db = new EFDBContext();

        #region Cab Types
        public async Task<IEnumerable<utblMstCabType>> GetCabTypesAsync()
        {
            return await db.utblMstCabTypes.ToListAsync();
        }
        public async Task<string> SaveCabTypeAsync(utblMstCabType model)
        {
            try
            {
                if (model.CabTypeID == 0)
                {
                    db.utblMstCabTypes.Add(model);
                    await db.SaveChangesAsync();
                    return "New Cab Type Added";
                }
                else
                {
                    utblMstCabType curObj = await db.utblMstCabTypes.FindAsync(model.CabTypeID);
                    curObj.CabTypeName = model.CabTypeName;
                    curObj.CabTypeDesc = model.CabTypeDesc;
                    curObj.BaseFare = model.BaseFare;
                    await db.SaveChangesAsync();
                    return "Cab Type Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message;
            }
        }
        public async Task<utblMstCabType> GetCabTypeByIDAsync(long id)
        {
            return await db.utblMstCabTypes.Where(x => x.CabTypeID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteCabTypeAsync(long id)
        {
            try
            {
                utblMstCabType curObj = await db.utblMstCabTypes.FindAsync(id);
                db.utblMstCabTypes.Remove(curObj);
                await db.SaveChangesAsync();
                return "Cab Type Details Removed";
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

        #region Cabs
        public async Task<CabVM> GetCabsAsync(int pageno, int pagesize, string sterm)
        {
            CabVM model = new CabVM();
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

            model.Cabs = await db.Database.SqlQuery<CabView>("udspMstCabPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveCabAsync(utblMstCab model)
        {
            try
            {

                var parCabID = new SqlParameter("@CabID", model.CabID);
                var parCabName = new SqlParameter("@CabName", model.CabName);
                var parCabNo = new SqlParameter("@CabNo", model.CabNo);
                var parTypeID = new SqlParameter("@CabTypeID", model.CabTypeID);
                var parHead = new SqlParameter("@CabHeadID", model.CabHeadID);

                return await db.Database.SqlQuery<string>("udspMstCabSave @CabID, @CabName, @CabNo, @CabTypeID, @CabHeadID",
                    parCabID, parCabName, parCabNo, parTypeID, parHead).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstCab> GetCabByIDAsync(long id)
        {
            return await db.utblMstCabs.Where(x => x.CabID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteCabAsync(long id)
        {
            try
            {
                utblMstCab curObj = await db.utblMstCabs.FindAsync(id);
                db.utblMstCabs.Remove(curObj);
                await db.SaveChangesAsync();
                return "Cab Details Removed";
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


        #region Drivers
        public async Task<DriverVM> GetDriversAsync(int pageno, int pagesize, string sterm)
        {
            DriverVM model = new DriverVM();
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

            model.Drivers = await db.Database.SqlQuery<utblMstCabDriver>("udspMstCabDriverPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveDriverAsync(utblMstCabDriver model)
        {
            try
            {
                if (model.DriverID == 0)
                {
                    db.utblMstCabDrivers.Add(model);
                    await db.SaveChangesAsync();
                    return "New Driver Added";
                }
                else
                {
                    utblMstCabDriver curObj = await db.utblMstCabDrivers.FindAsync(model.DriverID);
                    curObj.DriverName = model.DriverName;
                    curObj.DriverContact = model.DriverContact;
                    return "Driver Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstCabDriver> GetDriverByIDAsync(long id)
        {
            return await db.utblMstCabDrivers.Where(x => x.DriverID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteDriverAsync(long id)
        {
            try
            {
                utblMstCabDriver curObj = await db.utblMstCabDrivers.FindAsync(id);
                db.utblMstCabDrivers.Remove(curObj);
                await db.SaveChangesAsync();
                return "Driver Details Removed";
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

        #region Cab Heads
        public async Task<CabHeadVM> GetCabHeadsAsync(int pageno, int pagesize, string sterm)
        {
            CabHeadVM model = new CabHeadVM();
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

            model.CabHeads = await db.Database.SqlQuery<utblMstCabHead>("udspMstCabHeadPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveCabHeadAsync(utblMstCabHead model)
        {
            try
            {
                if (model.CabHeadID == 0)
                {
                    db.utblMstCabHeads.Add(model);
                    await db.SaveChangesAsync();
                    return "New Cab Head Added";
                }
                else
                {
                    utblMstCabHead curObj = await db.utblMstCabHeads.FindAsync(model.CabHeadID);
                    curObj.CabHeadName = model.CabHeadName;
                    curObj.CabHeadContact = model.CabHeadContact;
                    return "Cab Head Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstCabHead> GetCabHeadByIDAsync(long id)
        {
            return await db.utblMstCabHeads.Where(x => x.CabHeadID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteCabHeadAsync(long id)
        {
            try
            {
                utblMstCabHead curObj = await db.utblMstCabHeads.FindAsync(id);
                db.utblMstCabHeads.Remove(curObj);
                await db.SaveChangesAsync();
                return "Cab Head Details Removed";
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
        public async Task<IEnumerable<utblMstCabHead>> GetAllCabHeadsAysnc()
        {
            return await db.utblMstCabHeads.ToListAsync();
        }
        #endregion
    }
}
