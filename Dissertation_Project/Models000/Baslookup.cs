using System;
using System.Collections.Generic;

namespace Dissertation_Project.Models000;

public partial class Baslookup
{
    public long Id { get; set; }

    public int? Code { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; } = new List<AspNetUser>();
}
