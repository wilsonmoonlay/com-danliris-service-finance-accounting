using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
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
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBRealizationDocumentWithPO
{
    public class VBRealizationDocumentWithPOControllerTest
    {
        protected VBRealizationDocumentWithPOController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            VBRealizationDocumentWithPOController controller = new VBRealizationDocumentWithPOController(serviceProvider.Object);
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

        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(formDto, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        FormDto formDto
        {
            get
            {
                return new FormDto()
                {
                    Id = 1,
                    Currency = new CurrencyDto()
                    {
                        Id=1,
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    
                    Date = DateTimeOffset.Now,
                    SuppliantUnit = new UnitDto()
                    {
                        Id=1,
                        Code = "Code",
                        Name = "Name",
                        Division = new DivisionDto()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        VBDocumentLayoutOrder = 12
                    },
                    Type = "With PO",
                    VBRequestDocument = new VBRequestDocumentDto()
                    {
                        Id = 1,
                    },
                    Items = new List<FormItemDto>()
                    {
                        new FormItemDto()
                        {
                            UnitPaymentOrder=new UnitPaymentOrderDto()
                            {
                                Id=1,
                                No="1",
                                Amount=1,
                                Date=DateTimeOffset.Now,
                                IncomeTax=new IncomeTaxDto()
                                {
                                    Name="Name",
                                    Rate=1
                                },
                                VatTax=new VatTaxDto()
                                {
                                    Id="1",
                                    Rate="10"
                                },
                                IncomeTaxBy="Supplier",
                                UseIncomeTax=true,
                                UseVat=true
                                
                            }
                        }
                    }
                };
            }
        }

        VBRealizationPdfDto vBRealizationPdfDto
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency=new CurrencyViewModel()
                    {
                        Code="IDR",
                        Description= "Description",
                        Rate=1,
                        Symbol="Rp"
                    },
                    DocumentNo= "DocumentNo",
                    DocumentType= RealizationDocumentType.WithVB,
                    Type=VBType.WithPO,
                    Unit=new UnitViewModel()
                    {
                        Code="Code",
                        Division=new DivisionViewModel()
                        {
                            Code="Code",
                            Name="Name"
                        },
                        Name="Name",
                        VBDocumentLayoutOrder=1
                    },
                    VBDocument=new VBRequestDocumentNonPODto()
                    {
                        Amount=1,
                        DocumentNo= "DocumentNo",
                        Currency=new CurrencyDto()
                        {
                            Code="IDR",
                            Description= "Description",
                            Rate=1,
                            Symbol="Rp",

                        },
                        IsApproved=true,
                        SuppliantUnit=new UnitDto()
                        {
                            Code="Code",
                            Division=new DivisionDto()
                            {
                                Code="Code",
                                Name="Name"
                            }
                        }
                    },
                    VBNonPOType="",
                    
                    UnitCosts=new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Amount=1,
                            Unit=new UnitViewModel()
                            {
                                Code="Code",
                                Division=new DivisionViewModel()
                                {
                                    Code="Code",
                                    Name="Name",
                                },
                                Name="Name",
                                VBDocumentLayoutOrder=12
                            }
                        }
                    },
                    Active=true,
                    Position=new VBRealizationPosition(),
                    Amount=1,
                    Items=new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            Amount=1,
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
                            DateDetail=DateTimeOffset.Now,
                            IncomeTaxBy="supplier",
                            IsGetPPh=true,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header=new VBRealizationDocumentModel(viewModel),
                    
                    UnitCosts=new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel()
                    },
                    Items=new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel()
                    }
                };
            }
        }

        [Fact]
        public void Get_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRealizationDocumentModel>(new List<VBRealizationDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
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
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
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
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VBRealizationDocumentModel>(new List<VBRealizationDocumentModel>(),1,new Dictionary<string, string>(),new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
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
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadByUser(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetByUser();

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Post_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Post(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public void Post_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .Throws(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Post(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void Post_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Create(It.IsAny<FormDto>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Post(formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetById_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadById(It.IsAny<int>()))
                .Returns(new VBRealizationWithPODto());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetById_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadById(It.IsAny<int>()))
                .Returns(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetById_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadById(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetById(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Delete_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Delete(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public void Delete_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Delete(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Delete(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Put_Success_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Put(formDto.Id, formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public void Put_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .Returns(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Put(formDto.Id + 1, formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void Put_Throws_ServiceValidationException_Return_BadRequest()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .Throws(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Put(formDto.Id, formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public void Put_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.Update(It.IsAny<int>(), It.IsAny<FormDto>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Put(formDto.Id, formDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetPDFPO_Return_Success()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadModelById(It.IsAny<int>()))
                .Returns(vBRealizationPdfDto);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFPO(1);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("application/pdf", response.GetType().GetProperty("ContentType").GetValue(response, null));
            Assert.Equal("Realisasi VB Dengan PO - DocumentNo.pdf", response.GetType().GetProperty("FileDownloadName").GetValue(response, null));
            
        }

        [Fact]
        public void GetPDFPO_Return_NotFound()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadModelById(It.IsAny<int>()))
                .Returns(()=>null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void GetPDFPO_Throws_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationWithPOService>();

            service
                .Setup(s => s.ReadModelById(It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationWithPOService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetPDFPO(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
