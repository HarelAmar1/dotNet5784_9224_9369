using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class User
{
    public int UserId { set; get; }
    public string Password { set; get; }
    public bool IsAdmin { set; get; }
}
