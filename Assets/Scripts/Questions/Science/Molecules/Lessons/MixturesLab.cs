using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixturesLab : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public GameObject choosePanel;
    public Sprite[] mixtureSprites;
    public TMPro.TMP_Text outputText;

    public GameObject mixtureContainer;



    List<GameObject> newEntities;

    string[] mixturesDropdown = new string[] { "Solutions", "Suspensions", "Alloys", "Colloids" };
    int[] dropDownIndecies = new int[] { 3, 4, 8, 9 };
    int[] startIndecies = new int[] { 0, 1, 2 };

    string[] alloys = new string[] { "Bronze", "Steel", "Solder" };
    int[] alloyIndecies = new int[] { 5, 6, 7 };





    Dictionary<int, int> alloyPanel;
    Dictionary<int, int> mixturePanel;

    Dictionary<int, int> reactionPanel;

    string[] reactionNames = new string[] { "Creating water", "Photosynthesis", "Creating salt", "Splitting water", "Decomposing Lithium Carbonate", "Potassium Chloride", "Burning methane", "Zinc in acid",
        "Iron and Copper sulfate",  "neutralizing an acid", "Salt and Silver Nitrate", "Baking soda and Vinegar" };
    int[] reactionIndecies = { 0, 1, 2, 3, 4, 5, 6, 7, };

    public GameObject chooseCanvas;

    public MixturesLab() : base()
    {

    }

    public void initPS()
    {
        var main1 = ps.main;
        main1.startColor = Color.red;
        var main2 = ps2.main;
        main2.startColor = Color.blue;

        var col = ps.colorBySpeed;
        col.enabled = false;

        col = ps2.colorBySpeed;
        col.enabled = false;

        var em = ps2.emission;
        em.enabled = true;
        ps.Stop();
        ps2.Stop();
    }

    void initPanels()
    {

        alloyPanel = new Dictionary<int, int>();
        for (int i = 0; i < alloys.Length; i++)
        {
            alloyPanel.Add(i, alloyIndecies[i]);
        }

        mixturePanel = new Dictionary<int, int>();
        for (int i = 0; i < startIndecies.Length; i++)
        {
            mixturePanel.Add(i, startIndecies[i]);
        }



        reactionPanel = new Dictionary<int, int>();
        for (int i = 0; i < reactionIndecies.Length; i++)
        {
            reactionPanel.Add(i, reactionIndecies[i]);
        }

    }
    public void mixturesLab() //4
    {
        chooseCanvas.SetActive(true);
        choosePanel.SetActive(true);
        newEntities = new List<GameObject>();
        for (int i = 0; i < mixtureSprites.Length; i++)
        {
            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = mixtureSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = mixturesDropdown[i];

            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();

        initPS();
        initPanels();

        outputText.gameObject.SetActive(true);
        List<string> mixturesList = new List<string>(mixturesDropdown);
    }

}
