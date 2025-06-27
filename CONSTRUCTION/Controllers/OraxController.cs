using CONSTRUCTION.Models;
using HotelBill.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
        public ActionResult Entry2()
        {
            return View();
        }

        public ActionResult Entry3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllExpensive(GetExpensiveViewModel data)
        {
            tblBudgetMaster budgetList = null;
            IEnumerable<tblBudget> allList = null;
            List<RenderExpensiveViewModel2> expensiveList = new List<RenderExpensiveViewModel2>();
            try
            {
                budgetList = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
                var fromDate = SatyaDBClass.ISTZoneNull(budgetList.fromDate);
                var toDate = SatyaDBClass.ISTZoneNull(budgetList.todate);
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
                foreach (var s in allList)
                {
                    RenderExpensiveViewModel2 item = new RenderExpensiveViewModel2();
                    item.id = s.id.ToString();
                    item.group1 = s.group1;
                    item.group2 = s.group2;
                    item.group3 = s.group3;
                    item.details = s.details;
                    item.price = s.price.ToString();
                    item.createdDate = Convert.ToDateTime(SatyaDBClass.ISTZoneNull(s.createdDate) ?? SatyaDBClass.ISTZoneNull(DateTime.Now)).ToString("yyyy-MM-dd");
                    expensiveList.Add(item);
                }
                var result = new { monthTransList = expensiveList, monthMaster = budgetList };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                throw;
            }
            finally
            {
                budgetList = null;
                allList = null;
            }
        }

        public ActionResult MobileUpdate()
        {
            return View();
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
            tblBudgetMaster budgetMaster = null;
            try
            {
                budgetMaster = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
                var fromDate = SatyaDBClass.ISTZoneNull(budgetMaster.fromDate);
                var toDate = SatyaDBClass.ISTZoneNull(budgetMaster.todate);
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
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                budgetMaster = null;
            }
        }

        [HttpPost]
        public ActionResult GetAllDataBudget(GetExpensiveViewModel data)
        {
            tblBudgetMaster budgetMaster = null;
            try
            {
                budgetMaster = db.tblBudgetMasters.Where(x => x.month == data.forMonth && x.year == data.forYear).FirstOrDefault();
                var fromDate = SatyaDBClass.ISTZoneNull(budgetMaster.fromDate); 
                var toDate = SatyaDBClass.ISTZoneNull(budgetMaster.todate);
                var groupByAmt = db.tblBudgets.Where(m => m.createdDate >= fromDate && m.createdDate <= toDate).GroupBy(b => b.group2)
                                        .Select(g => new
                                        {
                                            label = g.Key,
                                            y = g.Sum(b => b.price)
                                        })
                                        .ToList();
                return Json(groupByAmt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                budgetMaster = null;
            }
        }

        public ActionResult GraphColumn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetColumnChart(GetExpensiveViewModel data)
        {
            IEnumerable<tblBudgetMaster> budgetMasters = null;
            List<BarGraph> barGraphData = new List<BarGraph>();
            IEnumerable<BarGraphdbArrange> datas = null;
            try
            {
                budgetMasters = db.tblBudgetMasters.Where(v => v.month != "all").ToList().OrderBy(m => m.todate);
                var index = 0;

                foreach (var ss in budgetMasters)
                {
                    index = index + 1;

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
                var result = new { monthMstr = budgetMasters, barGraphData = barGraphData };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                budgetMasters = null;
                barGraphData = null;
                datas = null;
            }
        }

        public ActionResult MobileGraph()
        {
            return View();
        }
        #endregion ------------------------------------

        #region ----------------------  common  --------
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
        #endregion ------------------------------------

        #region ---------------- common 2   -------------
        [HttpPost]
        public ActionResult AddExpensive(CreateExpensiveViewModel data)
        {
            try
            {
                string dateNow = SatyaDBClass.ISTZoneNull(data.createdDate)?.ToString("yyyy-MM-dd hh:mm:ss"); 
                string proccc = data.price.ToString();
                string param = "@group2,@createdDate,@details,@price,@group1";
                string paramvalue = data.group2 + "," + dateNow + "," + data.details + "," + proccc + "," + "n";
                SatyaDBClass.insertprocedure("sp_mobileInsert", param, paramvalue);
            }
            catch (Exception ex)
            {
                string sssss = ex.Message;
                throw;
            }
            return Json("success");
        }

        [HttpPost]
        public ActionResult GetExpensive(CreateExpensiveViewModel data)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var selectedData = db.tblBudgets.FirstOrDefault(x => x.id == data.id);
            tblBudget_MV newData = new tblBudget_MV();
            newData.id = selectedData.id;
            newData.createdDate = selectedData.createdDate == null ? "" : Convert.ToDateTime(SatyaDBClass.ISTZoneNull(selectedData.createdDate)).ToString("yyyy-MM-dd");//selectedData.createdDate;
            newData.price = selectedData.price;
            newData.details = selectedData.details;
            return Json(newData);
        }

        [HttpPost]
        public ActionResult UpdateExpensive(CreateExpensiveViewModel data)
        {
            try
            {
                string dateNow = SatyaDBClass.ISTZoneNull(data.createdDate)?.ToString("yyyy-MM-dd hh:mm:ss");
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
        #endregion ------------------------------------

        #region -----------  salary verified ---------
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
                obj.fromDate = SatyaDBClass.ISTZoneNull(data.fromDate);
                obj.todate = SatyaDBClass.ISTZoneNull(data.todate);
                obj.need = data.need;
                obj.want = data.want;
                obj.saving = data.saving;
                obj.monthName = data.monthName;
                obj.montOrder = data.montOrder;
                obj.year = data.year;
                obj.salaryDate = SatyaDBClass.ISTZoneNull(data.fromDate);
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
            obj.fromDate = SatyaDBClass.ISTZoneNull(data.fromDate);
            obj.todate = SatyaDBClass.ISTZoneNull(data.todate);
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
            string param = "@forYear,@forMonth";
            string paramvalue = data.forYear.ToString() + "," + data.forMonth.ToString();
            DataTable dTable = SatyaDBClass.SPReturnDataTable("sp_monthlyList", param, paramvalue);
            IEnumerable<RenderExpensiveViewModel2> dList = ConvertDataTableToList(dTable);
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
                    details = Convert.ToString(row["details"]),
                    price = Convert.ToString(row["price"]),
                    createdDate = Convert.ToDateTime(row["createdDate"] ?? DateTime.Now).ToString("yyyy-MM-dd")
                };
            }
        }

        #endregion ------------------------------------

        #region ---------------------- Group Master verified ----
        public ActionResult GroupList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListGroup()
        {
            return Json(db.tblGroupMasters.ToList());
        }

        [HttpPost]
        public ActionResult GroupSave(GroupViewModel data)
        {
            if (Convert.ToInt32(data.hidelId) == 0 && data.btnText == "Save")
            {
                int dbCount = db.CounterMasters.Where(x => x.counterName == "group2id").FirstOrDefault().counterValue;
                int newNo = BillSupport.increamentReferenceNO(dbCount);

                tblGroupMaster obj = new tblGroupMaster();
                obj.groupName = data.groupName;
                obj.haveFixPrice = data.haveFixPrice;
                obj.priceGroup = data.priceGroup;
                obj.IsShow = data.IsShow;
                obj.group2int = newNo;
                db.tblGroupMasters.Add(obj);
                db.SaveChanges();

                CounterMaster cnt = db.CounterMasters.Where(x => x.counterName == "group2id").FirstOrDefault();
                cnt.counterValue = newNo;
                db.SaveChanges();
                return Json("save");
            }
            else if (data.btnText == "Update?")
            {
                tblGroupMaster obj = db.tblGroupMasters.FirstOrDefault(x => x.id == data.hidelId);
                obj.groupName = data.groupName;
                obj.haveFixPrice = data.haveFixPrice;
                obj.priceGroup = data.priceGroup;
                obj.IsShow = data.IsShow;
                db.SaveChanges();
                return Json("update");
            }
            else
            {
                return Json("not");
            }

        }


        [HttpPost]
        public ActionResult GroupGet(GroupViewModel data)
        {
            tblGroupMaster obj = db.tblGroupMasters.FirstOrDefault(x => x.id == data.hidelId);
            return Json(obj);
        }

        [HttpPost]
        public ActionResult GetDDLGroup2()
        {
            IEnumerable<tblGroupMaster> obj = db.tblGroupMasters.Where(x => x.IsShow == true).ToList();
            return Json(obj);
        }

        [HttpPost]
        public ActionResult GetDDLGroup2All()
        {
            IEnumerable<tblGroupMaster> obj = db.tblGroupMasters.ToList();
            return Json(obj);
        }

        [HttpPost]
        public ActionResult GetDDLGroup2OnlyActive()
        {
            IEnumerable<tblGroupMaster> obj = db.tblGroupMasters.Where(x => x.IsShow == true).ToList();
            return Json(obj);
        }
        #endregion ------------------------------------
    }
}
