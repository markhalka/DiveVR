using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ReactionsLab : MonoBehaviour
{
    public Dropdown reactionDropdown;
    public Button reactionStart;
    public Slider slider;

    public GameObject choosePanel;
    public GameObject chooseCanvas;
    public GameObject currentShowPanel;


    public ParticleSystem ps;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;

    public Reactions reactionsScript;

    Lab util;

    Dictionary<int, int> reactionPanel;


    int[] reactionIndecies = { 0, 1, 2, 3, 4, 5, 6, 7, };

    public void Start()
    {
        util = new Lab();
        reactionPanel = new Dictionary<int, int>();
        for (int i = 0; i < reactionIndecies.Length; i++)
        {
            reactionPanel.Add(i, reactionIndecies[i]);
        }
    }

    string reactionType;

    public void initReactions(int index)
    {
        
        var col = ps.collision;
        var col2 = ps2.collision;

        col.enabled = true;
        col2.enabled = true;

        var main = ps.main;
        var main2 = ps2.main;
        main.loop = false;
        main2.loop = false;

        reactionIndex = index;
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
        Reaction currReaction = reactionsScript.reactions[reactionIndex];
        StartCoroutine(thermometerAnimation(currReaction.exo));
        var psr2 = ps2.GetComponent<ParticleSystemRenderer>();
        var col = ps2.colorBySpeed;
        if (currReaction.reactionType != Reaction.reaction[2]) //decomp
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

        var psr3 = ps3.GetComponent<ParticleSystemRenderer>();
        col = ps3.colorBySpeed;
        col.enabled = false;
        psr3.material = currReaction.end[0];
        var emission = ps3.emission;
        emission.rateOverTime = 20;
        ps3.Play();

        if (currReaction.reactionType == Reaction.reaction[2] || currReaction.reactionType == Reaction.reaction[4])
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
        util.reactionUpdator(reactionType == Reaction.reaction[2], ps, ps2);
    }


    public void Update()
    {

        if (reactionIndex != -1)
        {

            if (!currentShowPanel.activeSelf && !isReacting)
            {
                onReationStart();
            }
        }
    }

    private void OnDisable()
    {
        isReacting = false;
        reactionIndex = -1;
    }
}