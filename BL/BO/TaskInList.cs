using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public record TaskInList
(
    int Id,
    string Description,
    string Alias,
    Status? Status
);

