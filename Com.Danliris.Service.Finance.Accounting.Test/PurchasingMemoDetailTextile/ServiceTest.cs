using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PurchasingMemoDetailTextile
{
    public class ServiceTest
    {
        private const string ENTITY = "PurchasingMemoDetailTextile";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext GetDbContext(string testName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider(FinanceDbContext dbContext)
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            var mockCreditorAccountService = new Mock<ICreditorAccountService>();
            mockCreditorAccountService.Setup(service => service.CreateFromPurchasingMemoTextile(It.IsAny<CreditorAccountPurchasingMemoTextileFormDto>()))
                .Returns(1);

            mockCreditorAccountService.Setup(service => service.DeleteFromPurchasingMemoTextile(It.IsAny<CreditorAccountPurchasingMemoTextileFormDto>()))
                .Returns(1);

            serviceProvider
                .Setup(x => x.GetService(typeof(ICreditorAccountService)))
                .Returns(mockCreditorAccountService.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);


            return serviceProvider;
        }

        private FormDto GetValidFormDisposition()
        {
            return new FormDto()
            {
                Currency = new CurrencyDto(1, "IDR", 1),
                Date = DateTimeOffset.Now,
                Details = new List<FormDetailDto>(),
                Division = new DivisionDto(1, "UM", "TEST"),
                Items = new List<FormItemDto>()
                {
                    new FormItemDto(new DispositionDto(1, "1", DateTimeOffset.Now, new List<FormDetailDto>()
                    {
                        new FormDetailDto(new ExpenditureDto(1, "1", DateTimeOffset.Now), new SupplierDto(1, "Code", "Name"), "", new UnitPaymentOrderDto(1, "1", DateTimeOffset.Now), new List<UnitReceiptNoteDto>() 
                        {
                            new UnitReceiptNoteDto(1, "1", DateTimeOffset.Now)
                        }, 1, 1, 1, 1)
                    }))
                },
                Remark = "",
                SupplierIsImport = false,
                Type = PurchasingMemoType.Disposition
            };
        }

        private FormDto GetValidFormNonDisposition()
        {
            return new FormDto()
            {
                Currency = new CurrencyDto(1, "IDR", 1),
                Date = DateTimeOffset.Now,
                Details = new List<FormDetailDto>(),
                Division = new DivisionDto(1, "UM", "TEST"),
                Items = new List<FormItemDto>()
                {
                    new FormItemDto(new DispositionDto(1, "1", DateTimeOffset.Now, new List<FormDetailDto>()
                    {
                        new FormDetailDto(new ExpenditureDto(1, "1", DateTimeOffset.Now), new SupplierDto(1, "Code", "Name"), "", new UnitPaymentOrderDto(1, "1", DateTimeOffset.Now), new List<UnitReceiptNoteDto>()
                        {
                            new UnitReceiptNoteDto(1, "1", DateTimeOffset.Now)
                        }, 1, 1, 1, 1)
                    }))
                },
                Remark = "",
                SupplierIsImport = false,
                Type = PurchasingMemoType.NonDisposition
            };
        }

        private PurchasingMemoDetailTextileDto GetDataDisposition(string testName)
        {
            var form = GetValidFormDisposition();
            var dbContext = GetDbContext(testName);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdId = service.Create(form);
            return service.Read(createdId);
        }

        private PurchasingMemoDetailTextileDto GetDataNonDisposition(string testName)
        {
            var form = GetValidFormNonDisposition();
            var dbContext = GetDbContext(testName);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdId = service.Create(form);
            return service.Read(createdId);
        }

        [Fact]
        public void Should_Success_Get_Data()
        {
            var currentMethod = GetCurrentMethod();
            var dbContext = GetDbContext(currentMethod);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdData = GetDataDisposition(currentMethod);
            var response = service.Read(createdData.DocumentNo, createdData.Type);
            Assert.NotEmpty(response.Data);
        }

        [Fact]
        public void Should_Success_Get_AutoComplete()
        {
            var currentMethod = GetCurrentMethod();
            var dbContext = GetDbContext(currentMethod);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdData = GetDataDisposition(currentMethod);
            var response = service.Read(createdData.DocumentNo);
            Assert.NotEmpty(response);
        }

        //[Fact]
        //public void Should_Success_Get_Diposition()
        //{
        //    var currentMethod = GetCurrentMethod();
        //    var dbContext = GetDbContext(currentMethod);
        //    var serviceProviderMock = GetServiceProvider(dbContext);
        //    var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
        //    var createdData = GetDataDisposition(currentMethod);
        //    var createdItem = createdData.Items.FirstOrDefault();
        //    var response = service.ReadDispositions(createdItem.Disposition.DocumentNo, createdData.Division.Id, false, createdData.Currency.Code);
        //    Assert.NotEmpty(response);
        //}

        [Fact]
        public void Should_Success_Update_DataDisposition()
        {
            var currentMethod = GetCurrentMethod();
            var dbContext = GetDbContext(currentMethod);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdData = GetDataDisposition(currentMethod);
            var form = GetValidFormDisposition();
            var response = service.Update(createdData.Id, form);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public void Should_Success_Update_DataNonDisposition()
        {
            var currentMethod = GetCurrentMethod();
            var dbContext = GetDbContext(currentMethod);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdData = GetDataNonDisposition(currentMethod);
            var form = GetValidFormNonDisposition();
            var response = service.Update(createdData.Id, form);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public void Should_Success_Delete_Data()
        {
            var currentMethod = GetCurrentMethod();
            var dbContext = GetDbContext(currentMethod);
            var serviceProviderMock = GetServiceProvider(dbContext);
            var service = new PurchasingMemoDetailTextileService(serviceProviderMock.Object);
            var createdData = GetDataDisposition(currentMethod);
            var form = GetValidFormDisposition();
            var response = service.Delete(createdData.Id);
            Assert.NotEqual(0, response);
        }
    }
}
