using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Sport
{
    public int Sportid { get; set; }

    public string Sportname { get; set; } = null!;

    public virtual ICollection<League> Leagues { get; set; } = new List<League>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
