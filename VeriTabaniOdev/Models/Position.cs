using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Position
{
    public int Positionid { get; set; }

    public string Positionname { get; set; } = null!;

    public int? Sportid { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual Sport? Sport { get; set; }
}
