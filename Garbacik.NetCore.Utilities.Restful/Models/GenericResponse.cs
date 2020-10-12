using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace Garbacik.NetCore.Utilities.Restful.Models
{
    public class GenericResponse
    {
        public bool Success { get; internal set; }
        public HttpStatusCode HttpStatusCode { get; internal set; }
        public string Error { get; internal set; }

        public string Response { get; internal set; }

        public GenericResponse(IRestResponse restResponse)
        {
            Success = restResponse.IsSuccessful;
            HttpStatusCode = restResponse.StatusCode;

            if(!string.IsNullOrEmpty(restResponse.ErrorMessage))
            {
                Error = restResponse.ErrorMessage + restResponse.ErrorException.ToString();
            }
            else if (!restResponse.IsSuccessful || ( restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created))
            {
                Error = restResponse.Content;
            }

            if (restResponse.IsSuccessful && !string.IsNullOrEmpty(restResponse.Content))
                Response = restResponse.Content;
        }
    }

    public class GenericResponse<T> : GenericResponse
    {
        public T Response { get; }

        public GenericResponse(IRestResponse restResponse) 
            : base(restResponse)
        {
            if (restResponse.IsSuccessful)
            {
                try
                {
                    Response = JsonConvert.DeserializeObject<T>(restResponse.Content);
                }
                catch(Exception ex)
                {
                    
                }
            }
        }
    }
}
