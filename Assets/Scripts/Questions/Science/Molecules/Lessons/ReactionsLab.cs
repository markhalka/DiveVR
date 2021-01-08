using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ReactionsLab : Lab
{
    public Dropdown reactionDropdown;
    public Button reactionStart;

    List<Reaction> reactions;
    List<Reaction> currentReactions;

    public void chemicalReactionsLab()
    {
        reactions = new List<Reaction>();

        Reaction synth2 = new Reaction();
        synth2.name = reactionNames[0]; //creating water
        synth2.start.Add(reactionMaterials[2]); //h2
        synth2.start.Add(reactionMaterials[11]); //02
        synth2.end.Add(reactionMaterials[8]); //h20
        synth2.tempChange = 0;
        synth2.exo = true;
        synth2.reactionType = reaction[1];//reactionTypes.synthesis;
        reactions.Add(synth2);

        Reaction synth1 = new Reaction();
        synth1.name = reactionNames[1]; //photosynthesis
        synth1.start.Add(reactionMaterials[8]); //water
        synth1.start.Add(reactionMaterials[0]); //carbon dioxide 
        synth1.end.Add(reactionMaterials[1]); //glucose 
        synth1.tempChange = 0;
        synth1.exo = false;
        synth1.reactionType = reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth1);

        Reaction synth3 = new Reaction();
        synth3.name = reactionNames[2]; //creating salt
        synth3.start.Add(reactionMaterials[12]); //na
        synth3.start.Add(reactionMaterials[13]); //cl
        synth3.end.Add(reactionMaterials[10]); //na cl 
        synth3.tempChange = 0;
        synth3.exo = false;
        synth3.reactionType = reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth3);

        Reaction decom2 = new Reaction();
        decom2.name = reactionNames[3]; //splitting water
        decom2.start.Add(reactionMaterials[8]); //h20
        decom2.end.Add(reactionMaterials[11]); //02
        decom2.end.Add(reactionMaterials[2]); //h20
        decom2.tempChange = 0;
        decom2.exo = false;
        decom2.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom2);



        //you need to fix these two
        Reaction decom1 = new Reaction();
        decom1.name = reactionNames[4]; //lithium
        decom1.start.Add(reactionMaterials[4]); //Li2CO3
        decom1.end.Add(reactionMaterials[11]); //Li2O not done 
        decom1.end.Add(reactionMaterials[0]); //CO2
        decom1.tempChange = 0;
        decom1.exo = false;
        decom1.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom1);



        Reaction decom3 = new Reaction();
        decom3.name = reactionNames[5]; //pottasium chloride
        decom3.start.Add(reactionMaterials[14]); //potasium chloride
        decom3.end.Add(reactionMaterials[15]); //potassium
        decom3.end.Add(reactionMaterials[16]); //chloride
        decom3.tempChange = 0;
        decom3.exo = false;
        decom3.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom3);

        Reaction combustion = new Reaction();
        combustion.name = reactionNames[6]; //burning methane
        combustion.start.Add(reactionMaterials[5]); //ch4
        combustion.start.Add(reactionMaterials[11]); //02
        combustion.end.Add(reactionMaterials[0]); //co2
        combustion.end.Add(reactionMaterials[8]); //h20
        combustion.exo = true;
        combustion.tempChange = 0;
        combustion.reactionType = reaction[3];//reactionTypes.combustion;
        reactions.Add(combustion);


        Reaction displacment = new Reaction();
        displacment.name = reactionNames[7]; //zinc in acid
        displacment.start.Add(reactionMaterials[7]); //Zn
        displacment.start.Add(reactionMaterials[3]); //HCl
        displacment.end.Add(reactionMaterials[9]); //ZnCl
        displacment.end.Add(reactionMaterials[2]); //h2
        displacment.exo = false;
        displacment.tempChange = 0;
        displacment.reactionType = reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment);

        Reaction displacment2 = new Reaction();
        displacment2.name = reactionNames[8]; //iron and copper sulfate
        displacment2.start.Add(reactionMaterials[16]); //iron suflate
        displacment2.start.Add(reactionMaterials[20]); //copper
        displacment2.end.Add(reactionMaterials[18]); //copper sulfate
        displacment2.end.Add(reactionMaterials[17]); //iron
        displacment2.exo = false;
        displacment2.tempChange = 0;
        displacment2.reactionType = reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment2);


        Reaction doubleDisplacment = new Reaction();
        doubleDisplacment.name = reactionNames[9]; //neutralization
        doubleDisplacment.start.Add(reactionMaterials[2]); //H2SO4
        doubleDisplacment.start.Add(reactionMaterials[11]); //NaOH
        doubleDisplacment.end.Add(reactionMaterials[8]); //h20
        doubleDisplacment.end.Add(reactionMaterials[8]);  // Na2SO4
        doubleDisplacment.tempChange = 0;
        doubleDisplacment.exo = false;
        doubleDisplacment.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(doubleDisplacment);

        Reaction saltSilverNitrate = new Reaction();
        saltSilverNitrate.name = reactionNames[10]; //silver nitrate
        saltSilverNitrate.start.Add(reactionMaterials[10]); //nacl
        saltSilverNitrate.start.Add(reactionMaterials[21]); //silver nitrate
        saltSilverNitrate.end.Add(reactionMaterials[22]); //silver chloride
        saltSilverNitrate.end.Add(reactionMaterials[23]);  // sodium nitrate salt
        saltSilverNitrate.tempChange = 0;
        saltSilverNitrate.exo = false;
        doubleDisplacment.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(saltSilverNitrate);

        //ok, you need to add the sprites for this one 

        Reaction bakingSodaVinegar = new Reaction();
        bakingSodaVinegar.name = reactionNames[11]; //baking soda and vinear
        bakingSodaVinegar.start.Add(reactionMaterials[10]); //vinegar
        bakingSodaVinegar.start.Add(reactionMaterials[21]); //baking soda
        bakingSodaVinegar.end.Add(reactionMaterials[22]); //carbonic acid 
        bakingSodaVinegar.end.Add(reactionMaterials[23]);  // sodium nitrate salt
        bakingSodaVinegar.tempChange = 0;
        bakingSodaVinegar.exo = false;
        bakingSodaVinegar.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(bakingSodaVinegar);

        chooseCanvas.SetActive(true);
        choosePanel.SetActive(true);
        newEntities = new List<GameObject>();
        for (int i = 0; i < reactionSprites.Length; i++)
        {

            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = reactionSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = reaction[i + 1];
            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();


        var col = ps.collision;
        var col2 = ps2.collision;

        col.enabled = true;
        col2.enabled = true;

        var main = ps.main;
        var main2 = ps2.main;
        main.loop = false;
        main2.loop = false;

    }


    string reactionType;

    void showReactionPanel(string name)
    {
        for (int i = 0; i < reactionNames.Length; i++)
        {
            if (reactionNames[i] == name)
            {
                Information.panelIndex = i + 1;
                InformationPanel.SetActive(true);
                return;
            }
        }
    }

    IEnumerator thermometerAnimation(bool exo)
    {
        slider.interactable = false;
        int count = 0;
        int reactionLength = 200;
        slider.gameObject.SetActive(true);
        if (exo)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = 100;
        }
        while (count < reactionLength)
        {
            count++;
            if (exo)
            {
                slider.value++;
            }
            else
            {
                slider.value--;
            }

            yield return new WaitForSeconds(0.01f);
        }

    }


    bool isReacting = false;
    int reactionIndex = -1;

    void onReationStart()
    {
        isReacting = true;
        Reaction currReaction = reactions[reactionIndex];
        StartCoroutine(thermometerAnimation(currReaction.exo));
        var psr2 = ps2.GetComponent<ParticleSystemRenderer>();
        var col = ps2.colorBySpeed;
        if (currReaction.reactionType == reaction[2]) //decomp
        {
            reacting = true;

        }
        else
        {
            col.enabled = false;
            if (currReaction.start.Count > 1)
            {
                psr2.material = currReaction.start[1];
                var em = ps2.emission;
                em.SetBursts(
    new ParticleSystem.Burst[] {
               new ParticleSystem.Burst(0, 300)
    });
                ps2.Play();


            }
        }

        reacting = true;

        var psr3 = ps3.GetComponent<ParticleSystemRenderer>();
        col = ps3.colorBySpeed;
        col.enabled = false;
        psr3.material = currReaction.end[0];
        var emission = ps3.emission;
        emission.rateOverTime = 20;
        ps3.Play();

        if (currReaction.reactionType == reaction[2] || currReaction.reactionType == reaction[4])
        {
            var psr4 = ps4.GetComponent<ParticleSystemRenderer>();
            col = ps4.colorBySpeed;
            col.enabled = false;
            psr4.material = currReaction.end[1];
            emission = ps4.emission;
            emission.rateOverTime = 10;
            ps4.Play();
        }

        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = currReaction.start[0];
        col = ps.colorBySpeed;
        col.enabled = false;

        var emPs = ps.emission;

        emPs.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300)
    });

        ps.Play();
        endParticles = 300;
        StartCoroutine(reactionUpdator());
    }

    int reactionSppedOffset = 100;
    bool reacting = false;
    int endParticles;

    IEnumerator reactionUpdator()
    {

        while (reacting)
        {
            if (endParticles < 20)
            {

                reacting = false;
                yield break;
            }


            updateAmountOfParticles(ps, endParticles);
            if (reactionType == reaction[2])
            {

            }
            else
            {
                updateAmountOfParticles(ps2, endParticles);
            }

            endParticles -= 10;
            yield return new WaitForSeconds(1f);
        }

    }

    public override void update()
    {

        if (reactionIndex != -1)
        {
            if (!InformationPanel.activeSelf && !isReacting)
            {
                onReationStart();
            }
        }
    }
}