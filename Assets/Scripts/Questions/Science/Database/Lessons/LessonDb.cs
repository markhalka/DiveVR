using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LessonDb : MonoBehaviour
{
    public GameObject vsChild;
    public GameObject verticalScroll;
    public GameObject panelGb;

    public Panel panel;

    public Sprite[] currentSprites;

    public string[] currentNames;

    public int startOffset;


    public LessonDb(int startOffset, Sprite[] currentSprites)
    {
        this.startOffset = startOffset;
        this.currentSprites = currentSprites;
    }

    public virtual void initLadder()
    {

    }

    public virtual void update()
    {

    }
}

