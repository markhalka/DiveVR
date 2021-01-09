using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolutionSubLab : MonoBehaviour
{
    public Slider plainSlider;
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public Text outputText;

    Dictionary<int, int> solutionPanel;

    string[] mixtures = new string[] { "0.1", "1", "1.5", "2.5" };
    int[] solutionIndecies = new int[] { 10 };
    public void Start()
    {

        solutionPanel = new Dictionary<int, int>();
        for (int i = 0; i < solutionIndecies.Length; i++)
        {
            solutionPanel.Add(10 * (i + 1), solutionIndecies[i]);
        }

        outputText.text = "Set the dropdown to select you material, then move the slider to change the temprature!";
        outputText.gameObject.SetActive(true);
        plainSlider.gameObject.SetActive(true);

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

    void updateAmountOfParticles(ParticleSystem p, int amount)
    {

        if (p.particleCount < amount)
        {
            var em = p.emission;
            em.SetBursts(
       new ParticleSystem.Burst[]{
                new ParticleSystem.Burst(0, (amount - p.particleCount))

       });
        }
        else
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[p.particleCount];
            p.GetParticles(particles);
            List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>(particles);
            for (int i = particles.Length - 1; i > amount; i--)
            {
                newParticles.RemoveAt(i);
            }
            p.SetParticles(newParticles.ToArray());
        }

    }




}
