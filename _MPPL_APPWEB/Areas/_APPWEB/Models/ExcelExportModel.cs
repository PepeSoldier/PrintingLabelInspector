using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{
    public static class ExcelExportModel
    {
        public static void ExportData(object data, HttpResponseBase Response, string fileName)
        {
            //List<object> pdoList = repoDemand.GetListByYearWeek(yearWeek, productionType).ToList();
            var gv = new GridView();
            gv.DataSource = data;// pdoList;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.Write("ąęćżźńółĄŚŻŹĆŃŁÓĘ");
            Response.Write('\uFEFF');
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", fileName));
            Response.ContentType = "application/ms-excel";
            //Response.Charset = "";

            //Response.ContentType = "text/rtf; charset=UTF-8";

            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
    }
}