using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificNamesDb : MonoBehaviour
{
    public HorizontalSnap hs;
    public Sprite[] scientificNameSprites;

    public void Start()
    {
        hs.createHS(scientificNameSprites);
        Database.currentSprites = scientificNameSprites;
    }
}

