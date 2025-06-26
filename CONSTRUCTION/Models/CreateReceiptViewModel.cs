using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONSTRUCTION.Models
{
    public class CreateReceiptViewModel
    {
        public Nullable<int> refNo
        {
            get;
            set;
        }
        public Nullable<System.DateTime> receiptDate
        {
            get;
            set;
        }
        public int companyName
        {
            get;
            set;
        }
        public string rsp_name
        {
            get;
            set;
        }
        public string name_addr
        {
            get;
            set;
        }
        public string rsp_addrOfP
        {
            get;
            set;
        }
        public string rsp_addrOfD
        {
            get;
            set;
        }
        public Nullable<int> rsp_gstin
        {
            get;
            set;
        }
        public Nullable<decimal> rsp_grandTotal
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

        public List<tblTReceiptTransaction> transactionList
        {
            get;
            set;
        }
    }

   

    public partial class tblTReceiptTransaction
    {
        public Nullable<int> refNo
        {
            get;
            set;
        }
        public string trsp_goodsId
        {
            get;
            set;
        }
        public string trsp_goodsName
        {
            get;
            set;
        }
        public string trsp_goodsUnit
        {
            get;
            set;
        }
        public string trsp_hsn
        {
            get;
            set;
        }
        public Nullable<int> trsp_qty
        {
            get;
            set;
        }
        public Nullable<decimal> trsp_price
        {
            get;
            set;
        }
        public string trsp_valueOfGoods
        {
            get;
            set;
        }
        public Nullable<int> trsp_cgstP
        {
            get;
            set;
        }
        public Nullable<decimal> trsp_cgstA
        {
            get;
            set;
        }
        public Nullable<int> trsp_sgstP
        {
            get;
            set;
        }
        public Nullable<decimal> trsp_sgstA
        {
            get;
            set;
        }
        public Nullable<decimal> trsp_total
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










    public class PrintReceiptModel
    {
        public string CompanyHeader
        {
            get;
            set;
        }
        public string CompanyHeaderAddress
        {
            get;
            set;
        }
        public string CompanyHeaderAddress2
        {
            get;
            set;
        }


        

        public Nullable<int> receiptNo
        {
            get;
            set;
        }
        public Nullable<System.DateTime> receiptDate
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string address
        {
            get;
            set;
        }
        public string gstin
        {
            get;
            set;
        }


        public string purchaseAddress
        {
            get;
            set;
        }
       
        public string delivaryAddress
        {
            get;
            set;
        }
        //public Nullable<int> purchaseGstin
        //{
        //    get;
        //    set;
        //}
        //public Nullable<int> delivaryGstin
        //{
        //    get;
        //    set;
        //}
        public List<PrintReceiptTransactionModel> receiptTransaction
        {
            get;
            set;
        }
        public Nullable<decimal> grandTotal
        {
            get;
            set;
        }
        public string invoiceAmmountinWords
        {
            get;
            set;
        }
        public string bName
        {
            get;
            set;
        }
        public string acNumber
        {
            get;
            set;
        }
        public string ifcCode
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
    }


    public partial class PrintReceiptTransactionModel
    {
        public string descOfGoods
        {
            get;
            set;
        }
        public string goodsUnit
        {
            get;
            set;
        }
        public string hsn
        {
            get;
            set;
        }
        public Nullable<int> quantity
        {
            get;
            set;
        }
        public Nullable<decimal> goodsPrice
        {
            get;
            set;
        }
        public string goodsValue
        {
            get;
            set;
        }
        public Nullable<int> cgstP
        {
            get;
            set;
        }
        public Nullable<decimal> cgstA
        {
            get;
            set;
        }
        public Nullable<int> scgtP
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
}