using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermalEnergyLab : MonoBehaviour
{

    //ok, plan for adding the videos and new text:
    //1. for the new text, you can probably just copy paste it as usual into the data docs



    public Slider slider;
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public Dropdown dropdown;
    public TMPro.TMP_Text outputText;
    public Material[] changeMaterials;

    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;
    public GameObject forward;
    public GameObject backward;

    public InformationPanel infoPanel;

    public GameObject currentShowPanel;

    List<int> sliderValues;

    //    public 

    string[] changeCompounds = new string[] { "nitrogen", "alcohol", "water", "iron" };
    string[,] changeValue = new string[,] { { "-210", "-195" }, { "-115", "79" }, { "0", "100" }, { "1538", "2862" } }; //melting, boiling point


    public void Start()
    {      
        
        initSlider();
        initDropdown();
        initPS();
        initSliderValues();
        infoPanel.gameObject.SetActive(true);
        infoPanel.initStartPanels();

        outputText.text = "Set the drop-down to select you material, then move the slider to change the temperature!"; //FIX THIS SOMEHOW

    }

    void initSliderValues()
    {
        sliderValues = new List<int>();
        var triggerValues = new int[] { 5, 25, 45, 65, 75, 90};
        for (int i = 0; i < triggerValues.Length; i++)
        {
            sliderValues.Add(triggerValues[i]);
        }
    }

    void initSlider()
    {
        slider.gameObject.SetActive(true);
        slider.onValueChanged.AddListener(delegate { thermalSlider(); });
    }


    void initDropdown()
    {
        dropdown.gameObject.SetActive(true);
        dropdown.AddOptions(new List<string>(changeCompounds));
        dropdown.onValueChanged.AddListener(delegate { changeDropdown(); });
        changeDropdown();
    }

    void initPS()
    {
        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = changeMaterials[0];
        var col = ps.colorBySpeed;
        col.enabled = false;
    }

    void changeDropdown()
    {

        int curr = dropdown.value;
        //ok so here you can change the max and min values 
        slider.transform.GetChild(3).GetComponent<TMPro.TMP_Text>().text = changeValue[curr, 0].ToString();
        slider.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = changeValue[curr, 1].ToString();
        var main = ps.main;
        ps.GetComponent<ParticleSystemRenderer>().material = changeMaterials[curr];
        ps.Play();

    }

    int lastShown = 0;
    void checkSliderShow(int value)
    {
        int index = 0;
        for(index = 0; index < sliderValues.Count; index++)
        {
            if(sliderValues[index] > value)
            {
                break;
            }
        }
        if(index <= lastShown || index >= sliderValues.Count)
        {
            return;
        }

        Information.panelIndex = index + infoPanel.startOffset; // and then figure out what to do here
        infoPanel.loadNewModel();
        lastShown = index;
    }

    void thermalSlider()
    {        
        if (currentShowPanel.activeSelf)
        {
            slider.interactable = false;
            return;
        } else
        {
            checkSliderShow((int)slider.value);
        }

        
        var noise = ps.noise;
        float inversePercentage = (1 - slider.value / (slider.maxValue)) + 0.2f; ;

        float percentage = slider.value / slider.maxValue * 10;
        changeDensity(slider.value / 30);

        noise.frequency = inversePercentage * 2;
        noise.positionAmount = percentage * 1.3f;

        

        if (slider.value < 20)
        {
            outputText.text = "The molecules now form a solid";
            previousState = 0;
            //solid
        }
        else if (slider.value < 30)
        {
            if (previousState == 0)
            {
                //it is melting
                outputText.text = "The molecules are now melting";
            }
            else
            {
                outputText.text = "The molecules are now freezing";
                //it is freezing
            }
            //transition
        }
        else if (slider.value < 40)
        {
            previousState = 1;
            outputText.text = "The molecules now form a liquid";

            //liquid
        }
        else if (slider.value < 50)
        {
            if (previousState == 1)
            {
                //it is evaporating
                outputText.text = "The molecules are now boiling";
            }
            else
            {
                //it is condensating
                outputText.text = "The molecules are now condensing";
            }

        }
        else
        {
            previousState = 2;
            outputText.text = "The molecules now form a gas";
            //gas
        }
    }

    int offset = 20;
    float prevValue = 0;
    int previousState = 0;
    void changeDensity(float amount)
    {
        if (prevValue == 0)
        {
            prevValue = slider.value / 30;
            return;
        }
        top.transform.Translate(new Vector3(0, 0, (prevValue - amount) * offset));
        bottom.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));

        left.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));
        right.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));


        forward.transform.Translate(new Vector3(0, 0, (prevValue - amount) * offset));
        backward.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));
        prevValue = amount;

    }

    private void Update()
    {
        if (!currentShowPanel.activeSelf)
        {
            slider.interactable = true;
        }
    }
}
