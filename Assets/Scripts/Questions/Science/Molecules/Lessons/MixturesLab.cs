using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixturesLab : MonoBehaviour
{


    public ParticleSystem ps;
    public ParticleSystem ps2;
 
    public TMPro.TMP_Text outputText;
   
    List<GameObject> newEntities;


    int[] startIndecies = new int[] { 0, 1, 2 };

    string[] alloys = new string[] { "Bronze", "Steel", "Solder" };
    int[] alloyIndecies = new int[] { 5, 6, 7 };

    // int[] dropDownIndecies = new int[] { 3, 4, 8, 9 };

    Dictionary<int, int> alloyPanel;
    Dictionary<int, int> mixturePanel;

    public int infoPanelStart = 0;




   /* public void initPanels()
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
    } */
}
