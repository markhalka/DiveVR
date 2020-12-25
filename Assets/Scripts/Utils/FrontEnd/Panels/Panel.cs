using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel 
{

    public Vector3 start;
    public Vector3 end;

    public Panel(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    public Panel()
    {
        start = new Vector3(0, -400, 0);
        end = new Vector3(0, 0, 0);
    }

    public IEnumerator panelAniamtion(bool open, Transform currPanel)
    {
        float count = 0;

        if (open)
        {
            currPanel.gameObject.SetActive(true);
        }


        while (count <= 1)
        {
            count += 0.1f;
            if (open)
            {
                currPanel.transform.localPosition = Vector3.Lerp(start, end, count);
            }
            else
            {
                currPanel.transform.localPosition = Vector3.Lerp(end, start, count);
            }


            yield return new WaitForSeconds(0.02f);
        }

        if (!open)
        {
            currPanel.gameObject.SetActive(false);
        }
    }

    public void showPanel()
    {

    }
}
