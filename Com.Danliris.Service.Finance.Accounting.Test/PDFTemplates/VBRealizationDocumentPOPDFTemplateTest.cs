using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PDFTemplates
{
    public class VBRealizationDocumentPOPDFTemplateTest
    {
        VBRealizationPdfDto TemplatePDFDocumentWithPO
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    DocumentNo = "DocumentNo",
                    DocumentType = RealizationDocumentType.NonVB,
                    Type = VBType.WithPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1,
                        DocumentNo = "DocumentNo",
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp",

                        },
                        IsApproved = true,
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        }
                    },
                    VBNonPOType = "",

                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
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
                    Active = true,
                    Position = new VBRealizationPosition(),
                    Amount = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
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
                            IncomeTaxBy="Supplier",
                            IsGetPPh=true,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header = new VBRealizationDocumentModel(viewModel),

                    UnitCosts = new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.First())
                    },
                    Items = new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.First())
                    }
                };
            }
        }

        VBRealizationPdfDto TemplatePDFDocumentWithPOPPNNull
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency = new CurrencyViewModel()
                    {
                        Code = "USD",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "USD"
                    },
                    DocumentNo = "",
                    DocumentType = RealizationDocumentType.NonVB,
                    Type = VBType.WithPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Id = 1,
                        Amount = 1,
                        DocumentNo = "",
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp",

                        },
                        IsApproved = true,
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        }
                    },
                    VBNonPOType = "",

                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
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
                                VBDocumentLayoutOrder=12,
                                Id = 1
                            }
                        },
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
                                VBDocumentLayoutOrder=12,
                                Id = 2
                            }
                        }
                    },
                    Active = true,
                    Position = new VBRealizationPosition(),
                    Amount = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            Amount=1,
                            IncomeTax=new IncomeTaxViewModel()
                            {
                                Name="Name",
                                Rate=1
                            },
                            DateDetail=DateTimeOffset.Now,
                            IncomeTaxBy="Name",
                            IsGetPPh=false,
                            IsGetPPn=false,
                            Remark="Remark",
                            Total=1
                        },
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
                            IncomeTaxBy="Name",
                            IsGetPPh=false,
                            IsGetPPn=false,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header = new VBRealizationDocumentModel(viewModel),

                    UnitCosts = new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.First()),
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.Last())
                    },
                    Items = new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.First()),
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.Last())
                    }
                };
            }
        }

        VBRealizationPdfDto TemplatePDFDocumentWithPOPPHNull
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    DocumentNo = "DocumentNo",
                    DocumentType = RealizationDocumentType.WithVB,
                    Type = VBType.WithPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1,
                        DocumentNo = "DocumentNo",
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp",

                        },
                        IsApproved = true,
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        }
                    },
                    VBNonPOType = "",

                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
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
                    Active = true,
                    Position = new VBRealizationPosition(),
                    Amount = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
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
                            IncomeTaxBy="Supplier",
                            IsGetPPh=false,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header = new VBRealizationDocumentModel(viewModel),

                    UnitCosts = new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.First())
                    },
                    Items = new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.First())
                    }
                };
            }
        }

        VBRealizationPdfDto TemplatePDFDocumentWithPPNandPPH
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    DocumentNo = "DocumentNo",
                    DocumentType = RealizationDocumentType.WithVB,
                    Type = VBType.WithPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1,
                        DocumentNo = "DocumentNo",
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp",

                        },
                        IsApproved = true,
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        }
                    },
                    VBNonPOType = "",

                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
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
                                VBDocumentLayoutOrder=12,
                                Id = 1
                            }
                        },
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
                                VBDocumentLayoutOrder=12,
                                Id = 2
                            }
                        }
                    },
                    Active = true,
                    Position = new VBRealizationPosition(),
                    Amount = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
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
                            IncomeTaxBy="Supplier",
                            IsGetPPh=true,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        },
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
                            IncomeTaxBy="Supplier",
                            IsGetPPh=false,
                            IsGetPPn=false,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header = new VBRealizationDocumentModel(viewModel),

                    UnitCosts = new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.First()),
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.Last())
                    },
                    Items = new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.First()),
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.Last())
                    }
                };
            }
        }

        VBRealizationPdfDto TemplatePDFDocumentWithPOItemMoreThanOne
        {
            get
            {
                VBRealizationDocumentNonPOViewModel viewModel = new VBRealizationDocumentNonPOViewModel()
                {
                    Currency = new CurrencyViewModel()
                    {
                        Code = "IDR",
                        Description = "Description",
                        Rate = 1,
                        Symbol = "Rp"
                    },
                    DocumentNo = "DocumentNo",
                    DocumentType = RealizationDocumentType.WithVB,
                    Type = VBType.WithPO,
                    Unit = new UnitViewModel()
                    {
                        Code = "Code",
                        Division = new DivisionViewModel()
                        {
                            Code = "Code",
                            Name = "Name"
                        },
                        Name = "Name",
                        VBDocumentLayoutOrder = 1
                    },
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1,
                        DocumentNo = "DocumentNo",
                        Currency = new CurrencyDto()
                        {
                            Code = "IDR",
                            Description = "Description",
                            Rate = 1,
                            Symbol = "Rp",

                        },
                        IsApproved = true,
                        SuppliantUnit = new UnitDto()
                        {
                            Code = "Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name"
                            }
                        }
                    },
                    VBNonPOType = "",

                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
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
                                VBDocumentLayoutOrder=12,
                                Id = 1
                            }
                        },
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
                                VBDocumentLayoutOrder=12,
                                Id = 2
                            }
                        }
                    },
                    Active = true,
                    Position = new VBRealizationPosition(),
                    Amount = 1,
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
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
                            IncomeTaxBy="Supplier",
                            IsGetPPh=true,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        },
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
                            IncomeTaxBy="Name",
                            IsGetPPh=true,
                            IsGetPPn=true,
                            Remark="Remark",
                            Total=1
                        }
                    }

                };
                return new VBRealizationPdfDto()
                {
                    Header = new VBRealizationDocumentModel(viewModel),

                    UnitCosts = new List<VBRealizationDocumentUnitCostsItemModel>()
                    {
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.First()),
                        new VBRealizationDocumentUnitCostsItemModel(viewModel.Id, viewModel.UnitCosts.Last())
                    },
                    Items = new List<VBRealizationDocumentExpenditureItemModel>()
                    {
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.First()),
                        new VBRealizationDocumentExpenditureItemModel(viewModel.Id, viewModel.Items.Last())
                    }
                };
            }
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithSuccess()
        {
            VBRealizationDocumentPOPDFTemplate PdfTemplate = new VBRealizationDocumentPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(TemplatePDFDocumentWithPO, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPNNull()
        {
            VBRealizationDocumentPOPDFTemplate PdfTemplate = new VBRealizationDocumentPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(TemplatePDFDocumentWithPOPPNNull, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPHNull()
        {
            VBRealizationDocumentPOPDFTemplate PdfTemplate = new VBRealizationDocumentPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(TemplatePDFDocumentWithPOPPHNull, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPNandPPH()
        {
            VBRealizationDocumentPOPDFTemplate PdfTemplate = new VBRealizationDocumentPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(TemplatePDFDocumentWithPPNandPPH, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateMoreThanOne()
        {
            VBRealizationDocumentPOPDFTemplate PdfTemplate = new VBRealizationDocumentPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(TemplatePDFDocumentWithPOItemMoreThanOne, 7);
            Assert.NotNull(result);
        }
    }
}