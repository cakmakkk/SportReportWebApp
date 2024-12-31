using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Team
{
    public int Teamid { get; set; }

    public string Teamname { get; set; } = null!;

    public int? Leagueid { get; set; }

    public string? City { get; set; }

    public int? Points { get; set; }

    public int? Goalsscored { get; set; }

    public int? Goalsconceded { get; set; }

    public int? Goaldifference { get; set; }

    public int? Wins { get; set; }

    public int? Draws { get; set; }

    public int? Losses { get; set; }

    public int? Leagueposition { get; set; }

    public string Coachname { get; set; } = null!;

    public string Clubpresident { get; set; } = null!;

    public virtual ICollection<Fixture> FixtureAwayteams { get; set; } = new List<Fixture>();

    public virtual ICollection<Fixture> FixtureHometeams { get; set; } = new List<Fixture>();

    public virtual League? League { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
