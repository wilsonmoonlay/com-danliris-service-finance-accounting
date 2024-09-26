using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.GarmentDispositionExpedition
{
    public class GarmentDispositionExpeditionModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            int id = 1;
            string dispositionNoteNo = "Test";
            int dispositionNoteId = 1;
            double currencyTotalPaid = 1;
            double totalPaid = 1;
            int currencyId = 1;
            string currencyCode = "Test";
            string suppliername = "Test";
            string remark = "Test";
            string proformaNo = "Test";
            string createdBy = "Test";
            double currencyRate = 1;
            int supplierId = 1;
            string supplierCode = "Test";
            double vatAmount = 1;
            double currencyVatAmount = 1;
            double incomeTaxAmount = 1;
            double currencyIncomeTaxAmount = 1;
            double dppAmount = 1;
            double currencyDppAmount = 1;
            string sendToPurchasingRemark = "Test";

            GarmentDispositionExpeditionModel model = new GarmentDispositionExpeditionModel(
                id, dispositionNoteNo, DateTimeOffset.Now, DateTimeOffset.Now, dispositionNoteId, currencyTotalPaid,
                totalPaid, currencyId, currencyCode, suppliername, remark, proformaNo, createdBy, currencyRate,
                supplierId, supplierCode, vatAmount, currencyVatAmount, incomeTaxAmount, currencyIncomeTaxAmount,
                dppAmount, currencyDppAmount, DateTimeOffset.Now, DateTimeOffset.Now, sendToPurchasingRemark, DateTime.Now);

            Assert.NotNull(model);

        }
    }
}
