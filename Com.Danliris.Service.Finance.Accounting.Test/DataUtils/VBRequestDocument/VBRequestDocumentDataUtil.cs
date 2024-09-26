using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument
{
    public class VBRequestDocumentDataUtil
    {
        private readonly VBRequestDocumentService _service;

        public VBRequestDocumentDataUtil(VBRequestDocumentService service)
        {
            _service = service;
          
        }


        public async Task<VBRequestDocumentNonPODto> GetTestData_VBRequestDocumentNonPO()
        {

            var testData = GetNewData_VBRequestDocumentNonPOFormDto();
            int id = await _service.CreateNonPO(testData);
            var data = await _service.GetNonPOById(id);
            return data;
        }

        public async Task<VBRequestDocumentNonPODto> GetTestData_VBRequestDocumentNonPO_NotKlaring()
        {

            var testData = GetNewData_VBRequestDocumentNonPOFormDto();
            testData.IsInklaring = false;
            int id = await _service.CreateNonPO(testData);
            var data = await _service.GetNonPOById(id);
            return data;
        }


        public VBRequestDocumentWithPODto GetTestData_VBRequestDocumentWithPO()
        {
            var form = GetNewData_VBRequestDocumentWithPOFormDto();
            int id =_service.CreateWithPO(form);
            var data =_service.GetWithPOById(id);
            return data;
        }

        public VBRequestDocumentWithPODto GetTestData_VBRequestDocumentWithPO_Cancellation()
        {
            var form = GetNewData_VBRequestDocumentWithPOFormDto();
            form.IsInklaring = false;
            int id = _service.CreateWithPO(form);
            var data = _service.GetWithPOById(id);
            return data;
        }

        public VBRequestDocumentNonPOFormDto GetNewData_VBRequestDocumentNonPOFormDto()
        {
            VBRequestDocumentNonPOFormDto form = new VBRequestDocumentNonPOFormDto()
            {       
                IsInklaring=true,
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp"
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Id=1,
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "Code",
                        Name = "GARMENT",
                    },
                    Name = "Meter",
                    VBDocumentLayoutOrder = 1,
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Id=1,
                        IsSelected=true,
                        Unit=new UnitDto()
                        {
                            Code="Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name",
                                
                            },
                            Name = "Name",
                            VBDocumentLayoutOrder = 1
                        }
                    },
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Id=0,
                        IsSelected=true,
                        Unit=new UnitDto()
                        {
                            Code="Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name",

                            },
                            Name = "Name",
                            VBDocumentLayoutOrder = 1
                        }
                    }
                }
            };
            return form;
        }

        public VBRequestDocumentNonPOFormDto GetNewData_VBRequestDocumentNonPOFormDto_WithNullItemsId()
        {
            VBRequestDocumentNonPOFormDto form = new VBRequestDocumentNonPOFormDto()
            {
                IsInklaring = true,
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp"
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Id = 1,
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "Code",
                        Name = "GARMENT",
                    },
                    Name = "Meter",
                    VBDocumentLayoutOrder = 1,
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Id=0,
                        IsSelected=true,
                        Unit=new UnitDto()
                        {
                            Code="Code",
                            Division = new DivisionDto()
                            {
                                Code = "Code",
                                Name = "Name",

                            },
                            Name = "Name",
                            VBDocumentLayoutOrder = 1
                        }
                    }
                }
            };
            return form;
        }

        public VBRequestDocumentNonPOFormDto GetNewData_VBRequestDocumentNonPOFormDto__WithNullDivision()
        {
            VBRequestDocumentNonPOFormDto form = new VBRequestDocumentNonPOFormDto()
            {
                IsInklaring = true,
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp"
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Id = 1,
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "Code",
                        Name = "GARMENT",
                    },
                    Name = "Meter",
                    VBDocumentLayoutOrder = 1,
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Id=1,
                        IsSelected=true,
                        Unit=new UnitDto()
                        {
                            Code="Code",
                            Division = null,
                            Name = "Name",
                            VBDocumentLayoutOrder = 1
                        }
                    }
                }
            };
            return form;
        }

        public VBRequestDocumentWithPOFormDto GetNewData_VBRequestDocumentWithPOFormDto()
        {
            return new VBRequestDocumentWithPOFormDto()
            {
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp"
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                IsInklaring = true,
                SuppliantUnit = new UnitDto()
                {
                    Id = 1,
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "Code",
                        Name = "GARMENT"
                    },
                    Name = "Meter",
                    VBDocumentLayoutOrder = 1
                },
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {
                    new VBRequestDocumentWithPOItemFormDto()
                    {
                        
                       PurchaseOrderExternal=new PurchaseOrderExternal()
                       {
                           No="1",
                           Items=new List<PurchaseOrderExternalItem>()
                           {
                               new PurchaseOrderExternalItem()
                               {
                                   IncomeTax=new IncomeTaxDto()
                                   {
                                       Name="IncomeTax",
                                       Rate=1
                                   },
                                   Unit=new UnitDto()
                                   {
                                       Code="Code",
                                       Name="Name",
                                       Division=new DivisionDto()
                                       {
                                           Code="Code",
                                           Name="Name",
                                       }
                                   },
                                   UseIncomeTax=true,
                                   IncomeTaxBy="IncomeTaxBy",
                                   Conversion=1,
                                   DealQuantity=1,
                                   DealUOM=new UnitOfMeasurement()
                                   {
                                       Unit="meter",
                                   },
                                   DefaultQuantity=1,
                                   Price=1,
                                   Product=new Product()
                                   {
                                       Name="ProdukName",
                                       UOM=new UnitOfMeasurement()
                                       {
                                           Unit="meter"
                                       },
                                       Code="code"
                                   },
                                   UseVat=true,
                                   VatTax = new VatTaxDto()
                                   { 
                                        Id = "1",
                                        Rate = "1"
                                   }
                                   
                               }
                           },

                       }
                    }
                }
            };
        }

        public VBRequestDocumentWithPOFormDto GetNewData_VBRequestDocumentWithPOFormDto_WithItemsId()
        {
            return new VBRequestDocumentWithPOFormDto()
            {
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Code = "IDR",
                    Description = "Description",
                    Rate = 1,
                    Symbol = "Rp"
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Code = "Code",
                    Division = new DivisionDto()
                    {
                        Code = "Code",
                        Name = "GARMENT"
                    },
                    Name = "Meter",
                    VBDocumentLayoutOrder = 1
                },

                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {
                    new VBRequestDocumentWithPOItemFormDto()
                    {
                        Id = 1,
                       PurchaseOrderExternal=new PurchaseOrderExternal()
                       {
                           No="1",
                           Items=new List<PurchaseOrderExternalItem>()
                           {
                               new PurchaseOrderExternalItem()
                               {
                                   IncomeTax=new IncomeTaxDto()
                                   {
                                       Name="IncomeTax",
                                       Rate=1
                                   },
                                   Unit=new UnitDto()
                                   {
                                       Code="Code",
                                       Name="Name",
                                       Division=new DivisionDto()
                                       {
                                           Code="Code",
                                           Name="Name",
                                       }
                                   },
                                   UseIncomeTax=true,
                                   IncomeTaxBy="IncomeTaxBy",
                                   Conversion=1,
                                   DealQuantity=1,
                                   DealUOM=new UnitOfMeasurement()
                                   {
                                       Unit="meter",
                                   },
                                   DefaultQuantity=1,
                                   Price=1,
                                   Product=new Product()
                                   {
                                       Name="ProdukName",
                                       UOM=new UnitOfMeasurement()
                                       {
                                           Unit="meter"
                                       },
                                       Code="code"
                                   },
                                   UseVat=true,
                                   VatTax = new VatTaxDto()
                                   { 
                                        Id = "1",
                                        Rate = "1"
                                   }
                               }
                           },

                       }
                    }
                }
            };
        }
    }
}
