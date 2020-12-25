using PlayFab.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{



    public Slider slider;
    public Button lessonSettings;
    public Button reportProblem;
    public Button back;

    public GameObject problemPanel;
    public Button submitProblem;
    public Button backProblem;

    public TMPro.TMP_InputField problemField;

    public Toggle showPretest;
    public Toggle showSurvey;


    bool onSlider = false;


    void Start()
    {
        slider.onValueChanged.AddListener(delegate { takeSlider(); });
        showPretest.onValueChanged.AddListener(delegate { takePreTest(); });
        showSurvey.onValueChanged.AddListener(delegate { takeSurvey(); });

        lessonSettings.onClick.AddListener(delegate { takeLesson(); });
        reportProblem.onClick.AddListener(delegate { takeReport(); });
        back.onClick.AddListener(delegate { takeBack(); });
        submitProblem.onClick.AddListener(delegate { takeSubmitProblem(); });
        backProblem.onClick.AddListener(delegate { takeBackProblem(); });


        slider.value = Information.sensitivity;

        showPretest.isOn = Information.showPreTest;
        showSurvey.isOn = Information.shouldShowSurvey;



    }


    //make a function in mf xmlwriter that can save this 
    void takePreTest()
    {
        Information.showPreTest = !Information.showPreTest;
        XMLWriter.savePreTestConfig();
    }

    void takeSurvey()
    {
        Information.shouldShowSurvey = !Information.shouldShowSurvey;
        XMLWriter.saveSurveyConfig();
    }


    void takeSubmitProblem()
    {
        StartCoroutine(sendProblem());
        problemPanel.SetActive(false);
    }

    IEnumerator sendProblem()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();

        string toSend = ParseData.encodeDate() + " " + problemField.text;

        UnityWebRequest uwr = UnityWebRequest.Get(Information.sendProblemUrl + Information.username + "/" + toSend);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        Debug.LogError("saving name output; " + uwr.downloadHandler.text);

        //save the name here 
        yield break;
    }

    void takeBackProblem()
    {
        problemPanel.SetActive(false);
    }

    private void Update()
    {

    }

    void takeLesson()
    {
        if (Information.grade.Length > 3 && Information.subject.Length > 3)
        {
            SceneManager.LoadScene("Curriculum");
        }

    }

    void takeReport()
    {
        problemPanel.SetActive(true);
    }






    void takeSlider()
    {
        Information.sensitivity = slider.value;
    }

    void takePicture()
    {

    }

    void takeBack()
    {
        gameObject.SetActive(false);
    }



}
