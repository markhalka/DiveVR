using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalSnap : MonoBehaviour
{

    //figure out why everything is in the same location
    //pass the information panel, then on-click just show it
    //in the start method make sure you can see the title (just set it to true)


    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;

    public InformationPanel informationPanel;
    

//    public TMP_Text title;
    Sprite[] currentSprites; // MAKE SURE YOU INIT THIS 
    public GameObject page1;
   // int startOffset;

    public void Start()
    {
        informationPanel.showTitle(0); //?
    }
    public void createHS(Sprite[] sprites)
    {
        currentSprites = sprites;

        for (int i = 0; i < currentSprites.Length; i++)
        {
            GameObject newPage = Instantiate(page1, page1.transform, false);
            newPage.transform.SetParent(transform.GetChild(0));
            newPage.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];
            newPage.gameObject.SetActive(true);
        }

        var scrollSnap = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>();
        scrollSnap.enabled = true;
        scrollSnap.OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
        pageChanged();
    }

    void pageChanged()
    {
        Debug.LogError("page changed");
        source.clip = swipe;
        source.Play();

        int currentPage = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        informationPanel.showTitle(currentPage);
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

    // here you should make the current panel true, and that should work
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

      //  Information.panelIndex = currentIndex + startOffset;
        //   panel.showPanel();  
        informationPanel.showPanel(currentIndex);
    }

    bool setPanel = false;
    public void Update()
    {
        if (!setPanel)
        {
            informationPanel.locationPanel.setPosition(LocationPanel.MenuPosition.CENTER); //maybe?? //that should work 
            pageChanged();
            setPanel = true;
        }
        onHorizontalClick();
    }
}
