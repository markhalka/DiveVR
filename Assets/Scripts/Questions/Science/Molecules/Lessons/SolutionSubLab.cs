using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionSubLab : Lab
{
    Dictionary<int, int> solutionPanel;
    int[] solutionIndecies = new int[] { 10 };
    public SolutionSubLab()
    {

        solutionPanel = new Dictionary<int, int>();
        for (int i = 0; i < solutionIndecies.Length; i++)
        {
            solutionPanel.Add(10 * (i + 1), solutionIndecies[i]);
        }

        setText("Use the slider below to set the concentration!");
        setSlider();

        plainSlider.onValueChanged.AddListener(delegate { solutionSlider(); });

        ps.Play();
        var color = ps.main.startColor;
        color = Color.red;
        color = ps2.main.startColor;
        color = Color.blue;
    }

    void solutionSlider()
    {
        showPanel(solutionPanel);

        int amount = (int)plainSlider.maxValue / mixtures.Length;

        for (int i = 0; i < mixtures.Length; i++)
        {

            if (plainSlider.value < amount * (i + 1))
            {
                outputText.text = "This mixtures has a concentration of: " + mixtures[i] + " mol/L";

                updateAmountOfParticles(ps2, amount * (i)); //was i+1
                ps2.Play();
                break;
            }
        }
        var col = ps2.collision;
        col.enabled = true;
    }

  
}
