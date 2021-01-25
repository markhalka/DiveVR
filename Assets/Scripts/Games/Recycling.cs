using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Recycling : MonoBehaviour
{

    GameObject currentObject;
    List<RecyclingObject> currentObjects;

    public GameObject[] trash;
    public GameObject[] recycling;

    public Sprite[] trashSprites;
    public Sprite[] recyclingSprites;

    List<GameObject> rightClicks;

    public Button start;
    public Button exit;
    public Text scoreText;

    public GameObject buttons;
    public GameObject startPanel;

    public float time = 0;
    public int rightCount = 0;

    public GameObject panel;

    public GameObject endPanel;


    bool playingGame;


    float delay;
    float speed;
    int score;

    public AudioSource source;
    public AudioClip recycleSound;
    public AudioClip trashSound;
    public AudioClip buttonSound;

    List<Mistake> mistakeList;

    //ok, not bad: how to fix it up:
    //1. when they get something right, than just show like a green panel, or checkmark or something
    //2. when they get something wrong, show an red x 
    //3. at the end, make sure time.timescale = 0 (so nothing moves)
    //thers weird errors in that loop, fix that 
    //maybe you're not getting rid of the things in the list 


    void Start()
    {
        mistakeList = new List<Mistake>();
        rightClicks = new List<GameObject>();
        Time.timeScale = 1.5f; //that should speed it up more 

        if (!Information.isVrMode)
        {
            UnityEngine.XR.XRSettings.enabled = false;
        }
        else
        {
            UnityEngine.XR.XRSettings.enabled = true;
        }
        start.onClick.AddListener(delegate { takeStart(); });
        exit.onClick.AddListener(delegate { takeExit(); });
        score = 0;
        currentObjects = new List<RecyclingObject>();
        UnityEngine.Physics.gravity = new Vector3(0, -50, 0);
        //  takeStart();
    }

    void takeStart()
    {
        playingGame = true;
        startPanel.gameObject.SetActive(false);
        Information.click2d = false;
        delay = 10;
        speed = 0.8f;
        StartCoroutine(nextObject());
        StartCoroutine(checkClick());

    }

    public class RecyclingObject
    {
        public GameObject gameObject;
        public bool isTrash;

    }


    IEnumerator nextObject()
    {
        while (playingGame)
        {

            if (Random.Range(0,1) == 0)
            {
                getRecycling();
            }
            else
            {
                getTrash();

            }
            Information.updateEntities = new GameObject[currentObjects.Count];
            for (int i = 0; i < currentObjects.Count; i++)
            {
                Information.updateEntities[i] = currentObjects[i].gameObject;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    void getRecycling()
    {
        RecyclingObject curr = new RecyclingObject();
        GameObject newObject = recycling[Random.Range(0, recycling.Length - 1)];
        curr.gameObject = Instantiate(newObject, newObject.transform, true);
        curr.gameObject.transform.SetParent(curr.gameObject.transform.parent.parent);
        curr.isTrash = false;
        curr.gameObject.SetActive(true);

        currentObjects.Add(curr);
    }

    void getTrash()
    {
        RecyclingObject curr = new RecyclingObject();
        GameObject newObject = trash[Random.Range(0, trash.Length - 1)];
        curr.gameObject = Instantiate(newObject, newObject.transform, true);
        curr.gameObject.transform.SetParent(curr.gameObject.transform.parent.parent);
        curr.isTrash = true;
        curr.gameObject.SetActive(true);

        currentObjects.Add(curr);

    }

    IEnumerator checkClick()
    {
        while (playingGame)
        {


            if (Information.currentBox != null)
            {

                foreach (var curr in currentObjects)
                {
                    if (curr != null && curr.gameObject != null && curr.gameObject.name == Information.currentBox.name)
                    {
                        source.clip = trashSound;
                        source.Play();

                        if (curr.isTrash)
                        {
                            takeWrong(curr.gameObject);
                        }
                        else
                        {
                            takeRight(curr.gameObject);
                        }
                        Information.currentBox = null;
                    }
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    void takeRight(GameObject curr)
    {
        rightClicks.Add(curr);
        delay--;
        //  speed += 0.5f;
        Time.timeScale += 0.2f;
        UnityEngine.Physics.gravity = new Vector3(0, UnityEngine.Physics.gravity.y - 25, 0);
        curr.GetComponent<Rigidbody>().AddForce(5000, 5000, 0);
        score++;
        StartCoroutine(changeColor(true));
        rightCount++;
        delay -= 0.1f;
    }

    class Mistake
    {
        public Sprite image;
        public bool wasNotRecycled;

        public Mistake(Sprite i, bool a)
        {
            image = i;
            wasNotRecycled = a;
        }
    }


    void takeWrong(GameObject curr)
    {
        mistakeList.Add(new Mistake(getImageFromGameObject(curr), false));
        if (mistakeList.Count >= 3)
        {
            takeEndPanel();
        }
        //  mistakes.Add(curr);
        curr.GetComponent<Rigidbody>().AddForce(5000, 5000, 0);
        StartCoroutine(changeColor(false));

    }


    Sprite getImageFromGameObject(GameObject curr)
    {
        for (int i = 0; i < trash.Length; i++)
        {
            if (curr.name.Contains(trash[i].name))
            {
                return trashSprites[i];
            }
        }

        for (int i = 0; i < recycling.Length; i++)
        {

            if (curr.name.Contains(recycling[i].name))
            {
                return recyclingSprites[i];
            }
        }
        return null;
    }


    void takeEndPanel()
    {

        Time.timeScale = 0;

        endPanel.gameObject.SetActive(true);
        scoreText.text = "You scored: " + score;
        for (int i = 0; i < mistakeList.Count; i++)
        {
            endPanel.transform.GetChild(i).GetComponent<Image>().sprite = mistakeList[i].image;
            string text = "These should be recycled!";
            if (!mistakeList[i].wasNotRecycled)
            {
                text = "These should not be recycled!";
            }
            endPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = text;
        }
        endPanel.transform.GetChild(4).GetComponent<Text>().text = "You scored: " + score;

    }

    void takeExit()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("ScienceMain");
    }




    IEnumerator changeColor(bool right)
    {
        if (right)
        {
            panel.GetComponent<Image>().color = Information.rightColor;

        }
        else
        {
            panel.GetComponent<Image>().color = Information.wrongColor;
        }

        yield return new WaitForSeconds(1);


        panel.GetComponent<Image>().color = Information.defualtColor;
    }



    float minPosY = 0;

    void Update()
    {
        if (playingGame)
        {
            time += Time.deltaTime;
            for (int i = currentObjects.Count - 1; i >= 0; i--)
            {
                currentObjects[i].gameObject.transform.Translate(new Vector3(0, 0, -speed), Space.World);

                if (currentObjects[i].gameObject.transform.position.y < minPosY)
                {
                    source.clip = recycleSound;
                    source.Play();

                    if (rightClicks.Contains(currentObjects[i].gameObject))
                    {
                        rightClicks.Remove(currentObjects[i].gameObject);

                    }
                    else
                    {


                        if (!currentObjects[i].isTrash)
                        {
                            mistakeList.Add(new Mistake(getImageFromGameObject(currentObjects[i].gameObject), true));
                            if (mistakeList.Count >= 3)
                            {
                                takeEndPanel();
                            }
                        }
                        else
                        {
                            score++;
                        }
                    }

                    currentObjects[i].gameObject.SetActive(false);
                    currentObjects.RemoveAt(i);

                    break;
                }
            }
        }
    }
}
