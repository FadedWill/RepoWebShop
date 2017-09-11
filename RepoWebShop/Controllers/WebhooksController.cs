using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RepoWebShop.Controllers
{
    public class WebhooksController : Controller
    {
        public class WebhookTest
        {
            public int Id { get; set; }
            public bool Live_Mode { get; set; }
            public string Type { get; set; }
            public DateTime Date_Created { get; set; }
            public string User_Id { get; set; }
            public string Api_Version { get; set; }
            public string Action { get; set; }
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            if (Request.Body.CanSeek)
                Request.Body.Position = 0;

            var data = new StreamReader(Request.Body).ReadToEnd();
            
            /*
             * 
             * "{\"id\":123,\"live_mode\":false,\"type\":\"test\",\"date_created\":\"2017-09-09T21:12:32.706-04:00\",\"user_id\":\"58959397\",\"api_version\":\"v1\",\"action\":\"test.created\",\"data\":{\"id\":\"56456123212\"}}"
             * 
             * 
             * con esto, tengo que ir a preguntar la data del payment
             * */
            return Ok();
        }

        /*private void SeedWebhooks(APIContext apiContext)
        {
            var callBackUrl = Url.Action("Receive", "WebhookEvents", null, Request.Url.Scheme);
            //Request.Host.ToString()??

            if(Request.Url.Host == "localhost")
            {
                callBackUrl = "https://443tregf.ngrok.io/WebhookEvents/Receive";
            }
            var everythingWebhook = new Webhook()
            {
                url = callBackUrl,
                event_types = new List<WebhookEventType>
                {
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.REFUNDED"
                    },
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.REVERSED"
                    }
                };
             }
        }*/
    }
}
