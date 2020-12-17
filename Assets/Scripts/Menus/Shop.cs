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

    public GameObject teir1;
    public GameObject teir2;
    public GameObject teir3;

    public GameObject lowerBound;
    public GameObject upperBound;
    public GameObject cursor;
    public Button back;

    public GameObject priceTag;
    public GameObject shopContainer;

    public GameObject popup;
    public Text outputText;

    List<Item> items;
    List<GameObject> buttons;
    List<int> headerIndeciesList;

    public AudioSource source;
    public AudioClip buttonClick;
    public AudioClip soundBell;
    public AudioClip register;

    public Button giftCardbutton;
    public Button giftCardBack;
    public GameObject giftCardContainer;
    public GameObject giftCardPurchase;

    public GameObject savingPanel;
    public Button buyAnyway;
    public Button saveButton;


    int maxAllowedDivePoints = 0;


    void Start()
    {

        Information.lowerBoundary = lowerBound;
        Information.upperBoundary = upperBound;

        headerIndeciesList = new List<int>();
        initButtons();
        //      createShop();

        initSlider();
        amountslider.onValueChanged.AddListener(delegate { sliderValueChanged(); });

        gameObject.GetComponent<AudioSource>().clip = soundBell;
        gameObject.GetComponent<AudioSource>().Play();
        Information.currentScene = "Shop";
        giftCardbutton.onClick.AddListener(delegate { takeGiftCard(); });
        giftCardBack.onClick.AddListener(delegate { takeCardback(); });
        giftCardWarning.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeWarningBack(); });
        giftCardWarning.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { takeWarningNotOk(); });

        initGiftCard();

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

    public TMPro.TMP_Text emailText;
    public InputField emailField;
    public Image image;
    public Button changeEmailButton;
    public Button backButton;
    public Button confirmButton;
    public InputField amountField;
    public Button PurchaseOkButton;
    public Button okOverSpending;
    public Button MoreOverSpending;

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
        PurchaseOkButton.GetComponent<Button>().onClick.AddListener(delegate { takePurchaseOk(); });
        okOverSpending.onClick.AddListener(delegate { takeOkOverSpending(); });
        MoreOverSpending.onClick.AddListener(delegate { takeMoreOverSpending(); });

        buyAnyway.onClick.AddListener(delegate { takePurchaseConfirm(); }); //here is where you actually buy the gift card
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

    void takePurchaseOk()
    {
        giftCardWarning.SetActive(false);
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

    void takePurchaseBack()
    {
        source.clip = buttonClick;
        source.Play();

        giftCardPurchase.SetActive(false);
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
            erorrText.text = "Not enought Dive points!";
            amountslider.interactable = false;
        }

    }

    public GameObject giftCardWarning;
    void takePurchaseConfirm()
    {
        string email = emailText.text;

        int amountInt = 0;
        bool showAmountWarning = false;
        string amountWarningText = "Please make sure the amount of Dive points you enter is a whole number, and less then your current balance.";
        // string amountWarningTitle = "Invalid amount";

        string emailWarningText = "Please enter a valid email, this is where we will send the gift card to!";
        //   string emailWarningTitle = "Invalid email";


        if (showAmountWarning)
        {
            giftCardWarning.SetActive(true);
            // giftCardWarning.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = amountWarningText;
            // giftCardWarning.transform.GetChild(2).GetComponent<Text>().text = amountWarningText;

            return;
        }

        if (!checkEmail(email))
        {
            giftCardWarning.SetActive(true);
            //  giftCardWarning.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = emailWarningText;
            //   giftCardWarning.transform.GetChild(2).GetComponent<Text>().text = emailWarningText;
            return;
        }

        source.clip = register;
        source.Play();

        writeGiftCardToWebsite(email);

    }

    //public GameObject PurchasePopUp;


    IEnumerator handleGiftCard()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();

        //here just put how many pionts htey have 
        int newAmount = Information.totalEarnedPoints - dollarAmount * 500;
        Debug.LogError(newAmount + " new amount");

        string output = "";
        if (newAmount < 0)
        {
            output = "bad";
        }
        else
        {
            Information.maxDivePoints -= dollarAmount * 500;
            string buyInfo = Information.buyUrl + Information.username + "/" + Information.pastPointsDate + "&" + Information.totalEarnedPoints + "&" + Information.maxDivePoints + "/" + currentCardName + "&" + dollarAmount;
            UnityWebRequest uwr = UnityWebRequest.Get(buyInfo);
            uwr.chunkedTransfer = false;
            uwr.certificateHandler = certHandler;

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                giftCardWarning.transform.GetChild(1).GetComponent<TMP_Text>().text = "Error";
                giftCardWarning.transform.GetChild(2).GetComponent<TMP_Text>().text = "Something went wrong, please try again later!";
                yield break;
            }
            else
            {
                Debug.Log("Received");
            }
            output = uwr.downloadHandler.text;
        }


        if (Information.username == "guest@email.com")
        {
            output = "good";
        }

        if (output.Contains("good"))
        {

            // outputText.text = "Success!";
            giftCardWarning.transform.GetChild(1).GetComponent<TMP_Text>().text = "Success";
            giftCardWarning.transform.GetChild(2).GetComponent<TMP_Text>().text = "Gift card purchased! It will appear in your email in 3-5 days";
            source.clip = register;
            source.Play();

        }
        else if (output.Contains("bad"))
        {

            // outputText.text = "Not enought points";
            giftCardWarning.transform.GetChild(1).GetComponent<TMP_Text>().text = "Failed";
            giftCardWarning.transform.GetChild(2).GetComponent<TMP_Text>().text = "Not enough points! Earn some more points and come back later!";
        }
        savingPanel.SetActive(false);
        giftCardWarning.SetActive(true);
        giftCardPurchase.SetActive(false);

        //  outputText.gameObject.SetActive(true);
    }


    void writeGiftCardToWebsite(string email)
    {

        if (dollarAmount * 500 > Information.maxDivePoints)
        {
            warningObject.gameObject.SetActive(true);
            return;
        }

        StartCoroutine(handleGiftCard());

    }


    bool checkEmail(string email)
    {
        String[] atCharacter;
        String[] dotCharacter;
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
        emailText.text = Information.username;
        for (int i = 0; i < giftCardContainer.transform.GetChild(2).childCount; i++)
        {
            if (curr == giftCardContainer.transform.GetChild(2).GetChild(i).gameObject)
            {
                currentCardName = giftCardNames[i];
                break;

            }
        }
    }

    void takeGiftCard()
    {
        giftCardContainer.gameObject.SetActive(true);

    }




    Vector2 tagOffset = new Vector2(58, 24);

    void initButtons()
    {
        popup.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takePopUpYes(); });
        popup.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takePopUpNo(); });
        popup.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { takeChangeAddress(); });
        popup.transform.GetChild(4).GetChild(3).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeAddressSubmit(); });
        popup.transform.GetChild(4).GetChild(3).GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { takeAddressCancel(); });

        back.onClick.AddListener(delegate { takeBack(); });
    }

    void takeChangeAddress()
    {
        //here you need to handle changing their address, and saving it (it should be kept in the database, not the file)
        popup.transform.GetChild(4).GetChild(3).gameObject.SetActive(true);

    }

    void takeAddressSubmit()
    {
        string address1 = popup.transform.GetChild(4).GetChild(3).GetChild(1).GetComponent<InputField>().text;
        //  string address2 = "";//popup.transform.GetChild(4).GetChild(3).GetChild(2).GetComponent<InputField>().text;

        //   Website.GET(Information.putAddressUrl + Information.username + "/" + address1);
        popup.transform.GetChild(4).GetChild(3).gameObject.SetActive(false);
        Information.address = address1;
        popup.transform.GetChild(4).GetChild(0).GetChild(1).GetComponent<Text>().text = address1;

    }

    void takeAddressCancel()
    {
        popup.transform.GetChild(4).GetChild(3).gameObject.SetActive(false);
    }

    class Item
    {
        public string name;
        public string price;
        public string teir;
        public string image;
        public Sprite sprite;
        public List<string> choices;

        public Item()
        {
            name = "";
            image = "";
            choices = new List<string>();

        }
        public void checkImage(Button button)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/shop"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/shop");

            }
            // Try to create the directory.
            string filePath = Application.persistentDataPath + "/shop/" + name + ".png";
            if (!File.Exists(filePath))
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(image), filePath);
                }
            }

            byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);

            Texture2D tex = new Texture2D(2, 2);

            button.GetComponent<RawImage>().texture = tex;
        }
    }

    void takeBack()
    {
        SceneManager.LoadScene("StudentMenu");
    }


    /*  void createShop()
      {
          items = new List<Item>();
          string[] output = Website.GET(Information.shopUrl).Split('&'); //name points teir image

          if (output.Length < 1 || output[0] == "")
          {
              Debug.LogError("Couldn't get shop: " + output[0]);
              return;
          } 

          for(int i = 0; i < output.Length; i++) 
          {
              string[] currItem = output[i].Split(',');
              Item item = new Item();
              item.name = currItem[0];
              item.price = currItem[1];
              item.teir = currItem[2];
              item.image = @currItem[3];
              string[] currChoices = currItem[4].Split(' ');
              if(currChoices.Length > 0)
              {
                  item.choices = new List<string>(currChoices);
              }
              items.Add(item);
          }
          createButtons();
      }*/

    int[] teirIndecies = new int[] { 0, 0, 0 };
    List<GameObject> userButtons;
    void createButtons()
    {
        userButtons = new List<GameObject>();
        for (int i = 0; i < items.Count; i++)
        {
            int currTeir = int.Parse(items[i].teir) - 1;
            GameObject teirObject = shopContainer.transform.GetChild(currTeir).gameObject;
            if (teirIndecies[currTeir] > teirObject.transform.childCount - 1)
            {
                continue;
            }

            GameObject currentButton = teirObject.transform.GetChild(teirIndecies[currTeir]++).gameObject;
            GameObject currPrice = Instantiate(priceTag, currentButton.transform, false);
            currPrice.transform.SetParent(currPrice.transform.parent.parent);
            currPrice.transform.GetChild(0).GetComponent<Text>().text = items[i].price;
            currPrice.gameObject.SetActive(true);

            Vector2 objectPosition = currentButton.transform.localPosition;
            currPrice.transform.localPosition = objectPosition + tagOffset;

            currentButton.gameObject.SetActive(true);
            items[i].checkImage(currentButton.GetComponent<Button>());
            userButtons.Add(currentButton);
            currentButton.GetComponent<Button>().onClick.AddListener(delegate { showPopUp(currentButton); });
        }

    }


    int currentIndex = 0;
    void showPopUp(GameObject currButton)
    {

        Transform info = popup.transform.GetChild(4);
        info.GetChild(1).GetChild(0).GetComponent<Text>().text = DateTime.Today.AddDays(14).ToString(CultureInfo.CreateSpecificCulture("en-US"));
        info.GetChild(0).GetChild(1).GetComponent<Text>().text = Information.address;

        if (currButton != null)
            popup.transform.GetChild(3).GetComponent<RawImage>().texture = currButton.GetComponent<RawImage>().texture;

        popup.SetActive(true);
        int index = userButtons.IndexOf(currButton);
        popup.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to spend " + items[index].price + " Dive points?";
        currentIndex = index;
        GameObject dropdown = popup.transform.GetChild(4).GetChild(2).GetComponent<Dropdown>().gameObject;
        dropdown.GetComponent<Dropdown>().ClearOptions();
        if (items[index].choices.Count > 1)
        {
            dropdown.GetComponent<Dropdown>().AddOptions(items[index].choices);
            dropdown.SetActive(true);
        }
        else
        {
            dropdown.SetActive(false);
        }
    }


    public GameObject warningObject;
    void takePopUpYes()
    {

        /*    if(float.Parse(items[currentIndex].price) > Information.maxDivePoints)
            {
                warningObject.gameObject.SetActive(true);
                return;
            }

            popup.gameObject.SetActive(false);
            //call the put function here to take away the points, and add the item
            string buyInfo = Information.buyUrl + Information.username + "/" + items[currentIndex].name + "/" + items[currentIndex].price;
            if(items[currentIndex].choices.Count > 0)
            {
                buyInfo += "/" + items[currentIndex].choices[popup.transform.GetChild(4).GetChild(3).GetComponent<Dropdown>().value];
            }

       string output = Website.GET(buyInfo);
            if (output.Contains("good"))
            {
                outputText.text = "Success!";
                gameObject.GetComponent<AudioSource>().clip = register;
                gameObject.GetComponent<AudioSource>().Play();
            } else if (output.Contains("bad"))
            {

                outputText.text = "Not enought points";
            }
            outputText.gameObject.SetActive(true);   */
    }

    void takePopUpNo()
    {
        popup.gameObject.SetActive(false);
    }

}
