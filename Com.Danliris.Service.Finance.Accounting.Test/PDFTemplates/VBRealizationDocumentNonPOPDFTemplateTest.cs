using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.PDFTemplates;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.PDFTemplates
{
    public class VBRealizationDocumentNonPOPDFTemplateTest
    {
        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo = "1",
                    Unit = new UnitViewModel()
                    {
                        Name = "a"
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        },
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        }
                    },
                    Currency = new CurrencyViewModel()
                    {
                        Code = "a",
                        Description = "a"
                    },
                    Date = DateTimeOffset.Now,
                    Id = 1,
                    VBNonPOType = "Tanpa Nomor VB",
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = true,
                            IsGetPPh = true,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1,
                            IncomeTax = new IncomeTaxViewModel()
                            {
                                Rate = 1
                            },
                             VatTax = new VatTaxViewModel()
                            {
                                Rate = "10"
                            }

                        },
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = false,
                            IncomeTaxBy = "a",
                            Remark = "Remark"
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPPNNullViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo = "1",
                    Unit = new UnitViewModel()
                    {
                        Name = "a"
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        },
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        }
                    },
                    Currency = new CurrencyViewModel()
                    {
                        Code = "a",
                        Description = "a"
                    },
                    Date = DateTimeOffset.Now,
                    Id = 1,
                    VBNonPOType = "Tanpa Nomor VB",
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = true,
                            IncomeTaxBy = "a",
                            IncomeTax = new IncomeTaxViewModel()
                            {
                                Rate = 1,
                            },
                            Remark = "Remark",
                            Amount = 1
                        },
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = false,
                            IncomeTaxBy = "a",
                            IncomeTax = new IncomeTaxViewModel()
                            {
                                Rate = 1,
                            },
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPPHNullViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo = "1",
                    Unit = new UnitViewModel()
                    {
                        Name = "a"
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        },
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        }
                    },
                    Currency = new CurrencyViewModel()
                    {
                        Code = "a",
                        Description = "a"
                    },
                    Date = DateTimeOffset.Now,
                    Id = 1,
                    VBNonPOType = "Tanpa Nomor VB",
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = false,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1
                        },
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = false,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPONullPPHandPPNViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo = "1",
                    Unit = new UnitViewModel()
                    {
                        Name = "a"
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        }
                    },
                    Currency = new CurrencyViewModel()
                    {
                        Code = "a",
                        Description = "a"
                    },
                    Date = DateTimeOffset.Now,
                    Id = 1,
                    VBNonPOType = "Tanpa Nomor VB",
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = false,
                            IncomeTaxBy = "a",
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark",
                    VBDocument = new VBRequestDocumentNonPODto()
                    {
                        Amount = 1
                    }
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentItemMoreThanOneViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo = "1",
                    Unit = new UnitViewModel()
                    {
                        Name = "a"
                    },
                    UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                    {
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        },
                        new VBRealizationDocumentNonPOUnitCostViewModel()
                        {
                            Active = true,
                            Amount = 1,
                            IsSelected = true,
                            Unit = new UnitViewModel()
                            {
                                VBDocumentLayoutOrder = 10
                            }
                        }
                    },
                    Currency = new CurrencyViewModel()
                    {
                        Code = "a",
                        Description = "a"
                    },
                    Date = DateTimeOffset.Now,
                    Id = 1,
                    VBNonPOType = "Tanpa Nomor VB",
                    Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                    {
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = true,
                            IncomeTaxBy = "a",
                            Remark = "Remark",
                            Amount = 1,
                            IncomeTax = new IncomeTaxViewModel()
                            {
                                Rate = 1
                            },
                            VatTax = new VatTaxViewModel()
                            {
                                Rate = "10"
                            }
                        },
                        new VBRealizationDocumentNonPOExpenditureItemViewModel()
                        {
                            DateDetail = DateTimeOffset.Now,
                            BLAWBNumber = "1",
                            IsGetPPn = false,
                            IsGetPPh = true,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1,
                            IncomeTax = new IncomeTaxViewModel()
                            {
                                Rate = 1
                            }
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPNandPPH()
        {
            VBRealizationDocumentNonPOPDFTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPNNull()
        {
            VBRealizationDocumentNonPOPDFTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPPNNullViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPHNull()
        {
            VBRealizationDocumentNonPOPDFTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPPHNullViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateWithPPHAndPPNNull()
        {
            VBRealizationDocumentNonPOPDFTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPONullPPHandPPNViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessPDFTemplateItemMoreThanOne()
        {
            VBRealizationDocumentNonPOPDFTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentItemMoreThanOneViewModel, 7);
            Assert.NotNull(result);
        }
    }
}