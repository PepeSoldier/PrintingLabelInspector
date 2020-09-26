using _MPPL_WEB_START.Areas.IDENTITY.ViewModels;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace _MPPL_WEB_START.Areas.Models
{
    public class IdApi
    {

        public User GetUserData(string login, string password)
        {
            LoginViewModel model = new LoginViewModel { UserName = login, Password = password, RememberMe = false };
            WebResponse webResponse = SendPostRequestJson(new JavaScriptSerializer().Serialize(model), "http://localhost:55747/api/id");
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            User user = new JavaScriptSerializer().Deserialize<User>(reader.ReadToEnd());

            return user;
        }

        public static WebResponse SendPostRequestJson(string data, string url)
        {
            WebRequest httpRequest = HttpWebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.ContentLength = data.Length;

            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(data);
            streamWriter.Close();

            return httpRequest.GetResponse();
        }
    }
}