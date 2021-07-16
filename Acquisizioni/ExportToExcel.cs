using System;
using System.Collections.Generic;
using System.Web;
using FutureFog.U4N.General;
using System.Data;
using Ribes.OPENgov.Utilities;
using OPENgovDL;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OPENgov.Acquisizioni
{
    public partial class ExportToExcel : System.Web.UI.Page
    {
        public DataSet CreateDataSet<T>(List<T> list)
        {
            //list is nothing or has nothing, return nothing (or add exception handling)
            if (list == null || list.Count == 0) { return null; }

            //get the type of the first obj in the list
            var obj = list[0].GetType();

            //now grab all properties
            var properties = obj.GetProperties();

            //make sure the obj has properties, return nothing (or add exception handling)
            if (properties.Length == 0) { return null; }

            //it does so create the dataset and table
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            //now build the columns from the properties
            var columns = new DataColumn[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                columns[i] = new DataColumn(properties[i].Name, properties[i].PropertyType);
            }

            //add columns to table
            dataTable.Columns.AddRange(columns);

            //now add the list values to the table
            foreach (var item in list)
            {
                //create a new row from table
                var dataRow = dataTable.NewRow();

                //now we have to iterate thru each property of the item and retrieve it's value for the corresponding row's cell
                var itemProperties = item.GetType().GetProperties();

                for (int i = 0; i < itemProperties.Length; i++)
                {
                    dataRow[i] = itemProperties[i].GetValue(item, null);
                }

                //now add the populated row to the table
                dataTable.Rows.Add(dataRow);
            }

            //add table to dataset
            dataSet.Tables.Add(dataTable);

            //return dataset
            return dataSet;
        }
        public DataSet CreateDataSet(List<List<string>> list, int MaxCol)
        {
            //list is nothing or has nothing, return nothing (or add exception handling)
            if (list == null || list.Count == 0) { return null; }

            //get the type of the first obj in the list
            var obj = list[0].GetType();

            //now grab all properties
            var properties = obj.GetProperties();

            //make sure the obj has properties, return nothing (or add exception handling)
            if (properties.Length == 0) { return null; }

            //it does so create the dataset and table
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            //now build the columns from the properties
            var columns = new DataColumn[MaxCol];
            for (int i = 0; i < MaxCol; i++)
            {
                columns[i] = new DataColumn("");
            }
            //add columns to table
            dataTable.Columns.AddRange(columns);

            //now add the list values to the table
            foreach (List<string> myRow in list)
            {
                //create a new row from table
                DataRow dataRow = dataTable.NewRow();

                //now we have to iterate thru each property of the item and retrieve it's value for the corresponding row's cell
                int i = 0; 
                foreach (string myCol in myRow)
                {
                    dataRow[i] = myCol;
                    i++;
                }

                //now add the populated row to the table
                dataTable.Rows.Add(dataRow);
            }
            //add table to dataset
            dataSet.Tables.Add(dataTable);

            //return dataset
            return dataSet;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="templateFile"></param>
        public void ExportDataSetToExcel(DataSet ds, string templateFile)
        {
            Global.Log.Write2(LogSeverity.Information, "ExportDataSetToExcel::" + templateFile);
            FileInfo file = new FileInfo(templateFile);
            HttpResponse response = HttpContext.Current.Response;

            response.Charset = "";

            Global.Log.Write2(LogSeverity.Information, "ExportDataSetToExcel HtmlTextWriter " + file.Name);
            HttpContext.Current.Response.ClearContent();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Global.Log.Write2(LogSeverity.Information, "ExportDataSetToExcel HtmlTextWriter ");
                    // instantiate a datagrid
                    DataGrid dg = new DataGrid();
                    dg.BorderWidth=0;
                    dg.DataSource = ds.Tables[0];
                    dg.DataBind();
            //        string styles1 = " <style>    .text {padding-top:1px;" 
            //+ "     padding-right:1px;" 
            //+ "     padding-left:1px;" 
            //+ "     mso-ignore:padding;" 
            //+ "     color:windowtext;" 
            //+ "     font-size:12.0pt;" 
            //+ "     font-weight:400;" 
            //+ "     font-style:normal;" 
            //+ "     text-decoration:none;" 
            //+ "     font-family:·s2OcuAe, serif;" 
            //+ "     mso-font-charset:136; " 
            //+ "     mso-number-format:general; " 
            //+ "     text-align:general; " 
            //+ "     vertical-align:middle; " 
            //+ "     mso-background-source:auto; " 
            //+ "     mso-pattern:auto; " 
            //+ "      white-space:nowrap;} "
            //+ " </style> ";
            //        dg.Style.Add("text", styles1);
                    dg.RenderControl(htw);
                    //HttpContext.Current.Response.Write(sw.ToString());
                    //response.Write(styles1);
                    response.Write(sw.ToString());
                    //FileInfo fi = new FileInfo(Server.MapPath("StyleSheet.css"));
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //StreamReader sr = fi.OpenText();
                    //while (sr.Peek() >= 0)
                    //{
                    //    sb.Append(sr.ReadLine());
                    //}
                    //sr.Close();
                    //try
                    //{
                    //   //Response.Write("<html><head><style type='text/css'>" + sb.ToString() + "</style></head>" + sw.ToString() + "</html>");
                    //    Response.End();

                    //}
                    //catch (Exception err) {
                    //    string e = err.InnerException.ToString();
                    //}
                 
                  
                    Global.Log.Write2(LogSeverity.Information, "ExportDataSetToExcel End ");
                    response.End();
                }
            }
        }
    }
}