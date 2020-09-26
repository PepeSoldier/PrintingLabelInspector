using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Model
{
    public class AlertModel : IAlertModel
    {
        public AlertModel()
        {
        }

        private AlertMessageType messageType;

        public AlertMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; MessageTypeName = value.ToString(); }
        }
        public string MessageTypeName { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
