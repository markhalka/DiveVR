using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Science : MonoBehaviour
{


    public Text scoreCount;
    public GameObject loadingContainer;
    public GameObject inBetwenScene;


    void Start()
    {
        startAnimation();

        if (Information.subject == "science")
        {
            openScene();
        }
        else if (Information.subject == "social science")
        {
            openSceneSocialScience();
        }

        //   ParseData.startXML(); 
        Information.currentScene = "OtherScience";


        XMLWriter.savePastSubjectAndGrae();

    }

    void startAnimation()
    {
        int random = Random.Range(0, loadingContainer.transform.childCount - 1);
        loadingContainer.transform.GetChild(random).gameObject.SetActive(true);

        loadingContainer.gameObject.SetActive(true);
    }


    public void openSceneSocialScience()
    {
        Information.nextScene = Information.topics[Information.topicIndex++].topics[0];
        string sceneToLoad = "";

        switch (Information.nextScene)
        {
            case 69: //canada geography 

                sceneToLoad = "Science";
                break;

        }

        if (sceneToLoad.Length > 1)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public GameObject database;
    public void openScene()
    {
        if (Information.topicIndex > Information.topics.Count - 1)
        {
            Information.topicIndex = 0;
        }
        if (Information.retry)
        {
            Information.retry = false;
        }
        else
        {
            Information.nextScene = Information.topics[Information.topicIndex++].topics[0];
        }


        if (Information.nextScene < 0)
        {
            Information.nextScene = 0;
        }

        string sceneToLoad = "";
        Information.doneLoading = false;
        database.SetActive(false);
        //    Information.isQuiz = 0;
        Information.panelIndex = 0;
        Information.lableIndex = 0;
        Information.isSelect = false;


        switch (Information.nextScene)
        {

            case 1: //5 matter and mass 6 matter and mass 
                sceneToLoad = "Science";
                break;
            case 2: //5 heat and thermal 6 thermal energy
                sceneToLoad = "Molecules";
                break;
            case 3: //5 physical and chemical change   //not done
                sceneToLoad = "Molecules"; //temp shit
                Information.nextScene = 4;
                break;
            case 4: //5 mixtures 6 solutions
                sceneToLoad = "Molecules";
                break;
            case 5: //plants   <---------------------------
                sceneToLoad = "Science";
                break;
            case 6: //5 force and motion
                Information.nextScene = 6;
                sceneToLoad = "Rocket"; //ima make this the rocket shit too
                break;
            case 7: //5 magnets   //not done
                    // sceneToLoad = "Physics";
                sceneToLoad = "Physics";
                break;
            case 8: //5 Classification
                sceneToLoad = "Database";
                //  ClassificationLadder();
                break;
            case 9: //5 scientific names 6 class and names 
                sceneToLoad = "Database";
                break;
            case 10: //5 Animals
                     //just make it animal life cylces 
                sceneToLoad = "Database";
                break;
            case 11: //simple machines
                sceneToLoad = "Science";
                break;
            case 12:  //microscope
                sceneToLoad = "Science";
                break;
            case 13: //nothing here
                Information.nextScene = 14; //temp shit
                sceneToLoad = "Science";
                break;
            case 14: //5 animal cell 6 animal cell
                sceneToLoad = "Science";
                break;
            case 15: //5 plant cell 6 plant cell
                sceneToLoad = "Science";
                break;
            case 16: //5 ecology 6 ecology
                sceneToLoad = "Database";
                break;
            case 17: //5 conservation of natural resources 
                //natual, renewable, non renewable energy sources, recycling, not recycling
                //you can make this a gmae 
                sceneToLoad = "Recycling";
                break;
            case 18: //5 rocks and minerals 6 rocks and minerals
                sceneToLoad = "Science";
                break;
            case 19: //5 fossils 6 fossils
                sceneToLoad = "Science";
                break;
            case 20: //greenhouse gasses //5 weather and climate 6 weather climate
                sceneToLoad = "Science";
                break;
            case 21: //5 water cycle 6 watercycle
                sceneToLoad = "Science";
                break;
            case 22: //5 astronomy 6 astronomy
                sceneToLoad = "Science";
                break;
            case 23: //6 Science practices and tools    //not done
                sceneToLoad = "Science"; //ok have the 3d models of the tools, and satey tips 
                break;
            case 24: //6 Designing experiments
                sceneToLoad = "Design";     //not done
                                            //have them design a simple plant growth experiemnt, or maybe have a small collection, have them follow the right process (maybe make this a seperate class)
                break;
            case 25: //6 Engineering practices    //not done
                sceneToLoad = "Molecules";
                Information.nextScene = 26;
                //database 
                break;
            case 26: //6 Chemical reactions
                sceneToLoad = "Molecules";
                break;
            case 27: //6 Velocity, acceleration, and forces
                sceneToLoad = "Rocket"; //this is the rocket shit 
                break;
            case 28: //6 Kinetic and potential energy
                sceneToLoad = "Physics";
                break;
            case 29: //6 Waves
                sceneToLoad = "SampleScene";
                break;
            case 30: //6 Biochemistry
                sceneToLoad = "Science";
                break;
            case 31: //6 anatomy and physi
                sceneToLoad = "Science";
                break;
            case 32:  //flight
                sceneToLoad = "Science";
                break;
            case 33:
                Information.nextScene = 34;
                sceneToLoad = "Science";
                break;

            case 34: //weather and climate 
                sceneToLoad = "Science";
                break;
            case 35: //Photosynthesis    
                sceneToLoad = "Science"; //for these two you can add particle systems too, to simulate everything 
                break;
            case 36: //Ecological interactions
                sceneToLoad = "Science";
                //db food web 
                break;
            case 37:
                Information.nextScene = 38;
                sceneToLoad = "Database";
                break;
            case 38: //Plate tectonics
                sceneToLoad = "Database";
                break;
            case 39: //weather and atmosphere  //not done 
                sceneToLoad = "Science";
                break;
            case 40:  // < ------------------------------ not done!  current electricity
                sceneToLoad = "Physics";
                break;
            case 41:
                Information.nextScene = 41;
                //ecosystem (forest, marine) 
                sceneToLoad = "Database";
                break;

            case 42://5 energy sources, 6 energy sources 7 energy sources 
                sceneToLoad = "Science";
                break;

            case 43: //static electricity //<--------------- not done
                sceneToLoad = "Physics";
                break;

            case 44: //enviornment //< -------------------------- not done
                sceneToLoad = "Science";
                break;

            case 45: //weathering
                sceneToLoad = "Science";
                break;

            case 46:
                sceneToLoad = "Science";
                break;




        }


        if (sceneToLoad == "Database")
        {
            loadingContainer.gameObject.SetActive(false);
            database.gameObject.SetActive(true);
            return;
        }

        if (sceneToLoad != "")
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }
        else
        {
            Debug.LogError("the scene: " + Information.nextScene + " was not found");

        }

    }
    private float _loadingProgress;

    private IEnumerator LoadScene(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {

            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            if (asyncScene.progress >= 0.9f)
            {
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
