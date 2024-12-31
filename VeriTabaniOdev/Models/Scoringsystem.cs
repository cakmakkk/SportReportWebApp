using System;
using System.Collections.Generic;

namespace VeriTabaniOdev.Models;

public partial class Scoringsystem
{
    public int Sportid { get; set; }

    public int? Pointswin { get; set; }

    public int? Pointsdraw { get; set; }

    public int? Pointsloss { get; set; }
}
