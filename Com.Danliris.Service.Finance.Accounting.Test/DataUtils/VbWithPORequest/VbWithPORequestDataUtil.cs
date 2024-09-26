using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VbWithPORequest
{
    public class VbWithPORequestDataUtil
    {
        private readonly VbWithPORequestService _service;

        public VbWithPORequestDataUtil(VbWithPORequestService service)
        {
            _service = service;
        }

        public VbWithPORequestViewModel GetViewModelToValidate()
        {
            return new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                //ApprovedDate = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                VBMoney = 1,
                Usage = "Usage",
                Approve_Status = false,
                Realization_Status = false,
                Complete_Status = false,
                Unit = new Unit()
                {
                    Id = 0,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "USD",
                    Rate = 1,
                    Symbol = "$",
                    Description = "Description"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 0,
                            Code = "Code",
                            Name = "Name",
                        },
                        vattax = new VatTaxDto()
                        {
                            Id = "2",
                            Rate = "11",
                        },

                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 123,
                                dealQuantity = 123,
                                dealUom  = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 123,
                                defaultUom = new defaultUom()
                                {
                                    _id="id",
                                    unit = "unit"
                                },
                                priceBeforeTax = 123,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }
                    }
                }
            };
        }

        public VbWithPORequestViewModel GetViewModelToValidateDuplicate()
        {
            return new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                //ApprovedDate = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                VBMoney = 0,
                Usage = "",
                Approve_Status = false,
                Realization_Status = false,
                Complete_Status = false,
                Unit = new Unit()
                {
                    Id = 0,
                    Code = "Code",
                    Name = "Name",
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 0,
                            Code = "Code",
                            Name = "Name",
                        },

                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 123,
                                dealQuantity = 123,
                                dealUom  = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 123,
                                defaultUom = new defaultUom()
                                {
                                    _id="id",
                                    unit = "unit"
                                },
                                priceBeforeTax = 123,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }
                    },
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 0,
                            Code = "Code",
                            Name = "Name",
                        }
                    }
                }
            };
        }

        public VbWithPORequestViewModel GetViewModel()
        {
            return new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                VBMoney = 1,
                Usage = "Usage",
                Approve_Status = false,
                Realization_Status = false,
                Complete_Status = false,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "Code",
                    Rate = 1,
                    Symbol = "$",
                    Description = "Description"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name",
                        },
                        CurrencyCode = "CurrencyCode",
                        CurrencyRate = 1,
                        IncomeTax = new IncomeTax()
                        {
                            _id = "id",
                            Name = "Name",
                            Rate = "Rate"
                        },
                        IncomeTaxBy = "IncomeTaxBy",

                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Id = 1,
                                Conversion = 123,
                                dealQuantity = 123,
                                dealUom  = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 123,
                                defaultUom = new defaultUom()
                                {
                                    _id="id",
                                    unit = "unit"
                                },
                                priceBeforeTax = 123,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }
                    }
                }
            };
        }

        public VbWithPORequestViewModel GetViewModel1()
        {
            return new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                VBMoney = 1,
                Usage = "Usage",
                Approve_Status = false,
                Realization_Status = false,
                Complete_Status = false,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "Code",
                    Rate = 1,
                    Symbol = "$",
                    Description = "Description"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name",
                        },
                        CurrencyCode = "CurrencyCode",
                        CurrencyRate = 1,
                        IncomeTax = new IncomeTax()
                        {
                            _id = "",
                            Name = "",
                            Rate = ""
                        },
                        IncomeTaxBy = "",

                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Id = 1,
                                Conversion = 123,
                                dealQuantity = 123,
                                dealUom  = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 123,
                                defaultUom = new defaultUom()
                                {
                                    _id="id",
                                    unit = "unit"
                                },
                                priceBeforeTax = 123,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }
                    }
                }
            };
        }

        

        public VbRequestModel GetVbRequestModelToCreate()
        {
            return new VbRequestModel()
            {

                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                UnitId = 1,
                UnitCode = "UnitCode",
                UnitName = "UnitName",
                CurrencyId = 1,
                CurrencyCode = "CurrencyCode",
                CurrencyRate = 123,
                CurrencySymbol = "CurrencySymbol",
                CurrencyDescription = "CurrencyDescription",
                Amount = 123,
                Usage = "Usage",
                UnitLoad = "UnitLoad",
                Apporve_Status = true,
                Realization_Status = true,
                Complete_Status = true,
                VBRequestCategory = "PO",
                CreatedBy = "CreatedBy",
                VBMoney = 1,
                Usage_Input = "Usage_Input",
                VbRequestDetail = new List<VbRequestDetailModel>()
                {
                    new VbRequestDetailModel()
                    {
                        VBNo = "VBNo",
                        PONo = "PONo",
                        UnitId = 1,
                        UnitName = "UnitName",
                        DetailOthers = "DetailOthers",
                        ProductId = "ProductId",
                        ProductCode = "ProductCode",
                        ProductName = "ProductName",
                        DefaultQuantity = 1,
                        DefaultUOMId = "DefaultUOMId",
                        DefaultUOMUnit = "DefaultUOMUnit",
                        DealQuantity = 1,
                        DealUOMId = "DealUOMId",
                        DealUOMUnit = "DealUOMUnit",
                        Conversion = 1,
                        Price = 1,
                        ProductRemark = "ProductRemark"
                    }

                }
            };
        }

       
       
        public async Task<VbWithPORequestViewModel> GetCreatedData()
        {
            var viewmodel = GetViewModel();
            var model = GetVbRequestModelToCreate();
            await _service.CreateAsync(model, viewmodel);
            return await _service.ReadByIdAsync2(model.Id);
        }

        public async Task<VbRequestModel> GetTestData()
        {
            VbRequestModel vbRequest = GetVbRequestModelToCreate();
            var viewmodel = GetViewModel();
            await _service.CreateAsync(vbRequest, viewmodel);

            return vbRequest;
        }
    }
}
