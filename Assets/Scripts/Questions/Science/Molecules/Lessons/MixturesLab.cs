using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixturesLab : Lab
{
    string[] mixturesDropdown = new string[] { "Solutions", "Suspensions", "Alloys", "Colloids" };
    int[] dropDownIndecies = new int[] { 3, 4, 8, 9 };
    int[] startIndecies = new int[] { 0, 1, 2 };

    string[] alloys = new string[] { "Bronze", "Steel", "Solder" };
    int[] alloyIndecies = new int[] { 5, 6, 7 };

    int[] mixturePanels = new int[] { 3, 8, 4, 9 };



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

    public override void initPS()
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
        for (int i = 0; i < mixtuerSprites.Length; i++)
        {
            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = mixtuerSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = mixturesDropdown[i];

            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();

        initPS();
        initPanels();

        outputText.gameObject.SetActive(true);
        List<string> mixturesList = new List<string>(mixturesDropdown);
    }


    void showMixturePanel(int index)
    {
        Information.panelIndex = mixturePanels[index];
        InformationPanel.SetActive(true);
    }

    public GameObject mixtureContainer;

    string[] mixtures = new string[] { "0.1", "1", "1.5", "2.5" };

    int offset = 20;

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




    void resetAll()
    {
        ps.Stop();
        ps2.Stop();
        ps3.Stop();
        ps4.Stop();

        ps.Clear();
        ps2.Clear();
        ps3.Clear();
        ps4.Clear();

        var v = ps2.forceOverLifetime;
        v.enabled = false;

        slider.gameObject.SetActive(false);
        plainSlider.gameObject.SetActive(false);
        mix.gameObject.SetActive(false);

    }



    IEnumerator particleAnimation()
    {
        int shakeCount = 2;
        while (shakeCount > 0)
        {

            shakeCount--;

            yield return new WaitForSeconds(1);
        }
        //just increase the noise of the particles 
        var noise1 = ps.noise;
        var noise2 = ps2.noise;
        noise1.positionAmount = 1;
        noise2.positionAmount = 1;

    }

}
