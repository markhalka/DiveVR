using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction
{
    public string name;
    public List<Material> start;
    public List<Material> end;
    public bool exo;

    public float tempChange;
    public string reactionType;

    public Reaction()
    {
        start = new List<Material>();
        end = new List<Material>();
        tempChange = 0;
        reactionType = "";
        exo = false;
    }
}
