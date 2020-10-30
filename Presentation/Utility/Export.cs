using Presentation.Utiliity.Interface;
using Presentation.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Presentation.Utiliity
{
    public class Export<T> : IExport<T>
    {
        private bool isColumnAdded = false;

        public async Task<byte[]> CreateListCSV(List<T> model)
        {
            DataTable table = new DataTable();
            if (model != null)
            {
                foreach (var item in model)
                {
                    AddToDatatable(item, table, ref isColumnAdded);
                }
            }

            string guid = "Templates/" + Guid.NewGuid().ToString() + ".csv";
            File.Copy("Templates/WODetailTemplate.csv", guid);
            table.ToCSV(guid);
            byte[] filedata = await System.IO.File.ReadAllBytesAsync(guid);
            File.Delete(guid);
            return filedata;
        }

        private static void AddToDatatable(T model, DataTable table, ref bool isColumnAdded)
        {
            int len = model.GetType().GetFilteredProperties().Length;
            object[] values = new object[len];
            int i = 0;
            //adding rows rows to Data table
            foreach (var prop in model.GetType().GetFilteredProperties())
            {
                if (!isColumnAdded)
                    table.Columns.Add(prop.Name.Replace("_", " "), typeof(string));
                if (prop.PropertyType.Name.Equals("List`1"))
                {
                    string y = string.Empty;

                    if (prop.GetValue(model) != null)
                    {
                        try
                        {
                            var item = prop.GetValue(model) as List<KeyValuePair<string, string>>;
                            item.ForEach(x => y = y + x.Key + ",");
                        }
                        catch (Exception)
                        {
                            y = prop.GetValue(model) is List<string> ? string.Join(",", prop.GetValue(model) as List<string>) : "";
                        }
                    }
                    values[i++] = y;
                }
                else
                    values[i++] = prop.GetValue(model) != null ? prop.GetValue(model).ToString() : "";
            }
            table.Rows.Add(values);
            isColumnAdded = true;
        }

        public async Task<byte[]> CreateCSV(T model)
        {
            DataTable table = new DataTable();
            if (model != null)
            {
                AddToDatatable(model, table, ref isColumnAdded);
            }

            string guid = "Templates/" + Guid.NewGuid().ToString() + ".csv";
            File.Copy("Templates/WODetailTemplate.csv", guid);
            table.ToCSV(guid);
            byte[] filedata = await System.IO.File.ReadAllBytesAsync(guid);
            File.Delete(guid);
            return filedata;
        }
        public async Task<byte[]> CreateExcel(T model)
        {
            DataTable table = new DataTable();
            if (model != null)
            {
                AddToDatatable(model, table, ref isColumnAdded);
            }

            string guid = "Templates/" + Guid.NewGuid().ToString()+ ".xlsx";
            table.ToExcel(guid);
            byte[] filedata = await System.IO.File.ReadAllBytesAsync(guid);
            File.Delete(guid);
            return filedata;
        }

        public async Task<byte[]> CreateListExcel(List<T> model)
        {
            DataTable table = new DataTable();
            if (model != null)
            {
                foreach (var item in model)
                {
                    AddToDatatable(item, table, ref isColumnAdded);
                }
            }
            string guid = "Templates/" + Guid.NewGuid().ToString()+ ".xlsx";
            table.ToExcel(guid);
            byte[] filedata = await System.IO.File.ReadAllBytesAsync(guid);
            File.Delete(guid);
            return filedata;
        }
    }
}