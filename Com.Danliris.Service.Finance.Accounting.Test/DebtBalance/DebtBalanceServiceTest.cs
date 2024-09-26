using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.DebtBalance
{
    public class DebtBalanceServiceTest
    {
        private const string ENTITY = "DebtBalance";
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

            var dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private GarmentDebtBalanceModel GenerateDataUtil(FinanceDbContext dbContext)
        {
            var model = new GarmentDebtBalanceModel(1, "category", "billsNo", "paymentBills", 1, "deliveryOrderNo", 1, "supplier", "supplierName", false, 1, "IDR", 1, "", DateTimeOffset.Now, 1, 1, "");
            EntityExtension.FlagForCreate(model, "unit-test", "data-util");
            dbContext.GarmentDebtBalances.Add(model);
            dbContext.SaveChanges();

            return model;
        }

        private IServiceProvider GetServiceProvider(FinanceDbContext dbContext)
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
                .Returns(dbContext);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(FinanceDbContext)))
               .Returns(dbContext);

            serviceProviderMock
               .Setup(x => x.GetService(typeof(IIdentityService)))
               .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            return serviceProviderMock.Object;
        }

        private CustomsFormDto GetValidCustomsForm()
        {
            return new CustomsFormDto()
            {
                BillsNo = "BillsNo",
                CurrencyCode = "IDR",
                CurrencyId = 1,
                GarmentDeliveryOrderId = 1,
                GarmentDeliveryOrderNo = "SJ No",
                PaymentBills = "PaymentBills",
                PurchasingCategoryId = 1,
                PurchasingCategoryName = "Category",
                SupplierId = 1,
                SupplierName = "Supplier",
            };
        }

        private InvoiceFormDto GetValidInvoiceForm()
        {
            return new InvoiceFormDto()
            {
                IncomeTaxAmount = 1,
                InvoiceDate = DateTimeOffset.Now,
                InvoiceId = 1,
                InvoiceNo = "InvoiceNo",
                IsPayIncomeTax = true,
                IsPayVAT = true,
                VATAmount = 10
            };
        }

        private InternalNoteFormDto GetValidInternalNoteForm()
        {
            return new InternalNoteFormDto()
            {
                InternalNoteId = 1,
                InternalNoteNo = "InternalNoteNo"
            };
        }

        private BankExpenditureNoteFormDto GetValidBankExpenditureNoteForm()
        {
            return new BankExpenditureNoteFormDto()
            {
                BankExpenditureNoteId = 1,
                BankExpenditureNoteInvoiceAmount = 100,
                BankExpenditureNoteNo = "BankExpenditureNoteNo"
            };
        }

        private GarmentDebtBalanceService GetService(string methodName)
        {
            var dbContext = GetDbContext(methodName);

            var serviceProvider = GetServiceProvider(dbContext);

            return new GarmentDebtBalanceService(serviceProvider);
        }

        [Fact]
        public void Should_Success_Get_Data_With_Match_Params()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            GenerateDataUtil(dbContext);

            var serviceProvider = GetServiceProvider(dbContext);

            var service = new GarmentDebtBalanceService(serviceProvider);

            var result = service.GetDebtBalanceCardDto(1, DateTimeOffset.Now.Month, DateTimeOffset.Now.Year);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void Should_Success_Create_From_Customs()
        {
            var form = GetValidCustomsForm();
            var service = GetService(GetCurrentMethod());
            var result = service.CreateFromCustoms(form);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Update_From_Invoice()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            GenerateDataUtil(dbContext);
            var form = GetValidInvoiceForm();
            var service = GetService(GetCurrentMethod());
            var result = service.UpdateFromInvoice(1, form);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Update_From_InternalNote()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            GenerateDataUtil(dbContext);
            var form = GetValidInternalNoteForm();
            var service = GetService(GetCurrentMethod());
         
            var result = service.UpdateFromInternalNote(1, form);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Update_From_BankExpenditureNote()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.UpdateFromBankExpenditureNote(1, form);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceCardWithBeforeBalanceAndTotalDto()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data= GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBeforeBalanceAndTotalDto(data.SupplierId,data.ArrivalDate.Month,data.ArrivalDate.Year);
            Assert.True( result.Count >0 );
        }

        [Fact]
        public void Should_Success_GetDebtBalanceCardIndex()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardIndex(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceCardWithBalanceBeforeIndex()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBalanceBeforeIndex(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceSummary()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceSummary(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year,false,data.SupplierIsImport);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceSummaryAndTotalCurrency()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceSummaryAndTotalCurrency(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year, false, data.SupplierIsImport);
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_RemoveBalance()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.RemoveBalance(data.GarmentDeliveryOrderId);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_EmptyInternalNoteValue()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.EmptyInternalNoteValue(data.GarmentDeliveryOrderId);
            Assert.NotEqual(0, result);
        }

        //[Fact]
        //public void Should_Success_EmptyInvoiceValue()
        //{
        //    var dbContext = GetDbContext(GetCurrentMethod());
        //    var data = GenerateDataUtil(dbContext);
        //    var form = GetValidBankExpenditureNoteForm();
        //    var service = GetService(GetCurrentMethod());
        //    var result = service.EmptyInvoiceValue(data.GarmentDeliveryOrderId);
        //    Assert.NotEqual(0, result);
        //}

        [Fact]
        public void Should_Success_EmptyBankExpenditureNoteValue()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.EmptyBankExpenditureNoteValue(data.GarmentDeliveryOrderId);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceDetail()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            
            var filter = GarmentDebtBalanceDetailFilterEnum.All;
            var result = service.GetDebtBalanceDetail(data.ArrivalDate, filter,data.SupplierId,data.CurrencyId,data.PaymentType);
            Assert.True(0 < result.Count);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceCard()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBeforeBalanceAndSaldoAkhirDto(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year);
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceBeforeAndRemaining()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year);
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceBeforeAndRemainingIndexBillsNo()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex("BillsNo");
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceBeforeAndRemainingIndexPaymentBills()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceCardWithBalanceBeforeAndRemainBalanceIndex("PaymentBills");
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Success_GetDebtBalanceSummaryForeign()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.GetDebtBalanceSummary(data.SupplierId, data.ArrivalDate.Month, data.ArrivalDate.Year, true, true);
            Assert.NotNull(result);
        }


        [Fact]
        public void Should_Success_UpdateFromMemo()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var data = GenerateDataUtil(dbContext);
            var form = GetValidBankExpenditureNoteForm();
            var service = GetService(GetCurrentMethod());
            var result = service.UpdateFromMemo(data.GarmentDeliveryOrderId, 1, "", 1, 1);
            Assert.True(result > 0);
        }

        //[Fact]
        //public void Should_Success_GenerateExcel()
        //{

        //}
    }
}
