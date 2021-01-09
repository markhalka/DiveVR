using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalModel : MonoBehaviour
{
    void pageChanged()
    {
        Debug.LogError("page changed " + shouldRotate);
        source.clip = swipe;
        source.Play();

        int currentPage = horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        currentObject = horizontalSnap.transform.GetChild(0).GetChild(currentPage).GetChild(0).gameObject;

        if (!isQuiz)
        {
            simple.text = Information.userModels[currentPage + modelOffset].simpleInfo[0]; //-1, because the first page is just a filler value?
        }

        if (finishStart > 1)
        {
            didSwipe = true;
        }

        if (shouldRotate)
        {
            Debug.LogError("adding rotatyion animation");
            addRotationAnimation();
        }
    }


    void handleSwipeUpdate()
    {
        if (!isQuiz && !currInformationPanel.activeSelf && Information.currentBox != null)
        {
            didSwipe = true;
            clickCount = neededClicks + 1;

            showPanel();
            Information.currentBox = null;
        }
    }

    public GameObject horizontalSnap;

    GameObject currentModel;
    public GameObject particleSystem;
    public GameObject page1;

    List<GameObject> cursorInit;
    void initModel()
    {
        Debug.LogError("created new animations");
        animations = new Animations();

        ParseData.parseModel();

        setModelIndex();
        background.GetComponent<Image>().sprite = backgroundDefualt;//backgrounds[currentModelIndex];
        currentModel = models.transform.GetChild(currentModelIndex).gameObject;
        currentModel.SetActive(true);


        cursorInit = new List<GameObject>();
        if (isHorizontalSnap)
        {


            horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
            showIndecies = new bool[currentModel.transform.GetChild(0).childCount];
            int index = 0;
            List<GameObject> toAdd = new List<GameObject>();
            for (int i = 0; i < showIndecies.Length; i++)
            {
                toAdd.Add(currentModel.transform.GetChild(0).GetChild(i).gameObject);
                showIndecies[i] = true;
            }
            if (Information.pretestScore == -1)
            {
                foreach (var child in toAdd)
                {
                    GameObject newPage = Instantiate(page1, page1.transform, false);
                    newPage.transform.SetParent(newPage.transform.parent.parent);
                    child.transform.SetParent(newPage.transform, true);

                    horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().AddChild(newPage, true);
                    cursorInit.Add(child);

                }
            }
            else
            {
                Transform curr = horizontalSnap.transform.GetChild(0);
                for (int i = 0; i < curr.childCount; i++)
                {
                    cursorInit.Add(curr.GetChild(i).GetChild(0).gameObject);
                }
            }

            currentObject = horizontalSnap.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            if (shouldRotate)
            {
                addRotationAnimation();
            }
            simple.text = Information.userModels[modelOffset].simpleInfo[0];
        }
        else
        {
            showIndecies = new bool[currentModel.transform.GetChild(0).childCount];
            for (int i = 0; i < currentModel.transform.GetChild(0).childCount; i++)
            {
                GameObject currentObject = currentModel.transform.GetChild(0).GetChild(i).gameObject;
                cursorInit.Add(currentObject);
                showIndecies[i] = true;
            }
        }

        initDots(currentModel.transform.GetChild(0).gameObject);
        cursorInit.Add(speechBubble);

        Information.updateEntities = cursorInit.ToArray();

        entities = cursorInit.ToArray();
        animations.init(entities);

        labels = new GameObject[0];

        hideAllLables();
    }

    public void Update()
    {
        //probably make this static
        if (!didUpdateShowPanel)
        {
            didUpdateShowPanel = true;
            //  currInformationPanel.transform.parent.GetComponent<InformationPanel>().setCenter(false); 
        }

        handleSwipeUpdate();
    }
}
