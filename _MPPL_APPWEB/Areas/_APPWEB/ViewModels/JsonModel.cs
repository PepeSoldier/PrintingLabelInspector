using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.ViewModels
{
    public class JsonModel
    {
        public JsonModel()
        {
        }
        public JsonModel(object data)
        {
            Data = data;
        }

        public object Data { get; set; }
        public string Message { get; set; }
        public JsonMessageType MessageType { get; set; }
        public string MessageTypeString { get { return MessageType.ToString(); } }
        public int Status { get; set; }

        public void SetMessage(string message, JsonMessageType messageType)
        {
            this.Message = message;
            this.MessageType = messageType;
        }
    }

    public enum JsonMessageType
    {
        undefined = 0,
        success = 1,
        danger = 2,
        warning = 4,
        info = 8
    }

}