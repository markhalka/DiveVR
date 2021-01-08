using System.Collections.Generic;
using UnityEngine;

public class Animations
{


    class Animation
    {
        public int index;
        public int animationCount;
        public int animationLength;
        public bool isClick;
        public float growth;



        public Animation(int i, int length, bool c, float g)
        {
            animationCount = 0;
            animationLength = length;
            index = i;
            isClick = c;
            growth = g;
        }
    }

    List<Animation> animations = new List<Animation>();
    GameObject[] entities;


    //initialize anything you need to
    public void init(GameObject[] curr)
    {
        entities = curr;

    }



    //add an animation to the list
    public void addAnimation(int index, int length, bool isClick, float growthRate)
    {
        for (int i = animations.Count - 1; i >= 0; i--)
        {
            if (animations[i].index == index)
            {
                if (animations[i].isClick)
                {
                    return;
                }
            }
        }
        animations.Add(new Animation(index, length, isClick, growthRate));
    }


    public void addAnimation2(int index, int length, bool isClick, float growthRate)
    {
        for (int i = animations.Count - 1; i >= 0; i--)
        {
            if (animations[i].index == index)
            {
                if (animations[i].isClick && !isClick)
                {

                    int currAnimationLength = 0;
                    if (animations[i].animationCount > animations[i].animationLength / 2)
                    {
                        currAnimationLength = animations[i].animationLength - animations[i].animationCount;
                    }
                    else
                    {
                        currAnimationLength = animations[i].animationCount;
                    }
                    float currentSize = entities[animations[i].index].transform.localScale.x;
                    float baseSize = currentSize / Mathf.Pow(animations[i].growth, currAnimationLength);
                    float desiredSize = baseSize * Mathf.Pow(growthRate, length);

                    growthRate = Mathf.Pow(desiredSize / currentSize, 1 / (float)length);
                    animations.RemoveAt(i);
                    break;
                }
            }
        }
        animations.Add(new Animation(index, length, isClick, growthRate));
    }


    public void resetSize(int index, bool selected)
    {
        float changeAmount = 1 / Information.animationGrowth;
        if (selected)
        {
            changeAmount = 1 / (1.007f);
            animations.Add(new Animation(index, Information.animationLength, false, changeAmount));
        }

    }

    public void resetButtons(int index)
    {
        float size = entities[index].transform.localScale.x;
        float changeAmount = Mathf.Pow(1 / size, 0.2f);


        for (int i = 0; i < animations.Count; i++)
        {
            if (animations[i].index == index)
            {
                float futureSize = Mathf.Pow(animations[i].growth, animations[i].animationLength);
                changeAmount = Mathf.Pow(futureSize / size, 0.2f);
                break;

            }
        }

        animations.Add(new Animation(index, 5, false, changeAmount));


    }


    public void updateAnimations(int currIndex)
    {

        for (int i = animations.Count - 1; i >= 0; i--)

        {
            var animation = animations[i];
            animation.animationCount++;

            if (animation.animationCount > animation.animationLength)
            {
                //remove the animation
                int index = animation.index;
                animations.Remove(animation);

                continue;

            }
            if (animation.index < 0 || animation.index > entities.Length)
            {
                continue;
            }
            Vector3 currScale = entities[animation.index].transform.localScale;
            if (animation.isClick && animation.animationCount > animation.animationLength / 2)
            {
                entities[animation.index].transform.localScale = new Vector3(currScale[0] / animation.growth, currScale[1] / animation.growth, currScale[2] / animation.growth);

            }
            else
            {
                entities[animation.index].transform.localScale = new Vector3(currScale[0] * animation.growth, currScale[1] * animation.growth, currScale[2] * animation.growth);
            }
        }
    }



    public void updateAnimations()
    {

        for (int i = animations.Count - 1; i >= 0; i--)

        {
            var animation = animations[i];
            animation.animationCount++;
            if (animation.animationCount > animation.animationLength)
            {

                animations.Remove(animation);

                continue;

            }
            if (animation.index < 0 || animation.index > entities.Length)
            {
                continue;
            }
            Vector3 currScale = entities[animation.index].transform.localScale;
            if (animation.isClick && animation.animationCount > animation.animationLength / 2)
            {
                entities[animation.index].transform.localScale = new Vector3(currScale[0] / animation.growth, currScale[1] / animation.growth, currScale[2] / animation.growth);
            }
            else
            {
                entities[animation.index].transform.localScale = new Vector3(currScale[0] * animation.growth, currScale[1] * animation.growth, currScale[2] * animation.growth);
            }
        }
    }



}
