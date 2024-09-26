using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentPurchasingExpedition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentPurchasingExpedition
{
  public  class GarmentPurchasingExpeditionServiceTest
    {
        private const string Entity = "GarmentPurchasingExpeditions";

        private string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        private FinanceDbContext GetDbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            return new FinanceDbContext(optionsBuilder.Options);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { TimezoneOffset = 0, Token = "token", Username = "username" });

            return serviceProviderMock;
        }


        private GarmentPurchasingExpeditionDataUtil GetDataUtil(GarmentPurchasingExpeditionService service)
        {
            return new GarmentPurchasingExpeditionDataUtil(service);
        }


        [Fact]
        public async Task Should_Success_SendToAccounting()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);
           
            var service = new GarmentPurchasingExpeditionService( serviceProviderMock.Object);
            var formDto = GetDataUtil(service).GetNewData_SendToVerificationAccountingForm();

            //Act
            var result = await service.SendToAccounting(formDto);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_SendToPurchasing()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);
           
            var dto =await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.SendToPurchasing(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_SendToVerification()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);
            var formDto = GetDataUtil(service).GetNewData_SendToVerificationAccountingForm();

            //Act
            var result = await service.SendToVerification(formDto);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_GetByPosition()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = service.GetByPosition("1", 1, 10, "{}", GarmentPurchasingExpeditionPosition.Purchasing,1,1, "IDR");

            //Assert
            Assert.NotNull(result);
        }




        [Fact]
        public async Task Should_Success_GetSendToVerificationOrAccounting()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result =  service.GetSendToVerificationOrAccounting("",1,10,"{}");
            var result2 =  service.GetSendToVerificationOrAccounting("1",1,10,"{}");

            //Assert
            Assert.NotNull( result);
            Assert.NotEqual(0,result.Count);
            Assert.Equal(0,result2.Count);
        }


        [Fact]
        public async Task Should_Success_GetVerified()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = service.GetVerified("", 1, 10, "{}");
            var result2 = service.GetVerified("1", 1, 10, "{}");

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Count);
            Assert.Equal(0, result2.Count);
        }


        [Fact]
        public async Task Should_Success_VoidVerificationAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.VoidVerificationAccepted(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_VoidCashierAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.VoidCashierAccepted(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_VoidAccountingAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.VoidAccountingAccepted(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_VerificationAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.VerificationAccepted(new List<int>() { dto.Id });

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_CashierAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.CashierAccepted(new List<int>() { dto.Id });

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_AccountingAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.AccountingAccepted(new List<int>() { dto.Id });

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_PurchasingAccepted()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.PurchasingAccepted(new List<int>() { dto.Id });

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_SendToAccounting_With_Id()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.SendToAccounting(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_SendToCashier_With_Id()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            //Act
            var result = await service.SendToCashier(dto.Id);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_SendToPurchasingRejected()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
               .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("garment-purchasing-expeditions/internal-notes/position")), It.IsAny<HttpContent>()))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new GarmentPurchasingExpeditionService(serviceProviderMock.Object);

            var dto = await GetDataUtil(service).GetTestData();
            var ids = new List<int>();
            ids.Add(dto.Id);
            //Act
            var result = await service.SendToPurchasingRejected(dto.Id,dto.Remark);
            var result2 = await service.SendToPurchasingRejected(ids, dto.Remark);

            //Assert
            Assert.NotEqual(0, result);
            Assert.NotEqual(0, result2);
        }

    }
}
