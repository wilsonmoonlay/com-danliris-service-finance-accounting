using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class ChartOfAccountViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            int id = 1;
            string code = "code test";
            string name = "name test";

            ChartOfAccountViewModel viewmodel = new ChartOfAccountViewModel()
            {
                Id = id.ToString(),
                Code = code,
                Name = name,
            };



            Assert.Equal(id, Convert.ToInt32(viewmodel.Id));
            Assert.Equal(code, viewmodel.Code);
            Assert.Equal(name, viewmodel.Name);

        }
    }
}
