using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_BASE.Models.Base
{
    public enum AttachmentParentTypeEnum
    {
        none = 0,
        ActivityAttachment = 1,
        ObservationAttachment = 2,
        ActionAttachment = 3,
        iLogisData = 20,
        PFEPPakingCard = 60,
    }
}