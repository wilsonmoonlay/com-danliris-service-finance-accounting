using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPOControllerTest
    {
        protected VBRealizationDocumentNonPOController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            VBRealizationDocumentNonPOController controller = new VBRealizationDocumentNonPOController(serviceProvider.Object);
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

        Mock<IServiceProvider> GetServiceProvider()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            var validateService = new Mock<IValidateService>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IValidateService)))
              .Returns(validateService.Object);
            return serviceProvider;
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        VBRealizationDocumentNonPOViewModel viewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    IsInklaring = true,
                    Id = 1,
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rupiah"
                    },
                    DocumentNo = "DocumentNo",
                    Date = DateTimeOffset.Now,
                    IsDeleted = false,
                    Index = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
            {
                new VBRealizationDocumentNonPOExpenditureItemViewModel()
                {
                    DateDetail=DateTimeOffset.Now,
                    IncomeTax=new IncomeTaxViewModel()
                    {
                        Name="Name",
                        Rate=1
                    },
                    VatTax=new VatTaxViewModel()
                    {
                        Id="1",
                        Rate="10"
                    },
                    Amount=1,
                    IncomeTaxBy="Supplier",
                    IsGetPPh=true,
                    IsGetPPn=true,
                    Remark="Remark",
                    Total=1
                }
            },
                    Type = Lib.BusinessLogic.VBRequestDocument.VBType.NonPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 10
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            {
                new VBRealizationDocumentNonPOUnitCostViewModel()
                {

                    Amount=1,
                    IsSelected=true,
                    Unit=new UnitViewModel()
                    {
                        VBDocumentLayoutOrder=12,
                        Code="Code",
                        Division=new DivisionViewModel()
                        {
                            Code= "Code",
                            Name= "Name"
                        },
                        Name="Name",

                    }
                },
                new VBRealizationDocumentNonPOUnitCostViewModel()
                {

                    Amount=1,
                    IsSelected=false,
                    Unit=new UnitViewModel()
                    {
                        VBDocumentLayoutOrder=10,
                        Code="Code",
                        Division=new DivisionViewModel()
                        {
                            Code= "Code",
                            Name= "Name"
                        },
                        Name="Name",

                    }
                }
            },
                    VBNonPOType = "Tanpa Nomor VB",
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1,
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp"
                        },
                        DocumentNo = "DocumentNo",
                        IsApproved = true,
                        Items = new List<VBRequestDocumentNonPOItemDto>()
               {
                   new VBRequestDocumentNonPOItemDto()
                   {
                       IsSelected=true,
                       Unit=new UnitDto()
                       {
                           Code="Code",
                           Division=new DivisionDto()
                           {
                               Code="Code",
                               Name ="Name"
                           }
                       },

                   }
               },
                        Purpose = "Purpose",
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        },
                        RealizationEstimationDate = DateTimeOffset.Now

                    }

                };
            }
        }



        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }


        [Fact]
        public void Get_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRealizationDocumentModel>(new List<VBRealizationDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get();

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get();

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void GetByUser_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.ReadByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRealizationDocumentModel>(new List<VBRealizationDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetByUser();

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetByUser_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.ReadByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetByUser();

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task PostNonPO_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.CreateAsync(It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRealizationDocumentNonPOViewModel());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }


        [Fact]
        public async Task PostNonPO_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.CreateAsync(It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRealizationDocumentNonPOViewModel());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PostNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.CreateAsync(It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRealizationDocumentNonPOViewModel());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task GetNonPOById_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetNonPOById_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(viewModel);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public async Task GetNonPOById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task DeleteNonPO_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).DeleteNonPO(1); ;

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).DeleteNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(viewModel.Id + 1, viewModel);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(viewModel.Id, viewModel);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Throws_ServiceValidationException_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(viewModel.Id, viewModel);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<VBRealizationDocumentNonPOViewModel>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(viewModel.Id, viewModel);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetPDFNonPO_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(viewModel);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
            Assert.Equal("Realisasi VB Inklaring Tanpa PO - DocumentNo.pdf", response.GetType().GetProperty("FileDownloadName").GetValue(response, null));
        }

        [Fact]
        public async Task GetPDFNonPO_When_NotInklaring_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            var viewModelTemp = viewModel;
            viewModelTemp.IsInklaring = false;

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(viewModelTemp);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
            Assert.Equal("Realisasi VB Tanpa PO - DocumentNo.pdf", response.GetType().GetProperty("FileDownloadName").GetValue(response, null));
        }



        [Fact]
        public async Task GetPDFNonPO_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service.Setup(s => s.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetPDFNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentNonPOService>();

            service
                .Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentNonPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

    }
}
