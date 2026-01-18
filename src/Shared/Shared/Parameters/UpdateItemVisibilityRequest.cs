using Common.Enumerations.Visibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Parameters
{
    public class UpdateItemVisibilityRequest
    {
        public Guid ItemId { get; set; }
        public ProductVisibilityStatus VisibilityScope { get; set; }
    }
}
