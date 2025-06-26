using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CONSTRUCTION.Models;
using HotelBill.Models;
using System.Data;

namespace CONSTRUCTION.Controllers
{
    public class FinalInvoiceController : Controller
    {
        Bill_DBEntities db = new Bill_DBEntities();

        #region----------------------------- invoice & print ------------------------
        public ActionResult CreateInvoice()
        {
            ViewBag.pageInfo = "Create Invoice";
            CreateInvoiceViewModel tbl = new CreateInvoiceViewModel();
            tbl.serialNoOfInvoice = getGoodsInvoice();
            ViewBag.goods = new SelectList(db.GoodsMasters.ToList().OrderBy(y => y.descriptionOfGoods), "goodsId", "descriptionOfGoods");
            ViewBag.customer = new SelectList(GetCustomerList(), "CustBasicInfo", "CustName");
            ViewBag.tanentList = new SelectList(getTanentList(), "TanentID", "TanentName");
            var AvailTimes = new SelectList(new[] {
               new SelectListItem { Value = "0", Text = "--" },
            new SelectListItem { Value = "1", Text = "c1" },
            new SelectListItem { Value = "2", Text = "c2" },
            });

            tbl.AvailTimes = AvailTimes;

            return View(tbl);
        }

        [HttpPost]
        public ActionResult CreateInvoice(CreateInvoiceViewModel model)
        {
            try
            {
                var isAvlSerialNo = db.tblInvoices.Any(x => x.serialNoOfInvoice == model.serialNoOfInvoice);

                if (model != null && isAvlSerialNo == false)
                {
                    tblInvoice tbl1 = new tblInvoice();
                    tbl1.companyName = model.companyName;
                    tbl1.dateOfInvoice = SatyaDBClass.ISTZoneNull(model.dateOfInvoice);
                    tbl1.agreementDate = SatyaDBClass.ISTZoneNull(model.agreementDate);
                    tbl1.serialNoOfInvoice = model.serialNoOfInvoice;
                    tbl1.projectManager = model.projectManager;
                    tbl1.panNo = model.panNo;
                    tbl1.projectName = model.projectName;
                    tbl1.billTo = model.billTo;
                    tbl1.billToGSTIN = model.billToGSTIN;
                    tbl1.shipTo = model.shipTo;
                    tbl1.shipToGSTIN = model.shipToGSTIN;
                    tbl1.TotalInvoice = model.TotalInvoice;
                    tbl1.isActive = model.isActive;
                    tbl1.createdDate = SatyaDBClass.ISTZoneNull(model.createdDate);
                    tbl1.updatedDate = SatyaDBClass.ISTZoneNull(model.updatedDate);
                    db.tblInvoices.Add(tbl1);
                    db.SaveChanges();

                    foreach (var s in model.transactionData)
                    {
                        tblGoogsTran tbl2 = new tblGoogsTran();
                        tbl2.tInvoiceNo = s.tInvoiceNo;
                        tbl2.tGoodsDesc = s.tGoodsDesc;
                        tbl2.tQty = s.tQty;
                        tbl2.tRate = s.tRate;
                        tbl2.tValue = s.tValue;
                        tbl2.cgstP = s.cgstP;
                        tbl2.cgstA = s.cgstA;
                        tbl2.scstP = s.scstP;
                        tbl2.sgstA = s.sgstA;
                        tbl2.total = s.total;
                        tbl2.isActive = s.isActive;
                        tbl2.createdDate = SatyaDBClass.ISTZoneNull(s.createdDate);
                        tbl2.updatedDate = SatyaDBClass.ISTZoneNull(s.updatedDate);
                        db.tblGoogsTrans.Add(tbl2);
                        db.SaveChanges();
                    }
                    if (model.IsManualInvoice == false)
                    {
                        //OSC/2023/2024/
                        int newDbCounter = Convert.ToInt32(model.serialNoOfInvoice.Remove(0, 14));
                        CounterMaster cnt = db.CounterMasters.Where(x => x.counterName == "goodsInv").FirstOrDefault();
                        cnt.counterValue = newDbCounter;
                        db.SaveChanges();
                    }
                }
                else
                {
                    return Json("alreadyExist");
                }
                return Json("success");
            }
            catch (Exception)
            {
                return Json("error");
                throw;
            }

        }

