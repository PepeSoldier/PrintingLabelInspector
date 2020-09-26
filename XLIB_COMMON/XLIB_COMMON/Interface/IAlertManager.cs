using System.Collections.Generic;

namespace XLIB_COMMON.Interface
{
    public interface IAlertManager
    {
        void AddAlert(AlertMessageType messageType, string message, string userName);
        void AddAlertDBSaveProblem(string userName, string message);
        IEnumerable<IAlertModel> GetAlerts(string userName);
    }

    public interface IAlertModel
    {
        AlertMessageType MessageType { get; set; }
        string Message { get; set; }
        string UserName { get; set; }
    }

    public enum AlertMessageType
    {
        success,
        info,
        warning,
        danger,
        @default,
        notice
    }
}
