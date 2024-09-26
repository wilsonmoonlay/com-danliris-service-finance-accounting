using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRealizationDocumentNonPO
{
  public  class VBRealizationDocumentNonPOServiceTest
    {
        private const string ENTITY = "VBRealizationDocumentNonPOService";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext GetDbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProviderMock()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            //serviceProvider
            //    .Setup(x => x.GetService(typeof(IHttpClientService)))
            //    .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        public VBRealizationDocumentNonPODataUtil GetDataUtil(VBRealizationDocumentNonPOService service)
        {
            return new VBRealizationDocumentNonPODataUtil(service);
        }

        private VBRequestDocumentDataUtil GetdataUtil(VBRequestDocumentService service)
        {
            return new VBRequestDocumentDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_CreateAsync()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var vm = GetDataUtil(service).GetNewData_VBRealizationDocumentNonPOViewModel();
            
            //Act
            var result =await service.CreateAsync(vm);

            //Assert
            Assert.NotEqual(0, result);

        }


        [Fact]
        public async Task Should_Fail_CreateAsync()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var vm = GetDataUtil(service).GetNewData_VBRealizationDocumentNonPOViewModel();

            //Act and Assert
            await Assert.ThrowsAsync<System.NullReferenceException>(() => service.CreateAsync(null));
           
        }

        [Fact]
        public async Task Should_Success_DeleteAsync()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var vm = await GetDataUtil(service).GetTestData();

            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, serviceProviderMock.Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            //Act
            var result = await service.DeleteAsync(vm.Id);

            //Assert
            Assert.NotEqual(0, result);

        }

        [Fact]
        public async Task Should_Success_Read()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var vm = await GetDataUtil(service).GetTestData();

            //Act
            var result = service.Read(1, 25, "{}", new List<string> (), "", "{}");

            //Assert
            Assert.True(0 < result.Data.Count);
        }

        [Fact]
        public async Task Should_Success_UpdateAsync()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var data = await GetDataUtil(service).GetTestData();
            var vm = GetDataUtil(service).GetNewData_VBRealizationDocumentNonPOViewModel();

            //Act
            var result = await service.UpdateAsync(data.Id, vm);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_UpdateAsync_When_VB_DocumentId_Exists()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var data = await GetDataUtil(service).GetTestData();
            var vm = GetDataUtil(service).GetNewData_VBRealizationDocumentNonPOViewModel();

            vm.VBDocument.Id = 2;
            var result = await service.UpdateAsync(data.Id, vm);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Input_When_VB_InvoiceNo_Exists()
        {
            //Setup
            var serviceProviderMock = GetServiceProviderMock();
            var dbContext = GetDbContext(GetCurrentMethod());
            VBRealizationDocumentNonPOService service = new VBRealizationDocumentNonPOService(serviceProviderMock.Object, dbContext);
            var data = GetDataUtil(service).GetTestData();
            var vm = GetDataUtil(service).GetNewData_VBRealizationDocumentNonPOViewModel();

            vm.VBDocument.Id = 1;
            vm.InvoiceNo = "Test123";
            var result = service.CheckInvoiceNo(vm.VBDocument.Id, vm.InvoiceNo);

            //Assert
            Assert.Equal(0, result);
        }
    }
}
