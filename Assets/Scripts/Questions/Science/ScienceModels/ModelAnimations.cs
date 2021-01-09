using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimations : MonoBehaviour
{
    public Vector3 lerpPosition = new Vector3(-350, 0, 300);
    public Vector3 pastPosition;

    //  public GameObject ToMoveObject;

    public GameObject outlinePanel;
    public GameObject currentMoveObjet; //you already have this stored in a different object, but im too lazy to check the name 
    GameObject currentMoveObject;


    Vector3 rotationVector = new Vector3(0, 3, 0);
    Vector3 pastRotationVector = new Vector3(-1, -1, -1);
    bool firstRotate = true;
    public IEnumerator rotate(GameObject currentObject)
    {
        if (!firstRotate)
        {
            yield break;
        }
        while (currentObject != null)
        {

            if (firstRotate)
            {
                pastRotationVector = currentObject.transform.rotation.eulerAngles; //local
                firstRotate = false;
            }

            currentObject.transform.Rotate(rotationVector, Space.Self); //self
            yield return new WaitForSeconds(0.01f);
        }


        int count = 0;
        while (count < 100)
        {
            count++;
            if (Mathf.Abs(pastCurr.transform.rotation.eulerAngles.y - pastRotationVector.y) % 360 == 0)
            {
                break;
            }
            else
            {
                pastCurr.transform.Rotate(rotationVector);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }


    public IEnumerator moveObject(bool moveBack)
    {
        if (!moveBack)
        {
            currentMoveObject = Information.currentBox;
        }

        float count = 0;
        Vector3 start, end;
        if (moveBack)
        {
            start = lerpPosition;
            end = pastPosition;
        }
        else
        {
            pastPosition = currentMoveObject.transform.localPosition;
            start = pastPosition;
            end = lerpPosition;
        }
        while (count <= 1.1)
        {
            count += 0.1f;
            currentMoveObject.transform.localPosition = Vector3.Lerp(start, end, count);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