        public ActionResult PrintInvoice(string invNo)
        {
            string invNum = invNo;
            if (invNo != null)
            {
                var res = (from i in db.tblInvoices
                           join t in db.tblTanents on i.companyName equals t.tanentId
                           where i.serialNoOfInvoice == invNum
                           select new PrintInvoiceViewModel
                           {
                               companyName = t.tanentName,
                               address1 = t.tanentaddr1,
                               address2 = t.tanentaddr2,
                               companyGst = t.tanentGST,
                               dateOfInvoice = i.dateOfInvoice,
                               agreementDate = i.agreementDate,
                               serialNoOfInvoice = i.serialNoOfInvoice,
                               projectManager = i.projectManager,
                               panNo = i.panNo,
                               projectName = i.projectName,
                               billTo = i.billTo,
                               billToGSTIN = i.billToGSTIN,
                               shipTo = i.shipTo,
                               shipToGSTIN = i.shipToGSTIN,
                               TotalInvoice = i.TotalInvoice,
                               beneficiaryName = t.bName,
                               beneficiaryACNo = t.acNo,
                               iFSCCode = t.ifsc,
                               acType = t.type,
                               branch = t.branch
                           }).ToList().FirstOrDefault();

                var listResult = (from g in db.tblGoogsTrans
                                  join m in db.GoodsMasters on g.tGoodsDesc equals m.descriptionOfGoods
                                  where g.tInvoiceNo == invNum
                                  select new PrintInvoiceTransaction
                                  {
                                      tInvoiceNo = g.tInvoiceNo,
                                      tGoodsDesc = g.tGoodsDesc,
                                      tGoodsUnit=m.unit,
                                      tHSN = m.hsn,
                                      tQty = g.tQty,
                                      tRate = g.tRate,
                                      tValue = g.tValue,
                                      cgstP = g.cgstP,
                                      cgstA = g.cgstA,
                                      scstP = g.scstP,
                                      sgstA = g.sgstA,
                                      total = g.total
                                  }).ToList();


                PrintInvoiceViewModel obj = new PrintInvoiceViewModel();
                obj.companyName = res.companyName;
                obj.address1 = res.address1;
                obj.address2 = res.address2;
                obj.companyGst = res.companyGst;
                obj.dateOfInvoice = SatyaDBClass.ISTZoneNull(res.dateOfInvoice);
                obj.agreementDate = SatyaDBClass.ISTZoneNull(res.agreementDate);
                obj.serialNoOfInvoice = res.serialNoOfInvoice;
                obj.projectManager = res.projectManager;
                obj.panNo = res.panNo;
                obj.projectName = res.projectName;
                obj.billTo = res.billTo;
                obj.billToGSTIN = res.billToGSTIN;
                obj.shipTo = res.shipTo;
                obj.shipToGSTIN = res.shipToGSTIN;
                obj.TotalInvoice = res.TotalInvoice;
                obj.beneficiaryName = res.beneficiaryName;
                obj.beneficiaryACNo = res.beneficiaryACNo;
                obj.iFSCCode = res.iFSCCode;
                obj.acType = res.acType;
                obj.branch = res.branch;
                obj.transactionData = listResult;

                int v2 = obj.TotalInvoice == null ? default(int) : Convert.ToInt32(obj.TotalInvoice);
                double d = v2;
                int itt = (int)d;
                string text = NumberToWord.NumberToText(itt, true);
                obj.totalInfigure = text;
                return View(obj);
            }
            else
            {
                PrintInvoiceViewModel obj = new PrintInvoiceViewModel();
                return View(obj);
            }
        }

        [HttpPost]
        public JsonResult DeleteInvoice(DeleteInvViewModel model)
        {
            try
            {
                if (model.serialNoOfInvoice != null)
                {
                    var invoice = db.tblInvoices.First(c => c.serialNoOfInvoice == model.serialNoOfInvoice);
                    db.tblInvoices.Remove(invoice);
                    db.SaveChanges();

                    var tData = db.tblGoogsTrans.First(c => c.tInvoiceNo == model.serialNoOfInvoice);
                    db.tblGoogsTrans.Remove(tData);
                    db.SaveChanges();
                    return Json("deletedSuccessfully");
                }
                else
                {
                    return Json("notDeleted");
                }
            }
            catch (Exception)
            {
                return Json("notDeleted");
                throw;
            }
        }

        public ActionResult InvoiceList()
        {
            var data = (from inv in db.tblInvoices
                        join cus in db.tblTanents
                       on inv.companyName equals cus.tanentId
                        select new InvoiceListViewModel
                        {
                            custName = cus.tanentName,
                            serialNoOfInvoice = inv.serialNoOfInvoice,
                            dateOfInvoice = inv.dateOfInvoice,
                            agreementDate = inv.agreementDate,
                            projectName = inv.projectName,
                            billTo = inv.billTo,
                            shipTo = inv.shipTo,
                            TotalInvoice = inv.TotalInvoice
                        }).ToList().OrderByDescending(x => x.serialNoOfInvoice);
            return View(data);
        }

