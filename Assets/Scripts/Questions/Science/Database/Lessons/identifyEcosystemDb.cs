using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class identifyEcosystemDb : MonoBehaviour
{
    public HorizontalSnap hs;
    public Sprite[] ecosystemSprites;
    public void Start()
    {
        hs.createHS(ecosystemSprites);
        Database.currentSprites = ecosystemSprites;
    }
}

