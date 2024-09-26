using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPOViewModelTest
    {

        [Fact]
        public void ShouldSuccessInstantiate()
        {
            VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
            {
                Type = VBType.NonPO,
                Index = 1
            };

            Assert.Equal(VBType.NonPO, viewModel.Type);
            Assert.Equal(1, viewModel.Index);
        }


        [Fact]
        public void ShouldHaveError_Validate_When_Data_Null()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>(),
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_CurrencyId_Equal_Zero()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Currency = new CurrencyViewModel()
                {
                    Id = 0
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>(),
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_VBDocumentId_Equal_Zero()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 0
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>(),
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_DateDetail_HasNoValue()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        DateDetail=null,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_DateDetail_GreaterThan_Date()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        DateDetail=DateTimeOffset.Now.AddDays(2),
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_VBDocumentId_NotExist()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                         IsGetPPh=true,
                        DateDetail=DateTimeOffset.Now,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_IncomeTax_NotExist()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                         IsGetPPh=true,
                        DateDetail=DateTimeOffset.Now,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_VatTax_NotExist()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                         IsGetPPn=true,
                        DateDetail=DateTimeOffset.Now,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_VatTax_Equal_Zero()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                         IsGetPPn=true,
                        VatTax=new VatTaxViewModel()
                        {
                            Id="0"
                        },
                        DateDetail=DateTimeOffset.Now,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_IncomeTaxId_Equal_Zero()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                         IsGetPPh=true,
                        IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=0
                        },
                        DateDetail=DateTimeOffset.Now,
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_IncomeTaxBy_NotExist()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        IsGetPPh=true,
                         IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now,

                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_Unit_NotSelected()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        IsGetPPh=true,
                         IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now,

                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        IsSelected=false
                    }
                }
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_UnitName_and_Amount_NotExist()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        IsGetPPh=true,
                         IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now,

                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        IsSelected=true,

                    }
                }
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_Amount_NotEqual_Total()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        Total=1,
                        IsGetPPh=true,
                         IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now,

                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        Amount=2,
                        IsSelected=true,

                    }
                }
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_InvoiceNo_Null()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                InvoiceNo = null,
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },

                Unit = new UnitViewModel()
                {
                    Id = 1,
                    Code = "Code",

                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        Amount=1,
                        Remark="Remark",
                        Total=1,
                        IsGetPPh=true,
                        IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now.AddDays(-2),
                        IncomeTaxBy="Supplier"
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        Amount=1,
                        IsSelected=true,
                        Unit=new UnitViewModel()
                        {
                            Id=1,

                        }
                    }
                }
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_InvoiceNo_Exists()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },

                Unit = new UnitViewModel()
                {
                    Id = 1,
                    Code = "Code",

                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        Amount=1,
                        Remark="Remark",
                        Total=1,
                        IsGetPPh=true,
                        IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now.AddDays(-2),
                        IncomeTaxBy="Supplier"
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        Amount=1,
                        IsSelected=true,
                        Unit=new UnitViewModel()
                        {
                            Id=1,

                        }
                    }
                }
            };

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            Mock<IVBRealizationDocumentNonPOService> IService = new Mock<IVBRealizationDocumentNonPOService>();
            IService.Setup(s => s.CheckInvoiceNo(1, "TestNo123"));
            serviceProvider.Setup(s => s.GetService(typeof(IVBRealizationDocumentNonPOService))).Returns(IService.Object);

            ValidationContext validationContext = new ValidationContext(dto, serviceProvider.Object, null);

            var result = dto.Validate(validationContext);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveNoError_Validate_Data()
        {
            VBRealizationDocumentNonPOViewModel dto = new VBRealizationDocumentNonPOViewModel()
            {
                Date = DateTimeOffset.Now,
                VBNonPOType = "Dengan Nomor VB",
                InvoiceNo = "Test123",
                Currency = new CurrencyViewModel()
                {
                    Id = 1
                },

                Unit = new UnitViewModel()
                {
                    Id = 1,
                    Code = "Code",

                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id = 1
                },
                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {

                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        Amount=1,
                        Remark="Remark",
                        Total=1,
                        IsGetPPh=true,
                         IncomeTax=new IncomeTaxViewModel()
                        {
                            Id=1
                        },
                        DateDetail=DateTimeOffset.Now.AddDays(-2),
                        IncomeTaxBy="Supplier"
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        Amount=1,
                        IsSelected=true,
                        Unit=new UnitViewModel()
                        {
                            Id=1,

                        }
                    }
                }
            };

            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            Mock<IVBRealizationDocumentNonPOService> IService = new Mock<IVBRealizationDocumentNonPOService>();
            IService.Setup(s => s.CheckInvoiceNo(It.IsAny<int>(), It.IsAny<string>()));
            serviceProvider.Setup(s => s.GetService(typeof(IVBRealizationDocumentNonPOService))).Returns(IService.Object);

            ValidationContext validationContext = new ValidationContext(dto, serviceProvider.Object, null);

            var result = dto.Validate(validationContext);
            Assert.True(0 == result.Count());
        }
    }
}
