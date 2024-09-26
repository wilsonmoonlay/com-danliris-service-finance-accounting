using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Reports.LocalSalesDebtorReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.Reports.LocalSalesDebtorReport
{
    public class LocalSalesDebtorViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            LocalSalesDebtorReportViewModel viewmodel = new LocalSalesDebtorReportViewModel()
            {
                beginingBalance = 1,
                buyerCode = "code",
                buyerName = "name",
                endBalance = 1,
                index = "1",
                moreThanNinety = 1,
                normal = 1,
                oneThirty = 1,
                receipt = 1,
                sales = 1,
                sixtyNinety = 1,
                thirtySixty = 1,
                timeSpan = TimeSpan.MinValue,
                total = "total",
            };



            Assert.Equal(1, viewmodel.beginingBalance);
            Assert.Equal("code", viewmodel.buyerCode);
            Assert.Equal("name", viewmodel.buyerName);
            Assert.Equal(1, viewmodel.endBalance);
            Assert.Equal("1", viewmodel.index);
            Assert.Equal(1, viewmodel.moreThanNinety);
            Assert.Equal(1, viewmodel.normal);
            Assert.Equal(1, viewmodel.oneThirty);
            Assert.Equal(1, viewmodel.receipt);
            Assert.Equal(1, viewmodel.sales);
            Assert.Equal(1, viewmodel.sixtyNinety);
            Assert.Equal(1, viewmodel.thirtySixty);
            Assert.Equal(TimeSpan.MinValue, viewmodel.timeSpan);
            Assert.Equal("total", viewmodel.total);


        }
    }
}
