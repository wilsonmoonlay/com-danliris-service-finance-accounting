using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument;
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
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRealizationDocument
{
  public  class VBRealizationWithPOServiceTest
    {
        private const string ENTITY = "VBRealizationDocuments";
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod()
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

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }

        private VBRealizationDocumentDataUtil _dataUtil(VBRealizationWithPOService service)
        {
            return new VBRealizationDocumentDataUtil(service);
        }

        private VBRequestDocumentDataUtil _dataUtil(VBRequestDocumentService service)
        {
            return new VBRequestDocumentDataUtil(service);
        }


        [Fact]
        public void Create_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            VBRealizationWithPOService service = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);

            FormDto formData = _dataUtil(service).GetNewData_FormDto_Type_TanpaNomorVB();

            //Act
            int result = service.Create(formData);

            //Assert
            Assert.NotEqual(0,result);

        }

        [Fact]
        public void Create_with_FormTypeDenganNomorVB_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            VBRealizationWithPOService service = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);

            FormDto form = _dataUtil(service).GetNewData_FormDto_Type_DenganNomorVB();

            var vBRequestDocumentService = new VBRequestDocumentService(_dbContext, serviceProviderMock.Object);
            var vBRequestDocumentData = _dataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentWithPO();

            //Act
            int result = service.Create(form);

            //Assert
            Assert.NotEqual(0, result);

        }

        [Fact]
        public void Create_With_Existing_Data_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            VBRealizationWithPOService service = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);

            var vBRealizationDocumenData = _dataUtil(service).GetTestData_TanpaNomorVB();
            var formData = _dataUtil(service).GetNewData_FormDto_Type_TanpaNomorVB();


            //Act
            int result = service.Create(formData);

            //Assert
            Assert.NotEqual(0, result);

        }

        [Fact]
        public void Delete_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRequestDocumentService = new VBRequestDocumentService(_dbContext, serviceProviderMock.Object);
            var vBRequestDocumentData = _dataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentWithPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();


            //Act
            int result = vBRealizationWithPOService.Delete(vBRealizationDocumenData.Id);

            //Assert
            Assert.NotEqual(0, result);

        }

        

        [Fact]
        public void Read_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            ReadResponse<VBRealizationDocumentModel> result = vBRealizationWithPOService.Read(1,1,"{}",new List<string>(),"","{}");

            //Assert
            Assert.NotNull(result);
            Assert.True(0 < result.Count);

        }


        [Fact]
        public void ReadById_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            VBRealizationWithPODto result = vBRealizationWithPOService.ReadById(vBRealizationDocumenData.Id);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void ReadByUser_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            ReadResponse<VBRealizationDocumentModel> result = vBRealizationWithPOService.ReadByUser(1, 1, "{}", new List<string>(), "", "{}");

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void Update_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRequestDocumentService = new VBRequestDocumentService(_dbContext, serviceProviderMock.Object);
            var vBRequestDocumentData = _dataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentWithPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var form = _dataUtil(vBRealizationWithPOService).GetNewData_FormDto_Type_TanpaNomorVB();
            var data = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            
            //Act
            int result = vBRealizationWithPOService.Update(data.Id, form);

            //Assert
            Assert.NotEqual(0 ,result);

        }

        [Fact]
        public void ReadModelById_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            VBRealizationPdfDto result = vBRealizationWithPOService.ReadModelById(vBRealizationDocumenData.Id);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void ReadModelById_Return_Null()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            VBRealizationPdfDto result = vBRealizationWithPOService.ReadModelById(vBRealizationDocumenData.Id+1);

            //Assert
            Assert.Null(result);

        }
    }
}
