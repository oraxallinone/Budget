using CONSTRUCTION.Models;
using HotelBill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CONSTRUCTION.Controllers
{
    public class GoodsMasterController : Controller
    {
        Bill_DBEntities db = new Bill_DBEntities();

        #region----------------------------- goods master ---------------------
        public ActionResult AddGoods()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGoods(GoodsMaster good)
        {
            var checkDuplicate = duplicateCheckGood(good.descriptionOfGoods);
            if (!checkDuplicate)
            {
                try
                {
                    good.isActive = true;
                    good.createdDate = SatyaDBClass.ISTZoneNull(DateTime.Now);
                    db.GoodsMasters.Add(good);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.isPresent = ex;
                }

                return RedirectToAction("GoodsList");
            }
            ViewBag.isPresent = "Goods Name exist please chanege the Goods Name";
            return View();

        }

        public ActionResult ViewGoods(int id)
        {
            var goods = db.GoodsMasters.Where(x => x.goodsId == id).FirstOrDefault();
            return View(goods);
        }

        public ActionResult EditGoods(int id)
        {
            var goods = db.GoodsMasters.Where(x => x.goodsId == id).FirstOrDefault();
            return View(goods);
        }

        [HttpPost]
        public ActionResult EditGoods(GoodsMaster cust)
        {
            try
            {
                GoodsMaster cust2 = db.GoodsMasters.Where(x => x.goodsId == cust.goodsId).FirstOrDefault();
                cust2.descriptionOfGoods = cust.descriptionOfGoods;
                cust2.unit = cust.unit;
                cust2.hsn = cust.hsn;
                cust2.rate = cust.rate;
                cust2.cgst_p = cust.cgst_p;
                cust2.sgst_p = cust.sgst_p;
                cust2.isActive = cust.isActive;
                cust2.updatedDate = SatyaDBClass.ISTZoneNull(DateTime.Now);
                db.SaveChanges();
                return RedirectToAction("GoodsList");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        private bool duplicateCheckGood(string description)
        {
            var isThere = db.GoodsMasters.Any(x => x.descriptionOfGoods == description);
            return isThere;
        }

        public ActionResult GoodsList()
        {
            var List = db.GoodsMasters.OrderBy(x => new
            {
                x.isActive,
                x.goodsId
            }).ToList().OrderByDescending(x => x.goodsId);
            return View(List);
        }
        #endregion-------------------------------------------------------------


    }
}