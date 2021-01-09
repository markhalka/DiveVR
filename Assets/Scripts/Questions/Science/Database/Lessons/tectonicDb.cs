using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tectonicDb : MonoBehaviour
{
    public HorizontalSnap hs;
    public Sprite[] tectonicSprites;

    public void Start()
    {
        Debug.LogError("creating hs...");
        hs.createHS(tectonicSprites);
        Database.currentSprites = tectonicSprites; 
    }

}
