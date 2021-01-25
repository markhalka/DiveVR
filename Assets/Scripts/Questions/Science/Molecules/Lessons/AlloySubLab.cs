using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlloySubLab : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public Material[] alloyMaterials;

    public InformationPanel infoPanel;

    public void Start()
    {
        
    }

    public void startLab(int index)
    {
        // showPanel(alloyPanel, index); FIGURE OUT HOW TO SHOW THE RIGHT PANEL
        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = alloyMaterials[index * 2];

        var psr2 = ps2.GetComponent<ParticleSystemRenderer>();
        psr2.material = alloyMaterials[index * 2 + 1];


        var color = ps.main.startColor;
        color = Color.white;
        color = ps2.main.startColor;
        color = Color.white;

        var col = ps2.collision;
        col.enabled = true;

        var em = ps.emission;
        em.SetBursts(
new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300,0,1,0)
});

        var em2 = ps2.emission;

        em2.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 30,0,1,0)
    });

        ps.Play();
        ps2.Play();
    } 


}
