using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animalLifeCycleDb : LessonDb
{
    int[] lifecycleIndecies = new int[] { 0, 3, 7, 10, 14 };

    public animalLifeCycleDb(int startOffset, Sprite[] currentSprites) : base(startOffset, currentSprites)
    {

    }

    public override void initLadder()
    {
        for (int i = 0; i < currentSprites.Length; i++)
        {
            GameObject currChild = Instantiate(vsChild, vsChild.transform, false);
            currChild.transform.SetParent(verticalScroll.transform.GetChild(0)); //i think that should work
            currChild.gameObject.SetActive(true);
            GameObject page = currChild.transform.GetChild(0).GetChild(0).gameObject;
            currChild.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];

            GameObject image = page.transform.GetChild(0).GetChild(0).gameObject;
            for (int j = lifecycleIndecies[i] + 1; j < lifecycleIndecies[i + 1]; j++)
            {

                GameObject currAnimal = Instantiate(image, image.transform, true);
                currAnimal.transform.SetParent(image.transform.parent.parent.GetChild(0));
                currAnimal.GetComponent<Image>().sprite = currentSprites[j];   //ok, that should work               
                currAnimal.gameObject.SetActive(true);

            }
        }
    }


    public override void update()
    {
        if (Information.currentBox != null)
        {
            for (int i = 0; i < verticalScroll.transform.GetChild(0).childCount; i++)
            {
                GameObject curr = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).gameObject;
                if (curr.gameObject.activeSelf)
                {
                    for (int j = 0; j < curr.transform.GetChild(0).childCount; j++)
                    {
                        if (Information.currentBox == curr.transform.GetChild(0).GetChild(j).gameObject)
                        {
                            animalPartClick(i - 1, j);
                            Information.currentBox = null;
                            return;
                        }
                    }
                }
                Transform image = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0);
                if (image.gameObject == Information.currentBox)
                {
                    image.GetChild(0).gameObject.SetActive(true);
                    Information.panelIndex = lifecycleIndecies[i - 1] + startOffset;
                    panel.showPanel();
                }
                else
                {
                    image.GetChild(0).gameObject.SetActive(false);
                }
            }

            Information.currentBox = null;
        }
    }


    void animalPartClick(int mainAnimal, int animalPart)
    {
        Information.panelIndex = lifecycleIndecies[mainAnimal] + animalPart + startOffset;
        panel.showPanel();
        return;

    }

}
