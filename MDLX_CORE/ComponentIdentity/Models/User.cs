using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace MDLX_CORE.Models.IDENTITY
{
    [Table("IDENTITY_User", Schema = "_MPPL")]
    public class User : IdentityUser<string, UserLogin, UserRole, UserClaim>, IUser, IUser<string>//, IUser, //ApplicationUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            LastPasswordChangedDate = DateTime.Now.Date;
        }

        [DisplayName("Nazwa użytkownika")]
        public string Name { get; set; }

        [DisplayName("Imię")]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [DisplayName("Data ostatniej zmiany hasła")]
        public DateTime LastPasswordChangedDate { get; set; }

        [DisplayName("Przełożony")]
        public virtual User SuperVisorUser { get; set; }
        public string SuperVisorUserId { get; set; }
        public string SupervisorUserName { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(25)]
        public string Factory { get; set; }

        public bool Deleted { get; set; }

        [NotMapped]
        [Display(Name = "Pan/Pani:")]
        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null)
                    return UserName + " " + LastName + " " + FirstName;
                else
                    return LastName + " " + FirstName;
            }
        }

        [NotMapped]
        [Display(Name = "Inicjały:")]
        public string Initials
        {
            get
            {
                string fn, ln;
                fn = (FirstName == null) ? UserName : FirstName;
                ln = (LastName == null) ? UserName : LastName;
                return string.Concat(ln[0], fn[0]);
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public override string ToString()
        {
            return FullName;
        }
    }

    public class UserLight
    {
        public string FullName { get; set; }
        public string Id { get; set; }
    }
}