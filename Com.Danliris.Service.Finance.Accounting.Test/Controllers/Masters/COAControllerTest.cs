using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.Master;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.Masters
{
    public class COAControllerTest : BaseControllerTest<COAController, COAModel, COAViewModel, ICOAService>
    {
        [Fact]
        public void GetAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAll()).Returns(new List<COAModel>());
            mocks.Mapper.Setup(f => f.Map<List<COAViewModel>>(It.IsAny<List<COAModel>>())).Returns(ViewModels);

            var response = GetController(mocks).GetAll();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void GetAll_ReadThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAll()).Throws(new Exception());
            var response = GetController(mocks).GetAll();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void UploadFile_WithoutException_ReturnOK()
        {
            string header = "Kode, Nama, Report Type, Nature, Balance";
            string isi = "2112.12.2.12, Nama,Report Type,Nature, 1";
            var mockFacade = new Mock<ICOAService>();
            mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Returns(Task.CompletedTask);
            mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());
            
            mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<COAViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(true, new List<object>()));
            COAProfile profile = new COAProfile();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));
            var model = new COAModel()
            {
                Code = "2112.12.2.12",
                Name = "Nama",
                ReportType = "report",
                Nature = "nature",
                Balance = 1
            };
            mockMapper.Setup(x => x.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(new List<COAModel>() { model });
            var mockIdentityService = new Mock<IIdentityService>();
            var mockValidateService = new Mock<IValidateService>();

            var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
            controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + isi)), 0, Encoding.UTF8.GetBytes(header + "\n" + isi).LongLength, "Data", "test.csv");
            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            var response = controller.PostCSVFileAsync();
            Assert.NotNull(response.Result);
        }

        [Fact]
        public void UploadFile_WithException_ReturnError()
        {
            var mockFacade = new Mock<ICOAService>();
            mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Throws(new Exception());

            var mockMapper = new Mock<IMapper>();

            var mockIdentityService = new Mock<IIdentityService>();

            var mockValidateService = new Mock<IValidateService>();

            var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";

            var response = controller.PostCSVFileAsync();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response.Result));
        }

        [Fact]
        public void UploadFile_WithException_FileNotFound()
        {
            string header = "Kode, Nama,Path,Report Type,Nature,Cash Account";
            var mockFacade = new Mock<ICOAService>();
            mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Verifiable();
            mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());
            mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<COAViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(false, new List<object>()));
            COAProfile profile = new COAProfile();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

            mockMapper.Setup(x => x.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(new List<COAModel>() { Model });
            var mockIdentityService = new Mock<IIdentityService>();
            var mockValidateService = new Mock<IValidateService>();

            var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
            controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + header)), 0, Encoding.UTF8.GetBytes(header + "\n" + header).LongLength, "Data", "test.csv");
            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { });

            var response = controller.PostCSVFileAsync();
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response.Result));
        }

        [Fact]
        public void UploadFile_WithException_CSVError()
        {
            string header = "Kode, Nama,Path,Report Type,Nature,Cash Account";
            var mockFacade = new Mock<ICOAService>();
            mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Verifiable();
            mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(';').ToList());
            var data = It.IsAny<List<COAViewModel>>();
            mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<COAViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(false, new List<object>()));
            COAProfile profile = new COAProfile();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

            mockMapper.Setup(x => x.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(new List<COAModel>() { Model });
            var mockIdentityService = new Mock<IIdentityService>();
            var mockValidateService = new Mock<IValidateService>();

            var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
            controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + header)), 0, Encoding.UTF8.GetBytes(header + "\n" + header).LongLength, "Data", "test.csv");
            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            var response = controller.PostCSVFileAsync();
            Assert.Equal((int)HttpStatusCode.NotFound, GetStatusCode(response.Result));
        }

        [Fact]
        public void UploadFile_WithException_ErrorInFile()
        {
            string header = "Kode, Nama, Report Type, Nature, Balance";
            string isi = "2112.12.2.12, Nama,Report Type,Nature, 1";
            var mockFacade = new Mock<ICOAService>();
            mockFacade.Setup(f => f.UploadData(It.IsAny<List<COAModel>>())).Verifiable();
            mockFacade.Setup(f => f.CsvHeader).Returns(header.Split(',').ToList());
            var data = It.IsAny<List<COAViewModel>>();
            mockFacade.Setup(f => f.UploadValidate(ref It.Ref<List<COAViewModel>>.IsAny, It.IsAny<List<KeyValuePair<string, StringValues>>>())).Returns(new Tuple<bool, List<object>>(false, new List<object>()));
            COAProfile profile = new COAProfile();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.ConfigurationProvider).Returns(new MapperConfiguration(cfg => cfg.AddProfile(profile)));

            mockMapper.Setup(x => x.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(new List<COAModel>() { Model });
            var mockIdentityService = new Mock<IIdentityService>();
            var mockValidateService = new Mock<IValidateService>();

            var controller = GetController((mockIdentityService, mockValidateService, mockFacade, mockMapper));
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = $"{It.IsAny<int>()}";
            controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(header + "\n" + isi)), 0, Encoding.UTF8.GetBytes(header + "\n" + isi).LongLength, "Data", "test.csv");
            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            var response = controller.PostCSVFileAsync();
            Assert.NotNull(response.Result);
        }

        [Fact]
        public void GetCSV_ReturnFile()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DownloadTemplate()).Returns(new MemoryStream());

            var response = GetController(mocks).DownloadTemplate();
            Assert.NotNull(response);
        }

        [Fact]
        public void GetCSV_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DownloadTemplate()).Throws(new Exception());

            var response = GetController(mocks).DownloadTemplate();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        //[Fact]
        //public void Get_COA_Header_Enum()
        //{
        //    var mocks = GetMocks();

        //    var response = GetController(mocks).GetCOAHeaderSubheader();
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        //}

        [Fact]
        public async Task GetEmptyNameCoa_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetEmptyNames()).ReturnsAsync(new List<COAModel>());
            mocks.Mapper.Setup(f => f.Map<List<COAViewModel>>(It.IsAny<List<COAModel>>())).Returns(ViewModels);

            var response = await GetController(mocks).GetWithEmptyNames();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task GetEmptyNameCoa_ReadThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetEmptyNames()).ThrowsAsync(new Exception());
            var response = await GetController(mocks).GetWithEmptyNames();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task ReviseNameCoa_WithoutException_NoContent()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReviseEmptyNamesCoa(It.IsAny<List<COAModel>>())).ReturnsAsync(1);
            List<COAModel> models = new List<COAModel>()
            {
                Model
            };
            mocks.Mapper.Setup(f => f.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(models);

            var response = await GetController(mocks).ReviseEmptyNameCoa(new List<COAViewModel>());
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public async Task ReviseNameCoa_ReadThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReviseEmptyNamesCoa(It.IsAny<List<COAModel>>())).ThrowsAsync(new Exception());
            List<COAModel> models = new List<COAModel>()
            {
                Model
            };
            mocks.Mapper.Setup(f => f.Map<List<COAModel>>(It.IsAny<List<COAViewModel>>())).Returns(models);

            var response = await GetController(mocks).ReviseEmptyNameCoa(new List<COAViewModel>());
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        public override async Task Post_WithoutException_ReturnCreated()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(s => s.Validate(It.IsAny<COAViewModel>())).Verifiable();
            mocks.Service.Setup(s => s.CreateAsync(It.IsAny<COAModel>())).ReturnsAsync(1);
            COAModel model = new COAModel()
            {
                Code = "1111.11.1.11"
            };
            mocks.Mapper.Setup(m => m.Map<COAModel>(It.IsAny<COAViewModel>())).Returns(model);
            var controller = GetController(mocks);
            COAViewModel vm = new COAViewModel()
            {
                Code = "1111.11.1.11"
            };
            IActionResult response = await controller.Post(vm);

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        public override async Task Put_ValidId_ReturnNoContent()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<COAViewModel>())).Verifiable();
            var id = 1;
            var viewModel = new COAViewModel()
            {
                Id = id,
                Code = "1111.11.1.11"
            };
            COAModel model = new COAModel()
            {
                Id = id,
                Code = "1111.11.1.11"
            };
            mocks.Mapper.Setup(m => m.Map<COAModel>(It.IsAny<COAViewModel>())).Returns(model);
            mocks.Service.Setup(f => f.UpdateAsync(It.IsAny<int>(), It.IsAny<COAModel>())).ReturnsAsync(1);
            var controller = GetController(mocks);
            IActionResult response = await controller.Put(id, viewModel);

            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public void GetCOAHeaderSubheader_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetAll()).Returns(new List<COAModel>());
            mocks.Mapper.Setup(f => f.Map<List<COAViewModel>>(It.IsAny<List<COAModel>>())).Returns(ViewModels);

            var response = GetController(mocks).GetCOAHeaderSubheader();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }
    }
}
