using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.LocalDebiturBalance;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.GarmentLocalDebiturBalance
{
    public class GarmentLocalDebiturBalanceModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            GarmentLocalDebiturBalanceModel model = new GarmentLocalDebiturBalanceModel {
                Id=1,
                BalanceAmount=1000,
                BalanceDate=DateTimeOffset.Now,
                BuyerCode="code",
                BuyerName="name",
                BuyerId=1
            };

            Assert.NotNull(model);

        }
    }
}
