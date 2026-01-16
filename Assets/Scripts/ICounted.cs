using System;

public interface ICounted
{
    Action DecreaseCount { get; set; }
}