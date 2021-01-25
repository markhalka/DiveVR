using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// for all this isQuiz shit, you should use information.isquiz....

public class HorizontalModel : MonoBehaviour
{
    public AudioSource source;
    public AudioClip swipe;
    public InformationPanel infoPanel;
    public GameObject currentShowPanel;
    public GameObject modelAnimationGb;
    public ModelAnimations modelAnimation;
    public bool shouldRotate = false;

    public void Start()
    {
        modelAnimation = modelAnimationGb.GetComponent<ModelAnimations>();
        var scrollSnap = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>();
        scrollSnap.enabled = true;
        scrollSnap.OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
    }

    void pageChanged()
    {
        source.clip = swipe;
        source.Play();

        int currentPage = horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        ScienceModels.currentObject = horizontalSnap.transform.GetChild(0).GetChild(currentPage).GetChild(0).gameObject;

        Information.panelIndex = currentPage;

        if (Information.isQuiz == 0)
        {
           // simple.text = Information.userModels[currentPage + modelOffset].simpleInfo[0]; //here you can just call show title or something
            infoPanel.showTitle(currentPage);
        }


        if (shouldRotate)
        {
            StartCoroutine(modelAnimation.rotate()); // make this static
        }
    }

    void handleSwipeUpdate()
    {
        

        if (Information.isQuiz == 0 && !currentShowPanel.activeSelf && Information.currentBox != null)
        {
            infoPanel.loadNewModel(); ; //FIX THAT
            Information.currentBox = null;
        }
    } 

    public GameObject horizontalSnap;

    public GameObject page1;

    bool firstPage = true;
    public void Update()
    {
        if(firstPage && !currentShowPanel.activeSelf)
        {
            firstPage = false;
            pageChanged();
        }
        handleSwipeUpdate();
    }
    
}
