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
    public class VBRealizationDocumentNonPOPDFInklaringTemplateTest
    {
        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPDFInklaringTemplateViewModel
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
                            IsGetPPn = true,
                            IsGetPPh = true,
                            PPhAmount = 1,
                            PPnAmount = 1,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPDFInklaringTemplatePPNNullViewModel
        {
            get
            {
                return new VBRealizationDocumentNonPOViewModel()
                {
                    DocumentNo  = "1",
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
                            IsGetPPh = true,
                            PPhAmount = 1,
                            IncomeTaxBy = "a",
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPDFInklaringTemplatePPHNullViewModel
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
                            IsGetPPn = true,
                            IsGetPPh = false,
                            PPnAmount = 1,
                            IncomeTaxBy = "Supplier",
                            Remark = "Remark",
                            Amount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPDFInklaringTemplatePPHandPPNNullViewModel
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
                    Remark = "Remark"
                };
            }
        }

        public VBRealizationDocumentNonPOViewModel VBRealizationDocumentNonPOPDFInklaringTemplateItemMoreThanOneViewModel
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
                            PPhAmount = 1,
                            PPnAmount = 1
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
                            PPhAmount = 1,
                            PPnAmount = 1
                        }
                    },
                    Remark = "Remark"
                };
            }
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplate()
        {
            VBRealizationDocumentNonPOPDFInklaringTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFInklaringTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPDFInklaringTemplateViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplateWithPPNNull()
        {
            VBRealizationDocumentNonPOPDFInklaringTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFInklaringTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPDFInklaringTemplatePPNNullViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplateWithPPHNull()
        {
            VBRealizationDocumentNonPOPDFInklaringTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFInklaringTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPDFInklaringTemplatePPHNullViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplateWithPPNandPPHNull()
        {
            VBRealizationDocumentNonPOPDFInklaringTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFInklaringTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPDFInklaringTemplatePPHandPPNNullViewModel, 7);
            Assert.NotNull(result);
        }

        [Fact]
        public void shouldSuccessDocumentNonPOPDFInklaringTemplateItemMoreThanOne()
        {
            VBRealizationDocumentNonPOPDFInklaringTemplate PdfTemplate = new VBRealizationDocumentNonPOPDFInklaringTemplate();
            MemoryStream result = PdfTemplate.GeneratePdfTemplate(VBRealizationDocumentNonPOPDFInklaringTemplateItemMoreThanOneViewModel, 7);
            Assert.NotNull(result);
        }
    }
}
