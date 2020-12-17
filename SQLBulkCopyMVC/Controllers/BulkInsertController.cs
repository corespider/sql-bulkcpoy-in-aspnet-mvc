using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQLBulkCopyMVC.Controllers
{
    public class BulkInsertController : Controller
    {
        // GET: BulkInsert
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string x) {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection sqlConn = new SqlConnection(cs))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/StudentFile.xml"));
                    DataTable dtStudentMaster = ds.Tables["Student"];
                    sqlConn.Open();
                    using (SqlBulkCopy sqlbc = new SqlBulkCopy(sqlConn))
                    {
                        sqlbc.DestinationTableName = "StudentInfo";
                        sqlbc.ColumnMappings.Add("Name", "Name");
                        sqlbc.ColumnMappings.Add("Phone", "Phone");
                        sqlbc.ColumnMappings.Add("Address", "Address");
                        sqlbc.ColumnMappings.Add("Class", "Class");
                        sqlbc.WriteToServer(dtStudentMaster);
                        Response.Write("Bulk data stored successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
    }
}