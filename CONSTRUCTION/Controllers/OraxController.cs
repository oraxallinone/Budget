using CONSTRUCTION.Models;
using HotelBill.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CONSTRUCTION.Controllers
{
    public class OraxController : Controller
    {
        Bill_DBEntities db = new Bill_DBEntities();
        #region ----------------------    -------------
        #endregion ------------------------------------
        


        #region ----------------------  entry  --------
        public ActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllExpensive(GetExpensiveViewModel data)
        {

            var monthMaster = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
            var fromDate = monthMaster.fromDate; //AllFromDate(data.forMonth);
            var toDate = monthMaster.todate;//AllToDate(data.forMonth);

            IEnumerable<tblBudget> allList = null;
            if (data.group1 != null)
            {
                allList = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate && m.group1 == data.group1).OrderBy(d => d.createdDate).ToList();
            }
            else if (data.group2 != null)
            {
                allList = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate && m.group2 == data.group2).OrderBy(d => d.createdDate).ToList();
            }
            else if (data.group3 != null)
            {
                allList = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate && m.group3 == data.group3).OrderBy(d => d.createdDate).ToList();
            }
            else
            {
                allList = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate).OrderBy(d => d.createdDate).ToList();
            }


            List<RenderExpensiveViewModel2> list1 = new List<RenderExpensiveViewModel2>();

            foreach (var s in allList)
            {
                RenderExpensiveViewModel2 lst = new RenderExpensiveViewModel2();
                lst.id = s.id.ToString();
                lst.group1 = s.group1;
                lst.group2 = s.group2;
                lst.group3 = s.group3;
                lst.details = s.details;
                lst.price = s.price.ToString();
                lst.createdDate = Convert.ToDateTime(s.createdDate ?? DateTime.Now).ToString("yyyy-MM-dd");

                list1.Add(lst);
            }

            var result = new { monthTransList = list1, monthMaster = monthMaster };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion ------------------------------------


        #region ----------------------  graph  ---------
        public ActionResult Graph()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAsBudgetRule(GetExpensiveViewModel data)
        {

            var getDate = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
            var fromDate = getDate.fromDate; //AllFromDate(data.forMonth);
            var toDate = getDate.todate;//AllToDate(data.forMonth);
            //var fromDate = AllFromDate(data.forMonth);
            //var toDate = AllToDate(data.forMonth);

            var monthMaster = db.tblBudgetMasters.Where(m => m.month == data.forMonth).FirstOrDefault();

            var groupByBudgetRule = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate).GroupBy(b => b.group1)
                                    .Select(g => new
                                    {
                                        label = g.Key,
                                        y = g.Sum(b => b.price)
                                    })
                                    .ToList();





            var result = new { monthMaster = monthMaster, groupByBudgetRule = groupByBudgetRule };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAllDataBudget(GetExpensiveViewModel data)
        {

            var getDate = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
            var fromDate = getDate.fromDate; //AllFromDate(data.forMonth);
            var toDate = getDate.todate;//AllToDate(data.forMonth);
            //var fromDate = AllFromDate(data.forMonth);
            //var toDate = AllToDate(data.forMonth);



            var groupByAmt = db.tblBudgets.Where(m => m.createdDate >= fromDate && m.createdDate <= toDate).GroupBy(b => b.group2)
                                    .Select(g => new
                                    {
                                        label = g.Key,
                                        y = g.Sum(b => b.price)
                                    })
                                    .ToList();
            return Json(groupByAmt, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GraphColumn()
        {


            return View();
        }
        #endregion ------------------------------------



        [HttpPost]//where data comes
        public ActionResult GetColumnChart(GetExpensiveViewModel data)
        {

            var monthMstr = db.tblBudgetMasters.Where(v => v.month != "all").ToList().OrderBy(m => m.todate);


            List<BarGraph> barGraphData = new List<BarGraph>();
            var index = 0;

            IEnumerable<BarGraphdbArrange> datas = null;

            foreach (var ss in monthMstr)
            {
                if (data.group1 != null)
                {
                    datas = db.tblBudgets
                    .Where(b => b.createdDate > ss.fromDate && b.createdDate < ss.todate && b.group1 == data.group1)
                    .GroupBy(c => c.group1)
                    .Select(g => new BarGraphdbArrange { y = g.Sum(c => c.price), label = ss.month })
                    .ToList();
                }
                if (data.group2 != null)
                {
                    datas = db.tblBudgets
                    .Where(b => b.createdDate > ss.fromDate && b.createdDate < ss.todate && b.group2 == data.group2)
                    .GroupBy(c => c.group2)
                    .Select(g => new BarGraphdbArrange { y = g.Sum(c => c.price), label = ss.month })
                    .ToList();
                }
                if (data.group3 != null)
                {
                    datas = db.tblBudgets
                    .Where(b => b.createdDate > ss.fromDate && b.createdDate < ss.todate && b.group3 == data.group3)
                    .GroupBy(c => c.group3)
                    .Select(g => new BarGraphdbArrange { y = g.Sum(c => (c.price == null ? 0m : c.price)), label = ss.month })
                    .ToList();
                }

                index = index + 1;

                if (datas.Count() == 0)
                {
                    BarGraph dat = new BarGraph();
                    dat.x = index;
                    dat.y = 0;
                    dat.label = ss.monthName + "-" + "0";
                    barGraphData.Add(dat);
                }
                else
                {
                    int? intValue1 = datas.FirstOrDefault().y.HasValue ? (int?)Convert.ToInt32(datas.FirstOrDefault().y.Value) : null;
                    int intValue2 = 0;
                    if (intValue1 != null)
                    {
                        intValue2 = intValue1 ?? 0;
                    }

                    BarGraph dat = new BarGraph();
                    dat.x = index;
                    dat.y = intValue2;
                    dat.label = MonthName(Convert.ToInt32(datas.FirstOrDefault().label)) + "-" + ((int)datas.FirstOrDefault().y).ToString();
                    barGraphData.Add(dat);
                }
            }



            var result = new { monthMstr = monthMstr, barGraphData = barGraphData };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ColumnGraphList()
        {
            return View();
        }

        public string MonthName(int i)
        {
            string monthName = "";
            switch (i)
            {
                case 1:
                    monthName = "Jan";
                    break;
                case 2:
                    monthName = "Feb";
                    break;
                case 3:
                    monthName = "Mar";
                    break;
                case 4:
                    monthName = "Apr";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "Jun";
                    break;
                case 7:
                    monthName = "Jul";
                    break;
                case 8:
                    monthName = "Aug";
                    break;
                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;
                case 11:
                    monthName = "Nov";
                    break;
                case 12:
                    monthName = "Dec";
                    break;
            }
            return monthName;
        }

        #region ----------------------  common  --------
        #endregion ------------------------------------






        [HttpPost]
        public ActionResult AddExpensive(CreateExpensiveViewModel data)
        {
            try
            {
                string dateNow = data.createdDate?.ToString("yyyy-MM-dd hh:mm:ss"); //data.createdDate.ToString();
                string proccc = data.price.ToString();


                string param = "@group2,@createdDate,@details,@price,@group1";
                string paramvalue = data.group2 + "," + dateNow + "," + data.details + "," + proccc + "," + "n";
                SatyaDBClass.insertprocedure("sp_mobileInsert", param, paramvalue);



                //string[] CWParam1 = { "@group2", "@createdDate", "@details", "@price", "@group1" };
                //string[] CWParamValue1 = { data.group2, dateNow, data.details, proccc, "n" };


                //SatyaDBClass.insertprocedurestockcoma("sp_mobileInsert", CWParam1, CWParamValue1);
            }
            catch (Exception ex)
            {
                string sssss = ex.Message;
                throw;
            }






            //tblBudget obj = new tblBudget();
            //obj.group2 = data.group2;
            //obj.createdDate = data.createdDate;
            //obj.details = data.details;
            //obj.price = data.price;
            //obj.group1 = "n";

            //db.tblBudgets.Add(obj);
            //db.SaveChanges();
            return Json("success");
        }


        [HttpPost]
        public ActionResult GetExpensive(CreateExpensiveViewModel data)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var selectedData = db.tblBudgets.FirstOrDefault(x => x.id == data.id);

            tblBudget_MV newData = new tblBudget_MV();
            newData.id = selectedData.id;
            newData.createdDate = selectedData.createdDate == null ? "" : Convert.ToDateTime(selectedData.createdDate).ToString("yyyy-MM-dd");//selectedData.createdDate;
            newData.price = selectedData.price;
            newData.details = selectedData.details;



            return Json(newData);
        }


        [HttpPost]
        public ActionResult UpdateExpensive(CreateExpensiveViewModel data)
        {
            try
            {
                string dateNow = data.createdDate?.ToString("yyyy-MM-dd hh:mm:ss");
                string proccc = data.price.ToString();


                string param = "@id,@details,@price";
                string paramvalue = data.id + "," + data.details + "," + data.price;
                SatyaDBClass.insertprocedure("sp_mobileUpdate", param, paramvalue);

            }
            catch (Exception ex)
            {
                string sssss = ex.Message;
                throw;
            }

            return Json("success");
        }




        [HttpPost]
        public ActionResult UpdateGroup1(CreateExpensiveViewModel data)
        {

            tblBudget obj = db.tblBudgets.FirstOrDefault(xy => xy.id == data.id);
            obj.group1 = data.group1;
            db.SaveChanges();
            return Json("success");
        }


        [HttpPost]
        public ActionResult UpdateGroup2(CreateExpensiveViewModel data)
        {
            tblBudget obj = db.tblBudgets.FirstOrDefault(xy => xy.id == data.id);
            obj.group2 = data.group2;
            db.SaveChanges();
            return Json("success");
        }

        [HttpPost]
        public ActionResult UpdateGroup3(CreateExpensiveViewModel data)
        {
            tblBudget obj = db.tblBudgets.FirstOrDefault(xy => xy.id == data.id);
            obj.group3 = data.group3;
            db.SaveChanges();
            return Json("success");
        }


        [HttpPost]
        public ActionResult removeRecord(CreateExpensiveViewModel data)
        {
            tblBudget obj = db.tblBudgets.FirstOrDefault(xy => xy.id == data.id);
            db.tblBudgets.Remove(obj);
            db.SaveChanges();
            return Json("success");
        }


        private DateTime AllFromDate(string month)
        {
            if (month == "all")
            {
                return Convert.ToDateTime("2024-05-27");
            }
            if (month == "7")
            {
                return Convert.ToDateTime("2024-06-27");
            }
            if (month == "8")
            {
                return Convert.ToDateTime("2024-07-30");
            }
            if (month == "9")
            {
                return Convert.ToDateTime("2024-08-30");
            }
            return DateTime.Now;
        }

        private DateTime AllToDate(string month)
        {
            if (month == "all")
            {
                return Convert.ToDateTime("2027-05-27");
            }
            if (month == "7")
            {
                return Convert.ToDateTime("2024-07-31");
            }
            if (month == "8")
            {
                return Convert.ToDateTime("2024-08-30");
            }
            if (month == "9")
            {
                return Convert.ToDateTime("2024-10-01");
            }
            return DateTime.Now;
        }



        #region ----------------------  salary ------------------------------
        public ActionResult SalaryEntry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SalaryEntry(tblBudgetMaster data)
        {
            try
            {
                tblBudgetMaster obj = new tblBudgetMaster();
                obj.month = data.month;
                obj.salary = data.salary;
                obj.fromDate = data.fromDate;
                obj.todate = data.todate;
                obj.need = data.need;
                obj.want = data.want;
                obj.saving = data.saving;
                obj.monthName = data.monthName;
                obj.montOrder = data.montOrder;
                obj.year = data.year;
                obj.salaryDate = data.fromDate;
                db.tblBudgetMasters.Add(obj);
                db.SaveChanges();
                calculateAllMonth();
            }
            catch (Exception)
            {
                ViewBag.message = "fail";
                throw;
            }

            return RedirectToAction("SalaryList");
        }

        public ActionResult SalaryList()
        {
            return View(db.tblBudgetMasters.ToList().OrderByDescending(x => x.fromDate));
        }

        public ActionResult SalaryEdit(int? id)
        {
            return View(db.tblBudgetMasters.FirstOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult SalaryEdit(tblBudgetMaster data)
        {
            tblBudgetMaster obj = db.tblBudgetMasters.FirstOrDefault(xy => xy.id == data.id);
            obj.month = data.month;
            obj.salary = data.salary;
            obj.fromDate = data.fromDate;
            obj.todate = data.todate;
            obj.need = data.need;
            obj.want = data.want;
            obj.saving = data.saving;
            db.SaveChanges();
            calculateAllMonth();
            return RedirectToAction("SalaryList");
        }


        private void calculateAllMonth()
        {
            var monthTotalIncome = db.tblBudgetMasters.Where(x => x.month != "all").Sum(x => x.salary);

            tblBudgetMaster obj = db.tblBudgetMasters.FirstOrDefault(xy => xy.month == "all");
            obj.salary = monthTotalIncome;

            obj.need = monthTotalIncome * 50 / 100;
            obj.want = monthTotalIncome * 30 / 100;
            obj.saving = monthTotalIncome * 20 / 100;

            db.SaveChanges();
        }
        #endregion ----------------------------------------------------------



        #region ----------------------  Mobile-----------
        public ActionResult MobileEntry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBymonthMobile(GetExpensiveViewModel data)
        {
            IEnumerable<RenderExpensiveViewModel2> allList = null;

            string param = "@forYear,@forMonth";
            string paramvalue = data.forYear.ToString() + "," + data.forMonth.ToString();
            DataTable dTable = SatyaDBClass.SPReturnDataTable("sp_monthlyList", param, paramvalue);
            IEnumerable<RenderExpensiveViewModel2> dList = ConvertDataTableToList(dTable);

            //allList = dList.ToList();



            //var monthMaster = db.tblBudgetMasters.Where(x => x.month == data.forMonth).FirstOrDefault();
            //var fromDate = monthMaster.fromDate;
            //var toDate = monthMaster.todate;


            //allList = db.tblBudgets.Where(m => m.createdDate > fromDate && m.createdDate < toDate).OrderBy(d => d.createdDate).ToList();



            //List<RenderExpensiveViewModel2> list1 = new List<RenderExpensiveViewModel2>();

            //foreach (var s in allList)
            //{
            //    RenderExpensiveViewModel2 lst = new RenderExpensiveViewModel2();
            //    lst.id = s.id.ToString();
            //    lst.details = s.details;
            //    lst.price = s.price.ToString();
            //    lst.createdDate = Convert.ToDateTime(s.createdDate ?? DateTime.Now).ToString("yyyy-MM-dd");
            //    list1.Add(lst);
            //}

            var result = new { monthTransList = dList.ToList() };

            return Json(result, JsonRequestBehavior.AllowGet);
        }






        private IEnumerable<RenderExpensiveViewModel2> ConvertDataTableToList(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new RenderExpensiveViewModel2
                {
                    id = Convert.ToString(row["id"]),
                    //group1 = Convert.ToString(row["group1"]),
                    //group2 = Convert.ToString(row["group2"]),
                    details = Convert.ToString(row["details"]),
                    price = Convert.ToString(row["price"]),
                    createdDate = Convert.ToDateTime(row["createdDate"] ?? DateTime.Now).ToString("yyyy-MM-dd")
                    //group3 = Convert.ToString(row["group3"]),
                    //group4 = Convert.ToString(row["group4"]),

                    //id = Convert.ToInt32(row["id"]),
                    //group1 = Convert.ToString(row["group1"]),
                    //group2 = Convert.ToString(row["group2"]),
                    //details = Convert.ToString(row["details"]),
                    //price = Convert.ToDecimal(row["price"]),
                    //createdDate = Convert.ToDateTime(row["createdDate"]),
                    //group3 = Convert.ToString(row["group3"]),
                    //group4 = Convert.ToString(row["group4"]),
                };
            }

        }



        #endregion ------------------------------------


        #region ----------------------Test-------------
        public ActionResult TreeView()
        {
            return View();
        }
        public ActionResult TreeViewTest()
        {
            return View();
        }
        public ActionResult TreeView2()
        {
            return View();
        }
        public ActionResult KendoUI()
        {
            return View();
        }


        #endregion ------------------------------------

        #region ----------------------track------------
        public ActionResult TrackShow()
        {
            return View(db.task_TaskDetails.ToList());
        }

        public ActionResult TrackCreate()
        {
            return View();
        }

        public ActionResult TaskShow()
        {
            return View();
        }


        #endregion ------------------------------------
    }
}
