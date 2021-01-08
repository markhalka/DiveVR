using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InstructionAnimation : MonoBehaviour
{

    public Image animationImage;
    public Sprite swipeUpSprite;
    public Sprite swipeLeftSprite;
    public Sprite clickSprite;

    public Button nextButton;

    public TMP_Text infoText;


    int animationStep = 0;
    public void Start()
    {
        animationStep = 0;
        nextButton.onClick.AddListener(delegate { next(); });
        StartCoroutine(swipeAnimation());
      
    }

    public IEnumerator swipeAnimation()
    {
        animationImage.gameObject.SetActive(true);
        animationImage.sprite = swipeUpSprite;

        infoText.text = "Swipe left and right, or up and down to see all the information!";

        int currCount = 0;
        int moveCount = 20;
        bool goingDown = false;
        int animationCount = 0;
        Vector3 translateVector = new Vector3(0, 0.9f, 0);
 /*       if (!swipeUp)
        {
            translateVector = new Vector3(0.9f, 0, 0);
            animationImage.sprite = swipeLeftSprite;
        } */


        while (animationStep == 0)
        {
            currCount++;
            if (currCount > moveCount)
            {
                currCount = 0;
                goingDown = !goingDown;
                animationCount++;
            }
            if (goingDown)
            {
                animationImage.transform.Translate(translateVector);
            }
            else
            {
                animationImage.transform.Translate(-translateVector);
            }

            yield return new WaitForSeconds(0.05f);
        }

      //  animationImage.gameObject.SetActive(false);
        StartCoroutine(clickAnimation());
    }

    public IEnumerator clickAnimation()
    {
        animationImage.sprite = clickSprite;

        infoText.text = "Click once or twice to see the information!";

        float currCount = 0;
        int moveCount = 10;
        bool goingDown = false;
        Vector3 growVector = new Vector3(1.3f, 1.3f, 1.3f);

        while (animationStep == 1)
        {

            currCount++;
            if (currCount > moveCount)
            {
                currCount = 0;
                goingDown = !goingDown;
            }
            if (goingDown)
            {
                animationImage.transform.localScale = animationImage.transform.localScale * 1.06f;
            }
            else
            {
                animationImage.transform.localScale = animationImage.transform.localScale * 1 / 1.06f;
            }

            yield return new WaitForSeconds(0.05f);
        }

        gameObject.SetActive(false);
        yield break;
    }

    public void next()
    {
        animationStep++;
    }
}

