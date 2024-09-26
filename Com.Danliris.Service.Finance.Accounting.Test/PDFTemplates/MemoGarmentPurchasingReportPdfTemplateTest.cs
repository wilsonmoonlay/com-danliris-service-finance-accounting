using Com.Danliris.Service.Finance.Accounting.Lib.Models.MemoGarmentPurchasing;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PDFTemplates
{
    public class MemoGarmentPurchasingReportPdfTemplateTest
    {
        public MemoGarmentPurchasingModel MemoGarmentPurchasingViewModel
        {
            get
            {
                return new MemoGarmentPurchasingModel()
                {
                    MemoNo = "MemoNo",
                    MemoDate = DateTimeOffset.Now,
                    Remarks = "Remarks",
                    MemoGarmentPurchasingDetails = new List<MemoGarmentPurchasingDetailModel>()
                    {
                        new MemoGarmentPurchasingDetailModel()
                        {
                            COANo = "COANo",
                            COAName = "COAName",
                            DebitNominal = 0,
                            CreditNominal = 0,
                            MemoGarmentPurchasing = new MemoGarmentPurchasingModel()
                            {
                                Remarks = "Remarks"
                            }
                        },
                        new MemoGarmentPurchasingDetailModel()
                        {
                            COANo = "COANo",
                            COAName = "COAName",
                            DebitNominal = 0,
                            CreditNominal = 0,
                            MemoGarmentPurchasing = new MemoGarmentPurchasingModel()
                            {
                                Remarks = "Remarks"
                            }
                        }
                    }
                };
            }
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplateItemMoreThanOne()
        {
            List<MemoGarmentPurchasingModel> model = new List<MemoGarmentPurchasingModel>();
            model.Add(MemoGarmentPurchasingViewModel);
            MemoryStream result = MemoGarmentPurchasingReportPdfTemplate.GeneratePdfTemplate(model, "", "Test");
            Assert.NotNull(result);
        }
    }
}
