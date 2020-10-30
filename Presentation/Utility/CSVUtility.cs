using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Net.Mime;

namespace Presentation.Utiliity
{
    internal static class DataTableUtility
    {
        public static void ToCSV(this DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(";");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        public static void ToExcel(this DataTable dtDataTable,string strFilePath)
        {
            var workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("WorkOrder");
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                worksheet.Cell(1, i+1).Value = dtDataTable.Columns[i];
                
            }
            int row = 2;
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            worksheet.Cell(row, i+1).Value = value;
                            
                        }
                        else
                        {
                            worksheet.Cell(row, i+1).Value = dr[i].ToString();
                           


                        }
                    }
                  
                       
                    
                }
                row++;
            }
           
            workbook.SaveAs(strFilePath);
        }
    }
}