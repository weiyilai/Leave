﻿namespace ChainResponsibility.Entities;

public class Leave
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<PositionLevel> PositionLevels { get; set; }
}
