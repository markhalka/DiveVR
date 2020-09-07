using System;
using System.Collections;
using System.Collections.Generic;
//using UnityNative.Sharing.Example;
//using UnityNative.Sharing;
using System.IO;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Achievments : MonoBehaviour
{


    int shareCounter;

    public Button back;
    public GameObject shareError;
    public Button shareContinue;


    void Start()
    {

        ParseData.startXML();
        getImages();
        parseAchievments();
        back.onClick.AddListener(delegate { takeBack(); });
        shareContinue.onClick.AddListener(delegate { takeShareBack(); });


        Information.updateEntities = userAcheivments.ToArray();
        Information.currentScene = "Achievement";
    }

    void takeBack()
    {
        SceneManager.LoadScene("StudentMenu");
    }

    void takeShareBack()
    {
        shareError.gameObject.SetActive(false);
    }



    public List<XElement> certificates;
    List<GameObject> userAcheivments;
    void parseAchievments()
    {
        var items = Information.xmlDoc.Root.Element("certificates").Elements();
        certificates = new List<XElement>();
        userAcheivments = new List<GameObject>();
        currentOffset = new Vector2(0, 0);
        shareCounter = 0;
        foreach (var item in items)
        {
            if (item.Name == "ach")
            {
                createNewAchievment(item);
            }
            else if (item.Name == "medal")
            {
                createNewMedal(item);
            }

            string[] dates = item.Attribute("date").Value.Split(',');
            foreach (var date in dates)
            {
                if (date == DateTime.Today.ToString("MM/dd/yyyy"))
                {
                    shareCounter++;
                }
            }
            certificates.Add(item);

        }
    }

    public GameObject certificate;
    public GameObject medal;



    Vector3 offsetAmount = new Vector3(200, -100);
    Vector3 currentOffset = new Vector3(0, 0);
    int maxX = 600;

    List<string> fileNames;
    void getImages()
    {
        fileNames = new List<string>();
        try
        {
            fileNames = new List<string>(Directory.GetFiles(Application.persistentDataPath + "/Certificates"));
        }
        catch (Exception e)
        {
            Debug.LogError("error reading the files");
        }
    }

    int imageIndex = 0;
    void createNewAchievment(XElement curr)
    {

        string acheivment = curr.Attribute("name").Value;
        string name = Information.name;

        GameObject newAch = Instantiate(certificate, certificate.transform, true);

        newAch.transform.GetChild(1).GetComponent<TMP_Text>().text = name;
        newAch.transform.GetChild(2).GetComponent<TMP_Text>().text = acheivment;

        newAch.transform.SetParent(newAch.transform.parent.parent);
        newAch.gameObject.SetActive(true);

        userAcheivments.Add(newAch);


    }


    void updateOffest()
    {
        currentOffset.x += offsetAmount.x;
        if (currentOffset.x > maxX)
        {
            currentOffset.x = 0;
            currentOffset.y += offsetAmount.y;
        }
    }
    public Sprite[] medalSprites;

    void createNewMedal(XElement curr)
    {
        int spriteIndex = 0;
        switch (curr.Attribute("type").Value)
        {
            case "gold":
                spriteIndex = 0;
                break;
            case "silver":
                spriteIndex = 1;
                break;
            case "bronze":
                spriteIndex = 2;
                break;
        }
        GameObject newMedal = Instantiate(medal, medal.transform, true);
        newMedal.transform.SetParent(newMedal.transform.parent.parent);
        newMedal.GetComponent<Image>().sprite = medalSprites[spriteIndex];
        newMedal.transform.localPosition += currentOffset;
        newMedal.gameObject.SetActive(true);
        userAcheivments.Add(newMedal);

        updateOffest();

    }

    public GameObject socialMedia;
    public Text fadeText;

    void openSocialMedia2(int index)
    {

        if (index < 0)
        {
            Debug.LogError("could not find index");
            return;
        }

        currentCertificate = certificates[index];

        if (currentCertificate.Attribute("date").Value.Length < 3)
        {
            if (shareCounter >= 3)
            {
                shareError.SetActive(true);
                return;
            }
            StartCoroutine(shareTextAndScreenShot(fileNames[index], "Check out my achievment on Dive!"));
            currentCertificate.Attribute("date").Value = DateTime.Today.ToString("MM/dd/yyyy");
            XMLWriter.saveFile();
            shareCounter++;


        }
    }

    private IEnumerator shareTextAndScreenShot(string screenshotPath, string text)
    {
        int sanityCheck = 0;
        while (!File.Exists(screenshotPath))
        {
            sanityCheck++;
            if (sanityCheck > 10)
            {
                Debug.LogError("could not find screenshot to share");
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    void errorShare()
    {
        fadeText.text = "Error: you can only share 3 times a day. Please come back tommorow and share again!";
        fadeText.gameObject.SetActive(true);
    }

    public GameObject container;
    int getIndex()
    {
        int output = -1;
        for (int i = 0; i < userAcheivments.Count; i++)
        {
            if (userAcheivments[i].transform == Information.currentBox.transform)
            {
                return i;
            }
        }
        return output;
    }


    bool getSocialMedia(string name)
    {
        var medias = Information.xmlDoc.Descendants("media");
        foreach (var curr in medias)
        {
            if (curr.Attribute("name").Value == name)
            {
                //then you found it, check if a username and password exsit 
                string us = curr.Attribute("username").Value;
                string pass = curr.Attribute("password").Value;
                if (us.Length > 1 && pass.Length > 1)
                {
                    Information.socialUsername = us;
                    Information.socialPassword = pass;
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject points;
    public GameObject enterInfo;

    // Update is called once per frame
    XElement currentCertificate;

    void Update()
    {
        if (Information.currentBox != null)
        {
            openSocialMedia2(getIndex());
            Information.currentBox = null;
        }
    }
}
