
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentDebtBalance;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.GarmentDebtBalance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.GarmentDebtBalance
{
  public  class GarmentDebtBalanceControllerTest
    {
        protected GarmentDebtBalanceController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentDebtBalanceService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            GarmentDebtBalanceController controller = new GarmentDebtBalanceController(mocks.IdentityService.Object,mocks.ValidateService.Object,mocks.Service.Object,mocks.Mapper.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IGarmentDebtBalanceService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IGarmentDebtBalanceService>(), Mapper: new Mock<IMapper>());
        }

       
        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

       

        //[Fact]
        //public void Get_Succes_Return_OK()
        //{
        //    //Setup
        //    var mocks = GetMocks();
        //    GarmentDebtBalanceModel model = new GarmentDebtBalanceModel(1, "purchasingCategoryName", "billsNo", "paymentBilss", 1, "garmentDeliveryOrderNo", 1, "suplierCode", "suplierName", false, 1, "IDR", 1, "productNames", DateTimeOffset.UtcNow, 1, 1, "paymentType");
           
        //    GarmentDebtBalanceIndexDto dto = new GarmentDebtBalanceIndexDto()
        //    {
        //        Count=1,
        //        Data=new List<GarmentDebtBalanceCardDto>()
        //        {
        //            new GarmentDebtBalanceCardDto(model)
                    
                   
        //        },
        //        Order=new List<string>()
        //        {
        //            ""
        //        },
        //        Selected=new List<string>()
        //        {
        //            ""
        //        }
        //    };
        //    mocks.Service
        //        .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        //        .Returns(dto);

        //    //Act
          
        //    IActionResult response = GetController(mocks).Get(new GarmentDebtBalanceFilterViewModel());

        //    //Assert
        //    int statusCode = this.GetStatusCode(response);
        //    Assert.Equal((int)HttpStatusCode.OK, statusCode);
        //}

        [Fact]
        public void Get_Return_InternalServerError()
        {
            //Setup
            var mocks = GetMocks();

            mocks.Service
                .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            //Act
            IActionResult response = GetController(mocks).Get(new GarmentDebtBalanceFilterViewModel());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        //[Fact]
        //public async Task GetXls_Return_FileExcel()
        //{
        //    //Setup
        //    var mocks = GetMocks();
        //    GarmentDebtBalanceModel model = new GarmentDebtBalanceModel(1, "purchasingCategoryName", "bilssNo", "paymentBills", 1, "garmentDeliveryOrderNumber", 1, "supplierCode", "supplierName", true, 1, "IDR", 1, "productNames", DateTimeOffset.Now,1,1, "paymentType");

        //    GarmentDebtBalanceIndexDto dto = new GarmentDebtBalanceIndexDto()
        //    {
        //        Data = new List<GarmentDebtBalanceCardDto>()
        //        {
        //            new GarmentDebtBalanceCardDto(model)

        //        },
        //        Count=1,
        //        Order=new List<string>()
        //        {
        //        },
                
        //    };
        //    mocks.Service
        //        .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        //        .Returns(dto);

        //    //Act
        //    var filter = new GarmentDebtBalanceFilterViewModel()
        //    {
        //        month = 1,
        //        import = true,
        //        supplierId = 1,
        //        supplierName = "supplierName",
        //        year = DateTimeOffset.Now.Year
        //    };
        //    IActionResult response =await GetController(mocks).GetXls(filter);

        //    //Assert
        //    Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
        //}

        [Fact]
        public async Task GetXls_Return_InternalServerError()
        {
            //Setup
            var mocks = GetMocks();
           
            mocks.Service
                .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            //Act
            var filter = new GarmentDebtBalanceFilterViewModel()
            {
                month = 1,
                import = true,
                supplierId = 1,
                supplierName = "supplierName",
                year = DateTimeOffset.Now.Year
            };
            IActionResult response = await GetController(mocks).GetXls(filter);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetPdf_Return_FilePdf()
        {
            //Setup
            var mocks = GetMocks();
            GarmentDebtBalanceModel model = new GarmentDebtBalanceModel(1, "purchasingCategoryName", "bilssNo", "paymentBills", 1, "garmentDeliveryOrderNumber", 1, "supplierCode", "supplierName", true, 1, "IDR", 1, "productNames", DateTimeOffset.Now,1,1, "paymentType");

            GarmentDebtBalanceIndexDto dto = new GarmentDebtBalanceIndexDto()
            {
                Data = new List<GarmentDebtBalanceCardDto>()
                {
                    new GarmentDebtBalanceCardDto(model)

                },
                Count = 1,
                Order = new List<string>()
                {
                },

            };
            mocks.Service
                .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(dto);

            //Act
            var filter = new GarmentDebtBalanceFilterViewModel()
            {
                month = 1,
                import = true,
                supplierId = 1,
                supplierName = "supplierName",
                year = DateTimeOffset.Now.Year
            };
            IActionResult response =  GetController(mocks).GetPDF(filter);

            //Assert
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }


        [Fact]
        public void GetPDF_Return_InternalServerError()
        {
            //Setup
            var mocks = GetMocks();

            mocks.Service
                .Setup(s => s.GetDebtBalanceCardWithBalanceBeforeIndex(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception());

            //Act
            var filter = new GarmentDebtBalanceFilterViewModel()
            {
                month = 1,
                import = true,
                supplierId = 1,
                supplierName = "supplierName",
                year = DateTimeOffset.Now.Year
            };
            IActionResult response =  GetController(mocks).GetPDF(filter);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

    }
}
