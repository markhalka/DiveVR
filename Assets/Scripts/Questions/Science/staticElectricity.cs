using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class staticElectricity : MonoBehaviour, IPointerDownHandler
{

    //make the charging look better
    //the partciles don't stay there??

    //make the spark go right to the rod, make the rod bigger, make sure the spark doesnt go throught the rod

    //than done 

    //you need to add more contnet, for section 1, and more quiz questions
    //add the person, he will be the one who is shocked





    public ParticleSystem blueParticles; //these are what will be generated in the ballon
    public ParticleSystem redParticles;

    //ok, lets get this show on the road....

    public GameObject balloon; //actually, this script will be assigned to the balloon
    public GameObject person;
    public GameObject carpet;

    public Image collectingImage;

    public Sprite[] collectingSprites;
    public Sprite[] sparkSprits;

    public bool movingBallon = false;


    int delay = 0;

    public GameObject currInformationPanel;
    int finishStart = 0;

    List<staticPanels> panels;
    void Start()
    {
        Information.isSelect = true;
        panels = new List<staticPanels>();

        panels.Add(new staticPanels("Friction", 2));
        panels.Add(new staticPanels("Particles", 3));
        panels.Add(new staticPanels("Static Electricity", 4));
        panels.Add(new staticPanels("Spark", 5));

        tempLoad();
        Information.grade = "Grade 3";
        Information.subject = "science";
        Information.nextScene = 43;

        ParseData.startXML();
        ParseData.parseModel();
        Information.panelIndex = -1;

        initStartPanels();

    }

    void tempLoad()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);

        Debug.LogError(Information.xmlDoc.ToString());
        Debug.LogError(Information.loadDoc.ToString());

    }





    class staticPanels
    {
        public string name;
        public int index;
        public bool shown;

        public staticPanels(string n, int i)
        {
            name = n;
            index = i;
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






    //Do this when the mouse click on this selectable UI object is released.

    // Update is called once per frame
    int collectingIndex = 0;
    bool collecting = false;
    void Update()
    {
        handleStartPanels();

        if (Input.GetMouseButtonUp(0))
        {
            movingBallon = false;
        }

        if (movingBallon)
        {
            if (colliding)
            {
                if (Information.currPosition.y < transform.localPosition.y)
                {
                    Debug.LogError("collding");
                    return;
                }
            }
            if (Vector3.Distance(balloon.transform.localPosition, Information.currPosition) > 2)
            {
                collectingImage.gameObject.SetActive(true);
                if (collecting && delay % 3 == 0) //only show it when its moving 
                {
                    collectingIndex++;
                    collectingImage.sprite = collectingSprites[collectingIndex % collectingSprites.Length];
                    colliding = false;
                }
            }
            else
            {
                collectingImage.gameObject.SetActive(false);
            }
            balloon.transform.localPosition = new Vector3(Information.currPosition.x, Information.currPosition.y, 0);
            delay++;


        }
        else
        {
            collectingImage.gameObject.SetActive(false);

        }

    }


    //this will check if it should discharge (if it is near the thing)
    int particleThresh = 20;
    void checkParticles()
    {
        if (blueParticles.particleCount > particleThresh)
        {
            createSpark();
        }
    }

    public Image spark;

    //this will create the spark animation and clear all the particles 
    void createSpark()
    {
        checkPanel("Spark");

        StartCoroutine(sparkAnimation());
        blueParticles.Clear();

    }

    void checkPanel(string name)
    {
        foreach (var p in panels)
        {
            if (p.name == name)
            {
                if (!p.shown)
                {
                    Information.panelIndex = p.index;
                    p.shown = true;
                    showPanel();
                }
                else
                {
                    Debug.LogError(name + " was shown");
                }
                return;
            }
        }
        Debug.LogError(name + " wsa not found...");
    }


    //here just cycle between the two sprites for a bit, than return to the original

    utilities utility;
    IEnumerator sparkAnimation()
    {
        //  spark.gameObject.SetActive(true);
        int count = 0;
        while (count < 10)
        {
            person.GetComponent<Image>().sprite = sparkSprits[count % 2];
            count++;
            yield return new WaitForSeconds((float)utility.getRandom(5, 20) / 100.0f);
        }
        person.GetComponent<Image>().sprite = sparkSprits[1];
    }

    bool colliding = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Finish")
        {
            checkParticles();
            return;
        }
        /*  if (col.gameObject.tag == "Respawn")
          {
              Debug.LogError("here1");
              colliding = false;
              collecting = true;
              collectingImage.gameObject.SetActive(true);
              return;
          }
          Debug.LogError(col.gameObject.tag + " colldiing with " + col.gameObject.name);*/

        colliding = true;

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.LogError("stay1");
        if (other.gameObject.tag == "Respawn")
        {
            collecting = true;
            Debug.LogError("stay 2");
            if (delay > 5)
            {
                if (blueParticles.particleCount >= 20)
                {
                    checkPanel("Static Electricity");
                }
                else if (blueParticles.particleCount > 10)
                {
                    checkPanel("Particles");
                }
                else
                {
                    checkPanel("Friction");
                }
                delay = 0;
                blueParticles.emission.GetBurst(0);
            }
        }
        else
        {

            colliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Respawn")
        {
            collectingImage.gameObject.SetActive(false);
            collecting = false;
        }

        colliding = false;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("ballonn clicked!");
        movingBallon = true;
        //    throw new System.NotImplementedException();
    }
}
