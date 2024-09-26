using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services
{
    public class GarmentInvoicePurchasingDispositionServiceTest
    {
        private const string ENTITY = "GarmentPurchasingDispositionService";
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod()
        {

            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
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

            var dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IAutoJournalService)))
                .Returns(new AutoJournalServiceTestHelper());

            var mockAutoDailyBankTransaction = new Mock<IAutoDailyBankTransactionService>();
            mockAutoDailyBankTransaction
                .Setup(x => x.AutoCreateFromGarmentInvoicePurchasingDisposition(It.IsAny<GarmentInvoicePurchasingDispositionModel>()))
                .ReturnsAsync(1);
            serviceProvider
                .Setup(x => x.GetService(typeof(IAutoDailyBankTransactionService)))
                .Returns(mockAutoDailyBankTransaction.Object);

            mockAutoDailyBankTransaction
                .Setup(x => x.AutoCreateVbApproval(It.IsAny<List<ApprovalVBAutoJournalDto>>()))
                .ReturnsAsync(1);

            var documentNoContentResult = new BaseResponse<string>();

            var mockHttpClient = new Mock<IHttpClientService>();
            mockHttpClient
                .Setup(http => http.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(documentNoContentResult)) });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(mockHttpClient.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Create()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            var result = await service.CreateAsync(new GarmentInvoicePurchasingDispositionModel() { InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(1, expedition.Id, "Test") } });
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_Paid_Before_Empty()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            var result = await service.CreateAsync(new GarmentInvoicePurchasingDispositionModel() { InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(-1, expedition.Id, "Test") } });
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Not_Empty_Read_Created_Data()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            await service.CreateAsync(new GarmentInvoicePurchasingDispositionModel() { InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, expedition.Id, "Test") } });
            var result = service.Read(1, 10, "{}", new List<string>(), null, "{}");

            Assert.NotEmpty(result.Data);

        }

        [Fact]
        public async Task Should_Success_Delete_Created_Data()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, expedition.Id, "Test") } };
            await service.CreateAsync(model);

            var result = await service.DeleteAsync(model.Id);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Update_Created_Data()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, expedition.Id, "Test") } };
            await service.CreateAsync(model);

            var result = await service.UpdateAsync(model.Id, model);
            Assert.NotEqual(0, result);
        }
		//error
        //[Fact]
        //public void Should_Success_Read_Details_By_EPOId()
        //{
        //    var serviceProviderMock = GetServiceProvider();
        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

        //    var expedition = new GarmentDispositionExpeditionModel();
        //    EntityExtension.FlagForCreate(expedition, "Test", "Test");
        //    dbContext.GarmentDispositionExpeditions.Add(expedition);
        //    dbContext.SaveChanges();

        //    var result = service.ReadDetailsByEPOId("Test");
        //    Assert.NotNull(result);
        //}

        [Fact]
        public async Task Should_Success_Post_Data()
        {
            var serviceProviderMock = GetServiceProvider();
            var dbContext = GetDbContext(GetCurrentMethod());

            var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

            var expedition = new GarmentDispositionExpeditionModel();
            EntityExtension.FlagForCreate(expedition, "Test", "Test");
            dbContext.GarmentDispositionExpeditions.Add(expedition);
            dbContext.SaveChanges();

            var model = new GarmentInvoicePurchasingDispositionPostingViewModel() { ListIds = new List<GarmentInvoicePurchasingDispositionPostingIdViewModel>() { new GarmentInvoicePurchasingDispositionPostingIdViewModel() { Id = 1 } } };

            var result = await service.Post(model);
            Assert.Equal(0, result);
		}
		

		[Fact]
		public void Should_Success_getMonitoring()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate=DateTimeOffset.Now,InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };
			var model1 = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };
			var model2 = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test1") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			EntityExtension.FlagForCreate(model1, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model1);
			EntityExtension.FlagForCreate(model2, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model2);
			dbContext.SaveChanges();


			var result = service.GetMonitoring(null,null,DateTimeOffset.Now.AddDays(-1),DateTimeOffset.Now,7);
			Assert.NotNull(result);
		}
		[Fact]
		public void Should_Success_getMonitoringNullDate()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };
			var model1 = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };
			var model2 = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test1") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			EntityExtension.FlagForCreate(model1, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model1);
			EntityExtension.FlagForCreate(model2, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model2);
			dbContext.SaveChanges();


			var result = service.GetMonitoring(null, null, Convert.ToDateTime("0001-01-01 00:00:00.0000000 +00:00"), Convert.ToDateTime( "0001-01-01 00:00:00.0000000 +00:00"), 7);
			Assert.NotNull(result);
		}

		[Fact]
		public void Should_Success_getExcel()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			dbContext.SaveChanges();

			var result = service.DownloadReportXls("Test", "Test", DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now);
			Assert.NotNull(result);
		}
		[Fact]
		public void Should_Success_getExcelInvoiceNull()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			dbContext.SaveChanges();

			var result = service.DownloadReportXls( null, "Test", DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now);
			Assert.NotNull(result);
		}
		[Fact]
		public void Should_Success_getExcelDispoNull()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			dbContext.SaveChanges();

			var result = service.DownloadReportXls("Test", null, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now);
			Assert.NotNull(result);
		}



		[Fact]
		public void Should_Success_getLoader()
		{
			var serviceProviderMock = GetServiceProvider();
			var dbContext = GetDbContext(GetCurrentMethod());

			var service = new GarmentInvoicePurchasingDispositionService(serviceProviderMock.Object, dbContext);

			var model = new GarmentInvoicePurchasingDispositionModel() { InvoiceDate = DateTimeOffset.Now, InvoiceNo = "Test", SupplierName = "Test", CurrencyCode = "Code", BankName = "BankName", Items = new List<GarmentInvoicePurchasingDispositionItemModel>() { new GarmentInvoicePurchasingDispositionItemModel(0, 1, "Test") } };

			EntityExtension.FlagForCreate(model, "Test", "Test");
			dbContext.GarmentInvoicePurchasingDispositions.Add(model);
			dbContext.SaveChanges();
		}
	}
}
