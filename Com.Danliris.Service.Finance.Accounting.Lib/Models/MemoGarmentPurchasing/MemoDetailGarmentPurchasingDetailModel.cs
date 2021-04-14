using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing

{
    public class MemoDetailGarmentPurchasingDetailModel : Moonlay.Models.StandardEntity
    {
        public int GarmentDeliveryOrderId { get; set; }
        public string GarmentDeliveryOrderNo { get; set; }
        public string RemarksDetail { get; set; }
        public int PaymentRate { get; set; }
        public int PurchasingRate { get; set; }
        public int MemoAmount { get; set; }
        public int MemoIdrAmount { get; set; }
        public int MemoDetailId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }

        [ForeignKey("MemoDetailId")]
        public virtual MemoDetailGarmentPurchasingModel MemoDetailGarmentPurchasing { get; set; }
    }
}
