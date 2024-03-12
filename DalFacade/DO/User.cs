using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

public record User
(
    int UserId,
    string Password,
    bool IsAdmin
)
{
    public User() : this(0,"",false)
    {}
}
