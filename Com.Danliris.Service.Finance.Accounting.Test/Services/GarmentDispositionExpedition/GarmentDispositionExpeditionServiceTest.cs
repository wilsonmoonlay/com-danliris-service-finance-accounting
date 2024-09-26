using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentDispositionExpedition
{
    public class GarmentDispositionExpeditionServiceTest
    {
        private const string ENTITY = "DailyBankTransactions";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(FinanceDbContext)))
                .Returns(_dbContext(GetCurrentMethod()));

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        [Fact]
        public void Should_Success_GetVerified()
        {
            GarmentDispositionExpeditionService service = new GarmentDispositionExpeditionService(GetServiceProvider().Object);
            var Response = service.GetVerified("Test", 1, 1, "{}");
            Assert.Empty(Response.Data);
        }

        [Fact]
        public void Should_Success_GetByPosition()
        {
            GarmentDispositionExpeditionService service = new GarmentDispositionExpeditionService(GetServiceProvider().Object);
            var Response = service.GetByPosition("Test", 1, 1, "{}", Lib.Enums.Expedition.GarmentPurchasingExpeditionPosition.Purchasing, 1, 1, "Test");
            Assert.Empty(Response.Data);
        }

        [Fact]
        public void Should_Success_GetById()
        {
            GarmentDispositionExpeditionService service = new GarmentDispositionExpeditionService(GetServiceProvider().Object);
            var Response = service.GetById(1);
            Assert.Null(Response);
        }
    }
}
