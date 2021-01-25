using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GoldRuberg : MonoBehaviour
{

    //ok, good shit
    //now just add the sprites 
    //then you can maybe try adding different balls (maybe not balls, maybe differnet color balls, and the green one is the main one, that will keep it easeier)

    //when the ball touches the rocket, make it go off
    //whem something lands on a teater totter, launch, than return to the previous state 

    //leave the pully for now, its annoying 

    //ok, so everything works here now
    //how to get the sprites working?

    //pully, find which side the ball lands on, then choose the right sprite, see if anything is in its hitbox and apply an upward force to it
    //just have 2 hitboxes 
    //have an ienumerator that returns it to its previosu sate after

    //for rocket, if anything is in the hitbox, have an ienumertor for the rocket launch

    //do this shit later, it is annoying collider stuff rippp

    //ok, so how to get this started:
    //so you don't really need to change too much, just change the names, than add the rocket

    //ok todo:
    //copy the other buttons, change the images
    //for the rocket, add a box collider, if it touches something else stop the fire animation, and just let it fall (just turn gravity on for it, or stop the force)
    //you don't need the ramp, just include the squares?? (because you can just make a ramp with the planks)
    //for the dominos, just add  a domino sprite
    //fix the info panel (when make sure what you put donw matches up)

    public Sprite[] rocketSprites;
    IEnumerator rocketLaunch(GameObject currRocket)
    {
        int count = 0;
        while (count < rocketSprites.Length)
        {
            count++;
            currRocket.GetComponent<Image>().sprite = rocketSprites[count];
        }

        yield break;
    }





    public enum energyTypes { thermal, kinetic, gravitional, elastic, mechanical, chemical };

    public class energyChange
    {
        public energyTypes start;
        public energyTypes end;
        public energyChange(energyTypes a, energyTypes b)
        {
            start = a;
            end = b;
        }
    }

    List<Object> userObjects;
    List<Object> objectTypes;
    Object ball;

    public Button start;

    public class Object
    {
        public energyTypes type;
        public GameObject gameObject;
        public bool added;
        public float kineticEnergy;
        public float potentialEnergy;
        public int spriteIndex;

        public Object()
        {
            added = false;
            kineticEnergy = 0;
            potentialEnergy = 0;
            spriteIndex = 0;

        }

        public Object(Object toCopy)
        {
            added = false;
            kineticEnergy = toCopy.kineticEnergy;
            potentialEnergy = toCopy.potentialEnergy;
            type = toCopy.type;
        }
        /*
         * <model int sprite index, int energy type kintetic energy, potential energy>
         * <position></position>
         * <rotation></rotation>
         * 
         * 
         */
        public XElement getFileInfo()
        {
            //this will return all the relevant file info 
            string position = gameObject.transform.position.ToString();
            string rotation = gameObject.transform.rotation.ToString();
            //get kinetic and potential energy values 
            //get the index of the energy type, or just make it a string
            //the last thing you need is the sprite 
            XElement currObject = new XElement("object");
            currObject.Add(new XAttribute("index", spriteIndex), new XAttribute("kEnergy", kineticEnergy), new XAttribute("pEnergy", potentialEnergy), new XElement("position", position),
                new XElement("rotation", rotation));

            return currObject;

        }
    }

    MouseClickPos mousePos;
    public GameObject mousePosGb;
    void Start()
    {
        mousePos = mousePosGb.GetComponent<MouseClickPos>();

        Information.nextScene = 28;
        Information.grade = "Grade 5";
        Information.subject = "science";

        Information.isSelect = true;
        shownPanelIndexs = new List<int>();
        ParseData.parseModel();
        start.onClick.AddListener(delegate { takeStart(); });
        lableIndex = 0;
        initAddPanels();
        initChangePanels();
        //    initPanelButtons();
        showPanel();
        initObjects();
        initObjectButtons();

    }

    void takeStart()
    {
        if (ball != null)
        {
            Debug.Log("here");
            ball.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }

        start.transform.GetChild(0).GetComponent<Text>().text = "Save";
        start.onClick.RemoveAllListeners();
        start.onClick.AddListener(delegate { takeSave(); });

    }

    void takeSave() //that looks good 
    {
        //here you can save each of the positions and orientations of the objects into an xml file 
        XElement gr = Information.xmlDoc.Element("goldRuberg");
        XElement newModel = new XElement("model", new XAttribute("Date", DateTime.Today));
        foreach (var curr in userObjects)
        {
            //ok you needto create a new entry
            newModel.Add(curr.getFileInfo());
            //ok now you would just save this to the website 
            //now just save the type of object (this will save its energy type, its energy, and its image  

        }
        gr.Add(newModel);

    }

    void loadFromFile() //here this will load a previously saved gold ruberg thing 
    {
        //so for this, you need to open module menu your something, and then they can choose which date
        //in the future, you can take a screenshot of the objects and show that in the image part of the open module, that way the user can see what each model looks like, and you dont need to gneerate anything 
    }




    void initObjects() //create it in the same order the buttons are in
    {
        objectTypes = new List<Object>();
        Object spring = new Object();
        spring.type = energyTypes.elastic;
        spring.potentialEnergy = 10;
        objectTypes.Add(spring);

        Object ramp = new Object();
        ramp.type = energyTypes.gravitional;
        ramp.potentialEnergy = 10;
        ramp.kineticEnergy = 0;
        objectTypes.Add(ramp);

        Object ball = new Object();
        ball.type = energyTypes.kinetic;
        ball.potentialEnergy = 10;
        ball.kineticEnergy = 0;
        objectTypes.Add(ball);

        Object pully = new Object();
        pully.type = energyTypes.gravitional;
        pully.potentialEnergy = 10;
        pully.kineticEnergy = 0;
        objectTypes.Add(pully);

        Object fire = new Object();
        fire.type = energyTypes.thermal;
        fire.potentialEnergy = 0;
        fire.kineticEnergy = 10;
        objectTypes.Add(fire);

        Object plank = new Object();
        plank.type = energyTypes.kinetic;
        plank.potentialEnergy = 10;
        plank.kineticEnergy = 0;
        objectTypes.Add(plank);

        Object teaterTotter = new Object();
        plank.type = energyTypes.kinetic;
        plank.potentialEnergy = 10;
        plank.kineticEnergy = 0;
        objectTypes.Add(teaterTotter);




    }

    //delegate each of the buttons \
    public GameObject buttonContainer;
    public GameObject spriteCpntainer;
    //ok, so just find the index of the clicked button?
    //for each button, create a new gameobject that that button represents 
    //then for handle new just pass the gameobject, keep the gameobject in the 
    List<Button> tempButtons;
    void initObjectButtons()
    {
        //here init userobjects
        userObjects = new List<Object>();
        tempButtons = new List<Button>();

        foreach (var button in buttonContainer.GetComponentsInChildren<Button>())
        {
            Debug.Log("added listener");
            tempButtons.Add(button);

            button.onClick.AddListener(delegate { handleNewObject(button); });
        }

        //delegate each of the buttons to a certain object

    }



    //when the user clicks on one of the buttons on the side panels, this will be caleld, it will make a new object, and handle its initialization
    Object currentObject;
    void handleNewObject(Button sender)
    {


        //change information.currobject to let the user drag and drop it 
        //instantiate a new gameobject for that 
        int idx = tempButtons.IndexOf(sender);
        Debug.LogError(idx + " button index");
        currentObject = new Object(objectTypes[idx]);
        GameObject temp = Instantiate(spriteCpntainer.transform.GetChild(idx).gameObject, spriteCpntainer.transform.GetChild(idx), true);

        temp.transform.SetParent(temp.transform.parent.parent);
        temp.SetActive(true);

        if (currentObject.type == energyTypes.kinetic)
        {
            //then its the ball?
            Debug.Log("added ball");
            ball = new Object(objectTypes[idx]);
            ball.gameObject = temp;
            currentObject.gameObject = ball.gameObject;
            ball.spriteIndex = idx;
        }
        else
        {
            currentObject.gameObject = temp;
            currentObject.spriteIndex = idx;
        }

    }


    //todo: 
    //real quick, get rid of the current information panel, add the prefab, get rid of your current method, show the panel at the begining

    List<int> shownPanelIndexs; //this will keep track of which panels were shown so they are not shown again (this can be turned off)

    // Update is called once per frame
    int minX = 130;
    bool shouldRotate = false;
    public Transform orb;
    public float radius;
    bool wasMouseUp = false;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            wasMouseUp = true;
        }
        if (currentObject != null && !shouldRotate)
        {
            GameObject toMove;
            if (currentObject.gameObject != null)
            {
                toMove = currentObject.gameObject;
            }
            else
            {
                toMove = ball.gameObject;
            }
            toMove.transform.localPosition = new Vector3(mousePos.position.x, mousePos.position.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                if (toMove.transform.localPosition.x > minX)
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
                showAddPanel(currentObject.type);
                shouldRotate = false;
                currentObject = null;
            }
        }


        if (ball != null && Information.currentBox != null)
        {
            Debug.Log("here, ball collision");
            //the ball collided with something, find out what it is based on the gameobject
            foreach (var currObject in userObjects)
            {

                if (currObject.gameObject != null && Information.currentBox.transform == currObject.gameObject.transform)
                {
                    //then you found it 
                    //ok so if its elastic, or fire then launch it up
                    //you can also show the panel here
                    if (currObject.type == energyTypes.elastic)
                    {
                        Debug.Log("elastic collsiion");
                        ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                    }
                    else if (currObject.type == energyTypes.thermal)
                    {
                        ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                    }
                    else if (currObject.type == energyTypes.gravitional)
                    {
                        ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                    }
                    //so, first check if the ball is triggering something, then check the other way around 
                    energyChange ballFirst = new energyChange(ball.type, currObject.type);
                    energyChange ballLast = new energyChange(currObject.type, ball.type);

                    showChangePanel(ballFirst);
                    showChangePanel(ballLast);
                    //       showChangePanel()
                }
            }
            Information.currentBox = null;
        }

        if (!InformationPanel.activeSelf)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            if (userObjects != null && userObjects.Count > 0 && !userObjects[0].gameObject.activeSelf)
            {
                hidePanel();
            }
        }
    }

    void showPanel()
    {
        Debug.LogError("showing panel");
        //   hideObjects();
        InformationPanel.SetActive(true);
    }

    void hidePanel()
    {
        Time.timeScale = 1;
        Debug.LogError("hiding panel");
        //  showObjects();
        InformationPanel.SetActive(false);
    }

    Dictionary<energyTypes, int> addPanels;
    Dictionary<energyChange, int> changePanels;

    void showChangePanel(energyChange type)
    {
        Time.timeScale = 0;

        foreach (var currChange in changePanels)
        {
            if (currChange.Key.start == type.start && currChange.Key.end == type.end)
            {
                int index = currChange.Value;
                if (!Information.userModels[index].wasShown)
                {
                    Information.panelIndex = index;
                    Information.userModels[index].wasShown = true;
                    //       InformationPanel.SetActive(true);
                    showPanel();
                    Debug.Log("showing: " + type.start + " " + type.end);
                }
                return;
            }
        }
        Debug.Log("couldnt find: " + type.start + " " + type.end);
        return;
    }

    void showAddPanel(energyTypes type)
    {
        if (!addPanels.ContainsKey(type))
        {
            Debug.Log("didnt find key: " + type);
            return;
        }

        Debug.LogError(type.ToString());
        int index = addPanels[type] + 1;
        if (!Information.userModels[index].wasShown)
        {
            Information.panelIndex = index;
            Information.userModels[index].wasShown = true;
            Debug.Log("showing: " + Information.userModels[index].simpleInfo[0]);
            showPanel();
        }
    }



    void initAddPanels()
    {
        addPanels = new Dictionary<energyTypes, int>();
        addPanels.Add(energyTypes.thermal, 1);
        addPanels.Add(energyTypes.kinetic, 2);
        addPanels.Add(energyTypes.mechanical, 3); //mechanical
        addPanels.Add(energyTypes.gravitional, 4);
        addPanels.Add(energyTypes.elastic, 5);
        addPanels.Add(energyTypes.chemical, 6);


    }

    void initChangePanels()
    {
        changePanels = new Dictionary<energyChange, int>();
        changePanels.Add(new energyChange(energyTypes.elastic, energyTypes.kinetic), 7);
        changePanels.Add(new energyChange(energyTypes.gravitional, energyTypes.kinetic), 8);
        changePanels.Add(new energyChange(energyTypes.thermal, energyTypes.kinetic), 9);
        changePanels.Add(new energyChange(energyTypes.kinetic, energyTypes.elastic), 10);
        changePanels.Add(new energyChange(energyTypes.kinetic, energyTypes.gravitional), 11);
        changePanels.Add(new energyChange(energyTypes.kinetic, energyTypes.thermal), 12);

    }


    //so here when it touches the pully, just change the sprite to be in the right position
    //in the future have a sprite sheet and just go throught that 
    void animatePully()
    {

    }

    //animate all of the fire in the sceen 
    IEnumerator animateFire()
    {
        yield return null;
    }

    void animateSpring()
    {

    }

    void showObjects()
    {

        if (userObjects == null)
            return;

        foreach (var curr in userObjects)
        {
            curr.gameObject.SetActive(true);
        }
    }

    void hideObjects()
    {
        if (userObjects == null)
            return;
        foreach (var curr in userObjects)
        {

            curr.gameObject.SetActive(false);
        }

    }



    //when you add a new object, this will see if the panel was already shown and if not show it 




    int lableIndex = 0;
    int panelIndex = 0;
    public GameObject InformationPanel;
    /*   public Text simple;
       public Text advanced;
       public Button next;
       public Button back;
       bool skipPrevious;
       void showPanel()
       {
           if (shownPanelIndexs.Contains(panelIndex))
           {
               if (skipPrevious)
                   return;
           }
           shownPanelIndexs.Add(panelIndex);


           Time.timeScale = 0; //this should pause the physics
           lableIndex = 1;

           InformationPanel.SetActive(true);
           next.gameObject.SetActive(true);
           back.gameObject.SetActive(true);

           simple.text = Information.userModels[panelIndex].simpleInfo[0];
           advanced.text = Information.userModels[panelIndex].advancedInfo[0];
       }

       void initPanelButtons()
       {
           next.onClick.AddListener(delegate { takeInformationClick(true); });
           back.onClick.AddListener(delegate { takeInformationClick(false); });


       }

       void takeInformationClick(bool next)
       {

           int direction = -1;
           if (next)
           {
               direction = 1;
           }
           Debug.Log("here");



           if (lableIndex >= Information.userModels[panelIndex].advancedInfo.Count)
           {
               //lableIndex = 0;
               hidePanel();
               return;
           }
           advanced.text = Information.userModels[panelIndex].advancedInfo[lableIndex];
           lableIndex += direction;
       }


       void hidePanel()
       {
           Debug.Log("hiding panel");
           Time.timeScale = 1;

           if (panelIndex == 0)
           {
               initObjectButtons();

           }
           panelIndex++; //so that next time it shows the next panel
           InformationPanel.SetActive(false);

       }
       */
    //ok, so keep track of all the balls in the scene, when they come into contacet with something then you show it 
    //(maybe only have one ball)

}
