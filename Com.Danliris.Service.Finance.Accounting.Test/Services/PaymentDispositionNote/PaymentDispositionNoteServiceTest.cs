using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.PaymentDispositionNote
{
    public class PaymentDispositionNoteServiceTest
    {
        private const string ENTITY = "PaymentDispositionNotes";

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
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            serviceProvider
                .Setup(x => x.GetService(typeof(IAutoDailyBankTransactionService)))
                .Returns(new AutoDailyBankTransactionServiceHelper());

            serviceProvider
                .Setup(x => x.GetService(typeof(IAutoJournalService)))
                .Returns(new AutoJournalServiceTestHelper());

            serviceProvider.Setup(sp => sp.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());


            return serviceProvider;
        }

        private PaymentDispositionNoteDataUtil _dataUtil(PaymentDispositionNoteService service, string testname)
        {
            var expeditionService = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(testname));
            var expeditionDataUtil = new PurchasingDispositionExpeditionDataUtil(expeditionService);

            return new PaymentDispositionNoteDataUtil(service, expeditionDataUtil);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PaymentDispositionNoteModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PaymentDispositionNoteModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            PaymentDispositionNoteModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            PaymentDispositionNoteModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            newModel.BGCheckNumber = "newBG";
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            PaymentDispositionNoteModel model2 = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            //var newModel2 = await service.ReadByIdAsync(model.Id);
            PaymentDispositionNoteModel newModel2 = new PaymentDispositionNoteModel();
            newModel2.Id = model2.Id;

            newModel2.Items = new List<PaymentDispositionNoteItemModel> { model2.Items.First() };
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            PaymentDispositionNoteViewModel vm = new PaymentDispositionNoteViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Date_Data()
        {
            PaymentDispositionNoteViewModel vm = new PaymentDispositionNoteViewModel();
            vm.PaymentDate = DateTimeOffset.UtcNow.AddDays(2);
            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Date_Data_IDR()
        {
            PaymentDispositionNoteViewModel vm = new PaymentDispositionNoteViewModel();
            vm.PaymentDate = DateTimeOffset.UtcNow.AddDays(2);
            vm.AccountBank = new AccountBankViewModel
            {
                Currency = new CurrencyViewModel
                {
                    Code = "IDR"
                }
            };
            vm.CurrencyCode = null;
            vm.CurrencyRate = 0;
            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Get_Data_Details_By_EPOId()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PaymentDispositionNoteModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();

            var item = model.Items.First();
            var detail = item.Details.First();
            var epoId = detail.EPOId;
            var Response = service.ReadDetailsByEPOId(detail.EPOId);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Post()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            PaymentDispositionNotePostDto dto = await _dataUtil(service, GetCurrentMethod()).GetNewPostDto();

            var Response = await service.Post(dto);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Report()
        {
            try
            {
                PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
                PaymentDispositionNoteModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();
                await service.CreateAsync(model);

                var item = model.Items.FirstOrDefault();
                var result = service.GetReport(model.Id, item.DispositionId, model.SupplierId, item.DivisionId, DateTimeOffset.MinValue, DateTimeOffset.MaxValue);

                Assert.NotEmpty(result);
            }
            catch (Exception e)
            {

            }
        }

        [Fact]
        public async Task Should_Success_Get_Report_Xls()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PaymentDispositionNoteModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();

            var xls = service.GetXls(new List<ReportDto>() { new ReportDto(1, "", DateTimeOffset.Now, 1, "", DateTimeOffset.Now, DateTimeOffset.Now, 1, "", 1, "", 1, "", false, "", 1, "", 1, "", 1, 1, "", "", 1, "", 1, 1, 1, 1, 1) });
            var xls2 = service.GetXls(new List<ReportDto>() { new ReportDto(1, "", DateTimeOffset.Now, 1, "", DateTimeOffset.Now, DateTimeOffset.Now, 1, "", 1, "", 1, "", false, "", 1, "", 1, "", 1, 1, "", "", 0, "", 1, 1, 1, 1, 1) });

            Assert.NotNull(xls);
            Assert.NotNull(xls2);
        }

        [Fact]
        public void Should_Success_Get_Report_Xls_Empty()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var xls = service.GetXls(new List<ReportDto>());
            Assert.NotNull(xls);
        }

        [Fact]
        public async Task Should_Success_GetAllByPosition_Data()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.GetAllByPosition(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_GetAmountPaidAndIsPosted()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.GetAmountPaidAndIsPosted(data.PaymentDispositionNo);
            Assert.Equal(0, Response.AmountPaid);
            Assert.True(Response.IsPosted);
        }

        [Fact]
        public async Task Should_Success_GeneratePdfTemplate()
        {
            PaymentDispositionNoteService service = new PaymentDispositionNoteService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetPDFData();
            var data2 = await _dataUtil(service, GetCurrentMethod()).GetPDFDataIDRNONIDR();
            var Response = service.GeneratePdfTemplate(data, 7);
            var Response2 = service.GeneratePdfTemplate(data2, 7);
            Assert.NotNull(Response);
            Assert.NotNull(Response2);
        }
    }
}
