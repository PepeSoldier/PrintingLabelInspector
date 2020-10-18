using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDLX_CORE.Models.IDENTITY
{
    public class UserLogin : IdentityUserLogin<string>
    {
        [Key]
        public virtual string UserId { get; set; }
    }
}