using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab
{
        public static Text outputText;
        public static Dropdown dropdown;
        public static Slider slider;
        public static Slider plainSlider;

        public static ParticleSystem ps; //this needs to be initiated
        public static ParticleSystem ps2; //this needs to be initiated
    

    public static GameObject panel; //this is the information apnel


        public static void setSlider()
        {
            plainSlider.gameObject.SetActive(true);
        }

        public static void setText(string text)
        {
            outputText.text = "Set the dropdown to select you material, then move the slider to change the temprature!";
            outputText.gameObject.SetActive(true);
        }

        public static void showPanel(Dictionary<int, int> dict)
        {
            float sliderValue = plainSlider.value;
            if (dict != null && !panel.activeSelf)
            {
                int lowestIndex = 100;
                foreach (var value in dict)
                {


                    if (value.Key <= sliderValue && !Information.userModels[value.Value].wasShown)
                    {
                        if (value.Value < lowestIndex)
                            lowestIndex = value.Value;
                    }
                }

                if (lowestIndex >= 0 && lowestIndex < Information.userModels.Count)
                {
                    Information.panelIndex = lowestIndex;
                    panel.SetActive(true);
                    Information.userModels[lowestIndex].wasShown = true;
                }
            }
        }



    


    Dictionary<int, int> sliderPanelValues;
    Dictionary<int, int> sliderTimeValues;

    public virtual void initPS() { }
    public virtual void initDropdown() { }

    public Lab()
    {

    }

    public void loadPanelValues(List<int> time, int[] indecies)
    {
        sliderPanelValues = new Dictionary<int, int>();
        for (int i = 0; i < time.Count; i++)
        {
            sliderPanelValues.Add(time[i], indecies[i] + modelOffset); //? double check that 
        }
    }

    public virtual void update()
    {
        updateSlider();
    }

    public void updateSlider()
    {
        if (sliderPanelValues != null)
        {
            showPanel(sliderPanelValues, slider.value);
        }
    }
}
