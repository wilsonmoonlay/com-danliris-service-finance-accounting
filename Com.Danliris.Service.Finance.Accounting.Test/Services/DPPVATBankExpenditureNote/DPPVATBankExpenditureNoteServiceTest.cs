using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteServiceTest
    {
        private const string ENTITY = "service-finance";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext GetDbContext(string testName)
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

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        private DPPVATBankExpenditureNoteDataUtil _dataUtil(DPPVATBankExpenditureNoteService service)
        {
            return new DPPVATBankExpenditureNoteDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
              .Setup(x => x.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }

        //[Fact]
        //public async Task Should_Success_Create()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);


        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    FormDto form = _dataUtil(service).GetNewForm();

        //    //Act
        //    var response = await service.Create(form);

        //    //Assert
        //    Assert.NotEqual(0, response);
        //}

        //[Fact]
        //public async Task Should_Success_ReadById()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data =await _dataUtil(service).GetTestData();

        //    //Act
        //    var response =  service.Read(data.Id);

        //    //Assert
        //    Assert.NotEqual(0, response.Id);
        //}

        //[Fact]
        //public async Task Should_Success_ReadAll()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data = await _dataUtil(service).GetTestData();

        //    //Act
        //    var response = service.Read("",1,1,"{}");

        //    //Assert
        //    Assert.True(response.Data.Count > 0);
        //}

        //[Fact]
        //public async Task Should_Success_Update()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data = await _dataUtil(service).GetTestData();
        //    var newForm =  _dataUtil(service).GetNewForm();

        //    //Act
        //    var response =await service.Update(data.Id, newForm);

        //    //Assert
        //    Assert.NotEqual(0,response);
        //}

        //[Fact]
        //public async Task Should_Success_ExpenditureReport()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data = await _dataUtil(service).GetTestData();
        //    var newForm = _dataUtil(service).GetNewForm();

        //    //Act
        //    var response =  service.ExpenditureReport(1,1,1,1,data.Date,data.Date);

        //    //Assert
        //    Assert.True(response.Count > 0);
        //}

        //[Fact]
        //public async Task Should_Success_Posting()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data = await _dataUtil(service).GetTestData();
        //    var newForm = _dataUtil(service).GetNewForm();

        //    //Act
        //    var response =await service.Posting(new List<int>() { 1});

        //    //Assert
        //    Assert.NotEqual( 0, response );
        //}


        //[Fact]
        //public async Task Should_Success_Delete()
        //{
        //    //Arrange
        //    var dbContex = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProvider = GetServiceProvider();

        //    serviceProvider
        //      .Setup(x => x.GetService(typeof(IHttpClientService)))
        //      .Returns(new HttpClientTestService());

        //    serviceProvider
        //        .Setup(s => s.GetService(typeof(FinanceDbContext)))
        //        .Returns(dbContex);

        //    DPPVATBankExpenditureNoteService service = new DPPVATBankExpenditureNoteService(serviceProvider.Object);
        //    var data = await _dataUtil(service).GetTestData();
           
        //    //Act
        //    var response = await service.Delete(data.Id);

        //    //Assert
        //    Assert.NotEqual(0, response);
        //}
    }
}
