using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
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
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentServiceTest
    {
        private const string ENTITY = "OthersExpenditureProofDocument";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

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

        private OthersExpenditureProofDocumentCreateUpdateViewModel GetCreateDataUtil()
        {
            return new OthersExpenditureProofDocumentCreateUpdateViewModel()
            {
                AccountBankCode = "BankCode",
                AccountBankId = 1,
                Date = DateTimeOffset.Now,
                Items = new List<OthersExpenditureProofDocumentCreateUpdateItemViewModel>()
                {
                    new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                    {
                        COAId = 1,
                        Debit = 1,
                        Remark = "Remark"
                    },
                    new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                    {
                        COAId = 2,
                        Debit = 2,
                        Remark = "Remark"
                    }
                },
                CekBgNo = "CekBgNo",
                Remark = "Remark",
                Type = "Operasional"
            };
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);
            var model = GetCreateDataUtil();

            var response = await service.CreateAsync(model);

            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Posting_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);
            var model = GetCreateDataUtil();

            await service.CreateAsync(model);


            var response = await service.Posting(dbContext.OthersExpenditureProofDocuments.Select(entity => entity.Id).ToList());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.DeleteAsync(createdModel.Id);

            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Get_Single_Data_By_Id()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.GetSingleByIdAsync(createdModel.Id);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_PDF_Data_By_Id()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.GetPDFByIdAsync(createdModel.Id);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Should_Success_Get_Paged_List()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.GetPagedListAsync(1, 25, "{}", createdModel.DocumentNo, "{}");

            Assert.NotEqual(0, response.Data.Count);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            model.Items.Add(new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
            {
                COAId = 3,
                Debit = 3,
                Remark = "Remark"
            });
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();

            var modelToUpdate = new OthersExpenditureProofDocumentCreateUpdateViewModel()
            {
                AccountBankCode = model.AccountBankCode,
                AccountBankId = createdModel.AccountBankId,
                Date = createdModel.Date,
                Items = new List<OthersExpenditureProofDocumentCreateUpdateItemViewModel>()
                {
                    new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                    {
                        COAId = 10,
                        Debit = 6,
                        Id = dbContext.OthersExpenditureProofDocumentItems.FirstOrDefault(item => item.OthersExpenditureProofDocumentId == createdModel.Id).Id,
                        Remark = "New remark"
                    },
                    new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                    {
                        COAId = 5,
                        Debit = 2,
                        Remark = "New remark"
                    }
                },
                CekBgNo = createdModel.CekBgNo,
                Remark = createdModel.Remark,
                Type = createdModel.Type
            };

            var response = await service.UpdateAsync(createdModel.Id, modelToUpdate);
            Assert.NotNull(response);
        }

        //[Fact]
        //public async Task Should_Success_Posting_Data()
        //{
        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var serviceProviderMock = new Mock<IServiceProvider>();
        //    serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
        //    serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
        //    serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
        //    serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

        //    var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);
        //    var model = GetCreateDataUtil();

        //    await service.CreateAsync(model);

        //    var response = await service.Posting(dbContext.OthersExpenditureProofDocuments.Select(entity => entity.Id).ToList());

        //    Assert.NotEqual(0, response);
        //}

        [Fact]
        public async Task Should_Success_GetReportlist()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.GetReportList(model.Date, model.Date, model.Date, createdModel.DocumentNo, string.Empty, 1, 25, "{}", createdModel.DocumentNo, "{}");

            Assert.NotNull(response.Data.Count);
        }

        [Fact]
        public async Task Should_Success_GetLoader()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoJournalService))).Returns(new AutoJournalServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IAutoDailyBankTransactionService))).Returns(new AutoDailyBankTransactionServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(new IdentityService() { Username = "Username", Token = "token", TimezoneOffset = 1 });
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new OthersExpenditureProofDocumentService(dbContext, serviceProviderMock.Object);

            var model = GetCreateDataUtil();
            await service.CreateAsync(model);

            var createdModel = dbContext.OthersExpenditureProofDocuments.FirstOrDefault();
            var response = await service.GetLoaderAsync(createdModel.DocumentNo, "{}");

            Assert.NotEqual(0, response.Data.Count);
        }
    }
}
