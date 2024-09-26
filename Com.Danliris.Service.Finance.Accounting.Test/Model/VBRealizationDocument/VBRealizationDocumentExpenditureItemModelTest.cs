using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.VBRealizationDocument
{
    public class VBRealizationDocumentExpenditureItemModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            VBRealizationDocumentNonPOExpenditureItemViewModel viewModel = new VBRealizationDocumentNonPOExpenditureItemViewModel()
            {
                IncomeTax = new IncomeTaxViewModel()
                {
                    Name = "Name",
                    Rate = 1
                },
                VatTax = new VatTaxViewModel()
                {
                    Id = "1",
                    Rate = "10"
                }
            };

            VBRealizationDocumentExpenditureItemModel model = new VBRealizationDocumentExpenditureItemModel(1, viewModel);
            model.SetDate(DateTimeOffset.Now, "user", "userAgent");
            model.SetRemark("newRemark", "user", "userAgent");
            model.SetAmount(1, "user", "userAgent");
            model.SetUseVat(true, "user", "userAgent");
            model.SetUseIncomeTax(true, "user", "userAgent");
            model.SetIncomeTax(1,2, "newIncomeTaxName", "user", "userAgent");
            model.SetVatTax("1", "newVatTax", "user", "userAgent");
            model.SetIncomeTaxBy("newIncomeTaxBy", "user", "userAgent");
            
        }

    }
}
