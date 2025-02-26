using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CONSTRUCTION.Models
{
    public class CreateInvoiceViewModel
    {
        public IEnumerable<SelectListItem> AvailTimes
        {
            get;
            set;
        }
        public int companyName
        {
            get;
            set;
        }
        public Nullable<System.DateTime> dateOfInvoice
        {
            get;
            set;
        }
        public Nullable<System.DateTime> agreementDate
        {
            get;
            set;
        }
        public string serialNoOfInvoice
        {
            get;
            set;
        }
        public string projectManager
        {
            get;
            set;
        }
        public string panNo
        {
            get;
            set;
        }
        public string projectName
        {
            get;
            set;
        }
        public string billTo
        {
            get;
            set;
        }
        public string billToGSTIN
        {
            get;
            set;
        }
        public string shipTo
        {
            get;
            set;
        }
        public string shipToGSTIN
        {
            get;
            set;
        }
        public Nullable<decimal> TotalInvoice
        {
            get;
            set;
        }
        public Nullable<bool> isActive
        {
            get;
            set;
        }
        public Nullable<System.DateTime> createdDate
        {
            get;
            set;
        }
        public Nullable<System.DateTime> updatedDate
        {
            get;
            set;
        }
        public List<CreateInvoiceTransaction> transactionData
        {
            get;
            set;
        }
        //[NotMapped]
        public bool IsManualInvoice { get; set; }


    }

    public class CreateInvoiceTransaction
    {

        public string tInvoiceNo
        {
            get;
            set;
        }
        public string tGoodsDesc
        {
            get;
            set;
        }
        public Nullable<decimal> tQty
        {
            get;
            set;
        }
        public Nullable<decimal> tRate
        {
            get;
            set;
        }
        public Nullable<decimal> tValue
        {
            get;
            set;
        }
        public Nullable<decimal> cgstP
        {
            get;
            set;
        }
        public Nullable<decimal> cgstA
        {
            get;
            set;
        }
        public Nullable<decimal> scstP
        {
            get;
            set;
        }
        public Nullable<decimal> sgstA
        {
            get;
            set;
        }
        public Nullable<decimal> total
        {
            get;
            set;
        }
        public Nullable<bool> isActive
        {
            get;
            set;
        }
        public Nullable<System.DateTime> createdDate
        {
            get;
            set;
        }
        public Nullable<System.DateTime> updatedDate
        {
            get;
            set;
        }
    }

    public class PrintInvoiceViewModel
    {
        public string companyName
        {
            get;
            set;
        }
        public string address1
        {
            get;
            set;
        }
        public string address2
        {
            get;
            set;
        }
        public string companyGst
        {
            get;
            set;
        }
        public Nullable<System.DateTime> dateOfInvoice
        {
            get;
            set;
        }
        public Nullable<System.DateTime> agreementDate
        {
            get;
            set;
        }
        public string serialNoOfInvoice
        {
            get;
            set;
        }
        public string projectManager
        {
            get;
            set;
        }
        public string panNo
        {
            get;
            set;
        }
        public string projectName
        {
            get;
            set;
        }
        public string billTo
        {
            get;
            set;
        }
        public string billToGSTIN
        {
            get;
            set;
        }
        public string shipTo
        {
            get;
            set;
        }
        public string shipToGSTIN
        {
            get;
            set;
        }
        public Nullable<decimal> TotalInvoice
        {
            get;
            set;
        }
        public IEnumerable<PrintInvoiceTransaction> transactionData
        {
            get;
            set;
        }
        public string beneficiaryName
        {
            get;
            set;
        }
        public string beneficiaryACNo
        {
            get;
            set;
        }
        public string iFSCCode
        {
            get;
            set;
        }
        public string acType
        {
            get;
            set;
        }
        public string branch
        {
            get;
            set;
        }
        public string totalInfigure
        {
            get;
            set;
        }
    }

    public class PrintInvoiceTransaction
    {
        public string tInvoiceNo
        {
            get;
            set;
        }
        public string tGoodsDesc
        {
            get;
            set;
        }
        public Nullable<decimal> tQty
        {
            get;
            set;
        }
        public Nullable<decimal> tRate
        {
            get;
            set;
        }
        public Nullable<decimal> tValue
        {
            get;
            set;
        }
        public Nullable<decimal> cgstP
        {
            get;
            set;
        }
        public Nullable<decimal> cgstA
        {
            get;
            set;
        }
        public Nullable<decimal> scstP
        {
            get;
            set;
        }
        public Nullable<decimal> sgstA
        {
            get;
            set;
        }
        public Nullable<decimal> total
        {
            get;
            set;
        }
    }


    public class InvoiceListViewModel
    {
        public Nullable<System.DateTime> createdDate
        {
            get;
            set;
        }
        public string custName
        {
            get;
            set;
        }
        public string serialNoOfInvoice
        {
            get;
            set;
        }
        public Nullable<System.DateTime> dateOfInvoice
        {
            get;
            set;
        }
        public Nullable<System.DateTime> agreementDate
        {
            get;
            set;
        }
        public string projectName
        {
            get;
            set;
        }
        public string billTo
        {
            get;
            set;
        }
        public string shipTo
        {
            get;
            set;
        }
        public Nullable<decimal> TotalInvoice
        {
            get;
            set;
        }
    }

    public class ReceiptListViewModel
    {
        public string custName
        {
            get;
            set;
        }
        public int? refNo
        {
            get;
            set;
        }
        public Nullable<System.DateTime> receiptDate
        {
            get;
            set;
        }
        public string companyName
        {
            get;
            set;
        }
        public Nullable<decimal> rsp_grandTotal
        {
            get;
            set;
        }
    }

    public class CreateExpensiveViewModel
    {
        public int id { get; set; }
        public string group1 { get; set; }
        public string group2 { get; set; }
        public string group3 { get; set; }
        public string details { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
    }

    public class GroupViewModel
    {
        public int hidelId { get; set; }
        public string groupName { get; set; }
        public bool haveFixPrice { get; set; }
        public decimal priceGroup { get; set; }
        public bool IsShow { get; set; }
    }


    public class GetExpensiveViewModel
    {
        public int forYear { get; set; }
        public string forMonth { get; set; }
        public string group1 { get; set; }
        public string group2 { get; set; }
        public string group3 { get; set; }
    }

    public class RenderExpensiveViewModel2
    {
        public string id { get; set; }
        public string group1 { get; set; }
        public string group2 { get; set; }
        public string group3 { get; set; }
        public string details { get; set; }
        public string price { get; set; }
        public string createdDate { get; set; }
    }
}