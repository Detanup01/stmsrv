using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib;

namespace Steam3Server.HTTPServer
{
    public class CustomAPI
    {
        public static HttpResponse HandleAPIRequest(HttpRequest request)
        {
            var rsp = new HttpResponse();
            
            if (request == null)
                return rsp.MakeErrorResponse("Request Enmpty");

            if (request.Url.Contains("customapi/register"))
            {
                Debug.PWDebug(request.ToString(), "HandleAPIRequest");
                var body = request.Body;

                //get the body

            }

            return rsp.MakeOkResponse();
        }

    }
}
