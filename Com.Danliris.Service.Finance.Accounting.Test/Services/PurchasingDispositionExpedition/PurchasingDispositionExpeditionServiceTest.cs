using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionAcceptance;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionVerification;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.NewIntegrationDataUtils;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Newtonsoft.Json;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionServiceTest
    {
        private const string ENTITY = "PurchasingDispositionExpeditions";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private IServiceProvider PutServiceProvider()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update/position"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new PurchasingDispositionDataUtil().GetResultFormatterOkString()) });
            
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username", TimezoneOffset = 7 });
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            return serviceProviderMock.Object;
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

        private PurchasingDispositionExpeditionDataUtil _dataUtil(PurchasingDispositionExpeditionService service)
        {
            return new PurchasingDispositionExpeditionDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update/position")), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        private Mock<IServiceProvider> GetServiceProviderWrongHttpClient()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update/position")), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService);

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_Verification()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{'verificationFilter':''}");
            Assert.NotNull(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_No_Error_Validate_Data()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionViewModel vm = _dataUtil(service).GetNewViewModel();

            Assert.True(vm.Validate(null).Count() == 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            PurchasingDispositionExpeditionViewModel vm = new PurchasingDispositionExpeditionViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            

            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_With_two_Datas()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData2();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task UpdateAsync_Throws_NotImplementedException()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData2();
            var newModel = await service.ReadByIdAsync(model.Id);

            await Assert.ThrowsAsync<NotImplementedException>(() => service.UpdateAsync(newModel.Id, newModel));
          
        }
        [Fact]
        public async Task Update_Throws_NotImplementedException()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData2();
            var newModel = await service.ReadByIdAsync(model.Id);

            await Assert.ThrowsAsync<NotImplementedException>(() => service.Update(newModel.Id, newModel, "", 7));


        }


        [Fact]
        public async Task Should_Success_Post_Acceptance_Verification()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "VERIFICATION",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };

            var response = await service.PurchasingDispositionAcceptance(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_Post_Acceptance_Verification()
        {

            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "VERIFICATION",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };
            await Assert.ThrowsAnyAsync<Exception>(() => service.PurchasingDispositionAcceptance(null));

        }

        [Fact]
        public async Task Should_Success_Post_Acceptance_Cashier()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "CASHIER",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };

            var response = await service.PurchasingDispositionAcceptance(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Delete_Acceptance_Verification()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "VERIFICATION",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };
            var acceptedResponse = await service.PurchasingDispositionAcceptance(data);
            var newModel = await service.ReadByIdAsync(model.Id);
            var deleteResponse = await service.DeletePurchasingDispositionAcceptance(newModel.Id);
            Assert.NotEqual(0, deleteResponse);
        }

        [Fact]
        public async Task Should_Success_Delete_Acceptance_Cashier()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "CASHIER",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };
            var acceptedResponse = await service.PurchasingDispositionAcceptance(data);
            var newModel = await service.ReadByIdAsync(model.Id);
            var deleteResponse = await service.DeletePurchasingDispositionAcceptance(newModel.Id);
            Assert.NotEqual(0, deleteResponse);
        }

        [Fact]
        public async Task Should_Fail_Delete_Empty_Id()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var deleteResponse = await service.DeletePurchasingDispositionAcceptance(-1);
            Assert.Equal(0, deleteResponse);
        }

        [Fact]
        public void Should_Success_Validate_Purchasing_Disposition_Acceptance_Vm()
        {
            PurchasingDispositionAcceptanceViewModel nullVM = new PurchasingDispositionAcceptanceViewModel();
            nullVM.PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>();

            Assert.True(nullVM.Validate(null).Count() > 0);

            PurchasingDispositionAcceptanceViewModel vm = new PurchasingDispositionAcceptanceViewModel();
            vm.PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
            {
                new PurchasingDispositionAcceptanceItemViewModel()
                {
                    DispositionNo = "DispositonNo",
                    Id = 1
                }
            };

            Assert.True(vm.Validate(null).Count() == 0);

        }

        [Fact]
        public async Task Should_Success_Post_Disposition_Verification_Update_Cashier()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = model.Id,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_CASHIER_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            var response = await service.PurchasingDispositionVerification(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Post_Disposition_Verification_Create_Purchasing()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            var response = await service.PurchasingDispositionVerification(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_Post_Disposition_Verification()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            await Assert.ThrowsAnyAsync<Exception>(() => service.PurchasingDispositionVerification(null));

        }

        [Fact]
        public async Task Should_Fail_Delete_Acceptance()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            PurchasingDispositionAcceptanceViewModel data = new PurchasingDispositionAcceptanceViewModel()
            {
                Role = "CASHIER",
                PurchasingDispositionExpedition = new List<PurchasingDispositionAcceptanceItemViewModel>()
                {
                    new PurchasingDispositionAcceptanceItemViewModel()
                    {
                        DispositionNo  = model.DispositionNo,
                        Id = model.Id
                    }
                }
            };
            var acceptedResponse = await service.PurchasingDispositionAcceptance(data);
            var newModel = await service.ReadByIdAsync(model.Id);
            service = new PurchasingDispositionExpeditionService(GetServiceProviderWrongHttpClient().Object, _dbContext(GetCurrentMethod()));
            await Assert.ThrowsAnyAsync<Exception>(() => service.DeletePurchasingDispositionAcceptance(newModel.Id));

        }

        [Fact]
        public void Should_No_Error_Validate_VM_Verification_Disposition()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionVerificationViewModel vm = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = "DispositionNo",
                Id = 1,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };

            Assert.True(vm.Validate(null).Count() == 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_VM_Verification_Disposition()
        {
            PurchasingDispositionVerificationViewModel vm = new PurchasingDispositionVerificationViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }
        [Fact]
        public void Should_Success_Validate_Wrong_Verify_Date_VM_Verification_Disposition()
        {
            PurchasingDispositionVerificationViewModel vm = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = "DispositionNo",
                Id = 1,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow.AddDays(1)
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Get_Report()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var vm = new HttpClientFromPurchasingDisposition().GetPurchasingDispositionViewModel();
            string vmJson = JsonConvert.SerializeObject(vm);
            PurchasingDispositionResponseViewModel response = new PurchasingDispositionResponseViewModel()
            {
                apiVersion = "1.0.0",
                data = new List<PurchasingDispositionViewModel>() { vm },
                info = new APIInfo()
                {
                    count = 1,
                    order = new
                    {
                        LastModifiedUtc = "asc"
                    },
                    page = 1,
                    size = 25,
                    total = 1,
                },
                message = "OK",
                statusCode = "200"
            };
            string responseJson = JsonConvert.SerializeObject(response);
            //serviceProvider
            //    .Setup(x => x.GetService(typeof(IHttpClientService)))
            //    .Returns(new HttpClientFromPurchasingDisposition());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions"))))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(responseJson, Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.OK });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("external-purchase-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ExternalPurchaseOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("unit-payment-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new UnitPaymentOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update")), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);


            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(serviceProvider.Object, _dbContext(GetCurrentMethod()));

            var postedModel = _dataUtil(service).GetNewData();
            HttpClientFromPurchasingDisposition httpClientFromPurchasingDisposition = new HttpClientFromPurchasingDisposition();
            postedModel.DispositionNo = httpClientFromPurchasingDisposition.GetPurchasingDispositionViewModel().DispositionNo;
            await service.CreateAsync(postedModel);
            var model = await service.ReadByIdAsync(postedModel.Id);
            var reportResponse = await service.GetReportAsync(1, 25, "{}", "{}", null, null,null,null,null,null,null, 7);
            Assert.NotNull(reportResponse.Data);
            reportResponse = await service.GetReportAsync(1, 25, "{}", "{}", DateTimeOffset.UtcNow.AddDays(-1), null, DateTimeOffset.UtcNow.AddDays(-1), null, null,null,null, 7);
            Assert.NotNull(reportResponse.Data);
            reportResponse = await service.GetReportAsync(1, 25, "{}", "{}", null, DateTimeOffset.UtcNow.AddDays(1), null, DateTimeOffset.UtcNow.AddDays(1), null,null,null, 7);
            Assert.NotNull(reportResponse.Data);
            reportResponse = await service.GetReportAsync(1, 25, "{}", "{}", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), null,null,null, 7);
            Assert.NotNull(reportResponse.Data);
        }

        [Fact]
        public void Should_Fail_Get_Report()
        {
            var service = new PurchasingDispositionExpeditionService(GetServiceProviderWrongHttpClient().Object, _dbContext(GetCurrentMethod()));
            Assert.ThrowsAsync<Exception>(() => service.GetReportAsync(1, 25, "{}", "{}", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1), null,null,null, 7));

        }

        [Fact]
        public async Task Should_Success_Generate_Excel()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var vm = new HttpClientFromPurchasingDisposition().GetPurchasingDispositionViewModel();
            string vmJson = JsonConvert.SerializeObject(vm);
            PurchasingDispositionResponseViewModel response = new PurchasingDispositionResponseViewModel()
            {
                apiVersion = "1.0.0",
                data = new List<PurchasingDispositionViewModel>() { vm },
                info = new APIInfo()
                {
                    count = 1,
                    order = new
                    {
                        LastModifiedUtc = "asc"
                    },
                    page = 1,
                    size = 25,
                    total = 1,
                },
                message = "OK",
                statusCode = "200"
            };
            string responseJson = JsonConvert.SerializeObject(response);
            //serviceProvider
            //    .Setup(x => x.GetService(typeof(IHttpClientService)))
            //    .Returns(new HttpClientFromPurchasingDisposition());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions"))))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(responseJson, Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.OK });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("external-purchase-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ExternalPurchaseOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("unit-payment-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new UnitPaymentOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update")), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(serviceProvider.Object, _dbContext(GetCurrentMethod()));

            var postedModel = _dataUtil(service).GetNewData();
            HttpClientFromPurchasingDisposition httpClientFromPurchasingDisposition = new HttpClientFromPurchasingDisposition();
            postedModel.DispositionNo = httpClientFromPurchasingDisposition.GetPurchasingDispositionViewModel().DispositionNo;
            await service.CreateAsync(postedModel);
            var model = await service.ReadByIdAsync(postedModel.Id);
            var reportResponse = await service.GenerateExcelAsync(1, 25, "{}", "{}", null, DateTimeOffset.UtcNow.AddDays(2),null,null,null, null, null, 7);
            Assert.NotNull(reportResponse);
        }
		[Fact]
		public async Task Should_Success_Generate_ExcelIsPaid()
		{
			var serviceProvider = new Mock<IServiceProvider>();
			var vm = new HttpClientFromPurchasingDisposition().GetPurchasingDispositionViewModel();
			string vmJson = JsonConvert.SerializeObject(vm);
			PurchasingDispositionResponseViewModel response = new PurchasingDispositionResponseViewModel()
			{
				apiVersion = "1.0.0",
				data = new List<PurchasingDispositionViewModel>() { vm },
				info = new APIInfo()
				{
					count = 1,
					order = new
					{
						LastModifiedUtc = "asc"
					},
					page = 1,
					size = 25,
					total = 1,
				},
				message = "OK",
				statusCode = "200"
			};
			string responseJson = JsonConvert.SerializeObject(response);
			//serviceProvider
			//    .Setup(x => x.GetService(typeof(IHttpClientService)))
			//    .Returns(new HttpClientFromPurchasingDisposition());

			serviceProvider
				.Setup(x => x.GetService(typeof(IIdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
			var httpClientService = new Mock<IHttpClientService>();
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions"))))
				.ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(responseJson, Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.OK });
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("external-purchase-orders"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ExternalPurchaseOrderDataUtil().GetResultFormatterOkString()) });
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("unit-payment-orders"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new UnitPaymentOrderDataUtil().GetResultFormatterOkString()) });
			httpClientService
				.Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(httpClientService.Object);

			PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(serviceProvider.Object, _dbContext(GetCurrentMethod()));

			var postedModel = _dataUtil(service).GetNewData();
			HttpClientFromPurchasingDisposition httpClientFromPurchasingDisposition = new HttpClientFromPurchasingDisposition();
			postedModel.DispositionNo = httpClientFromPurchasingDisposition.GetPurchasingDispositionViewModel().DispositionNo;
			await service.CreateAsync(postedModel);
			var model = await service.ReadByIdAsync(postedModel.Id);
			var reportResponse = await service.GenerateExcelAsync(1, 25, "{}", "{}", null, DateTimeOffset.UtcNow.AddDays(2), null, null, "no", "SUDAH ADA","SUDAH DIBAYAR", 7);
			Assert.NotNull(reportResponse);
		}
		[Fact]
		public async Task Should_Success_Generate_ExcelNotPaid()
		{
			var serviceProvider = new Mock<IServiceProvider>();
			var vm = new HttpClientFromPurchasingDisposition().GetPurchasingDispositionViewModel();
			string vmJson = JsonConvert.SerializeObject(vm);
			PurchasingDispositionResponseViewModel response = new PurchasingDispositionResponseViewModel()
			{
				apiVersion = "1.0.0",
				data = new List<PurchasingDispositionViewModel>() { vm },
				info = new APIInfo()
				{
					count = 1,
					order = new
					{
						LastModifiedUtc = "asc"
					},
					page = 1,
					size = 25,
					total = 1,
				},
				message = "OK",
				statusCode = "200"
			};
			string responseJson = JsonConvert.SerializeObject(response);
			//serviceProvider
			//    .Setup(x => x.GetService(typeof(IHttpClientService)))
			//    .Returns(new HttpClientFromPurchasingDisposition());

			serviceProvider
				.Setup(x => x.GetService(typeof(IIdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
			var httpClientService = new Mock<IHttpClientService>();
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions"))))
				.ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(responseJson, Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.OK });
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("external-purchase-orders"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ExternalPurchaseOrderDataUtil().GetResultFormatterOkString()) });
			httpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("unit-payment-orders"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new UnitPaymentOrderDataUtil().GetResultFormatterOkString()) });
			httpClientService
				.Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(httpClientService.Object);

			PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(serviceProvider.Object, _dbContext(GetCurrentMethod()));

			var postedModel = _dataUtil(service).GetNewData();
			HttpClientFromPurchasingDisposition httpClientFromPurchasingDisposition = new HttpClientFromPurchasingDisposition();
			postedModel.DispositionNo = httpClientFromPurchasingDisposition.GetPurchasingDispositionViewModel().DispositionNo;
			await service.CreateAsync(postedModel);
			var model = await service.ReadByIdAsync(postedModel.Id);
			var reportResponse = await service.GenerateExcelAsync(1, 25, "{}", "{}", null, DateTimeOffset.UtcNow.AddDays(2), null, null, "no", "BELUM ADA", "BELUM DIBAYAR", 7);
			Assert.NotNull(reportResponse);
		}

		[Fact]
        public void Should_Success_Generate_Empty_Excel()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            PurchasingDispositionResponseViewModel response = new PurchasingDispositionResponseViewModel()
            {
                apiVersion = "1.0.0",
                data = new List<PurchasingDispositionViewModel>() { },
                info = new APIInfo()
                {
                    count = 1,
                    order = new
                    {
                        LastModifiedUtc = "asc"
                    },
                    page = 1,
                    size = 25,
                    total = 1,
                },
                message = "OK",
                statusCode = "200"
            };
            string responseJson = JsonConvert.SerializeObject(response);
            //serviceProvider
            //    .Setup(x => x.GetService(typeof(IHttpClientService)))
            //    .Returns(new HttpClientFromPurchasingExpeditionGetEmpty());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("purchasing-dispositions"))))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(responseJson, Encoding.UTF8, "application/json"), StatusCode = HttpStatusCode.OK });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("external-purchase-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ExternalPurchaseOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("unit-payment-orders"))))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new UnitPaymentOrderDataUtil().GetResultFormatterOkString()) });
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update")), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(serviceProvider.Object, _dbContext(GetCurrentMethod()));

            var reportResponse = service.GenerateExcelAsync(1, 25, "{}", "{}", null, null,null,null,null,null,null, 7);
            Assert.NotNull(reportResponse);
        }

		[Fact]
		public async Task Should_Success_GetBankExpenditure_Data()
		{
			PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
			var data = await _dataUtil(service).GetTestData();
			var Response = service.ReadBankExpenditureNoteNo(1, 25, "{}", null, null, "{}");
			Assert.NotEmpty(Response.Data);
		}

	}
}
