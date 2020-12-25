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



    void Start()
    {
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
        //  StartCoroutine(startAnimation());
        // StartCoroutine(Screenshot()); //that should work
        showCertificate();
        createCertificate();
        //  Debug.Log(SocialMediaPanel.transform + " Social media panel");
        points.SetActive(false);
    }


    int fileCounter = 0;
    int timeCounter = 0;
    int timeDelay = 100;

    public Canvas canvas;
    /*  IEnumerator Screenshot()
      {


          fileCounter = 0;

          while (timeCounter < timeDelay)
          {
              timeCounter++;

              yield return new WaitForSeconds(0.01f);
          }



          yield return new WaitForEndOfFrame();

          int width = Screen.width;
          int height = Screen.height;
          int imageWidth =(int) Mathf.Round(141.45f * 3.41358f * canvas.scaleFactor); 
          int imageHeight = (int) Mathf.Round(68.4f * 3.41358f * canvas.scaleFactor);
          Texture2D tex = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
          yield return new WaitForEndOfFrame();

             tex.ReadPixels(new Rect(width / 2 - 70.725f * 3.41358f * canvas.scaleFactor, (height / 2 + 34.2f * canvas.scaleFactor) - 34.2f * 3.41358f * canvas.scaleFactor, imageWidth, imageHeight), 0, 0);
          tex.Apply();



          byte[] bytes = tex.EncodeToPNG();
          Destroy(tex);
          var temp = Information.xmlDoc.Root.Element("certificates").Elements();
          foreach(var a in temp)
          {
              fileCounter++;
          }
          if (!Directory.Exists(Application.persistentDataPath + "/Certificates"))
          {
              Directory.CreateDirectory(Application.persistentDataPath + "/Certificates");          
          }

          screenshotPath = Application.persistentDataPath + "/Certificates/" + fileCounter + ".png";

          File.WriteAllBytes(screenshotPath, bytes);
          //you also need to write it to the file somewhere 
    //      fileCounter++;

          didTakeScreenshot = true;
          timeCounter = 0; //for the next screenshot

          yield return (0);
      }*/


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

        //ok, now just lerp it from the bottom
        sharePopUp.SetActive(true);
        count = 0;

        while (count <= 1)
        {
            count += 0.1f;
            sharePopUp.transform.localPosition = Vector3.Lerp(showStart, showEnd, count);

            yield return new WaitForSeconds(0.02f);
        }

    }


    int count = 0;
    int growLength = 70;
    float cycleLength;
    float cycleMultiplier;
    int cycleCount = 0;
    float growthAmount = 1.02f;


    IEnumerator startAnimation()
    {

        count = 0;
        cycleCount = 0;
        cycleLength = growLength / 3;
        cycleMultiplier = 0.4f;

        imageTransform.GetChild(0).GetComponent<TMP_Text>().text = Information.name;
        imageTransform.GetChild(1).GetComponent<TMP_Text>().text = Information.acheivment;

        while (cycleCount == 0 && count < growLength)
        {
            count++;

            imageTransform.transform.localScale *= growthAmount;
            yield return new WaitForSeconds(0.01f);
        }
        count = 0;
        cycleCount++;
        cycleLength *= cycleMultiplier;

        while (cycleLength > 1)
        {
            if (count > cycleLength)
            {
                cycleLength *= cycleMultiplier;
                cycleCount++;
                count = 0;
                continue;
            }

            count++;
            if (cycleCount % 2 == 0) //then it is growing
            {
                imageTransform.transform.localScale *= growthAmount;
            }
            else //then it is shrinking 
            {
                imageTransform.transform.localScale /= growthAmount;
            }
            yield return new WaitForSeconds(0.01f);
        }

        yield break;
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

    #region hideAnimation
    public Vector3 pointsStart = new Vector3(0, 0, 0);
    public Vector3 pointsEnd = new Vector3(-2, -2, 0);

    public float journeyTime = 1.5f;
    public float speed = 0.1f;

    float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    float fraction = 0;

    IEnumerator hideAnimation()
    {

        float count = 0;
        while (count < 1)
        {
            count += 0.1f;
            sharePopUp.transform.localPosition = Vector3.Lerp(showEnd, showStart, count);
            yield return new WaitForSeconds(0.02f);
        }

        transform.parent.gameObject.SetActive(false);

    }


    public void GetCenter(Vector3 direction)
    {
        centerPoint = (pointsStart + pointsEnd) * .5f;
        centerPoint -= direction;
        startRelCenter = pointsStart - centerPoint;
        endRelCenter = pointsEnd - centerPoint;
    }
    #endregion

    void takeContinue()
    {
        source.clip = buttonSound;
        source.Play();
        sharePopUp.SetActive(false);


    }

    void takeCollect()
    {
        StartCoroutine(hideAnimation());
    }

    void takeShare()
    {
        source.clip = buttonSound;
        source.Play();
        toShare = false;
        warningPanel.SetActive(true);
        /*

        #if UNITY_ANDROID || UNITY_IOS
                toShare= true;
        #else
                toShare = false;
                warningPanel.SetActive(true);
        #endif */


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

    IEnumerator pointsAnimation()
    {
        points.gameObject.SetActive(true);
        int count = 0;
        while (count < 2)
        {
            count++;
            yield return new WaitForSeconds(1);
        }

        while (fraction < 100)
        {
            GetCenter(Vector3.up);
            float fracComplete = fraction / 100;
            points.transform.localPosition = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            points.transform.localPosition += centerPoint;

            fraction += 1;
            yield return new WaitForSeconds(0.01f);

        }

        yield break;

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

