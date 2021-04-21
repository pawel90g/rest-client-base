using Garbacik.NetCore.Utilities.Restful.Consts;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace Garbacik.NetCore.Utilities.Restful.Models
{
    public class GenericResponse
    {
        public bool Success { get; internal set; }
        public HttpStatusCode HttpStatusCode { get; internal set; }
        public string Error { get; internal set; }

        public string Response { get; internal set; }
        public byte[] RawBytes { get; internal set; }

        public GenericResponse(IRestResponse restResponse)
        {
            Success = restResponse.IsSuccessful;
            HttpStatusCode = restResponse.StatusCode;
            RawBytes = restResponse.RawBytes;

            if (!string.IsNullOrEmpty(restResponse.ErrorMessage))
                Error = $"{restResponse.ErrorMessage} {restResponse.ErrorException}";
            else if (!restResponse.IsSuccessful 
                     || (restResponse.StatusCode != HttpStatusCode.OK 
                         && restResponse.StatusCode != HttpStatusCode.Created))
                Error = restResponse.Content;

            if (restResponse.IsSuccessful && !string.IsNullOrEmpty(restResponse.Content))
                Response = restResponse.Content;
        }
    }

    public class GenericResponse<T> : GenericResponse
    {
        public new T Response { get; }

        public GenericResponse(IRestResponse restResponse)
            : base(restResponse)
        {
            if (!restResponse.IsSuccessful) return;
            
            try
            {
                Response = restResponse.ContentType.Equals(ContentTypes.XML)
                    ? DeserializeXml(restResponse.Content)
                    : DeserializeJson(restResponse.Content);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is null)
                    throw;
                    
                throw ex.InnerException;
            }
        }


        private static T DeserializeJson(string content)
            => JsonConvert.DeserializeObject<T>(content);

        private static T DeserializeXml(string content)
        {
            using (TextReader reader = new StringReader(content))
            {
                var serializer = new XmlSerializer(typeof(T));
                var obj = (T)serializer.Deserialize(reader);
                return obj;
            }
        }
    }
}
