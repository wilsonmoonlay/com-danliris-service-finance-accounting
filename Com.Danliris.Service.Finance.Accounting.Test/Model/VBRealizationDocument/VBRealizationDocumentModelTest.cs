using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.VBRealizationDocument
{
    public class VBRealizationDocumentModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            VBRealizationDocumentModel model = new VBRealizationDocumentModel();
            model.SetIsCompleted(DateTimeOffset.Now.AddDays(1), "user", "userAgent", null);
            model.SetCurrency(1, "IDR", "Rp", 1, "description", "user", "userAgent");
            model.SetUnit(1, "newUnitCode", "newUnitName", "user", "userAgent");
            model.SetDivision(1, "newDivisionCode", "newDivisionName", "user", "userAgent");
            model.SetVBRequest(1, "newVBRequestNo", DateTimeOffset.Now, DateTimeOffset.Now, "newVBRequestCreatedBy", 1, "newPurpose", "user", "userAgent");
            model.SetAmount(1, "user", "userAgent");
        }
    }
}
