using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalSnap : MonoBehaviour
{

    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;
    public GameObject instructionsGb;
    public TMP_Text test;
    Sprite[] currentSprites; // MAKE SURE YOU INIT THIS 
    public GameObject page1;
    bool didSwipe = false;
    int startOffset;

    public GameObject panelGb;
    public Panel panel;


    public void Start()
    {
        // find a way to init Startoffset
        panel = panelGb.GetComponent<Panel>();
    }
    void createHS()
    {
        GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
        for (int i = 0; i < currentSprites.Length; i++)
        {
            GameObject newPage = Instantiate(page1, page1.transform, false);
            newPage.transform.SetParent(transform.GetChild(0));
            newPage.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];
            newPage.gameObject.SetActive(true);
        }

        pageChanged();
    }

    void pageChanged()
    {
        source.clip = swipe;
        source.Play();

        int currentPage = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        if (currentPage > 0)
        {
            Debug.LogError("did swipe...");
            didSwipe = true;
        }

        test.text = Information.userModels[currentPage + startOffset].simpleInfo[0];
    }

    void onHorizontalClick()
    {
        if (Information.currentBox != null)
        {
            source.clip = click;
            source.Play();
            showHSPanel();
            Information.currentBox = null;
        }
    }

    void showHSPanel()
    {
        int currentIndex = -1;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).GetChild(0).gameObject == Information.currentBox)
            {
                currentIndex = i;
                break;
            }
        }
        if (currentIndex == -1)
        {
            Debug.LogError("could not find the index for: " + Information.currentBox.name);
            return;
        }

        // fancyAnimation();

        Information.panelIndex = currentIndex + startOffset;
        panel.showPanel();
    }

    bool setPanel = false;
    public void Update()
    {
        if (!setPanel)
        {
            //   panel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true); //maybe?? //that should work 
            pageChanged();
        }
        onHorizontalClick();
    }
}
