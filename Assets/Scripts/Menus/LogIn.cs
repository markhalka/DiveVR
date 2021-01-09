using PlayFab.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    public TMPro.TMP_InputField usernameField;
    public TMPro.TMP_InputField passwordField;
    public TMPro.TMP_Text output;

    public AudioSource source;
    public AudioClip buttonClick;
    public AudioClip ping;

    public GameObject logInScreen;
    public GameObject initScreen;

    public Button guest;
    public Button logInButton;
    public Button submit;
    public Button webpage;

    Website network;


    void Start()
    {
        network = new Website();

        submit.onClick.AddListener(delegate { takeSubmit(); });
        webpage.onClick.AddListener(delegate { openWebsite(); });
        guest.onClick.AddListener(delegate { takeGuest(); });
        logInButton.onClick.AddListener(delegate { takeLogIn(); });
    }

    void takeGuest()
    {
        Information.name = "none";
        Information.username = "guest@email.com";
        SceneManager.LoadScene("StudentMenu");
    }

    void takeSubmit()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        StartCoroutine(network.GetRequest(Information.loginUrl + username + "/" + password, handleLogIn));
    }

    void takeLogIn()
    {
        initScreen.SetActive(false);
        logInScreen.SetActive(true);
    }

    int checkOutput = -1;

    void handleLogIn(string result)
    {
        source.clip = buttonClick;
        source.Play();

        if (result.Contains("no user"))
        {
            Debug.LogError("no user, checking parent");
            checkOutput = 0;
            buttonClicked();
        }
        else
        {
            checkOutput = checkLogIn(result, usernameField.text);
            buttonClicked();
        }
    }


    int checkLogIn(string result, string username)
    {

        if (result.Split(',').Length == 2)
        {
            Information.name = result.Split(',')[1];
        }
        else
        {
            Information.name = "none";
        }

        if (result.Contains("no plan"))
        {
            Information.username = username;
            return 2;
            //return 1;
        }
        else if (result.Contains("basic"))
        {
            Information.username = username;
            return 2;
        }
        else if (result.Contains("deep"))
        {
            Information.username = username;
            return 3;
        }
        else if (result.Contains("parent"))
        {
            Debug.LogError("found parent");
            string[] split = result.Split(' ');
            Information.username = split[1];
            Debug.LogError("the curent username is " + Information.username);
            return 4;
        }




        return -1;
    }


    public GameObject accountType;

    void buttonClicked()
    {
        source.clip = buttonClick;
        source.Play();

        string outputText = "";
        if (usernameField.text != "" && passwordField.text != "")
        {


            if (!Information.isVrMode)
            {
                UnityEngine.XR.XRSettings.enabled = false;
                UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(Camera.main, true);
                UnityEngine.XR.InputTracking.disablePositionalTracking = true;

            }
            else
            {
                UnityEngine.XR.XRSettings.enabled = true;
                UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(Camera.main, true);
                UnityEngine.XR.InputTracking.disablePositionalTracking = true;

            }
            bool open = false;
            Debug.LogError(checkOutput + " output");
            switch (checkOutput)
            {
                case -1:
                    outputText = "There was an error, please check your network connection and try again";
                    break;
                case 0:
                    outputText = "Please enter a differnet username and password, or make an account by clicking the button";
                    webpage.gameObject.SetActive(true);
                    break;

                case 1:
                    outputText = "Expired Dive plan, please renew it by heading to our website! ";
                    webpage.gameObject.SetActive(true);
                    break;
                case 2:
                    outputText = "Dive plan found, opening portal";
                    open = true;
                    break;
                case 3:
                    outputText = "Deep Sea plan found, opening portal";
                    open = true;
                    break;
                case 4:
                    outputText = "Parent account found, opening portal";
                    Information.isParent = true;
                    open = true;
                    break;

            }

            if (open)
            {
                source.clip = ping;
                source.Play();

                //   Information.address = Website.GET(Information.addressUrl);
                SceneManager.LoadScene("StudentMenu");
                     
            }

        }
        else
        {
            outputText = "Please enter values for both username and password fields";
        }
        output.text = outputText;

        Debug.LogError("the name is: " + Information.name);
    }


    void openWebsite()
    {
        if (webpage.gameObject.activeSelf)
        {
            Application.OpenURL("https://divevr.org");
        }

    }
}
