
using Presentation.Utiliity.Interface;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;


namespace Presentation.Utiliity
{
    public class Export : IExport
    {
        public async Task<byte[]> CreateCSV(WorkOrderDetail model)
        {
            DataTable table = new DataTable();
            if (model != null)
            {
                    object[] values = new object[16];
                    int i = 0;
                    //adding rows rows to Data table
                    foreach (var prop in model.GetType().GetProperties())
                    {
                      
                        table.Columns.Add(prop.Name.Replace("_"," "),typeof(string));
                    if (prop.Name.Equals("Attachment")) {
                        var item = prop.GetValue(model) as List<KeyValuePair<string, string>>;
                        string y = string.Empty;
                        item.ForEach(x => y = y + x.Key);
                        values[i++] = y;
                    }
                    else if (prop.PropertyType.Name.Equals("String"))
                        values[i++] = prop.GetValue(model);
                    }
                    table.Rows.Add(values);
                }
                
            
            string guid = "Templates/"+Guid.NewGuid().ToString()+".csv";
            File.Copy("Templates/WODetailTemplate.csv",guid);
            table.ToCSV("Templates/guid.csv");
            byte[] filedata = System.IO.File.ReadAllBytes(guid);
            File.Delete(guid);
            return filedata;
        }
       
    }
   
}
