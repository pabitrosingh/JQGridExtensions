using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQGridExtensions;
using JQGridDemo.Models;
using Newtonsoft.Json;

namespace JQGridDemo.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Sales/
        [HttpGet]
        public JsonResult GetEmployeeCollection(JQGridSettings JQGrid)
        {
            int TotapPages = 0;
            int TotalRecords = 0;
            IEnumerable<Employee> IEEmployee = null;
            IQueryable<Employee> IQEmployee = null;

            using (Employee DB = new Employee())
            {

                #region :::Soring-Searching:::
                if (JQGrid._search)
                {
                    //for Filter Toolbar 
                    JQGridFilters _filters = JsonConvert.DeserializeObject<JQGridFilters>(JQGrid.filters);
                    IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridToolBarSearch(_filters);

                    //for Advanced Search
                    //JQGridAdvanceFilter _filters1 = JsonConvert.DeserializeObject<JQGridAdvanceFilter>(JQGrid.filters);
                    //IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridAdvancedSearch(_filters1);
                }
                else
                {
                    IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridSorting(JQGrid.sidx, JQGrid.sord);
                }
                #endregion

                #region:::Pagination
                IEEmployee = IQEmployee.AsEnumerable();
                try
                {
                    TotapPages = Convert.ToInt32(Math.Ceiling((double)IEEmployee.Count() / (double)JQGrid.rows));
                    TotalRecords = IEEmployee.Count();
                    IEEmployee = IEEmployee.Skip((JQGrid.page - 1) * JQGrid.rows).Take(JQGrid.rows).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion
            }

            var jsonReader = new
            {
                root = IEEmployee,
                page = JQGrid.page,
                total = TotapPages,
                records = TotalRecords
            };


            return Json(jsonReader, JsonRequestBehavior.AllowGet);
        }
    }
}
