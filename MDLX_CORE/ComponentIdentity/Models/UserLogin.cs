using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using MDL_BASE.Models.MasterData;
using System.Threading.Tasks;

namespace MDL_BASE.Models.IDENTITY
{
    public class UserLogin : IdentityUserLogin<string>
    {
        [Key]
        public virtual string UserId { get; set; }
    }
}