using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XLIB_COMMON.Model
{
    public static class DataTableHelper
    {
        public static T ToObject<T>(this DataRow row) where T : class, new()
        {
            T obj = new T();

            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.Name.Contains("Nullable"))
                    {
                        if (!string.IsNullOrEmpty(row[prop.Name].ToString()))
                            prop.SetValue(obj, Convert.ChangeType(row[prop.Name],
                            Nullable.GetUnderlyingType(prop.PropertyType), null));
                        //else do nothing
                    }
                    else
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType), null);
                }
                catch
                {
                    continue;
                }
            }
            return obj;
        }
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    var obj = row.ToObject<T>();

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static List<T> DataTableColumnToList<T>(this DataTable table) where T : class
        {
            try
            {
                var list = table.Rows.OfType<DataRow>();
                var list2 = list.Select(dr => dr.Field<T>(0)).ToList();
                //var list2 = list.Select(dr => (T)(dr[0]));

                return list2;

                //List<T> list3 = new List<T>();

                //foreach(string val in list2)
                //{
                //    //byte[] b = 
                //    //list3.Add((T)(val));
                //}

                //return list2.Select(x => (int)x).ToList<int>();
                //return new List<T>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}