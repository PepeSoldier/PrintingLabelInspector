using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace MDLX_CORE.Models.IDENTITY
{
    public class DefRoles
    {
        public const string USER = "USER";
        public const string ADMIN = "ADMIN";
        public const string ACCOUNT_ADMIN = "ACCOUNT_ADMIN";

        public const string PFEP_PACKINGINSTR_CONFIRMER = "PFEP_PACKINGINSTR_CONFIRMER";
        public const string PFEP_PACKINGINSTR_EXAMINER = "PFEP_PACKINGINSTR_EXAMINER";
        public const string PFEP_DEFPRINT_EDITOR = "PFEP_DEFPRINT_EDITOR";
        public const string PRD_SCHEDULER = "PRD_SCHEDULER";

        //public const string ONEPROD_VIEWER = "TechnologyUser";
        //public const string ONEPROD_ADMIN = "TechnologyAdmin";
        //public const string ONEPROD_MES_OPERATOR = "TechnologyOperator";
        //public const string ONEPROD_MES_SUPEROPERATOR = "TechnologyLider";

        public const string ONEPROD_VIEWER = "ONEPROD_VIEWER";
        public const string ONEPROD_ADMIN = "ONEPROD_ADMIN";
        public const string ONEPROD_MES_OPERATOR = "ONEPROD_MES_OPERATOR";
        public const string ONEPROD_MES_SUPEROPERATOR = "ONEPROD_MES_SUPEROPERATOR";

        public const string ILOGIS_ADMIN = "ILOGIS_ADMIN";
        public const string ILOGIS_VIEWER = "ILOGIS_VIEWER";
        public const string ILOGIS_OPERATOR = "ILOGIS_OPERATOR";
        public const string ILOGIS_OPERATOR_PICKING = "ILOGIS_OPERATOR_PICKING";
        public const string ILOGIS_OPERATOR_FEEDING = "ILOGIS_OPERATOR_FEEDING";
        public const string ILOGIS_OPERATOR_INCOMING = "ILOGIS_OPERATOR_INCOMING";
        public const string ILOGIS_WHDOC_VIEWER = "ILOGIS_WHDOC_VIEWER";
        public const string ILOGIS_WHDOC_EDITOR = "ILOGIS_WHDOC_EDITOR";
        public const string ILOGIS_WHDOC_APPROVER = "ILOGIS_WHDOC_APPROVER";
        public const string ILOGIS_WHDOC_SECURITY = "ILOGIS_WHDOC_SECURITY";

        //Roles not used yet
        public const string ILOGIS_CONFIG_EDITOR_WH = "ILOGIS_CONFIG_EDITOR_WH";
        public const string ILOGIS_CONFIG_EDITOR_PRD = "ILOGIS_CONFIG_EDITOR_PRD";

        public const string HSE_EDITOR = "HSE_EDITOR";

        //Helpers
        public const string Managers = PFEP_PACKINGINSTR_CONFIRMER; // + "," + ProdMaster;

        public static bool HasUserRole(IPrincipal user, string roles)
        {
            string[] roles1 = roles.Split(',');
            foreach(string role in roles1)
            {
                if (user.IsInRole(role))
                    return true;
            }
            return false;
        }

        //Generuje stringa z rolami po przecinku np "ProdManager,QualityManager"
        private static string CreateRolesArray(params string[] roles)
        {
            StringBuilder rolesStr = new StringBuilder();
            foreach(string role in roles)
            {
                if(rolesStr.Length > 0)
                    rolesStr.Append(",");

                rolesStr.Append(role);
            }
            return rolesStr.ToString();
        }
    }

}