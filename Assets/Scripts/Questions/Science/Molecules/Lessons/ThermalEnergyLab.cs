using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalEnergyLab : Lab
{


    string[] changeCompounds = new string[] { "nitrogen", "alcohol", "water", "iron" };
    string[,] changeValue = new string[,] { { "-210", "-195" }, { "-115", "79" }, { "0", "100" }, { "1538", "2862" } }; //melting, boiling point


    public ThermalEnergyLab()
    {
        
        slider.gameObject.SetActive(true);
        initDropdown();
        initPS();
        loadPanelValues(new List<int>(new int[] { 5, 15, 25, 35, 45, 55, 65 }), new int[] { 0, 1, 2, 3, 4, 5, 6 }); //this is the thing you need to change <---

    }

    public override void initDropdown()
    {
        LabGameObjects.dropdown.gameObject.SetActive(true);
        dropdown.AddOptions(new List<string>(changeCompounds));
        dropdown.onValueChanged.AddListener(delegate { changeDropdown(); });
        changeDropdown();
    }

    public override void initPS()
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

}
