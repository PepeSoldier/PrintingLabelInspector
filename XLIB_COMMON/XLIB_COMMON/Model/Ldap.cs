using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;

namespace XLIB_COMMON.Model
{
    public class LDAP
    {
        public LDAP()
        {
            Settings = new CustomSettings("LDAP.config");
        }

        CustomSettings Settings;

        public LDAP_result SearchUser(string username, string password)
        {
            LDAP_result ldapResult = new LDAP_result();
            SearchResult result = null;
            bool isLDAPactive = Convert.ToBoolean(Settings.GetValue("LDAP_isActive")); //Properties.Settings.Default.LDAP_isActive;

            if (isLDAPactive)
            {
                string server = Settings.GetValue("LDAP_Server"); //"vds-polska.pl";
                string path = "LDAP://" + server + ""; // "//DC=biz,DC=electrolux,DC=com"; //OU=People,O=mycompany";
                string user = Settings.GetValue("LDAP_Domail") + "\\" + username;
                string pwd = password;

                try
                {
                    DirectoryEntry de = new DirectoryEntry(path, user, pwd, AuthenticationTypes.ReadonlyServer);
                    DirectorySearcher deSearch = new DirectorySearcher();

                    deSearch.SearchRoot = de;
                    deSearch.Filter = "(" + Settings.GetValue("LDAP_Field_UserName") + "=" + username + ")";
                    deSearch.ClientTimeout = new TimeSpan(0, 0, 5);
                    try
                    {
                        result = deSearch.FindOne();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ldapResult.Status = LDAP_ResponseStatus.Authenticated;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The server is not operational.\r\n")
                        ldapResult.Status = LDAP_ResponseStatus.ServerUnavailable;
                    else
                        ldapResult.Status = LDAP_ResponseStatus.BadLoginOrPassword;
                }
            }
            else
            {
                ldapResult.Status = LDAP_ResponseStatus.LDAP_IsOFF;
            }

            ldapResult.Result = result;
            return ldapResult;
        }
        public LDAP_user GetUserData(SearchResult result)
        {
            LDAP_user userL = null;

            if (result != null)
            {
                userL = new LDAP_user();
                // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  

                ResultPropertyCollection fields = result.Properties;

                foreach (String ldapField in fields.PropertyNames)
                {
                    // cycle through objects in each field e.g. group membership  
                    // (for many fields there will only be one object such as name)  

                    foreach (Object myCollection in fields[ldapField])
                    {
                        Console.WriteLine(String.Format("{0,-20} : {1}", ldapField, myCollection.ToString()));

                        if (ldapField == "sAMAccountName")
                            userL.Login = myCollection.ToString();
                        else if (ldapField == "givenname")
                            userL.FirstName = myCollection.ToString();
                        else if (ldapField == "sn")
                            userL.LastName = myCollection.ToString();
                        else if (ldapField == "mail")
                            userL.mail = myCollection.ToString();
                        else if (ldapField == "department")
                            userL.DepartmentName = myCollection.ToString();
                        else if (ldapField == "title")
                            userL.Title = myCollection.ToString();
                        else if (ldapField == "mobile")
                            userL.mobile= myCollection.ToString();
                    }
                }
            }

            return userL;
        }
    }

    public class LDAP_result
    {
        public SearchResult Result { get; set; }
        public LDAP_ResponseStatus Status { get; set; }
    }

    public enum LDAP_ResponseStatus
    {
        Authenticated,
        BadLogin,
        BadPassword,
        BadLoginOrPassword,
        ServerUnavailable,
        LDAP_IsOFF
    }

    public class LDAP_user
    {
        public string Login { get; set; }    //cn
        public string FirstName { get; set; }//givenname
        public string LastName { get; set; } //sn
        public string Country { get; set; }  //c
        public string mail { get; set; }     //mail
        public string tel { get; set; }      //telephonenumber
        public string mobile { get; set; }   //mobile
        public string managerUserName { get; set; } //manager
        public string Title { get; set; }    //title
        public string DepartmentName { get; set; } //department
        public string Factory { get; set; }  //elxCIDLabel
    }
}