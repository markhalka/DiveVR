using UnityEngine;

public class BarDive : MonoBehaviour
{

    //aight, time to fix this bitch


    Scroll scroll;
    void Start()
    {
        scroll = new Scroll();
        GameObject[] arrows = new GameObject[4];

        GameObject container = transform.parent.parent.GetChild(0).gameObject;
        Debug.Log(container.name + " container");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(container.transform.GetChild(i).gameObject.name + " added arrow");
            arrows[i] = container.transform.GetChild(i).gameObject;
        }
        scroll.arrows = arrows;
    }

    // Update is called once per frame
    void Update()
    {
        /*  if(transform.childCount > 0)
          {
              scroll.moveSubButtons(transform);
              checkShow();
          }
    */
    }
    float minY = -65;
    float maxY = 65;
    void checkShow()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform curr = transform.GetChild(i);
            //    Debug.Log(curr.position.y + " y pos " + i);

            if (curr.position.y < Information.lowerBoundary.transform.position.y)
            {
                //then go down

                curr.gameObject.SetActive(false);


            }
            else if (curr.position.y > Information.upperBoundary.transform.position.y)
            {
                //then go up
                curr.gameObject.SetActive(false);
            }
            else
            {
                curr.gameObject.SetActive(true);
            }

        }
    }
}
