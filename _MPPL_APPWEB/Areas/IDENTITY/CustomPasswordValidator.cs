using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace _MPPL_WEB_START.Areas.IDENTITY
{
    public class CustomPasswordValidator : PasswordValidator
    {
        public override Task<IdentityResult> ValidateAsync(string item)
        {
            string returnMsg = "";
            bool isSuccess = true;
            if (String.IsNullOrEmpty(item) || RequiredLength > item.Length)
            {
                isSuccess = false;
                returnMsg += "Hasło zbyt krótkie. ";
            }
            if (RequireNonLetterOrDigit)
            {
                foreach(char a in item)
                {
                    if (!IsLetterOrDigit(a))
                    {
                        isSuccess = false;
                        returnMsg += "W haśle brakuje znaku specjalnego. ";
                    }
                }
            }

            if(RequireDigit)
            {
                bool isDigit = false;
                foreach (char a in item)
                {
                    if (IsDigit(a))
                    {
                        isDigit = true;
                    }
                }
                if (!isDigit)
                {
                    isSuccess = false;
                    returnMsg += "W haśle musi znajdować się przynajmniej jedna cyfra. ";
                }
            }

            if (RequireLowercase)
            {
                bool isLowerCase = false;
                foreach (char a in item)
                {
                    if (IsLower(a))
                    {
                        isLowerCase = true;
                    }
                }
                if (!isLowerCase)
                {
                    isSuccess = false;
                    returnMsg += "W haśle musi znajdować się przynajmniej jedna mała litera a-z. ";
                }
            }

            if (RequireUppercase)
            {
                bool isUpperCase = false;
                foreach (char a in item)
                {
                    if (IsUpper(a))
                    {
                        isUpperCase = true;
                    }
                }
                if (!isUpperCase)
                {
                    isSuccess = false;
                    returnMsg += "W haśle musi znajdować się przynajmniej jedna duża litera A-Z. ";
                }
            }

            if (isSuccess)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(returnMsg));
            }
        }
    }
}