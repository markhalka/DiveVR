using UnityEngine;
using UnityEngine.UI;

public class SocialMedia : MonoBehaviour
{


    void Start()
    {
        delegateButtons();
    }

    void delegateButtons()
    {
        transform.GetChild(6).GetComponent<Button>().onClick.AddListener(delegate { takeContinue(); }); //continue;
        transform.GetChild(7).GetComponent<Button>().onClick.AddListener(delegate { takeBack(); }); //continue;
    }

    void takeContinue()
    {

    }
/*
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

    void takeContinue()
    {
        Information.socialUsername = transform.GetChild(3).GetComponent<InputField>().text;
        Information.socialPassword = transform.GetChild(4).GetComponent<InputField>().text;
        Information.socialLogIn = true;

        if (transform.GetChild(2).GetComponent<Toggle>().isOn)
        {
            var socialMedias = Information.xmlDoc.Descendants("media");
            foreach (var media in socialMedias)
            {
                if (media.Attribute("name").Value == Information.socialMediaName)
                {
                    media.Attribute("username").Value = Information.socialUsername;
                    media.Attribute("password").Value = Information.socialPassword;
                    XMLWriter.saveFile();
                }
            }
        }
        takeBack();
    }*/

    void takeBack()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
       // transform.GetChild(1).GetComponent<Text>().text = Information.socialMediaName + " Log In";
    }
    void Update()
    {

    }
}
