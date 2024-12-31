using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class League
{
    public int Leagueid { get; set; }

    public string Leaguename { get; set; } = null!;

    public int? Sportid { get; set; }

    public virtual ICollection<Fixture> Fixtures { get; set; } = new List<Fixture>();

    public virtual Sport? Sport { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
