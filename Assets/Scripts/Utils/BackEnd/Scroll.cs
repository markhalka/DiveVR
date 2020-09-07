using UnityEngine;
using UnityEngine.UI;


public class Scroll
{
    public Button currentButton;
    public GameObject[] arrows;
    public bool lockUp = true;

    public Animations animations;
    GameObject[] buttons;


    public Scroll(Button button)
    {
        currentButton = button;
        animations = new Animations();
    }

    public Scroll()
    {
        animations = new Animations();
    }



    public void setButtons(GameObject[] curr, int[] animationIndecies, int index)
    {
        buttons = curr;
        animations.init(buttons);

        currentButton = buttons[0].GetComponent<Button>();
        currentButton.Select();
        int newIndex = index;

        //   setAnimations(newIndex, animationIndecies);
    }

    void setAnimations(int newIndex, int[] animationIndecies)
    {
        int max = animationIndecies.Length;

        for (int i = newIndex + 1; i < newIndex + 4 && i < max; i++)
        {
            animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, newIndex + 4 - i - 1));

        }

        for (int i = newIndex - 1; i > newIndex - 4 && i >= 0; i--)
        {
            animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, i - newIndex + 4 - 1));
        }
        if (newIndex > 0 && newIndex < animationIndecies.Length)
        {
            animations.addAnimation(animationIndecies[newIndex], length, false, Mathf.Pow(growthRate, 3));
        }
    }


    //you might need to fix position here as well (try setting them to be 0,0 to get rid of a level of caluclation
    public void checkButton()
    {
        /*  animations.updateAnimations();
          if (Information.controller.checkClick() == 1)
          {
              if (lockUp)
              {
                  currentButton.onClick.Invoke();
                  lockUp = false;
                  Information.controller.initUDPOffset();
                  Information.controller.axisLock = new Vector3(1, 0, 0);
                  Information.controller.cursor.transform.localPosition = new Vector3(Information.controller.cursor.transform.localPosition.x, getSubHeaderY(currentButton.gameObject), Information.controller.cursor.transform.localPosition.z);

                  currentButton = currentButton.transform.GetChild(1).GetChild(0).GetComponent<Button>();
                  currentButton.Select();

              }
              else
              {
                  currentButton.onClick.Invoke();
                  lockUp = true;
                  Information.controller.initUDPOffset();
                  Information.controller.axisLock = new Vector3(0, 1, 0);

                  Information.controller.cursor.transform.localPosition = new Vector3(0, Information.controller.cursor.transform.localPosition.y, Information.controller.cursor.transform.localPosition.z);// new Vector3(currentButton.transform.localPosition.x, Information.controller.cursor.transform.localPosition.y, Information.controller.cursor.transform.localPosition.z);
              }
          }*/
    }

    float getSubHeaderY(GameObject curr) //this is the header 
    {

        return curr.transform.parent.localPosition.y - 15 + curr.transform.parent.parent.transform.localPosition.y;
    }

    public void moveSubButtons(Transform parent)
    {

        if (Information.isVrMode)
        {
            moveSubButtonsController(parent);
        }
        else
        {
            /*
            #if UNITY_IPHONE || UNITY_ANDROID
                        moveSubButtonsFinger(parent);
            #else 
                     moveSubButtonsScroll(parent);
            #endif*/
            moveSubButtonsScroll(parent);

        }

    }




    float moveAmount = 1;
    float offset = 10;
    public float sideButtonOffset = 100;

    Vector2 offsetVector = new Vector2(100, 0);
    public void moveSubButtonsController(Transform parent)
    {

        int direction = 0;
        for (int i = 0; i < 2; i++)
        {

            /*    if (Information.controller.cursor.transform.localPosition[i] + offsetVector[i] >= Information.upperBoundary.transform.localPosition[i])//Information.controller.maxBoundery[i])
                {
                    direction = -1;


                }
                else if (Information.controller.cursor.transform.localPosition[i] -offsetVector[i] <= Information.lowerBoundary.transform.localPosition[i])//Information.controller.minBoundery[i])
                {
                    direction = 1;


                }*/
            Vector3 newOffset;

            if (lockUp)
            {

                moveAmount = 3;
                if (direction > 0)
                {

                    if (parent.GetChild(parent.childCount - 1).transform.position.y > Information.lowerBoundary.transform.position.y + offset)
                    {
                        direction = 0;
                    }
                }
                else if (direction < 0)
                {
                    if (parent.GetChild(0).transform.position.y < Information.upperBoundary.transform.position.y - offset)
                    {
                        direction = 0;
                    }
                }
                newOffset = new Vector3(0, moveAmount * direction);
                parent.transform.localPosition += newOffset;

            }
            else
            {

                moveAmount = 2;

                if (direction > 0)
                {
                    if (parent.GetChild(parent.childCount - 1).transform.position.x > Information.upperBoundary.transform.position.x + sideButtonOffset)//Information.controller.minBoundery.y + boundaryOffset)
                    {
                        direction = 0;
                    }
                }
                else if (direction < 0)
                {
                    if (parent.GetChild(0).transform.position.x < Information.lowerBoundary.transform.position.x - sideButtonOffset)
                    {
                        direction = 0;
                    }
                }
                newOffset = new Vector3(moveAmount * direction, 0);

                for (int j = 0; j < parent.childCount; j++)
                {
                    parent.GetChild(j).transform.localPosition += newOffset;
                }
            }


        }
    }

    //here, you can also check to see if the cursor itself is close to the edge 

    public void moveSubButtonsScroll(Transform parent)
    {
        if (parent.childCount <= 0)
        {
            return;
        }

        int direction = 0;
        Vector3 newOffset;
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        moveAmount = 10; //was 1
        if (scrollValue > 0)
        {
            direction = -1;
        }
        else if (scrollValue < 0)
        {
            direction = 1;
        }
        else //the value is zero, so check the position of the cursor 
        {

            var cursorPos = Input.mousePosition;//Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            moveAmount = 5; //move it less
            if (lockUp)
            {
                if (cursorPos.y > Camera.main.pixelHeight * 0.8)
                {
                    direction = -1;
                }
                else if (cursorPos.y < Camera.main.pixelHeight * 0.2)
                {
                    direction = 1;
                }
            }
            else
            {
                if (cursorPos.x > Camera.main.pixelWidth * 0.8)
                {
                    direction = -1;
                }
                else if (cursorPos.x < Camera.main.pixelWidth * 0.2)
                {
                    direction = 1;
                }
            }


            /*        if (lockUp)
       {

           newOffset = new Vector3(0, moveAmount * direction);
           parent.transform.localPosition += newOffset;
       }
       else
       {
           newOffset = new Vector3(moveAmount * direction, 0);

           for (int j = 0; j < parent.childCount; j++)
           {
               parent.GetChild(j).transform.localPosition += newOffset;
           }

       }

            */
        }
        if (lockUp)
        {
            // Debug.Log(parent.GetChild(parent.childCount - 1).transform.position.y + " " + (Information.lowerBoundary.transform.position.y + offset) + " " + parent.GetChild(0).transform.position.y + " " + (Information.upperBoundary.transform.position.y - offset));
            int i = 0;
            for (i = parent.childCount - 1; i >= 0; i--)
            {
                if (parent.GetChild(i).gameObject.activeSelf)
                    break;
            }


            if (direction > 0)
            {

                if (parent.GetChild(i).transform.position.y > Information.lowerBoundary.transform.position.y + offset)
                {
                    //     direction = 0;
                }
            }
            else if (direction < 0)
            {
                if (parent.GetChild(0).transform.position.y < Information.upperBoundary.transform.position.y - offset)
                {
                    //      direction = 0;
                }
            }
            newOffset = new Vector3(0, moveAmount * direction);
            parent.transform.localPosition += newOffset;

        }
        else
        {
            //     Debug.Log(parent.GetChild(parent.childCount - 1).transform.position.x + " " + (Information.upperBoundary.transform.position.x + sideButtonOffset) + " " + parent.GetChild(0).transform.position.x + " " + (Information.lowerBoundary.transform.position.x + sideButtonOffset));
            if (direction > 0)
            {
                if (parent.GetChild(0).transform.position.x > Information.lowerBoundary.transform.position.x + sideButtonOffset)
                {
                    //  Debug.Log(parent.GetChild(0).transform.position.x + " " + (Information.lowerBoundary.transform.position.x + sideButtonOffset));
                    //          direction = 0;
                }
            }
            else if (direction < 0)
            {
                if (parent.GetChild(parent.childCount - 1).transform.position.x < Information.upperBoundary.transform.position.x - sideButtonOffset)
                {
                    //            direction = 0;
                }
            }
            /*   if (direction > 0)
               {
                   if (parent.GetChild(parent.childCount - 1).transform.position.x > Information.upperBoundary.transform.position.x + sideButtonOffset)//Information.controller.minBoundery.y + boundaryOffset)
                   {

                       direction = 0;
                   }
               }
               else if (direction < 0)
               {
                   if (parent.GetChild(0).transform.position.x < Information.lowerBoundary.transform.position.x - sideButtonOffset)
                   {
                       direction = 0;
                   }
               }*/
            newOffset = new Vector3(moveAmount * direction, 0);

            for (int j = 0; j < parent.childCount; j++)
            {
                parent.GetChild(j).transform.position += newOffset;
            }
        }
        showArrows(direction, parent);

    }
    //call this function at the end(after direction check)
    void showArrows(int direction, Transform parent)
    {
        //  Debug.Log(parent.GetChild(parent.childCount - 1).transform.position.x + " " + (Information.upperBoundary.transform.position.x + sideButtonOffset) + " " + parent.GetChild(0).transform.position.x + " " + (Information.lowerBoundary.transform.position.x - sideButtonOffset));

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);

        }
        if (lockUp)
        {
            if (parent.GetChild(parent.childCount - 1).transform.position.y < Information.lowerBoundary.transform.position.y + offset)
            {
                //it is possible to move up
                arrows[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);

            }
            if (parent.GetChild(0).transform.position.y > Information.upperBoundary.transform.position.y - offset)
            {
                //it is possible to move down
                arrows[1].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);


            }
            if (direction < 0)
            {
                //they are moving up
                arrows[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);

            }
            else if (direction > 0)
            {
                //they are moving down
                arrows[1].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);

            }
        }
        else
        {
            if (parent.GetChild(0).transform.position.x <= Information.lowerBoundary.transform.position.x + sideButtonOffset)// if (parent.GetChild(parent.childCount - 1).transform.position.x < Information.upperBoundary.transform.position.x - sideButtonOffset)
            {
                arrows[2].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            }
            if (parent.GetChild(parent.childCount - 1).transform.position.x >= Information.upperBoundary.transform.position.x - sideButtonOffset)// if(parent.GetChild(0).transform.position.x > Information.lowerBoundary.transform.position.x - sideButtonOffset)
            {
                arrows[3].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);

            }
            if (direction < 0)
            {
                //they are moving up
                arrows[3].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);

            }
            else if (direction > 0)
            {
                //they are moving down
                arrows[2].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);

            }
        }
    }




    private Vector2 startTouchPosition, endTouchPosition;


    Vector2 scrollPosition;
    Touch touch;

    public void moveSubButtonsFinger(Transform parent)
    {
        int direction = 0;
        Vector3 newOffset;
        moveAmount = 15; // or 15

        /*   if (Input.touchCount > 0)
           {
               touch = Input.touches[0];
               if (touch.phase == TouchPhase.Moved)
               {

                   if(touch.deltaPosition.y < 0)
                   {

                       direction = -1;
                   } else if(touch.deltaPosition.y > 0)
                   {

                       direction = 1;
                   }
               }
           }




           if (lockUp)
           {

               newOffset = new Vector3(0, moveAmount * direction);
               parent.transform.localPosition += newOffset;

           }
           else
           {
               newOffset = new Vector3(moveAmount * direction, 0);

               for (int j = 0; j < parent.childCount; j++)
               {
                   parent.GetChild(j).transform.localPosition += newOffset;
               }


           }*/
        if (Input.touchCount > 0)
        {
            touch = Input.touches[0];
            if (touch.phase == TouchPhase.Moved)
            {
                parent.transform.localPosition += new Vector3(0, touch.deltaPosition.y); //ok, and just make the middle one selected
            }
        }

    }


    double sensitivity = 0.1;
    int length = 3;
    float growthRate = 1.03f;
    int prevIndex = 0;

    /*   void resetAnimations(int[] indecies)
       {
           for(int i = 0; i < buttons.Length; i++)
           {
          //     animations.resetButtons(getAnimationIndex(buttons[i], indecies), buttons[i].transform.localScale.x);
              // Debug.Log("here");
             //  buttons[i].transform.localScale = new Vector3(1, 1, 1);
           }
       }

       */
    public void updateButton(int[] animationIndecies, GameObject curr)
    {
        if (!lockUp)
        {
            //   Debug.Log("was not locked up");
            return;
        }
        int max = animationIndecies.Length;
        int newIndex = getAnimationIndex(curr, animationIndecies);

        int direction = newIndex - prevIndex;

        //here check if one was skipped 
        /*    bool wasSkipped = false;
            if(Mathf.Abs(direction) > 1) //ok instead, if one was skipped, then just reset all of the buttons (change all their sizes, and apply the animation to the current selected button
            {
                wasSkipped = true;
                if(direction > 0)
                {
                    newIndex = newIndex - 1;
                } else
                {
                    newIndex = newIndex + 1;
                }
                direction = newIndex - prevIndex;
            }*/
        if (direction == 0)
        {

            return;
        }

        if (direction > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        //  Debug.Log(direction + " direction");

        if (lockUp)
        {

            for (int i = newIndex + 1; i < newIndex + 4 && i < max; i++)
            {
                animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, 3 + (newIndex - i)));
            }

            for (int i = newIndex - 1; i > newIndex - 4 && i >= 0; i--)
            {
                animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, (3 - (newIndex - i))));

            }
            if (newIndex >= 0 && newIndex < max)
                animations.addAnimation(animationIndecies[newIndex], length, false, (float)Mathf.Pow(growthRate, 3));
        }
        else
        {

            for (int i = prevIndex + 1; i < prevIndex + 4 && i < max; i++)
            {
                animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, direction));
            }

            for (int i = prevIndex - 1; i > prevIndex - 4 && i >= 0; i--)
            {
                animations.addAnimation(animationIndecies[i], length, false, (float)Mathf.Pow(growthRate, -direction));
            }

            animations.addAnimation(animationIndecies[prevIndex], length, false, (float)Mathf.Pow(growthRate, -1));

        }
        prevIndex = newIndex;
        for (int i = 0; i < animationIndecies.Length; i++)
        {
            animations.resetButtons(animationIndecies[i]);
        }
        /*   if (wasSkipped)
           {
               updateButton(animationIndecies, curr); //now you can call it again
           }*/
    }


    int getAnimationIndex(GameObject button, int[] indecies)
    {

        for (int i = 0; i < indecies.Length; i++)
        {

            if (button == (buttons[indecies[i]].gameObject))
            {
                return i;
            }
        }
        return -1;
    }

}
