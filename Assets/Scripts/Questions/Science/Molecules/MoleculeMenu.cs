using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeMenu : MonoBehaviour
{


    public GameObject choosePanel;
    public GameObject chooseSecondPanel;
    public Button back;

    private void Start()
    {
        back.onClick.AddListener(delegate { takeBack(); });
    }


    void takeBack()
    {
        isReacting = false;
        reactionIndex = -1;
        if (chooseSecondPanel.activeSelf)
        {
            chooseSecondPanel.gameObject.SetActive(false);
            choosePanel.gameObject.SetActive(true);
            newEntities = new List<GameObject>();
            for (int i = 0; i < choosePanel.transform.childCount; i++)
            {
                newEntities.Add(choosePanel.transform.GetChild(i).gameObject);

            }
            Information.updateEntities = newEntities.ToArray();

        }
        else if (choosePanel.activeSelf)
        {
            SceneManager.LoadScene("ModuleMenu");
        }
        else
        {
            if (Information.nextScene == 2) //thermal
            {
                SceneManager.LoadScene("ModuleMenu");
            }
            else if (Information.nextScene == 4) //mixtures
            {
                if (choosePanelIndex == 2)
                {
                    chooseSecondPanel.gameObject.SetActive(true);
                }
                else
                {
                    choosePanel.gameObject.SetActive(true);
                }

            }
            else
            {
                chooseSecondPanel.gameObject.SetActive(true);
            }
        }
    }

    //ok, so just double check the index???
    int choosePanelIndex = 0;
    void callSubLab(int index)
    {
        choosePanel.gameObject.SetActive(false);
        resetAll();
        outputText.gameObject.SetActive(false);
        choosePanelIndex = index;
        switch (Information.nextScene)
        {
            case 4: //mixtures 
                showMixturePanel(index);
                switch (index)
                {
                    case 0: //mixtures
                        SolutionSubLab();
                        Debug.Log("solutions sub lab");
                        break;
                    case 1: //suspension
                        Debug.Log("suspension sub lab");
                        SuspensionSubLab();
                        break;
                    case 2: //alloy
                        createSecondChoosePanel(index);
                        break;
                    case 3: //colloid
                        ColloidsSubLab();
                        break;
                }
                break;
            case 26: //reactions
                createSecondChoosePanel(index);
                break;
        }
    }
    List<GameObject> newEntities;
    void createSecondChoosePanel(int index)
    {
        resetAll();
        newEntities = new List<GameObject>();
        Sprite[] currentSprite = new Sprite[0];
        List<string> currentText = new List<string>();
        switch (Information.nextScene)
        {
            //mixtures
            case 4:
                currentSprite = alloySprites;
                break;
            //reactions
            case 26:
                currentSprite = new Sprite[3];
                currentSprite[0] = reactionInnerSprites[index * 3];
                currentSprite[1] = reactionInnerSprites[index * 3 + 1];
                currentSprite[2] = reactionInnerSprites[index * 3 + 2];

                currentText.Add(reactions[index * 3].name);
                currentText.Add(reactions[index * 3 + 1].name);
                currentText.Add(reactions[index * 3 + 2].name);
                break;
        }
        for (int i = 0; i < currentSprite.Length; i++)
        {
            chooseSecondPanel.transform.GetChild(i).GetComponent<Image>().sprite = currentSprite[i];
            if (i < currentText.Count)
                chooseSecondPanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = currentText[i];

            newEntities.Add(chooseSecondPanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();
        chooseSecondPanel.gameObject.SetActive(true);
    }


    void callSecondSubLab(int index)
    {
        chooseSecondPanel.gameObject.SetActive(false);
        switch (Information.nextScene)
        {
            case 4: //mixutres
                AlloySubLab(index);
                break;
            case 26: //reactions
                reactionIndex = choosePanelIndex * 3 + index;
                Reaction currReaction = reactions[reactionIndex];
                showReactionPanel(currReaction.name);
                break;
        }

    }

    float value = 0;
    bool started = false;

    int getIndex()
    {
        for (int i = 0; i < newEntities.Count; i++)
        {
            if (Information.currentBox.transform == newEntities[i].transform)
            {
                return i;
            }
        }
        return -1;
    }


    private void Update()
    {
        if (Information.currentBox != null)
        {
            int index = getIndex();
            if (index < 0)
            {
                Information.currentBox = null;
                return;
            }
            if (choosePanel.activeSelf)
            {
                callSubLab(index);
            }
            else if (chooseSecondPanel.activeSelf)
            {
                callSecondSubLab(index);
            }
            Information.currentBox = null;
        }
    }
}

