using System;
using System.Collections.Generic;
using System.Text;

namespace Profiles.Domain.Models;

public class PageParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
