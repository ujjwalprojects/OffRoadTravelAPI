using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TourTravels.Entities.Utility
{
    public class ConvertListToDT
    {
        public DataTable ConvertIEnumerableToDataTable<T>(IEnumerable<T> ItemList)
        {
            DataTable dtTable = new DataTable();
            Type temp = typeof(T);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor pd in props)
            {
                //create a data column for each employee attendance
                DataColumn dc = new DataColumn(pd.Name, Nullable.GetUnderlyingType(pd.PropertyType) ?? pd.PropertyType);//check if nullable datatype (e.g datetime)
                dtTable.Columns.Add(dc);
            }
            //now we iterate through all the items, take the corresponding values and add a new row in dt
            foreach (T item in ItemList)
            {
                DataRow dr = dtTable.NewRow();
                for (int property = 0; property < props.Count; property++)
                {
                    dr[property] = props[property].GetValue(item) ?? DBNull.Value;
                }
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }
    }
}
