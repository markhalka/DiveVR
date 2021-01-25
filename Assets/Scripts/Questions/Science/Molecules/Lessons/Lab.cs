using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab
{

    public void updateAmountOfParticles(ParticleSystem p, int amount)
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


    int endParticles = 300;
    public IEnumerator reactionUpdator(bool reactionType1, ParticleSystem ps, ParticleSystem ps2)
    {
       
        while (endParticles > 20)
        {

            updateAmountOfParticles(ps, endParticles);
            if (reactionType1)
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


    /* 

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
     } */
}
