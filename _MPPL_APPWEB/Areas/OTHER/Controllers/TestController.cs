using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.OTHER.Controllers
{
    public class TestController : Controller
    {
        private List<string> queData;
        private List<object> modeltab;
        public TestController()
        {
            //this.db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RebitTest()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RabitGetMSG()
        {
            string data = "";
            queData = new List<string>();
            modeltab = new List<object>();
            try
            {
                var factory = new ConnectionFactory() { HostName = "10.10.1.4", UserName = "oneprod", Password= "c5mvmZN2kqOrVKVYFRs_XX2sVfZBVUun" };
                
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "test-queue",
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        queData.Add(message.ToString());
                        modeltab.Add(model);                        
                    };
                    channel.BasicConsume(queue: "test-queque",
                                         autoAck: true,
                                         consumer: consumer);

                }
            }
            catch (Exception ex)
            {
                data += ex.Message;
                if (ex.InnerException != null)
                {
                    data += ex.InnerException.Message;
                }
            }
            return Json(queData);
        }

        [HttpPost]
        public JsonResult RabitSetMSG(string message ="")
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                var factory = new ConnectionFactory() { HostName = "10.10.1.4", UserName = "oneprod", Password = "c5mvmZN2kqOrVKVYFRs_XX2sVfZBVUun" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "test-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    string msg = message;
                    var body = Encoding.UTF8.GetBytes(msg);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "test-queue", basicProperties: properties, body: body);


                    jsonModel.MessageType = JsonMessageType.success;
                    jsonModel.Message = "nadano";
                }
            }
            catch (Exception ex)
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = ex.Message;
                
            }
            return Json(jsonModel);
        }
    }
}