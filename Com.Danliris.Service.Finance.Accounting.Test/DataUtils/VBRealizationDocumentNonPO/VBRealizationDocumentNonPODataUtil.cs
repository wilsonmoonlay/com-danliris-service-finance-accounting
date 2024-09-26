using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRealizationDocumentNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPODataUtil
    {
        private readonly VBRealizationDocumentNonPOService _service;

        public VBRealizationDocumentNonPODataUtil(VBRealizationDocumentNonPOService service)
        {
            _service = service;

        }


        public VBRealizationDocumentNonPOViewModel GetNewData_VBRealizationDocumentNonPOViewModel()
        {
            VBRealizationDocumentNonPOViewModel data = new VBRealizationDocumentNonPOViewModel()
            {
                IsInklaring = true,
                Amount = 145000,
                BLAWBNumber = "BL030AWB030",
                DocumentNo = "R-T-1020-018",
                ContractPONumber = "PO030",
                Currency = new CurrencyViewModel()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp",

                },
                DocumentType = RealizationDocumentType.WithVB,
                Index = 1,
                Date = DateTimeOffset.Now,
                Position = Lib.BusinessLogic.VBRealizationDocumentExpedition.VBRealizationPosition.Cashier,
                Type = VBType.NonPO,
                Unit = new UnitViewModel()
                {
                    Code = "Code",
                    Division = new DivisionViewModel()
                    {
                        Code = "G",
                        Name = "GARMENT",

                    }
                },
                VBDocument = new VBRequestDocumentNonPODto()
                {
                    Id=1,
                    Amount=10,
                    ApprovalStatus="CASHIER",
                    NoBL= "NoBL",
                    NoPO= "NoPO",
                    SuppliantUnit=new UnitDto()
                    {
                        Code="Code",
                        Division=new DivisionDto()
                        {
                            Code="G",
                            Name="GARMENT"
                        }
                    },
                    IsInklaring =true,
                    Items=new List<VBRequestDocumentNonPOItemDto>()
                    {
                        new VBRequestDocumentNonPOItemDto()
                        {
                            Unit=new UnitDto()
                            {
                                Code="Code",
                                Division=new DivisionDto()
                                {
                                    Name="GARMENT",
                                    Code="G"
                                }
                            }
                        }
                    },
                    DocumentNo = "R-T-1020-018",
                    IsApproved = true,
                    Currency = new CurrencyDto()
                    {
                        Code = "IDR",
                        Rate = 1,
                        Description = "Description",
                        Symbol = "Rp",
                    }
                },
                UnitCosts = new List<VBRealizationDocumentNonPOUnitCostViewModel>()
                {
                    new VBRealizationDocumentNonPOUnitCostViewModel()
                    {
                        Unit=new UnitViewModel()
                        {
                          Code="Code",
                          Division=new DivisionViewModel()
                          {
                              Code="Code",
                              Name="Name"
                          }
                        },
                    }
                },
                VBNonPOType = "Dengan Nomor VB",

                Items = new List<VBRealizationDocumentNonPOExpenditureItemViewModel>()
                {
                    new VBRealizationDocumentNonPOExpenditureItemViewModel()
                    {
                        Amount=145000,
                        BLAWBNumber="BL030AWB030",
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
                    }
                }
            };

            return data;
        }

        public async Task<VBRealizationDocumentNonPOViewModel> GetTestData()
        {
            var vm = GetNewData_VBRealizationDocumentNonPOViewModel();
            int id = await _service.CreateAsync(vm);
            var data = await _service.ReadByIdAsync(id);
            return data;
        }
    }
}
