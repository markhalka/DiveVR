using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reaction
{
    public string name;
    public List<Material> start;
    public List<Material> end;
    public bool exo;

    public float tempChange;
    public string reactionType;

    public static string[] reaction = new string[] { "Choose a reaction", "Synthesis", "Decomposition", "Displacment", "Double displacment" };

    public Reaction()
    {
        start = new List<Material>();
        end = new List<Material>();
        tempChange = 0;
        reactionType = "";
        exo = false;
    }
}

public class Reactions : MonoBehaviour
{
   

    public List<Reaction> reactions;
    public Material[] reactionMaterials;
    public string[] reactionNames = new string[] { "Creating water", "Photosynthesis", "Creating salt", "Splitting water", "Decomposing Lithium Carbonate", "Potassium Chloride", "Burning methane", "Zinc in acid",
        "Iron and Copper sulfate",  "neutralizing an acid", "Salt and Silver Nitrate", "Baking soda and Vinegar" };

    public void Start()
    {
        createReactions();
    }

    public void createReactions()
    {
        reactions = new List<Reaction>();

        Reaction synth2 = new Reaction();
        synth2.name = reactionNames[0]; //creating water
        synth2.start.Add(reactionMaterials[2]); //h2
        synth2.start.Add(reactionMaterials[11]); //02
        synth2.end.Add(reactionMaterials[8]); //h20
        synth2.tempChange = 0;
        synth2.exo = true;
        synth2.reactionType = Reaction.reaction[1];//reactionTypes.synthesis;
        reactions.Add(synth2);

        Reaction synth1 = new Reaction();
        synth1.name = reactionNames[1]; //photosynthesis
        synth1.start.Add(reactionMaterials[8]); //water
        synth1.start.Add(reactionMaterials[0]); //carbon dioxide 
        synth1.end.Add(reactionMaterials[1]); //glucose 
        synth1.tempChange = 0;
        synth1.exo = false;
        synth1.reactionType = Reaction.reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth1);

        Reaction synth3 = new Reaction();
        synth3.name = reactionNames[2]; //creating salt
        synth3.start.Add(reactionMaterials[12]); //na
        synth3.start.Add(reactionMaterials[13]); //cl
        synth3.end.Add(reactionMaterials[10]); //na cl 
        synth3.tempChange = 0;
        synth3.exo = false;
        synth3.reactionType = Reaction.reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth3);

        Reaction decom2 = new Reaction();
        decom2.name = reactionNames[3]; //splitting water
        decom2.start.Add(reactionMaterials[8]); //h20
        decom2.end.Add(reactionMaterials[11]); //02
        decom2.end.Add(reactionMaterials[2]); //h20
        decom2.tempChange = 0;
        decom2.exo = false;
        decom2.reactionType = Reaction.reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom2);



        //you need to fix these two
        Reaction decom1 = new Reaction();
        decom1.name = reactionNames[4]; //lithium
        decom1.start.Add(reactionMaterials[4]); //Li2CO3
        decom1.end.Add(reactionMaterials[11]); //Li2O not done 
        decom1.end.Add(reactionMaterials[0]); //CO2
        decom1.tempChange = 0;
        decom1.exo = false;
        decom1.reactionType = Reaction.reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom1);



        Reaction decom3 = new Reaction();
        decom3.name = reactionNames[5]; //pottasium chloride
        decom3.start.Add(reactionMaterials[14]); //potasium chloride
        decom3.end.Add(reactionMaterials[15]); //potassium
        decom3.end.Add(reactionMaterials[16]); //chloride
        decom3.tempChange = 0;
        decom3.exo = false;
        decom3.reactionType = Reaction.reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom3);

        Reaction combustion = new Reaction();
        combustion.name = reactionNames[6]; //burning methane
        combustion.start.Add(reactionMaterials[5]); //ch4
        combustion.start.Add(reactionMaterials[11]); //02
        combustion.end.Add(reactionMaterials[0]); //co2
        combustion.end.Add(reactionMaterials[8]); //h20
        combustion.exo = true;
        combustion.tempChange = 0;
        combustion.reactionType = Reaction.reaction[3];//reactionTypes.combustion;
        reactions.Add(combustion);


        Reaction displacment = new Reaction();
        displacment.name = reactionNames[7]; //zinc in acid
        displacment.start.Add(reactionMaterials[7]); //Zn
        displacment.start.Add(reactionMaterials[3]); //HCl
        displacment.end.Add(reactionMaterials[9]); //ZnCl
        displacment.end.Add(reactionMaterials[2]); //h2
        displacment.exo = false;
        displacment.tempChange = 0;
        displacment.reactionType = Reaction.reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment);

        Reaction displacment2 = new Reaction();
        displacment2.name = reactionNames[8]; //iron and copper sulfate
        displacment2.start.Add(reactionMaterials[16]); //iron suflate
        displacment2.start.Add(reactionMaterials[20]); //copper
        displacment2.end.Add(reactionMaterials[18]); //copper sulfate
        displacment2.end.Add(reactionMaterials[17]); //iron
        displacment2.exo = false;
        displacment2.tempChange = 0;
        displacment2.reactionType = Reaction.reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment2);


        Reaction doubleDisplacment = new Reaction();
        doubleDisplacment.name = reactionNames[9]; //neutralization
        doubleDisplacment.start.Add(reactionMaterials[2]); //H2SO4
        doubleDisplacment.start.Add(reactionMaterials[11]); //NaOH
        doubleDisplacment.end.Add(reactionMaterials[8]); //h20
        doubleDisplacment.end.Add(reactionMaterials[8]);  // Na2SO4
        doubleDisplacment.tempChange = 0;
        doubleDisplacment.exo = false;
        doubleDisplacment.reactionType = Reaction.reaction[4];//reactionTypes.neutrilization;
        reactions.Add(doubleDisplacment);

        Reaction saltSilverNitrate = new Reaction();
        saltSilverNitrate.name = reactionNames[10]; //silver nitrate
        saltSilverNitrate.start.Add(reactionMaterials[10]); //nacl
        saltSilverNitrate.start.Add(reactionMaterials[21]); //silver nitrate
        saltSilverNitrate.end.Add(reactionMaterials[22]); //silver chloride
        saltSilverNitrate.end.Add(reactionMaterials[23]);  // sodium nitrate salt
        saltSilverNitrate.tempChange = 0;
        saltSilverNitrate.exo = false;
        doubleDisplacment.reactionType = Reaction.reaction[4];//reactionTypes.neutrilization;
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
        bakingSodaVinegar.reactionType = Reaction.reaction[4];//reactionTypes.neutrilization;
        reactions.Add(bakingSodaVinegar);
    }

}
