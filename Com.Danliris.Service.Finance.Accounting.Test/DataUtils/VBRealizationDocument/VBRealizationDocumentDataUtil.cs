using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocument
{
    public class VBRealizationDocumentDataUtil
    {
        private readonly VBRealizationWithPOService _service;
        
        public VBRealizationDocumentDataUtil(VBRealizationWithPOService service)
        {
            _service = service;
           
        }

        public FormDto GetNewData_FormDto_Type_DenganNomorVB()
        {
            FormDto formDto = new FormDto()
            {
                Date = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Name="Name",
                    Id=1,
                    VBDocumentLayoutOrder=1,
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "G",
                        Name = "GARMENT"
                    }
                },
                VBRequestDocument = new VBRequestDocumentDto()
                {
                    
                    Id = 1
                },
                Type = "Dengan Nomor VB",
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Symbol = "Rp",
                    Description = "Description",
                    Rate = 1
                },
                
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                    {
                        
                        UnitPaymentOrder=new UnitPaymentOrderDto()
                        {
                            
                            No="1",
                            Id=1,
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
                            UseVat=true,
                            Division=new DivisionDto()
                            {
                                Code="Code",
                                Name="Name"
                            },
                            Supplier=new SupplierDto()
                            {
                                Code="Code",
                                Name="Name"
                            },
                            UnitCosts=new List<UnitCostDto>()
                            {
                               new UnitCostDto()
                               {
                                   Amount=1,
                                   Unit=new UnitDto()
                                   {
                                       Code="Code",
                                       Division=new DivisionDto()
                                       {
                                           Code="Code",
                                           Name="Name"
                                       },
                                       Name="Name",
                                       VBDocumentLayoutOrder=1
                                   }
                               }
                            },
                        }
                    }
                }
            };

            return formDto;
        }

        public FormDto GetNewData_FormDto_Type_TanpaNomorVB()
        {
            FormDto formDto = new FormDto()
            {
                
                Date = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "G",
                        Name = "GARMENT"
                    }
                },
                VBRequestDocument = new VBRequestDocumentDto()
                {
                    Id = 1
                },
                Type = "Tanpa Nomor VB",
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Symbol = "Rp",
                    Description = "Description",
                    Rate = 1
                },
                
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                    {
                        
                        UnitPaymentOrder=new UnitPaymentOrderDto()
                        {
                            No="1",
                            //Id=1,
                            Division=new DivisionDto()
                            {
                                Code="Code",
                                Name="Name"
                            },
                            Supplier=new SupplierDto()
                            {
                                Code="Code",
                                Name="Name"
                            },
                            UnitCosts=new List<UnitCostDto>()
                            {
                               new UnitCostDto()
                               {
                                   Amount=1,
                                   Unit=new UnitDto()
                                   {
                                       Code="Code",
                                       Division=new DivisionDto()
                                       {
                                           Code="Code",
                                           Name="Name"
                                       },
                                       Name="Name",
                                       VBDocumentLayoutOrder=1
                                   }
                               } 
                            },
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

            return formDto;
        }


        

        public VBRealizationWithPODto GetTestData_TanpaNomorVB()
        {
            FormDto formDto = GetNewData_FormDto_Type_TanpaNomorVB();
          
            int id =_service.Create(formDto);

            var data = _service.ReadById(id);
            return data;
        }

        public VBRealizationWithPODto GetTestData_DenganNomorVB()
        {
            FormDto formDto = GetNewData_FormDto_Type_DenganNomorVB();

            int id = _service.Create(formDto);

            var data = _service.ReadById(id);
            return data;
        }

        
    }
}
