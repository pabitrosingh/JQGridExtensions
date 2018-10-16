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
    public class SalesController : Controller
    {
        //
        // GET: /Sales/
        [HttpGet]
        public JsonResult GetSalesOrderHeaderRecords(JQGridSettings JQGrid)
        {
            int TotapPages = 0;
            int TotalRecords = 0;
            IEnumerable<SalesOrderHeader> SOHIE = null;
            IQueryable<SalesOrderHeader> SOHIQ = null;

            using (SalesEntities DB = new SalesEntities())
            {
                DB.Configuration.LazyLoadingEnabled = false;
                DB.Configuration.ProxyCreationEnabled = false;


                #region :::Soring-Searching:::
                if (JQGrid._search)
                {
                    JQGridFilters _filters = JsonConvert.DeserializeObject<JQGridFilters>(JQGrid.filters);
                    SOHIQ = DB.SalesOrderHeaders.AsQueryable().JQGridToolBarSearch(_filters);
                }
                else
                {
                    SOHIQ = DB.SalesOrderHeaders.AsQueryable().JQGridSorting(JQGrid.sidx, JQGrid.sord);
                }
                #endregion

                #region:::Pagination
                SOHIE = SOHIQ.AsEnumerable();
                try
                {
                    TotapPages = Convert.ToInt32(Math.Ceiling((double)SOHIE.Count() / (double)JQGrid.rows));
                    TotalRecords = SOHIE.Count();
                    SOHIE = SOHIE.Skip((JQGrid.page - 1) * JQGrid.rows).Take(JQGrid.rows).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion
            }

            var jsonReader = new
            {
                root = SOHIE,
                page = JQGrid.page,
                total = TotapPages,
                records = TotalRecords
            };


            return Json(jsonReader, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalesOrderDetailerRecords(JQGridSettings JQGrid)
        {
            int TotapPages = 0;
            int TotalRecords = 0;

            int SalesOrderID = int.Parse(Request.QueryString["SalesOrderID"]);

            IEnumerable<SalesOrderDetail> SOHIE = null;
            IQueryable<SalesOrderDetail> SOHIQ = null;

            using (SalesEntities DB = new SalesEntities())
            {
                DB.Configuration.LazyLoadingEnabled = false;
                DB.Configuration.ProxyCreationEnabled = false;

                SOHIQ = DB.SalesOrderDetails.AsQueryable().Where(d => d.SalesOrderID == SalesOrderID).Select(d => d);

                #region :::Soring-Searching:::
                if (JQGrid._search)
                {
                    JQGridFilters _filters = JsonConvert.DeserializeObject<JQGridFilters>(JQGrid.filters);
                    SOHIQ = SOHIQ.JQGridToolBarSearch(_filters);
                }
                else
                {
                    SOHIQ = SOHIQ.JQGridSorting(JQGrid.sidx, JQGrid.sord);
                }
                #endregion

                #region:::Pagination
                SOHIE = SOHIQ.AsEnumerable();
                try
                {
                    TotapPages = Convert.ToInt32(Math.Ceiling((double)SOHIE.Count() / (double)JQGrid.rows));
                    TotalRecords = SOHIE.Count();
                    SOHIE = SOHIE.Skip((JQGrid.page - 1) * JQGrid.rows).Take(JQGrid.rows).ToList();

                }
                catch (Exception e)
                {
                    throw;
                }
                #endregion
            }

            var jsonReader = new
            {
                root = SOHIE,
                page = JQGrid.page,
                total = TotapPages,
                records = TotalRecords
            };


            return Json(jsonReader, JsonRequestBehavior.AllowGet);
        }


        //For Advanced Search
        [HttpGet]
        public JsonResult GetSalesOrderHeaderRecords1(JQGridSettings JQGrid)
        {
            int TotapPages = 0;
            int TotalRecords = 0;
            IEnumerable<SalesOrderHeader> SOHIE = null;
            IQueryable<SalesOrderHeader> SOHIQ = null;

            using (SalesEntities DB = new SalesEntities())
            {
                DB.Configuration.LazyLoadingEnabled = false;
                DB.Configuration.ProxyCreationEnabled = false;


                #region :::Soring-Searching:::
                if (JQGrid._search)
                {
                    JQGridAdvanceFilter _filters = JsonConvert.DeserializeObject<JQGridAdvanceFilter>(JQGrid.filters);
                    SOHIQ = DB.SalesOrderHeaders.AsQueryable().JQGridAdvanceSearch(_filters);
                }
                else
                {
                    SOHIQ = DB.SalesOrderHeaders.AsQueryable().JQGridSorting(JQGrid.sidx, JQGrid.sord);
                }
                #endregion

                #region:::Pagination
                SOHIE = SOHIQ.AsEnumerable();
                try
                {
                    TotapPages = Convert.ToInt32(Math.Ceiling((double)SOHIE.Count() / (double)JQGrid.rows));
                    TotalRecords = SOHIE.Count();
                    SOHIE = SOHIE.Skip((JQGrid.page - 1) * JQGrid.rows).Take(JQGrid.rows).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion
            }

            var jsonReader = new
            {
                root = SOHIE,
                page = JQGrid.page,
                total = TotapPages,
                records = TotalRecords
            };


            return Json(jsonReader, JsonRequestBehavior.AllowGet);
        }

    }
}
