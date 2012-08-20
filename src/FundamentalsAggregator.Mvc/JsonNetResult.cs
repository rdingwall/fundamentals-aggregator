using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FundamentalsAggregator.Mvc
{
    // From http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net.aspx
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            // Custom for Fundamentals Aggregator
            SerializerSettings = new JsonSerializerSettings
                                     {
                                         NullValueHandling = NullValueHandling.Ignore,
                                         DefaultValueHandling = DefaultValueHandling.Ignore
                                     };

            Formatting = Formatting.Indented;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                                       ? ContentType
                                       : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}