        public void CallSP()
        {
            string Branch = Session["Branch"].ToString();
            //string param = "@Fromdate,@Todate,@Branch,@year";
            //string paramvalue = Convert.ToDateTime(txt_FromDate.Text, SmitaClass.dateformat()).ToString("yyyy-MM-dd") + " , " + Convert.ToDateTime(txt_ToDate.Text, SatyaDBClass.dateformat()).ToString("yyyy-MM-dd") + "," + Branch + "," + year;
            //DataTable dtr = SatyaDBClass.SPReturnDataTable("sp_DailySpareSales_WithReturn_Report1", param, paramvalue);
        }


        #endregion-------------------------------------------------------------

        #region----------------------------- receipt & print ------------------
        public ActionResult CreateReceipt()
        {
            ViewBag.pageInfo = "Create New Receipt";
            CreateReceiptViewModel model = new CreateReceiptViewModel();
            model.refNo = getRecptReferenceNo();
            ViewBag.goods = new SelectList(db.GoodsMasters.ToList().OrderBy(y => y.descriptionOfGoods), "goodsId", "descriptionOfGoods");
            //ViewBag.customer = new SelectList(GetCustomerList(), "CustBasicInfo", "CustName");
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateReceipt(CreateReceiptViewModel model)
        {
            try
            {
                tblReceiptDetail recept = new tblReceiptDetail();
                recept.companyName = model.companyName;
                recept.refNo = model.refNo;
                recept.receiptDate = SatyaDBClass.ISTZoneNull(model.receiptDate);
                recept.companyName = model.companyName;
                recept.rsp_name = model.rsp_name;
                recept.name_addr = model.name_addr;
                recept.rsp_addrOfP = model.rsp_addrOfP;
                recept.rsp_addrOfD = model.rsp_addrOfD;
                recept.rsp_gstin = model.rsp_gstin;
                recept.rsp_grandTotal = model.rsp_grandTotal;
                recept.isActive = model.isActive;
                recept.createdDate = SatyaDBClass.ISTZoneNull(model.createdDate);
                db.tblReceiptDetails.Add(recept);
                db.SaveChanges();

                foreach (var sD in model.transactionList)
                {
                    tblReceiptTransaction tbl2 = new tblReceiptTransaction();
                    tbl2.refNo = sD.refNo;
                    tbl2.trsp_goodsId = sD.trsp_goodsId;
                    tbl2.trsp_goodsName = sD.trsp_goodsName;
                    tbl2.trsp_goodsUnit = sD.trsp_goodsUnit;
                    tbl2.trsp_hsn = sD.trsp_hsn;
                    tbl2.trsp_qty = sD.trsp_qty;
                    tbl2.trsp_price = sD.trsp_price;
                    tbl2.trsp_valueOfGoods = sD.trsp_valueOfGoods;
                    tbl2.trsp_cgstP = sD.trsp_cgstP;
                    tbl2.trsp_cgstA = sD.trsp_cgstA;
                    tbl2.trsp_sgstP = sD.trsp_sgstP;
                    tbl2.trsp_sgstA = sD.trsp_sgstA;
                    tbl2.trsp_total = sD.trsp_total;
                    tbl2.isActive = sD.isActive;
                    tbl2.createdDate = SatyaDBClass.ISTZoneNull(sD.createdDate);
                    db.tblReceiptTransactions.Add(tbl2);
                    db.SaveChanges();
                }

                int newDbCounter = model.refNo == null ? default(int) : model.refNo.Value;
                CounterMaster cnt = db.CounterMasters.Where(x => x.counterName == "recptNo").FirstOrDefault();
                cnt.counterValue = newDbCounter;
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                return Json("error");
                throw;
            }


            return Json("success");
        }
        public ActionResult PrintReceipt(int? invNo)
        {

            int refNo = invNo ?? default(int);
            if (refNo != 0)
            {
                var res = (from receipt in db.tblReceiptDetails
                           join t in db.tblTanents on receipt.companyName equals t.tanentId
                           where receipt.refNo == refNo
                           select new PrintReceiptModel
                           {
                               CompanyHeader = t.tanentName,
                               CompanyHeaderAddress = t.tanentaddr1,
                               CompanyHeaderAddress2 = t.tanentaddr2,
                               receiptNo = receipt.refNo,
                               receiptDate = receipt.receiptDate,
                               name = receipt.rsp_name,
                               address = t.tanentaddr1 + " " + t.tanentaddr2,
                               gstin = t.tanentGST,
                               purchaseAddress = receipt.rsp_addrOfP,
                               delivaryAddress = receipt.rsp_addrOfD,
                               grandTotal = receipt.rsp_grandTotal,
                               invoiceAmmountinWords = "",
                               bName = t.bName,
                               acNumber = t.acNo,
                               ifcCode = t.ifsc,
                               acType = t.type,
                               branch = t.branch
                           }).ToList().FirstOrDefault();

                int zz = res.grandTotal == null ? default(int) : Convert.ToInt32(res.grandTotal);
                bool isActive = true;
                res.invoiceAmmountinWords = NumberToWord.NumberToText(zz, isActive);

                res.receiptTransaction = (from g in db.tblReceiptTransactions
                                          join i in db.GoodsMasters on g.trsp_goodsName equals i.descriptionOfGoods
                                          where g.refNo == refNo
                                          select new PrintReceiptTransactionModel
                                          {
                                              descOfGoods = g.trsp_goodsName,
                                              goodsUnit = i.unit,
                                              hsn = g.trsp_hsn,
                                              quantity = g.trsp_qty,
                                              goodsPrice = g.trsp_price,
                                              goodsValue = g.trsp_valueOfGoods,
                                              cgstP = g.trsp_cgstP,
                                              cgstA = g.trsp_cgstA,
                                              scgtP = g.trsp_sgstP,
                                              sgstA = g.trsp_sgstA,
                                              total = g.trsp_total
                                          }).ToList();
                return View(res);
            }
            else
            {
                PrintReceiptModel obj = new PrintReceiptModel();
                return View(obj);
            }
        }

        public ActionResult ReceiptList()
        {
            var data = (from rec in db.tblReceiptDetails
                        join cus in db.CustomerMasters
                       on rec.companyName equals cus.custId
                        select new ReceiptListViewModel
                        {
                            custName = rec.rsp_name,
                            refNo = rec.refNo,
                            companyName = rec.companyName == 1 ? "OMM SUPPLIER AND CONSTRUCTION" : "OMM SAI CONSTRUCTION",
                            receiptDate = rec.receiptDate,
                            rsp_grandTotal = rec.refNo
                        }).ToList().OrderByDescending(x => x.refNo);
            return View(data);
        }
        #endregion-------------------------------------------------------------

        public IEnumerable<TanentDetails> getTanentList()
        {
            List<TanentDetails> tanentList = new List<TanentDetails>();

            TanentDetails t = new TanentDetails();
            t.TanentID = 1;
            t.TanentName = "OMM SUPPLIER AND CONSTRUCTION";

            TanentDetails t2 = new TanentDetails();
            t2.TanentID = 2;
            t2.TanentName = "OMM SAI CONSTRUCTION";

            tanentList.Add(t);
            tanentList.Add(t2);
            return tanentList;
        }

        public string getGoodsInvoice()
        {
            int dbCount = db.CounterMasters.Where(x => x.counterName == "goodsInv").FirstOrDefault().counterValue;
            string newNo = BillSupport.increamentGoodsInvoice(dbCount);
            return newNo;
        }

        public int getRecptReferenceNo()
        {
            int dbCount = db.CounterMasters.Where(x => x.counterName == "recptNo").FirstOrDefault().counterValue;
            int newNo = BillSupport.increamentReferenceNO(dbCount);
            return newNo;
        }

        public string getNewInvoiceNo()
        {
            int dbCount = db.CounterMasters.Where(x => x.counterName == "invoiceGst").FirstOrDefault().counterValue;
            string newNo = BillSupport.increamentInvoice(dbCount);
            return newNo;
        }

        private List<CustomerInformation> GetCustomerList()
        {
            return (from cust in db.CustomerMasters
                    select new CustomerInformation
                    {
                        CustBasicInfo = cust.custId.ToString() + "-" + cust.gstType.ToString(),
                        CustName = cust.custName
                    }).ToList();
        }

        public string getNewDraftNo()
        {
            int dbCount = db.CounterMasters.Where(x => x.counterName == "invoiceDraft").FirstOrDefault().counterValue;
            string newNo = BillSupport.increamentDraft(dbCount);
            return newNo;
        }

        [HttpPost]
        public ActionResult GetGoodsById(int id)
        {
            var item = db.GoodsMasters.Where(x => x.goodsId == id).FirstOrDefault();
            try
            {
                string param = "@id";
                string paramvalue = id.ToString();
                DataTable dtr = SatyaDBClass.SPReturnDataTable("sp_getGoodsById", param, paramvalue);
                var ss = SatyaDBClass.ContDataTabloJSON(dtr);
                return Json(ss);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                throw;
            }
        }

        public ActionResult IntervalTest()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Login");
        }

    }
}