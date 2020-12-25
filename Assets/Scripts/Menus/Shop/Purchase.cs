using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchase : MonoBehaviour //this is giftcardpurhcase
{
    public GameObject warningObject;
    public GameObject savingPanel;
    public GameObject giftCardWarning;

    public Button PurchaseOkButton;

    public TMPro.TMP_Text emailText;

    public AudioSource source;
    public AudioClip buttonClick;
    public AudioClip soundBell;
    public AudioClip register;

    Website network;

    private void Start()
    {
        giftCardWarning.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeWarningBack(); });
        giftCardWarning.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { takeWarningNotOk(); });

        PurchaseOkButton.GetComponent<Button>().onClick.AddListener(delegate { takePurchaseOk(); });
        network = new Website();
    }

    void takePurchaseOk()
    {
        giftCardWarning.SetActive(false);
    }

    void takeWarningNotOk()
    {
        Application.OpenURL("www.divevr.org");
    }

    void takeWarningBack()
    {
        source.clip = buttonClick;
        source.Play();

        giftCardWarning.SetActive(false);
    }


    void handleGiftCard(string output)
    {
        if (Information.username == "guest@email.com")
        {
            output = "good";
        }

        if (output.Contains("good"))
        {
            giftCardWarning.transform.GetChild(1).GetComponent<TMP_Text>().text = "Success";
            giftCardWarning.transform.GetChild(2).GetComponent<TMP_Text>().text = "Gift card purchased! It will appear in your email in 3-5 days";
            source.clip = register;
            source.Play();

        }
        else if (output.Contains("bad"))
        {
            giftCardWarning.transform.GetChild(1).GetComponent<TMP_Text>().text = "Failed";
            giftCardWarning.transform.GetChild(2).GetComponent<TMP_Text>().text = "Not enough points! Earn some more points and come back later!";
        }
        savingPanel.SetActive(false);
        giftCardWarning.SetActive(true);
        gameObject.SetActive(false);
    }

    public void checkPurchase(string email, string currentCardName, int dollarAmount)
    {
        bool showAmountWarning = false;
        string amountWarningText = "Please make sure the amount of Dive points you enter is a whole number, and less then your current balance.";
        string amountWarningTitle = "Invalid amount";

        string emailWarningText = "Please enter a valid email, this is where we will send the gift card to!";
        string emailWarningTitle = "Invalid email";


        if (showAmountWarning)
        {
            giftCardWarning.SetActive(true);
            giftCardWarning.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = amountWarningTitle;
            giftCardWarning.transform.GetChild(2).GetComponent<Text>().text = amountWarningText;

            return;
        }

        if (!checkEmail(email))
        {
            giftCardWarning.SetActive(true);
            giftCardWarning.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = emailWarningTitle;
            giftCardWarning.transform.GetChild(2).GetComponent<Text>().text = emailWarningText;
            return;
        }

        if (dollarAmount * 500 > Information.maxDivePoints)
        {
            warningObject.gameObject.SetActive(true);
            return;
        }

        source.clip = register;
        source.Play();

        writeGiftCardToWebsite(email, currentCardName, dollarAmount);
    }


    void writeGiftCardToWebsite(string email, string currentCardName, int dollarAmount)
    {
        int newAmount = Information.totalEarnedPoints - dollarAmount * 500;

        Information.maxDivePoints -= dollarAmount * 500;
        string buyInfo = Information.buyUrl + Information.username + "/" + Information.pastPointsDate + "&" + Information.totalEarnedPoints + "&" + Information.maxDivePoints + "/" + currentCardName + "&" + dollarAmount;

        StartCoroutine(network.GetRequest(buyInfo, handleGiftCard));

    }


    bool checkEmail(string email)
    {
        string[] atCharacter;
        string[] dotCharacter;
        atCharacter = email.Split("@"[0]);
        if (email.Length < 2)
        {
            return false;
        }
        if (atCharacter.Length == 2)
        {
            dotCharacter = atCharacter[1].Split("."[0]);
            if (dotCharacter.Length >= 2)
            {
                if (dotCharacter[dotCharacter.Length - 1].Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}
