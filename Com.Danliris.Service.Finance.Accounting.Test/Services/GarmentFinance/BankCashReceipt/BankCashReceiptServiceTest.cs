using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptServiceTest
    {
        private const string ENTITY = "GarmentFinanceBankCashReceipts";

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
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        private BankCashReceiptDataUtil _dataUtil(BankCashReceiptService service, string testname)
        {
            return new BankCashReceiptDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {

            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new BankCashReceiptService(serviceProviderMock.Object);

            var model = _dataUtil(service, GetCurrentAsyncMethod()).GetNewData();
            var model2 = _dataUtil(service, GetCurrentAsyncMethod()).GetNewData();
            model2.CurrencyCode = "USD";
            //Act
            var Response = await service.CreateAsync(model);
            var Response2 = await service.CreateAsync(model2);

            //Assert
            Assert.NotEqual(0, Response);
            Assert.NotEqual(0, Response2);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new BankCashReceiptService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var Response = service.Read(1, 25, "{}", null, null, "{}");

            //Assert
            Assert.NotNull(Response);
            Assert.NotEqual(0, Response.Count);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new BankCashReceiptService(serviceProviderMock.Object);

            var dto = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var Response = service.ReadByIdAsync(dto.Id);

            //Assert
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new BankCashReceiptService(serviceProviderMock.Object);

            var model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            //Act
            var newModel = await service.ReadByIdAsync(model.Id);
            var Response = await service.DeleteAsync(newModel.Id);

            //Assert
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            //Arrange
            var dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var httpClientService = new Mock<IHttpClientService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            var service = new BankCashReceiptService(serviceProviderMock.Object);

            var model = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();
            var model2 = await _dataUtil(service, GetCurrentAsyncMethod()).GetTestData();

            BankCashReceiptModel newModel2 = new BankCashReceiptModel();
            newModel2.Id = model2.Id;
            //Act
            var newModel = await service.ReadByIdAsync(model.Id);
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            newModel2.Items = new List<BankCashReceiptItemModel> { model2.Items.First() };
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            BankCashReceiptItemModel newItem = new BankCashReceiptItemModel
            {
                BankCashReceiptId = 1,
                //C1A = 1,
                //C1B = 1,
                //C2A = 1,
                //C2B = 1,
                //C2C = 1,
            };

            newModel2.Items.Add(newItem);
            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            BankCashReceiptViewModel vm = new BankCashReceiptViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Buyer()
        {
            BankCashReceiptViewModel vm = new BankCashReceiptViewModel();
            vm.BankCashReceiptType = new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
            {
                Id = 1,
                Name = "PENJUALAN EKSPOR"
            };
            vm.Buyer = new Lib.ViewModels.NewIntegrationViewModel.NewBuyerViewModel
            {
                Id = 0
            };
            Assert.True(vm.Validate(null).Count() > 0);

            BankCashReceiptViewModel vm2 = new BankCashReceiptViewModel();
            vm2.BankCashReceiptType = new Lib.ViewModels.NewIntegrationViewModel.BankCashReceiptTypeViewModel
            {
                Id = 1,
                Name = "PENJUALAN LOKAL"
            };
            Assert.True(vm2.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Items_Data()
        {
            BankCashReceiptViewModel vm = new BankCashReceiptViewModel();
            vm.Items = new List<BankCashReceiptItemViewModel>
            {
                new BankCashReceiptItemViewModel()
                {
                    Id=0,
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_With_Data()
        {
            BankCashReceiptViewModel vm = new BankCashReceiptViewModel();
            vm.ReceiptDate = DateTimeOffset.Now.AddDays(7);
            vm.Bank = new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel
            {
                Id = 0
            };
            vm.Currency = new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel
            {
                Id = 0
            };
            vm.Items = new List<BankCashReceiptItemViewModel>
            {
                new BankCashReceiptItemViewModel()
                {
                    Summary = 1
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }
    }
}
