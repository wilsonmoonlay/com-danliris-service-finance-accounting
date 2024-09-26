using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.ClearaceVB
{
    public class ClearaceVBServiceTest
    {
        private const string ENTITY = "ClearanceVB";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext _dbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .UseInternalServiceProvider(serviceProvider);

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private ClearaceVBDataUtil _dataUtil(ClearaceVBService service)
        {
            return new ClearaceVBDataUtil(service);
        }

        public VBRealizationDocumentNonPODataUtil _dataUtil(VBRealizationDocumentNonPOService service)
        {
            return new VBRealizationDocumentNonPODataUtil(service);
        }

        private VBRealizationDocumentExpeditionDataUtil _dataUtil(VBRealizationDocumentExpeditionService service, FinanceDbContext financeDbContext)
        {
            return new VBRealizationDocumentExpeditionDataUtil(service, financeDbContext);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider.Setup(s => s.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProvider.Setup(s => s.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceTestHelper());

            return serviceProvider;
        }

        [Fact]
        public  void Should_Success_Filter()
        {
            //Arrange
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();
            var service = new ClearaceVBService(serviceProviderMock.Object, dbContext);
            var clearaceVBViewModel =  _dataUtil(service).GetNewData_ClearaceVBViewModel();
            var data = new List<ClearaceVBViewModel>()
            {
                clearaceVBViewModel
            }.AsQueryable();

            var filterDictionary = new Dictionary<string, object>();
            filterDictionary.Add("Appliciant", "Appliciant");

            //Act
            var result = ClearaceVBService.Filter(data, filterDictionary);

            ////Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_Read_Data()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();
            var service = new ClearaceVBService(serviceProviderMock.Object, dbContext);
            var data =await _dataUtil(service).GetTestData();
           
            var result = service.Read(1, 10, "{}", new List<string>(), "", "{}");

            Assert.NotNull(result);
        }



        //[Fact]
        //public async Task Success_Success_ClearanceVBPost()
        //{
        //    var dbContext = _dbContext(GetCurrentMethod());
        //    var serviceProvider = GetServiceProvider().Object;
        //    var service = new ClearaceVBService(serviceProvider, dbContext);

        //    var data = await _dataUtil(service).GetTestData();

        //    var vBRealizationDocumentNonPOService = new VBRealizationDocumentNonPOService(serviceProvider, dbContext);
        //    var vm = await _dataUtil(vBRealizationDocumentNonPOService).GetTestData();

        //    var VBRealizationDocumentExpedition = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
        //    var model = _dataUtil(VBRealizationDocumentExpedition, dbContext).GetTestData_VBRealizationDocumentExpedition();


        //    var dto = _dataUtil(service).GetNewData_ClearenceFormDto();

        //    var Response = await service.ClearanceVBPost(dto);
        //    Assert.NotEqual(0, Response);
        //}


        [Fact]
        public async void PreSalesPost_Success()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProvider = GetServiceProvider().Object;
            ClearaceVBService service = new ClearaceVBService(serviceProvider, dbContext);

            var data = await _dataUtil(service).GetTestData();
            var listData = new List<ClearencePostId> { new ClearencePostId() { VBRequestId = data.Id } };
            var Response = await service.ClearanceVBPost(listData);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async void PreSalesUnPost_Success()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProvider = GetServiceProvider().Object;
            ClearaceVBService service = new ClearaceVBService(serviceProvider, dbContext);

            var data = await _dataUtil(service).GetTestData();
            var Response = await service.ClearanceVBUnpost(data.Id);
            Assert.NotEqual(0, Response);
        }
    }
}
