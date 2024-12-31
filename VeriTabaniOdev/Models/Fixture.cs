using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Fixture
{
    public int Matchid { get; set; }

    public int? Leagueid { get; set; }

    public int? Hometeamid { get; set; }

    public int? Awayteamid { get; set; }

    public DateOnly Matchdate { get; set; }

    public int? Homescore { get; set; }

    public int? Awayscore { get; set; }

    public string? Matchresult { get; set; }

    public virtual Team? Awayteam { get; set; }

    public virtual Team? Hometeam { get; set; }

    public League League { get; set; }
    
}
