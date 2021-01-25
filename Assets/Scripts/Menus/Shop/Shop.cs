using PlayFab.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject giftCardContainer;
    public GameObject giftCardPurchase;
    public GameObject warningObject;
    public GameObject savingPanel;

    public Button giftCardBack;
    public Button saveButton;
    public Button buyAnyway;
    public Button changeEmailButton;
    public Button backButton;
    public Button confirmButton;
    public Button okOverSpending;
    public Button MoreOverSpending;
    public Button back;

    public Text outputText;
    public TMPro.TMP_Text emailText;
    public InputField emailField;
    public InputField amountField;
    public Image image;

    public AudioSource source;
    public AudioClip buttonClick;
    public AudioClip soundBell;
    public AudioClip register;

    void Start()
    {
        initSlider();
        amountslider.onValueChanged.AddListener(delegate { sliderValueChanged(); });

        gameObject.GetComponent<AudioSource>().clip = soundBell;
        gameObject.GetComponent<AudioSource>().Play();
        Information.currentScene = "Shop";
 //       giftCardBack.onClick.AddListener(delegate { takeCardback(); });
        buyAnyway.onClick.AddListener(delegate { takePurchaseConfirm(); }); 

        initGiftCard();
    }


    void takeCardback()
    {
        source.clip = buttonClick;
        source.Play();
        giftCardContainer.SetActive(false);
    }

    void initGiftCard()
    {
        backButton.GetComponent<Button>().onClick.AddListener(delegate { takePurchaseBack(); });
        confirmButton.GetComponent<Button>().onClick.AddListener(delegate { takeSaving(); });
        changeEmailButton.GetComponent<Button>().onClick.AddListener(delegate { takeChangeEmail(); });
        okOverSpending.onClick.AddListener(delegate { takeOkOverSpending(); });
        MoreOverSpending.onClick.AddListener(delegate { takeMoreOverSpending(); });     
        saveButton.onClick.AddListener(delegate { takeSaveButton(); }); //this should close, it and return to student menu

        initGiftCardButtons();
    }

    void takeSaving()
    {
        savingPanel.SetActive(true);
        int savingAmount = (Information.totalEarnedPoints % 1500);
        if (savingAmount == 0)
        {
            savingAmount = 1500;
        }
        savingPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "If you saved " + savingAmount + " more Dive points, you would get $1 for free, just for saving!";
    }

    void takeSaveButton()
    {
        savingPanel.SetActive(false);
        //    SceneManager.LoadScene("StudentMenu");
    }


    void takeMoreOverSpending()
    {
        Application.OpenURL("www.divevr.org");

    }

    void takeOkOverSpending()
    {
        warningObject.SetActive(false);
    }

    void takePurchaseBack()
    {
        source.clip = buttonClick;
        source.Play();

        giftCardPurchase.SetActive(false);
    }

    void takeChangeEmail()
    {
        source.clip = buttonClick;
        source.Play();

        string text = "Change email";
        emailField.gameObject.SetActive(!emailField.gameObject.activeSelf);
        emailText.gameObject.SetActive(!emailField.gameObject.activeSelf);
        if (emailField.gameObject.activeSelf)
        {
            text = "Save email";
        }
        else
        {
            text = "Change email";
            emailText.text = emailField.text;

        }

        changeEmailButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = text;
    }


    int dollarAmount = 0;

    public TMP_Text amountText;
    public TMP_Text erorrText;
    void sliderValueChanged()
    {
        dollarAmount = (int)amountslider.value / 500;
        dollarAmount += (int)amountslider.value / 1500; //this is the extra shit 
        amountText.text = "$" + dollarAmount;

    }

    public Slider amountslider;
    void initSlider()
    {
        //the maximum will be their dive pionts 
        amountslider.maxValue = Information.totalEarnedPoints;

        if (Information.totalEarnedPoints < 500)
        {
            erorrText.text = "Not enough Dive points!";
            amountslider.interactable = false;
        }

    }


    void takePurchaseConfirm()
    {

        giftCardPurchase.GetComponent<Purchase>().checkPurchase(emailField.text, currentCardName, dollarAmount); //all info here
           
    }

    List<GameObject> giftCards;
    void initGiftCardButtons()
    {
        giftCards = new List<GameObject>();
        for (int i = 0; i < giftCardContainer.transform.GetChild(2).childCount; i++)
        {
            GameObject curr = giftCardContainer.transform.GetChild(2).GetChild(i).gameObject;
            giftCards.Add(curr);
            curr.GetComponent<Button>().onClick.AddListener(delegate { takeGiftCardClick(curr); });

        }
    }

    string currentCardName = "";
    string[] giftCardNames = new string[] { "amazon", "itunes", "google", "indigo", "playstation", "timHortons", "xbox", "walmart" };
    void takeGiftCardClick(GameObject curr)
    {
        source.clip = buttonClick;
        source.Play();

        giftCardPurchase.gameObject.SetActive(true);
        image.sprite = curr.GetComponent<Image>().sprite;
        emailText.text = "test@email.com"; //Information.username;
        for (int i = 0; i < giftCardContainer.transform.GetChild(2).childCount; i++)
        {
            if (curr == giftCardContainer.transform.GetChild(2).GetChild(i).gameObject)
            {
                currentCardName = giftCardNames[i];
                break;

            }
        }
    }
}