using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Class to represent moves (attacks in Pokemon), currently only holds a name that we're going to use for flavour
/// </summary>

[System.Serializable]
public class Move
{
    public string name;

    public Move(string name)
    {
        this.name = name;
    }
}

