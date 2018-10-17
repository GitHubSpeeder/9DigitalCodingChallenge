using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonWebService;
using JsonWebService.Controllers;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;

namespace JsonWebService.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void GetNotSupported()
        {
            // Arrange
            ValuesController controller = new ValuesController();
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            HttpResponseMessage response = controller.Get();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);

            response.Content.ReadAsAsync<ExpandoObject>().ContinueWith(task => {
                string error = ((dynamic)task.Result).error;
                Assert.AreEqual("The requested resource does not support http method 'GET'", error);
            }).Wait();
        }

        [TestMethod]
        public void PostValidJson()
        {
            // Arrange
            ValuesController controller = new ValuesController();
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            JObject jo = JObject.Parse(File.ReadAllText(@"..\..\ValidJson.txt"));

            // Act
            HttpResponseMessage response = controller.Post(jo);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            response.Content.ReadAsAsync<ExpandoObject>().ContinueWith(task => {
                dynamic shows = ((dynamic)task.Result).response;

                foreach (dynamic show in shows)
                {
                    Assert.AreEqual("http://mybeautifulcatchupservice.com/img/shows/16KidsandCounting1280.jpg", show.image);
                    Assert.AreEqual("show/16kidsandcounting", show.slug);
                    Assert.AreEqual("16 Kids and Counting", show.title);
                    break;
                }
            }).Wait();
        }

        [TestMethod]
        public void PostInvalidJsonField()
        {
            // Arrange
            ValuesController controller = new ValuesController();
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            JObject jo = JObject.Parse(File.ReadAllText(@"..\..\InvalidJsonField.txt"));

            // Act
            HttpResponseMessage response = controller.Post(jo);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            response.Content.ReadAsAsync<ExpandoObject>().ContinueWith(task => {
                string error = ((dynamic)task.Result).error;
                Assert.AreEqual("Could not decode request: JSON parsing failed", error);
            }).Wait();
        }

        [TestMethod]
        public void PostInvalidData()
        {
            // Arrange
            ValuesController controller = new ValuesController();
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            JObject jo = null;
            try
            {
                jo = JObject.Parse(File.ReadAllText(@"..\..\InvalidData.txt"));
            }
            catch (JsonReaderException )
            {
            }

            // Act
            HttpResponseMessage response = controller.Post(jo);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            response.Content.ReadAsAsync<ExpandoObject>().ContinueWith(task => {
                string error = ((dynamic)task.Result).error;
                Assert.AreEqual("Could not decode request: JSON parsing failed", error);
            }).Wait();
        }
    }
}
