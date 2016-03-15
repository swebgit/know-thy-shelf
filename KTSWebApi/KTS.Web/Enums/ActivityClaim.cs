using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS.Web.Enums
{
    [Flags]
    public enum ActivityClaim
    {
        None = 0,
        EditBookClaim = 1,
        CreateBookClaim = 2,
        DeleteBookClaim = 4,
        ViewUsersClaim = 8,
        EditUserClaim = 16,
        CreateUserClaim = 32,
        DeleteUserClaim = 64
    }
}
