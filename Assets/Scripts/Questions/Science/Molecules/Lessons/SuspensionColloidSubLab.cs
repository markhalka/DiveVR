using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspensionColloidSubLab : MonoBehaviour
{

    public Button mix; //INIT THIS
    public ParticleSystem ps;
    public ParticleSystem ps2;
    public Material defualtMaterial;
    public Text outputText;

    public void Start()
    {
        
    }


    void SuspensionSubLab()
    {
        mix.gameObject.SetActive(true);
        initSuspensionParticles(true);
        var main = ps.main;
    }

    void ColloidsSubLab()
    {
        mix.gameObject.SetActive(true);
        initSuspensionParticles(false);
        var main = ps.main;
    }


    void initSuspensionParticles(bool shouldFloat)
    {
        ps.Stop();
        ps.Clear();
        ps2.Clear();

        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = defualtMaterial;
        var col = ps.colorBySpeed;
        col.enabled = false;
        var main = ps.main;
        main.startColor = Color.blue;
        main.loop = false;

        var noise = ps.noise;
        noise.frequency = 0.1f;
        noise.positionAmount = 1;
        var em = ps.emission;

        ps2.Stop();
        psr = ps2.GetComponent<ParticleSystemRenderer>();
        psr.material = defualtMaterial;
        col = ps2.colorBySpeed;
        col.enabled = false;
        main = ps2.main;
        main.startColor = Color.red;
        main.loop = false;

        noise = ps2.noise;
        noise.frequency = 0.1f;
        noise.positionAmount = 1;
        em = ps2.emission;


        em.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300,300,1,0)
    });


        em.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 30,30,1,0)
    });
        if (shouldFloat)
        {


            var v = ps2.forceOverLifetime;
            v.enabled = true;
            v.y = -12;

        }
        else
        {
            var v = ps2.forceOverLifetime;
            v.enabled = false;

        }
        ps2.Play();
        ps.Play();
    }

    void shakeParticles()
    {
        outputText.text = "";
        // StartCoroutine(particleAnimation());
        var noise1 = ps.noise;
        var noise2 = ps2.noise;
        noise1.positionAmount = 10;
        noise2.positionAmount = 10;
        StartCoroutine(particleAnimation());
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

