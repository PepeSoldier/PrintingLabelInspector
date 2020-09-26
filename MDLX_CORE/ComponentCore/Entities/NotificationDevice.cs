
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_CORE.ComponentCore.Entities
{
    [Table("NotificationDevice", Schema = "CORE")]
    public class NotificationDevice : IModelDeletableEntity
    {
        public NotificationDevice()
        {
            RegistrationDate = DateTime.Now;
        }
        public int Id { get; set; }
        public virtual User User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }

        public DateTime RegistrationDate { get; set; }
        public bool Deleted { get; set; }
    }
}