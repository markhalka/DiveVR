using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// for all this isQuiz shit, you should use information.isquiz....


// you should add and test the videos too ..



public class HorizontalModel : MonoBehaviour
{

    public GameObject currentShowPanel;
    public GameObject modelAnimationGb;
    public GameObject horizontalSnap;
    public GameObject page1;

    public AudioSource source;
    public AudioClip swipe;

    public InformationPanel infoPanel;

    public ModelAnimations modelAnimation;

    public bool shouldRotate = false;
    bool firstPage = true;

    public void Start()
    {
        modelAnimation = modelAnimationGb.GetComponent<ModelAnimations>();
        var scrollSnap = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>();
        scrollSnap.enabled = true;
        scrollSnap.OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
    }

    int currentPage = 0;
    void pageChanged()
    {
        source.clip = swipe;
        source.Play();

        currentPage = horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

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
           
            infoPanel.locationPanel.setPosition(LocationPanel.MenuPosition.RIGHT);
            infoPanel.showPanel(currentPage);
         //   infoPanel.loadNewModel(); ; //FIX THAT
            StartCoroutine(moveObject(false));
            
            Information.currentBox = null;
        }
    }



    public GameObject outlinePanel;

    GameObject currentMoveObject;
    public Vector3 pastPosition;
    public Vector3 lerpPosition = new Vector3(-350, 0, 300);
    IEnumerator moveObject(bool moveBack)
    {
        if (!moveBack)
        {
            outlinePanel.SetActive(true);
            currentMoveObject = Information.currentBox;
        } else
        {
            outlinePanel.SetActive(false);
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

    void checkClosePanel()
    {
        if(outlinePanel.activeSelf && !infoPanel.panelContainer.activeSelf)
        {
            StartCoroutine(moveObject(true));
        }
    }


    public void Update()
    {
        if(firstPage && !currentShowPanel.activeSelf)
        {
            firstPage = false;
            pageChanged();
        }
        handleSwipeUpdate();
        checkClosePanel();
    }
    
}
