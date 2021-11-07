using System;
using System.Collections.Generic;
using System.Text;

namespace Nvk.Data
{
    public interface IMustHaveCurrentUser: IShouldConfigureBaseProperties
    {
        /// <summary>
        /// Id of user.
        /// </summary>
        Guid UserId { get; set; }
    }
}
