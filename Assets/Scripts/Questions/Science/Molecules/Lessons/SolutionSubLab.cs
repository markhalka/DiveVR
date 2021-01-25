using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolutionSubLab : MonoBehaviour
{

    //  ok, so there is no dropdown here, its just the slider and you change the concentration

    public Slider plainSlider;
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public TMP_Text outputText;
    Lab util;

    public InformationPanel infoPanel;

    string[] mixtures = new string[] { "0.1", "1", "1.5", "2.5" };

    List<int> sliderValues;

    public GameObject currentShowPanel;

    public int startIndex = 0;

    public void Start()
    {
        util = new Lab();

        initPS();
        initSliderValues();

        outputText.text = "Move the slider to change the concentration!";
        outputText.gameObject.SetActive(true);
        plainSlider.gameObject.SetActive(true);
        plainSlider.onValueChanged.AddListener(delegate { solutionSlider(); });        
    }

    void initPS()
    {
        ps.Play();
        var color = ps.main.startColor;
        color = Color.red;
        color = ps2.main.startColor;
        color = Color.blue;
    }

    void initSliderValues() // solution indicies is 10, check data and see wtf is up with this lesson
    {
        sliderValues = new List<int>();
        var triggerValues = new int[] { 5, 15, 25, 35, 45, 55 };
        for (int i = 0; i < triggerValues.Length; i++)
        {
            sliderValues.Add(triggerValues[i]);
        }
    }

    void solutionSlider()
    {
        if (currentShowPanel.activeSelf)
        {
            plainSlider.interactable = false;
            return;
        }
        else
        {
            showPanel((int)plainSlider.value);
        }

        int amount = (int)plainSlider.maxValue / mixtures.Length;

        for (int i = 0; i < mixtures.Length; i++)
        {

            if (plainSlider.value < amount * (i + 1))
            {
                outputText.text = "This mixtures has a concentration of: " + mixtures[i] + " mol/L";

                util.updateAmountOfParticles(ps2, amount * (i)); //was i+1
                ps2.Play();
                break;
            }
        }
        var col = ps2.collision;
        col.enabled = true;
    }
    int lastShown = 0;

    void showPanel(int value)
    {
        Debug.LogError("showing panel...");
        int index = 0;
        for (index = 0; index < sliderValues.Count; index++)
        {
            if (sliderValues[index] > value)
            {
                break;
            }
        }
        if (index < lastShown)
        {
            return;
        }

        Information.panelIndex = index + infoPanel.startOffset + startIndex; // from molecule menu you should set a thing here, and just add it as well?
        infoPanel.loadNewModel();
        lastShown = index;
    }


    private void Update()
    {
        if (!currentShowPanel.activeSelf)
        {
            plainSlider.interactable = true;
        }
    }


}
