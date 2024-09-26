using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Memorial;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PDFTemplates
{
    public class GarmentFinanceMemorialPDFTemplateTest
    {
        private GarmentFinanceMemorialViewModel viewModel
        {
            get
            {
                return new GarmentFinanceMemorialViewModel
                {
                    MemorialNo = "no",
                    AccountingBookCode = "no",
                    AccountingBookId = 1,
                    AccountingBookType = "type",
                    GarmentCurrency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
                    {
                        Id = 1,
                        Code = "code",
                        Rate = 1
                    },
                    Remark = "a",
                    Date = DateTimeOffset.Now,
                    IsUsed = false,
                    Items = new List<GarmentFinanceMemorialItemViewModel>
                    {
                        new GarmentFinanceMemorialItemViewModel()
                        {
                            COA= new Lib.ViewModels.MasterCOA.COAViewModel{
                                Id=1,
                                Code="code",
                                Name="name"
                            },
                            Credit=1,
                            Debit=0,
                        },
                        new GarmentFinanceMemorialItemViewModel()
                        {
                            COA= new Lib.ViewModels.MasterCOA.COAViewModel{
                                Id=2,
                                Code="code2",
                                Name="name2"
                            },
                            Credit=0,
                            Debit=2,
                        }
                    }
                };
            }
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithCurrencyIDR()
        {
            GarmentFinanceMemorialPDFTemplate PdfTemplate = new GarmentFinanceMemorialPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(viewModel, 7);
            Assert.NotNull(result);
        }
    }
}
