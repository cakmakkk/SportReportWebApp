using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Player
{
    public int Playerid { get; set; }

    public string Playername { get; set; } = null!;

    public int? Teamid { get; set; }

    public int? Positionid { get; set; }

    public int? Goals { get; set; }

    public virtual Position? Position { get; set; }

    public virtual Team? Team { get; set; }
}
