using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JsonWebService.Models;
using System;
using Microsoft.CSharp.RuntimeBinder;
using System.Text;

namespace JsonWebService.Controllers
{
    public class ValuesController : ApiController
    {
        // GET
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            ErrorModel em = new ErrorModel("The requested resource does not support http method 'GET'");
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, em);
        }

        // POST
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]JObject value)
        {
            // Invalid data not recognised format.
            if (value == null)
            {
                ErrorModel em = new ErrorModel("Could not decode request: JSON parsing failed");
                return Request.CreateResponse(HttpStatusCode.BadRequest, em);
            }

            try
            {
                // Using dynamic JSON parsing to extract only the relevant fields are interested to return after conditions are met.
                dynamic shows = value; 

                List<ResponseShowModel> responseShows = new List<ResponseShowModel>();

                foreach (dynamic show in shows.payload)
                {
                    if (show.drm != true || show.episodeCount < 1)
                        continue;

                    // Convert dynamic to string value
                    string image = show.image.showImage;
                    string slug = show.slug;
                    string title = show.title;

                    responseShows.Add(new ResponseShowModel(image, slug, title));
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseShowModelList(responseShows));
            }
            catch (RuntimeBinderException )
            {
                ErrorModel em = new ErrorModel("Could not decode request: JSON parsing failed");
                return Request.CreateResponse(HttpStatusCode.BadRequest, em);
            }
            catch (Exception )
            {
                ErrorModel em = new ErrorModel("Could not decode request: JSON parsing failed");
                return Request.CreateResponse(HttpStatusCode.BadRequest, em);
            }
        }
    }
}
