using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Moq;
using Xunit;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.JournalTransaction
{
    public class AutoJournalServiceTest
    {
        private const string ENTITY = "AutoJournal";
        [MethodImpl(MethodImplOptions.NoInlining)]
        private string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext GetDbContext(string testName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

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
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

           

            
            return serviceProvider;
        }

        private VBRequestDocumentDataUtil GetdataUtil(VBRequestDocumentService service)
        {
            return new VBRequestDocumentDataUtil(service);
        }

        private VBRealizationDocumentDataUtil GetDataUtil(VBRealizationWithPOService service)
        {
            return new VBRealizationDocumentDataUtil(service);
        }

        private VBRealizationDocumentExpeditionDataUtil _dataUtil(VBRealizationDocumentExpeditionService service, FinanceDbContext financeDbContext)
        {
            return new VBRealizationDocumentExpeditionDataUtil(service, financeDbContext);
        }

        [Fact]
        public async Task Should_Success_Auto_Journal_From_Others_Bank_Expenditure()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IJournalTransactionService))).Returns(new JournalTransactionServiceTestHelper());
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IHttpClientService))).Returns(new HttpClientOthersExpenditureServiceHelper());

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            var viewModel = new OthersExpenditureProofDocumentCreateUpdateViewModel()
            {
                Items = new List<OthersExpenditureProofDocumentCreateUpdateItemViewModel>()
                {
                    new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                }
            };

            var result = await service.AutoJournalFromOthersExpenditureProof(viewModel, "any");
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Auto_Journal_From_Others_Bank_Expenditure_Reverse()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IJournalTransactionService))).Returns(new JournalTransactionServiceTestHelper());

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            var result = await service.AutoJournalReverseFromOthersExpenditureProof("any");
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_AutoJournalVBNonPOClearence_With_ViewModel_When_NotKlaring()
        {
            var dbContext = GetDbContext(GetCurrentMethod());

            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());
            //IHttpClientService
            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            //  Mock<IMasterCOAService> masterCOAServiceMock = new Mock<IMasterCOAService>();
            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);


            VBRequestDocumentService vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPODto data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO_NotKlaring();

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            AccountBankViewModel viewModel = new AccountBankViewModel()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Code = "Code",
                Currency = new CurrencyViewModel()
                {
                    Code = "Rp",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "IDR"
                }
            };

            List<int> vbRealizationIds = new List<int>()
            {
                1
            };
            var result = await service.AutoJournalVBNonPOClearence(vbRealizationIds, viewModel, null);
            Assert.NotEqual(0, result);


        }

        [Fact]
        public async Task Should_Success_AutoJournalVBNonPOClearence_With_ViewModel()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());
         
            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);


            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var expeditionService = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            var model = _dataUtil(expeditionService, dbContext).GetTestData_VBRealizationDocumentExpedition();

            var realization = new Lib.ViewModels.VBRealizationDocumentNonPO.VBRealizationDocumentNonPOViewModel()
            {
                IsInklaring = true,
                Currency = new Lib.ViewModels.VBRealizationDocumentNonPO.CurrencyViewModel()
                {
                    Code = "IDR"
                }
            };

            var expenditureitem = new Lib.ViewModels.VBRealizationDocumentNonPO.VBRealizationDocumentNonPOExpenditureItemViewModel()
            {
                PPhAmount = 1,
                PPnAmount = 1,
            };

            var unitcostitem = new Lib.ViewModels.VBRealizationDocumentNonPO.VBRealizationDocumentNonPOUnitCostViewModel()
            {
                IsSelected = true
            };

            var vbRealizations = new VBRealizationDocumentModel(realization);
            var vbRealizationItems = new VBRealizationDocumentExpenditureItemModel(2, expenditureitem);
            var vbRealizationsUnitItems = new VBRealizationDocumentUnitCostsItemModel(2, unitcostitem);

            dbContext.VBRealizationDocuments.Add(vbRealizations);
            dbContext.VBRealizationDocumentExpenditureItems.Add(vbRealizationItems);
            dbContext.VBRealizationDocumentUnitCostsItems.Add(vbRealizationsUnitItems);
            dbContext.SaveChanges();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            AccountBankViewModel viewModel = new AccountBankViewModel()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Code = "Code",
                Currency = new CurrencyViewModel()
                {
                    Code = "Rp",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "IDR"
                },
                
            };

            List<int> vbRealizationIds = new List<int>()
            {
                1,
                2
            };

            //Act
            var result = await service.AutoJournalVBNonPOClearence(vbRealizationIds, viewModel, null);
            
            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_AutoJournalInklaring_With_ViewModel()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);


            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            AccountBankViewModel viewModel = new AccountBankViewModel()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Code = "Code",
                Currency = new CurrencyViewModel()
                {
                    Code = "Rp",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "IDR"
                },

            };

            List<int> vbRealizationIds = new List<int>()
            {
                1
            };

            //Act
            var result = await service.AutoJournalInklaring(vbRealizationIds, viewModel);

            //Assert
            Assert.NotEqual(0, result);
        }

       

        [Fact]
        public async Task Should_Success_AutoJournalVBNonPOClearence()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);

            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var realization = new Lib.ViewModels.VBRealizationDocumentNonPO.VBRealizationDocumentNonPOViewModel()
            {
                IsInklaring = true,
                Currency = new Lib.ViewModels.VBRealizationDocumentNonPO.CurrencyViewModel()
                {
                    Code = "USD"
                }
            };

            var vbRealizations = new VBRealizationDocumentModel(realization);

            dbContext.VBRealizationDocuments.Add(vbRealizations);
            dbContext.SaveChanges();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            List<int> vbRealizationIds = new List<int>()
            {
                1,
                2
            };

            //Act
            var result = await service.AutoJournalVBNonPOClearence(vbRealizationIds);

            //Assert
            Assert.NotEqual(0, result);
        }


       

        [Fact]
        public async Task Should_Success_AutoJournalVBNonPOClearence_When_NotKlaring()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);

            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO_NotKlaring();

            var vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            List<int> vbRealizationIds = new List<int>()
            {
                1
            };

            //Act
            var result = await service.AutoJournalVBNonPOClearence(vbRealizationIds);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task Should_Success_AutoJournalInklaring()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);


            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = await GetdataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            var vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_DenganNomorVB();

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            List<int> vbRealizationIds = new List<int>()
            {
                1
            };

            //Act
            var result = await service.AutoJournalInklaring(vbRealizationIds);

            //Assert
            Assert.NotEqual(0, result);
        }
        [Fact]
        public async Task Should_Success_AutoJournalFromOthersExpenditureProof_With_ViewModel()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            journalTransactionServiceMock.Setup(s => s.CreateAsync(It.IsAny<JournalTransactionModel>())).ReturnsAsync(1);

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);

            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            AccountBankViewModel viewModel = new AccountBankViewModel()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Code = "Code",
                Currency = new CurrencyViewModel()
                {
                    Code = "Rp",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "IDR"
                },

            };

            List<int> vbRealizationIds = new List<int>()
            {
                1
            };

            var viewModelOtherProof = new OthersExpenditureProofDocumentModel()
            {
                Date = DateTime.Now,
                DocumentNo = "test",
                AccountBankId = 1
            };
            var viewModelOtherProofItems = new List<OthersExpenditureProofDocumentItemModel>()
            {
                new OthersExpenditureProofDocumentItemModel
                {
                    COAId = 1,
                    Debit= 10
                }
            };

            //Act
            var result = await service.AutoJournalFromOthersExpenditureProof(viewModelOtherProof, viewModelOtherProofItems);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_AutoJournalFromDailyBankTransaction()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());

            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            journalTransactionServiceMock.Setup(s => s.CreateAsync(It.IsAny<JournalTransactionModel>())).ReturnsAsync(1);

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);

            var httpClientService = new Mock<IHttpClientService>();
            HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            message.Content = new StringContent("{\"data\":{\"Id\":7,\"Code\":\"BB\",\"Rate\":13700.0,\"Date\":\"2018/10/20\"}}");

            httpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(message);

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);
            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            AccountBank acc1 = new AccountBank()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Currency= new Currency()
                {
                    Code = "Rp",
                    Symbol = "IDR"
                },
            };
            AccountBank acc2 = new AccountBank()
            {
                AccountCOA = "AccountCOA",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankCode = "BankCode",
                BankName = "BankName",
                Currency = new Currency()
                {
                    Code = "dolar",
                    Symbol = "USD"
                },
            };

            DailyBankTransactionModel dailyModel = new DailyBankTransactionModel()
            {
                AccountBankAccountName = "AccountName",
                AccountBankAccountNumber = "AccountNumber",
                AccountBankCode = "BankCode",
                AccountBankCurrencyCode = "CurrencyCode",
                AccountBankCurrencyId = 1,
                AccountBankCurrencySymbol = "CurrencySymbol",
                AccountBankId = 1,
                AccountBankName = "BankName",
                AfterNominal = 0,
                BeforeNominal = 0,
                BuyerCode = "BuyerCode",
                BuyerId = 1,
                BuyerName = "BuyerName",
                Date = DateTimeOffset.UtcNow,
                Nominal = 1000,
                ReferenceNo = "",
                ReferenceType = "ReferenceType",
                Remark = "Remark",
                SourceType = "Pendanaan",
                SourceFundingType = "Internal",
                Status = "IN",
                SupplierCode = "SupplierCode",
                SupplierName = "SupplierName",
                SupplierId = 1,
                DestinationBankAccountName = "AccountName",
                DestinationBankAccountNumber = "AccountNumber",
                DestinationBankCode = "BankCode",
                DestinationBankCurrencyCode = "CurrencyCode",
                DestinationBankCurrencyId = 1,
                DestinationBankCurrencySymbol = "CurrencySymbol",
                DestinationBankId = 1,
                DestinationBankName = "BankName",
                IsPosted = true,
                AfterNominalValas = 1,
                BeforeNominalValas = 1,
                TransactionNominal = 1,
                NominalValas = 1,
                Receiver = "Receiver",
                CurrencyRate=10
            };

            //Act
            var result = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1,acc2);
            //Assert
            Assert.NotEqual(0, result);

            dailyModel.BankCharges = 100;
            var resultwithBankCharges = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1, acc2);
            Assert.NotEqual(0, resultwithBankCharges);

            dailyModel.DestinationBankCurrencyCode = "IDR";
            dailyModel.BankCharges = 100;
            dailyModel.Rates = 100;
            var resultDiffCurrencyToIDR = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1, acc2);
            Assert.NotEqual(0, resultDiffCurrencyToIDR);

            dailyModel.DestinationBankCurrencyCode = "IDR";
            dailyModel.BankCharges = 0;
            dailyModel.Rates = 100;
            var resultDiffCurrencyToIDRNoCharges = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1, acc2);
            Assert.NotEqual(0, resultDiffCurrencyToIDR);

            dailyModel.AccountBankCurrencyCode = "IDR";
            dailyModel.DestinationBankCurrencyCode = "USD";
            dailyModel.BankCharges = 0;
            dailyModel.Rates = 100;
            var resultDiffCurrencyNoCharges = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1, acc2);
            Assert.NotEqual(0, resultDiffCurrencyNoCharges);

            dailyModel.AccountBankCurrencyCode = "IDR";
            dailyModel.DestinationBankCurrencyCode = "USD";
            dailyModel.BankCharges = 100;
            dailyModel.Rates = 100;
            var resultDiffCurrency = await service.AutoJournalFromDailyBankTransaction(dailyModel, acc1, acc2);
            Assert.NotEqual(0, resultDiffCurrency);

        }

        [Fact]
        public async Task Should_Success_AutoJournalFromDisposition()
        {
            //Setup
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            serviceProviderMock.Setup(s => s.GetService(typeof(FinanceDbContext))).Returns(dbContext);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new JournalHttpClientTestService());
            Mock<IJournalTransactionService> journalTransactionServiceMock = new Mock<IJournalTransactionService>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IJournalTransactionService)))
                .Returns(journalTransactionServiceMock.Object);

            var masterCOAServiceMock = new MasterCOAService(serviceProviderMock.Object);
            serviceProviderMock
               .Setup(x => x.GetService(typeof(IMasterCOAService)))
               .Returns(masterCOAServiceMock);
            var service = new AutoJournalService(dbContext, serviceProviderMock.Object);

            long nowTicks = DateTimeOffset.Now.Ticks;
            string nowTicksA = $"{nowTicks}a";
            string nowTicksB = $"{nowTicks}b";
            PaymentDispositionNoteModel dispositionNoteModel = new PaymentDispositionNoteModel()
            {
                SupplierImport = true,
                SupplierCode = nowTicksA,
                SupplierName = nowTicksA,
                SupplierId = 1,
                BankCurrencyCode = nowTicksA,
                BankCurrencyId = 1,
                BankCurrencyRate = 1,
                BankAccountName = nowTicksA,
                BankAccountNumber = nowTicksA,
                BankCode = nowTicksA,
                BankId = 1,
                BankName = nowTicksA,
                BankAccountCOA = nowTicksA,
                TransactionType = "Any",
                BGCheckNumber = nowTicksA,
                Amount = 1000,
                PaymentDate = DateTimeOffset.Now,

                Items = new List<PaymentDispositionNoteItemModel>
                {
                    new PaymentDispositionNoteItemModel
                    {
                        PurchasingDispositionExpeditionId=1,
                        CategoryCode="CategoryId",
                        CategoryId=1,
                        CategoryName="CategoryName",
                        DispositionDate=DateTimeOffset.Now,
                        DispositionId=1,
                        DispositionNo="DispositionNo",
                        DivisionCode="DivisionCode",
                        DivisionId=1,
                        DivisionName="DivisionName",
                        DPP=1000,
                        VatValue=100,
                        IncomeTaxValue=0,
                        ProformaNo="ProformaNo",
                        TotalPaid=1100,
                        Details= new List<PaymentDispositionNoteDetailModel>
                        {
                            new PaymentDispositionNoteDetailModel
                            {
                                ProductCode="ProductCode",
                                Price=1000,
                                ProductId=1,
                                ProductName="ProductName",
                                UnitCode="UnitCode",
                                UnitId=1,
                                UnitName="UnitName",
                                UomId=1,
                                UomUnit="UomUnit",
                                Quantity=1,
                                PurchasingDispositionDetailId=1,
                                PurchasingDispositionExpeditionItemId=1,
                                EPOId="EPOId"
                            }
                        }

                    },
                    new PaymentDispositionNoteItemModel
                    {
                        PurchasingDispositionExpeditionId=1,
                        CategoryCode="CategoryId",
                        CategoryId=1,
                        CategoryName="CategoryName",
                        DispositionDate=DateTimeOffset.Now,
                        DispositionId=1,
                        DispositionNo="DispositionNo",
                        DivisionCode="DivisionCode",
                        DivisionId=1,
                        DivisionName="DivisionName",
                        DPP=1000,
                        VatValue=0,
                        IncomeTaxValue=0,
                        ProformaNo="ProformaNo",
                        TotalPaid=1000,
                        SupplierPayment = 500,
                        Details= new List<PaymentDispositionNoteDetailModel>
                        {
                            new PaymentDispositionNoteDetailModel
                            {
                                ProductCode="ProductCode",
                                Price=1000,
                                ProductId=1,
                                ProductName="ProductName",
                                UnitCode="UnitCode",
                                UnitId=1,
                                UnitName="UnitName",
                                UomId=1,
                                UomUnit="UomUnit",
                                Quantity=1,
                                PurchasingDispositionDetailId=1,
                                PurchasingDispositionExpeditionItemId=1,
                                EPOId="EPOId"
                            },
                            new PaymentDispositionNoteDetailModel
                            {
                                ProductCode="ProductCode",
                                Price=1000,
                                ProductId=1,
                                ProductName="ProductName",
                                UnitCode="UnitCode2",
                                UnitId=1,
                                UnitName="UnitName",
                                UomId=1,
                                UomUnit="UomUnit",
                                Quantity=1,
                                PurchasingDispositionDetailId=1,
                                PurchasingDispositionExpeditionItemId=1,
                                EPOId="EPOId"
                            }
                        }

                    }
                }
            };

            //Act
            var result = await service.AutoJournalFromDisposition(dispositionNoteModel, "Username", "UserAgent");

            dispositionNoteModel.SupplierImport = false;
            var result2 = await service.AutoJournalFromDisposition(dispositionNoteModel, "Username", "UserAgent");

            //Assert
            Assert.Equal(0, result);
            Assert.Equal(0, result2);
        }
    }

    

    internal class JournalTransactionServiceTestHelper : IJournalTransactionService
    {
        public JournalTransactionServiceTestHelper()
        {
        }

        public Task<int> CreateAsync(JournalTransactionModel model)
        {
            return Task.FromResult(1);
        }

        public Task<int> CreateManyAsync(List<JournalTransactionModel> models)
        {
            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(int id)
        {
            return Task.FromResult(1);
        }

        public MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllReferenceNo(string keyword)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllReferenceNo(string keyword, bool isVB)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllReferenceType(string keyword)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllReferenceType(string keyword, bool isVB)
        {
            throw new NotImplementedException();
        }

        public Task<List<GeneralLedgerWrapperReportViewModel>> GetGeneralLedgerReport(DateTimeOffset startDate, DateTimeOffset endDate, int timezoneoffset)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> GetGeneralLedgerReportXls(DateTimeOffset startDate, DateTimeOffset endDate, int timezoneoffset)
        {
            throw new NotImplementedException();
        }

        public (ReadResponse<JournalTransactionReportHeaderViewModel>, decimal, decimal) GetReport(int page, int size, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            throw new NotImplementedException();
        }

        public Task<SubLedgerReportViewModel> GetSubLedgerReport(int? coaId, int month, int year, int timeoffset)
        {
            throw new NotImplementedException();
        }

        public Task<SubLedgerXlsFormat> GetSubLedgerReportXls(int? coaId, int? month, int? year, int timeoffset)
        {
            throw new NotImplementedException();
        }

        public Task<int> PostTransactionAsync(int id)
        {
            return Task.FromResult(1);
        }

        public Task<int> PostTransactionAsync(int id, JournalTransactionModel model)
        {
            return Task.FromResult(1);
        }

        public ReadResponse<JournalTransactionModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            throw new NotImplementedException();
        }

        public ReadResponse<JournalTransactionModel> ReadByDate(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet, int page, int size, string order, List<string> select, string keyword, string filter)
        {
            throw new NotImplementedException();
        }

        public Task<JournalTransactionModel> ReadByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public List<JournalTransactionModel> ReadUnPostedTransactionsByPeriod(int month, int year, string referenceNo, string referenceType, bool isVB)
        {
            throw new NotImplementedException();
        }

        public Task<int> ReverseJournalTransactionByReferenceNo(string referenceNo)
        {
            return Task.FromResult(1);
        }

        public Task<int> UpdateAsync(int id, JournalTransactionModel model)
        {
            return Task.FromResult(1);
        }
    }
}