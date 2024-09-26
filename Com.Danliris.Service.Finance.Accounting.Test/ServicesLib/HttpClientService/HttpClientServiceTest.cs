using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ServicesLib.ValidateService
{
  public  class HttpClientServiceTest
    {
        [Fact]
        public async Task should_success_GetAsync()
        {
            //Setup
            Mock<IIdentityService> identity = new Mock<IIdentityService>();
            identity.Setup(s => s.Username).Returns("usernameTest");
            HttpClientService httpClient = new HttpClientService(identity.Object);

            //Act
            var result = await httpClient.GetAsync("https://stackoverflow.com/");

            //Assert
            Assert.NotNull(result);
        }

        

        //[Fact]
        //public async Task should_success_PostAsync()
        //{
        //    //Setup
        //    Mock<IIdentityService> identity = new Mock<IIdentityService>();
        //    identity.Setup(s => s.Username).Returns("usernameTest");
        //    HttpClientService httpClient = new HttpClientService(identity.Object);

        //    MemoItemModel model = new MemoItemModel()
        //    {
        //        CurrencyCode ="Rp",
        //        Interest =2
        //    };

        //    //Act
        //    var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        //    var result = await httpClient.PostAsync("https://stackoverflow.com/", stringContent);

        //    //Assert
        //    Assert.NotNull(result);
        //}

        [Fact]
        public async Task should_success_PutAsync()
        {
            //Setup
            Mock<IIdentityService> identity = new Mock<IIdentityService>();
            identity.Setup(s => s.Username).Returns("usernameTest");
            HttpClientService httpClient = new HttpClientService(identity.Object);

            MemoItemModel model = new MemoItemModel()
            {
                CurrencyCode = "Rp",
                Interest = 2
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PutAsync("https://stackoverflow.com/", stringContent);

            //Assert
            Assert.NotNull(result);
        }

    }
}
