using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBRequestDocument
{
    public class VBRequestDocumentControllerTest
    {
        protected VBRequestDocumentController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            VBRequestDocumentController controller = new VBRequestDocumentController(serviceProvider.Object);
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

        public VBRequestDocumentNonPOFormDto vBRequestDocumentNonPOFormDto
        {
            get
            {
                return new VBRequestDocumentNonPOFormDto()
                {
                    Id = 1,
                    Amount = 1,

                };
            }
        }

        VBRequestDocumentNonPODto vBRequestDocumentNonPODto {
            get
            {
                return new VBRequestDocumentNonPODto()
                {
                    Amount=1,
                    Date=DateTimeOffset.Now,
                    Currency=new CurrencyDto()
                    {
                        Id=1,
                        Code= "Code",
                        Description= "Description",
                        Rate=1,
                        Symbol="Rp"
                    },
                    Id=1,
                    DocumentNo="1",
                    IsApproved=true,
                    Purpose= "Purpose",
                    RealizationEstimationDate=DateTimeOffset.Now,
                    SuppliantUnit=new UnitDto()
                    {
                        Id=1,
                        Code="Code",
                        Division=new DivisionDto()
                        {
                            Id=1,
                            Code="Code",
                            Name="Name",
                            
                        }
                    },
                    Items =new List<VBRequestDocumentNonPOItemDto>()
                    {
                        new VBRequestDocumentNonPOItemDto()
                        {
                            Unit=new UnitDto()
                            {
                                Code="Code",
                                Division=new DivisionDto()
                                {
                                    Code="Code",
                                    Name="",
                                    Id=1
                                },
                                Id=1,
                                Name="Name",
                                VBDocumentLayoutOrder=1,
                                
                            }
                        }
                    }
                };
            }
           
            }
        VBRequestDocumentWithPOFormDto vBRequestDocumentWithPOFormDto
        {
            get
            {
                return new VBRequestDocumentWithPOFormDto()
                {
                    Id = 1,
                    Amount = 1,
                    Currency = new CurrencyDto(),
                    Date = DateTimeOffset.Now,
                    Purpose = "Purpose",
                    RealizationEstimationDate = DateTimeOffset.Now,
                    SuppliantUnit = new UnitDto(),
                    Items = new List<VBRequestDocumentWithPOItemFormDto>()
                };
            }
        }

        VBRequestDocumentWithPODto vBRequestDocumentWithPODto
        {
            get
            {
                return new VBRequestDocumentWithPODto()
                {
                    Amount = 1,
                    Date = DateTimeOffset.Now,
                    Purpose = "Purpose",
                    Currency = new CurrencyDto()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    RealizationEstimationDate = DateTimeOffset.Now,
                    SuppliantUnit = new UnitDto()
                    {
                        Code = "Code",
                        Division = new DivisionDto()
                        {
                            Code = "Code",
                            Name = "GARMENT"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    
                    DocumentNo = "DocumentNo",
                    Items = new List<VBRequestDocumentWithPOItemDto>()
                    {
                        new VBRequestDocumentWithPOItemDto()
                        {
                            
                            PurchaseOrderExternal =new PurchaseOrderExternal()
                            {  
                                No="1",
                                Items=new List<PurchaseOrderExternalItem>()
                                {
                                    new PurchaseOrderExternalItem()
                                    {
                                        Conversion=1,
                                        DealQuantity=1,
                                        DealUOM=new UnitOfMeasurement()
                                        {
                                            Unit="Unit"
                                        },
                                        DefaultQuantity=1,
                                        IncomeTax=new IncomeTaxDto()
                                        {
                                            Name="Name",
                                            Rate=1
                                        },
                                        IncomeTaxBy="IncomeTaxBy",
                                        UseIncomeTax=true,
                                        Price=1,
                                        Product=new Product()
                                        {
                                            Code="Code",
                                            Name="Name",
                                            UOM=new UnitOfMeasurement()
                                            {
                                                Unit="Unit"
                                            }
                                        },
                                        Unit=new UnitDto()
                                        {
                                            Code="Code",
                                            Division=new DivisionDto()
                                            {
                                                Code="Code",
                                                Name="Name"
                                            }
                                        },
                                        UseVat=true
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }


        ApprovalVBFormDto approvalVBFormDto
        {
            get
            {
                return new ApprovalVBFormDto();
            }
        }
        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(vBRequestDocumentNonPOFormDto, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

       

        [Fact]
        public async Task PostNonPO_Succes_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.CreateNonPO(It.IsAny<VBRequestDocumentNonPOFormDto>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRequestDocumentNonPOFormDto());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task PostNonPO_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CreateNonPO(It.IsAny<VBRequestDocumentNonPOFormDto>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRequestDocumentNonPOFormDto());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }


        [Fact]
        public async Task PostNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CreateNonPO(It.IsAny<VBRequestDocumentNonPOFormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PostNonPO(new VBRequestDocumentNonPOFormDto());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Get_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            Dictionary<string, string> order = new Dictionary<string, string>();

            service
                .Setup(s => s.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRequestDocumentModel>(new List<VBRequestDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get(1, 25, "{}", null, null, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get(1, 25, "{}", null, null, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetByUser_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            Dictionary<string, string> order = new Dictionary<string, string>();

            service
                .Setup(s => s.GetByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRequestDocumentModel>(new List<VBRequestDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetByUser(1, 25, "{}", null, null, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetByUser_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            Dictionary<string, string> order = new Dictionary<string, string>();

            service
                .Setup(s => s.GetByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetByUser(1, 25, "{}", null, null, "{}");

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetNonPOById_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetNonPOById(It.IsAny<int>()))
                .ReturnsAsync(new VBRequestDocumentNonPODto());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetNonPOById_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetNonPOById(It.IsAny<int>())).ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetNonPOById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetNonPOById(It.IsAny<int>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetNonPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task PutNonPO_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateNonPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentNonPOFormDto>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(vBRequestDocumentNonPOFormDto.Id, vBRequestDocumentNonPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Throws_ServiceValidationException_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(vBRequestDocumentNonPOFormDto.Id+1, vBRequestDocumentNonPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateNonPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentNonPOFormDto>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(vBRequestDocumentNonPOFormDto.Id, vBRequestDocumentNonPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task PutNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateNonPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentNonPOFormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).PutNonPO(vBRequestDocumentNonPOFormDto.Id, vBRequestDocumentNonPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task DeleteNonPO_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.DeleteNonPO(It.IsAny<int>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).DeleteNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteNonPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.DeleteNonPO(It.IsAny<int>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).DeleteNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetPDFNonPO_Return_Success()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetNonPOById(It.IsAny<int>()))
                .ReturnsAsync(vBRequestDocumentNonPODto);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert 
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
            Assert.Equal("Permohonan VB Tanpa PO - 1.pdf", response.GetType().GetProperty("FileDownloadName").GetValue(response, null));
        }

        [Fact]
        public async Task GetPDFNonPO_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetNonPOById(It.IsAny<int>())).ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
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
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetNonPOById(It.IsAny<int>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetPDFNonPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void PostWithPO_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CreateWithPO(It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).PostWithPO(vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }


        [Fact]
        public void PostWithPO_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CreateWithPO(It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Throws(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).PostWithPO(vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void PostWithPO_Throws_Exception()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CreateWithPO(It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).PostWithPO(vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetWithPOById_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetWithPOById(It.IsAny<int>()))
                .Returns(new VBRequestDocumentWithPODto());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetWithPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetWithPOById_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetWithPOById(It.IsAny<int>())).Returns(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetWithPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetWithPOById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetWithPOById(It.IsAny<int>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetWithPOById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void PutWithPO_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateWithPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            int id = (int)vBRequestDocumentWithPOFormDto.Id;
            IActionResult response = GetController(serviceProviderMock).PutWithPO(id, vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }


        [Fact]
        public void PutWithPO_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateWithPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Throws(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            int id = (int)vBRequestDocumentWithPOFormDto.Id;
            IActionResult response = GetController(serviceProviderMock).PutWithPO(id, vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void PutWithPO_Throws_Exception()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateWithPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            int id = (int)vBRequestDocumentWithPOFormDto.Id;
            IActionResult response = GetController(serviceProviderMock).PutWithPO(id, vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void PutWithPO_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.UpdateWithPO(It.IsAny<int>(), It.IsAny<VBRequestDocumentWithPOFormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            int id = (int)vBRequestDocumentWithPOFormDto.Id + 1;
            IActionResult response = GetController(serviceProviderMock).PutWithPO(id, vBRequestDocumentWithPOFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void DeleteWithPO_Succees_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.DeleteWithPO(It.IsAny<int>())).Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).DeleteWithPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public void DeleteWithPO_Succees_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.DeleteWithPO(It.IsAny<int>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).DeleteWithPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetPDFWithPO_Succees_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetWithPOById(It.IsAny<int>())).Returns(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFWithPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetPDFWithPO_Succees()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetWithPOById(It.IsAny<int>()))
                .Returns(vBRequestDocumentWithPODto);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFWithPO(1);

            //Assert
            Assert.NotNull(response);
           
        }

        [Fact]
        public void GetPDFWithPO_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetWithPOById(It.IsAny<int>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFWithPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetNotApprovedData_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetNotApprovedData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<string>()))
                .Returns(new List<VBRequestDocumentModel>());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetNotApprovedData(1, 25, 1, null, null);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetNotApprovedData_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetNotApprovedData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetNotApprovedData(1, 25, 1, null, null);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetApprovedData_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.ApprovalData(It.IsAny<ApprovalVBFormDto>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).ApprovalData(approvalVBFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetApprovedData_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.ApprovalData(It.IsAny<ApprovalVBFormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).ApprovalData(approvalVBFormDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task PostCancellation_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.CancellationDocuments(It.IsAny<CancellationFormDto>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).CanccellationDocuments(new CancellationFormDto());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public async Task PostCancellation_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.CancellationDocuments(It.IsAny<CancellationFormDto>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).CanccellationDocuments(new CancellationFormDto());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetVBForPurchasing_Succes_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service.Setup(s => s.GetVBForPurchasing(It.IsAny<int>())).Returns(true);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetVBForPurchasing(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVBForPurchasing_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRequestDocumentService>();

            service
                .Setup(s => s.GetVBForPurchasing(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRequestDocumentService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetVBForPurchasing(0);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}

