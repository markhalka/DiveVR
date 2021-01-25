using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitManager : MonoBehaviour
{
    // Start is called before the first frame update

    //ok, this will handle the main circuit shit

    //steps:
    //1. add the draging and dropping code to add and rotate resistors (just copy from physics)
    //2. add clicking code on end of nodes, to connect nodes in game
    //3. add code from download to add current simulation, and load simulation (add a light bulb, that is both on, and off)
    //4. add more explanations and quiz to file 
    //5. done 

    //extra shit (later);
    //let them save and load circuits 
    //have a few example circuits (maybe to teach them more advanced topics, and series/parallel circuits)
    //have more loads, and make the simulation better

    //ok right now todo:


    //here, test the following



    //4. add the rest of the things you need for the scene 
    //-

    class CircuitPanel
    {
        public string name;
        public int index;
        public bool shown;

        public CircuitPanel(string n, int i)
        {
            name = n;
            index = i;
        }
    }

    List<CircuitPanel> panels;

    MouseClickPos mousePos;
    public GameObject mousePosGb;
    void Start()
    {

        initObjectButtons();
        Information.isSelect = true;

        ParseData.startXML();
        ParseData.parseModel();
        Information.panelIndex = -1;
        initPanels();
        initStartPanels();

        mousePos = mousePosGb.GetComponent<MouseClickPos>();


    }

    void initPanels()
    {
        panels = new List<CircuitPanel>();

        panels.Add(new CircuitPanel("resistor", 1));
        panels.Add(new CircuitPanel("ground", 2));
        //  panels.Add(new CircuitPanel("resistor", 1));
        // panels.Add(new CircuitPanel("resistor", 1));
        //  panels.Add(new CircuitPanel("resistor", 1));
        //   panels.Add(new CircuitPanel("resistor", 1));

        //ok, so that should handle it..
    }




    List<Button> tempButtons;
    List<GameObject> userObjects;

    void initObjectButtons()
    {
        //here init userobjects
        userObjects = new List<GameObject>();
        tempButtons = new List<Button>();

        foreach (var button in buttonContainer.GetComponentsInChildren<Button>())
        {
            Debug.Log("added listener");
            tempButtons.Add(button);

            button.onClick.AddListener(delegate { handleNewObject(button); });
        }

        //delegate each of the buttons to a certain object

    }

    public GameObject currentObject;
    public GameObject spritecontainer; //this is where all the sprites are 
    public GameObject buttonContainer; //this is where all the buttons are 

    void handleNewObject(Button sender)
    {

        //change information.currobject to let the user drag and drop it 
        //instantiate a new gameobject for that 
        int idx = tempButtons.IndexOf(sender);
        Debug.LogError(idx + " button index");
        //    currentObject = new Object(objectTypes[idx]);
        GameObject temp = Instantiate(spritecontainer.transform.GetChild(idx).gameObject, spritecontainer.transform.GetChild(idx), true);

        temp.transform.SetParent(temp.transform.parent.parent);
        temp.SetActive(true);

        currentObject = temp;



    }

    bool wasMouseUp = false;
    bool shouldRotate = false;
    int minX = 0;


    // Update is called once per frame
    void Update()
    {
        handleStartPanels();
        if (Input.GetMouseButtonUp(0))
        {
            wasMouseUp = true;
        }
        if (currentObject != null && !shouldRotate)
        {

            Debug.LogError("not null");
            currentObject.transform.localPosition = new Vector3(mousePos.position.x, mousePos.position.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                if (currentObject.transform.localPosition.x > minX)
                {
                    //maybe change this a bit so that when you go over the line it shows a different sprite (just a red x )
                    Debug.Log("not passed thresh line");
                }
                else
                {
                    //add it to user objects
                    Debug.Log("added to current objects ");
                    userObjects.Add(currentObject);

                }
                shouldRotate = true;
                wasMouseUp = false;
                // pivot = orb.transform;
                //   transform.parent = pivot;
                //  transform.position += Vector3.up * radius;
                //  currentObject = null;
            }
        }
        else if (currentObject != null && shouldRotate && wasMouseUp)
        {
            Vector3 orbVector = Camera.main.WorldToScreenPoint(currentObject.gameObject.transform.position);
            orbVector = Input.mousePosition - orbVector;
            float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;

            // pivot.position = orb.position;
            currentObject.gameObject.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (Input.GetMouseButton(0))
            {
                Debug.Log("setting it to null");
                handleDropPanel(currentObject.name);
                shouldRotate = false;
                currentObject = null;
            }
        }
    }



    public void handleDropPanel(string name)
    {
        foreach (var p in panels)
        {
            Debug.LogError(p.name + " " + name);
            if (name.Contains(p.name))
            {
                if (!p.shown)
                {
                    Information.panelIndex = p.index;
                    showPanel();
                    p.shown = true;
                }
            }
        }
    }

    List<int> startPanels;
    int modelOffset = 0;
    void initStartPanels()
    {
        Debug.LogError("here at init start");
        startPanels = new List<int>();
        modelOffset = 0;
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == 1)
            {
                Debug.LogError("added " + i);
                modelOffset++;
                startPanels.Add(i);
            }
        }
    }


    public GameObject currInformationPanel;

    int finishStart = 0;

    void handleStartPanels()
    {
        if (!currInformationPanel.activeSelf && finishStart == 0)
        {
            if (startPanels.Count > 0)
            {
                if (startPanels[startPanels.Count - 1] > Information.panelIndex) //then not all the start panels have been shown
                {
                    for (int i = 0; i < startPanels.Count; i++)
                    {
                        if (startPanels[i] > Information.panelIndex)
                        {
                            Information.panelIndex = startPanels[i];
                            Information.lableIndex = 0;
                            //    simple.text = Information.userModels[startPanels[i]].simpleInfo[0];
                            showPanel();
                            if (i == startPanels.Count - 1)
                            {
                                finishStart = 1;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    void showPanel()
    {
        Debug.LogError("showing panel");
        currInformationPanel.SetActive(true);

    }

}
