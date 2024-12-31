using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Pointstable
{
    public string? Teamname { get; set; }

    public string? Leaguename { get; set; }

    public int? Matchesplayed { get; set; }

    public int? Matcheswon { get; set; }

    public int? Matcheslost { get; set; }

    public int? Matchesdrawn { get; set; }

    public int? Goalsscored { get; set; }

    public int? Goalsconceded { get; set; }

    public int? Goaldifference { get; set; }

    public int? Points { get; set; }

    public int? Leagueid { get; set; }
}
