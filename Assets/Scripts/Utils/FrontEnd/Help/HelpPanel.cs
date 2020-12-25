using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{

    public RawImage image;

    public Button next;
    public Button back;

    bool isLastPanel = false;
    bool isClose = false;
    void Start()
    {
        loadTopic();
        initButtons();
    }


    public void callHelp()
    {
        isLastPanel = false;
        isClose = false;
        image.gameObject.SetActive(true);
        answerText.gameObject.SetActive(false);
        next.transform.GetChild(0).GetComponent<TMP_Text>().text = "Next";
        back.transform.GetChild(0).GetComponent<TMP_Text>().text = "Back";
        loadTopic();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void initButtons()
    {
        next.onClick.AddListener(delegate { nextPanel(); });
        back.onClick.AddListener(delegate { backPanel(); });

    }

    void nextPanel()
    {
        if (isLastPanel)
        {
            nextExample();
            return;
        }
        loadImage();

    }

    void backPanel()
    {
        if (isLastPanel)
        {
            close();
            return;
        }
        panelIndex -= 2;
        loadImage();
    }


    void lastPanel()
    {
        isLastPanel = true;
        string nextText = "Next Example";
        if (exampleIndex > exampleLength - 1)
        {
            nextText = "I still don't get it";
            isClose = true;
        }

        next.transform.GetChild(0).GetComponent<TMP_Text>().text = nextText;
        back.transform.GetChild(0).GetComponent<TMP_Text>().text = "Back to Question";


    }

    //load the next example
    public TMP_Text answerText;
    void nextExample()
    {
        if (isClose)
        {
            image.gameObject.SetActive(false);
            answerText.gameObject.SetActive(true);

            if (differentiator.question.Count > 0)
            {
                answerText.text = "Solution: " + differentiator.question[0].stringAnswer;
            }

            next.transform.GetChild(0).GetComponent<TMP_Text>().gameObject.SetActive(false);

            lastPanel();
            return;
        }
        next.transform.GetChild(0).GetComponent<TMP_Text>().text = "Next";
        back.transform.GetChild(0).GetComponent<TMP_Text>().text = "Back";

        exampleIndex++;
        panelLength = getLength(examplePath + "/" + exampleIndex);
        panelIndex = 0;
        loadImage();
    }


    void close()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }


    string examplePath;
    int panelIndex;
    int exampleIndex;
    int exampleLength;
    int panelLength;
    public void loadTopic()
    {
        var currTopic = Information.topics[Information.nextScene];
        int topicIndex = currTopic.topics[currTopic.topicIndex];

        panelIndex = 0;
        exampleIndex = 0;
        examplePath = Application.persistentDataPath + "/examples/" + topicIndex;
        exampleLength = getLength(examplePath);
        panelLength = getLength(examplePath + "/0");
        if (exampleLength < 0 || panelLength < 0)
        {
            isClose = true;
            nextExample();
            return;
        }

        loadImage();

    }

    int getLength(string address)
    {
        FileInfo[] Files = null;
        DirectoryInfo d = null;
        try
        {
            d = new DirectoryInfo(address);//Assuming Test is your Folder  
            Files = d.GetFiles(); //return all files
        }
        catch
        {
            return -1;
        }


        if (Files == null)
        {
            return -1;
        }

        return Files.Length;
    }


    void loadImage()
    {
        if (panelIndex < 0)
        {
            panelIndex = 0;
        }
        string file = examplePath + "/" + exampleIndex + "/" + panelIndex + ".png";
        if (!File.Exists(file))
        {
            lastPanel();
            return;
        }

        byte[] pngBytes = System.IO.File.ReadAllBytes(file);

        Texture2D tex = new Texture2D(2, 2);

        image.texture = tex;

        panelIndex++;
        if (panelIndex > panelLength - 1)
        {
            lastPanel();
            return;
        }
    }
}
