using System;
using System.Collections;
using System.IO;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Certificate : MonoBehaviour
{

    //test the share panel, make sure it always shows up
    //if they share, than write that to the website, so you can get that information 


    public Button continueButton;
    public Button share;
    public Button collectButton;


    public GameObject warningPanel;
    public GameObject points;

    XElement certificate;

    public Transform imageTransform;

    public AudioSource source;
    public AudioClip certificateOpenSound;
    public AudioClip buttonSound;

    public Panel panel; // make sure this class has panel attached to it



    void Start()
    {
        panel = new Panel();
        initButtons();
    }

    bool toShare;
    string screenshotPath;
    bool shared;
    bool didTakeScreenshot = false;

    void OnEnable()
    {
        source.clip = certificateOpenSound;
        source.Play();

        toShare = false;
        screenshotPath = null;
        shared = false;
        didTakeScreenshot = false;
        shouldShare = false;

        imageTransform = transform.GetChild(1);
        showCertificate();
        createCertificate();
        points.SetActive(false);
    }


    void showCertificate()
    {
        imageTransform.GetChild(0).GetComponent<TMP_Text>().text = Information.name; //it was name, mabye replace it later 
        imageTransform.GetChild(1).GetComponent<TMP_Text>().text = Information.acheivment;
        imageTransform.gameObject.SetActive(true);
        StartCoroutine(showPopUp());

    }

    public Vector3 showStart;
    public Vector3 showEnd;

    public GameObject sharePopUp;

    IEnumerator showPopUp()
    {
        float count = 0;
        while (count < 1)
        {
            yield return new WaitForSeconds(1);
            count++;
        }

        sharePopUp.SetActive(true);
        count = 0;

        while (count <= 1)
        {
            count += 0.1f;
            sharePopUp.transform.localPosition = Vector3.Lerp(showStart, showEnd, count);

            yield return new WaitForSeconds(0.02f);
        }
    }

    void initButtons()
    {
        warningPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { takeWarningBack(); });

        continueButton.onClick.AddListener(delegate { takeContinue(); });
        share.onClick.AddListener(delegate { takeShare(); });
        collectButton.onClick.AddListener(delegate { takeCollect(); });
    }

    void takeWarningBack()
    {
        source.clip = buttonSound;
        source.Play();
        warningPanel.gameObject.SetActive(false);
    }


    void takeContinue()
    {
        source.clip = buttonSound;
        source.Play();
        sharePopUp.SetActive(false);
    }

    void takeCollect()
    {
        StartCoroutine(panel.panelAnimation(false, transform));
    }

    void takeShare()
    {
        source.clip = buttonSound;
        source.Play();

        #if UNITY_ANDROID || UNITY_IOS
                toShare= true;
        #else
                toShare = false;
                warningPanel.SetActive(true);
        #endif 


    }

    public void createCertificate()
    {
        if (Information.acheivment.Length > 1 && Information.xmlDoc != null)
        {
            certificate = new XElement("ach", new XAttribute("name", Information.acheivment), new XAttribute("date", ""), new XAttribute("socialMedia", ""));
            Information.xmlDoc.Root.Element("certificates").Add(certificate);
            Information.xmlDoc.Save(Information.xmlFileDir);
        }

        Information.totalEarnedPoints += 10;

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

        //    UnityNativeSharingHelper.ShareScreenshotAndText(text, screenshotPath, false, "Select App To Share With");
        new NativeShare().AddFile(screenshotPath).SetText("Check out my achievement on Dive!").Share();
        certificate.Attribute("date").Value = DateTime.Today.ToString("MM/dd/yyyy");
        XMLWriter.saveFile();
    }


    bool shouldShare = false;
    void Update()
    {
        if (toShare && screenshotPath != null && !shared)
        {
            StartCoroutine(shareTextAndScreenShot(screenshotPath, "Check out my achievment on Dive!"));
            shouldShare = true;
        }

        if (shouldShare && didTakeScreenshot)
        {
            points.SetActive(true);
            shared = true;
        }
    }
}

