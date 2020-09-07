using System.Collections;
using UnityEngine;

public class MathAI : MonoBehaviour
{


    float probability;
    float minDelay;
    float maxDelay;
    float level;

    int correctCount;
    int wrongCount;
    public bool isPlaying;
    utilities utility;


    void Start()
    {
        utility = new utilities();
        //  getStartLevel(); //?

        getDelay();
        getProbability();
        isPlaying = true;
        StartCoroutine(playGame());

    }

    IEnumerator playGame()
    {
        while (isPlaying)
        {
            if (utility.getRandom(0, 100) < probability * 100)
            {
                Information.isCorrect = true;
            }
            else
            {
                Information.isIncorrect = true;
            }
            yield return new WaitForSeconds(utility.getRandom(minDelay, maxDelay));
        }

    }

    void getStartLevel()
    {
        string currLevel = ParseData.parseGames("Quick Game");
        if (currLevel == "-1")
        {
            getNewStartLevel();
        }
        else
        {
            level = float.Parse(currLevel) / 10;
        }

    }



    //this will just get the user level from the file
    //ok, so the first time they play, use this to get their level, otherwise just get it from the game element 
    void getNewStartLevel()
    {
        Information.subject = "Math";
        if (Information.grade == null || Information.grade == "")
        {
            Information.grade = "5";
        }

        ParseData.startXML();
        bool foundTest = false;
        foreach (var topic in Information.topics)
        {
            if (topic.isTest)
            {
                int lastScore = topic.tests.Count - 1;
                if (lastScore < 0)
                    continue;


                foundTest = true;
                level = topic.level; //ok, so 
                float currScore = float.Parse(topic.tests[lastScore].score);
                levelFromScore(currScore);
                break;

            }
        }
        if (!foundTest)
        {
            float avg = 0;
            int count = 0;
            for (int i = 0; i < 3 && i < Information.topics.Count; i++)
            {
                avg += Information.topics[i].level;
                count++;
            }
            avg /= count;
            level = (int)(avg * 10) / 10;
        }
    }

    void levelFromScore(float score)
    {
        if (score < 0.2)
        {
            level = 0.1f;
        }
        else if (score < 0.3)
        {
            level = 0.2f;
        }
        else if (score < 0.4)
        {
            level = 0.3f;
        }
        else if (score < 0.5)
        {
            level = 0.4f;
        }
        else if (score < 0.6)
        {
            level = 0.5f;

        }
        else if (score < 0.7)
        {
            level = 0.6f;
        }
        else if (score < 0.75)
        {
            level = 0.7f;
        }
        else if (score < 0.8)
        {
            level = 0.8f;
        }
        else if (score < 0.85)
        {
            level = 0.9f;
        }
        else if (score < 0.9)
        {
            level = 1f;
        }
    }


    void updateLevelFromGame()
    {
        float points = Information.gamePoints;
        if (points < -0.5f)
        {
            level -= 0.2f;
        }
        else if (points < -0.25f)
        {
            level -= 0.1f;
        }
        else if (points < 0)
        {

        }
        else if (points < 0.25f)
        {
            level += 0.1f;
        }
        else if (points < 0.5f)
        {
            level += 0.2f;
        }
        else if (points < 0.75f)
        {
            level += 0.3f;
        }
        else
        {
            level += 0.4f;
        }
        if (level > 1)
        {
            level = 1;
        }
        else if (level < 0)
        {
            level = 0;
        }
    }


    void getDelay()
    {
        switch (level)
        {
            case 0.1f:
                minDelay = 7;
                maxDelay = 10;
                break;
            case 0.2f:
                minDelay = 5;
                maxDelay = 9;
                break;
            case 0.3f:
            case 0.4f:
                minDelay = 4;
                maxDelay = 9;
                break;
            case 0.5f:
            case 0.6f:
                minDelay = 3;
                maxDelay = 8;
                break;
            case 0.7f:
                minDelay = 2;
                maxDelay = 6;
                break;
            case 0.8f:
            case 0.9f:
                minDelay = 1;
                maxDelay = 5;
                break;
            case 1f:
                minDelay = 1;
                maxDelay = 3;
                break;
        }
    }


    void getProbability()
    {
        switch (level)
        {
            case 0.1f:
                probability = 0.3f;
                break;
            case 0.2f:
                probability = 0.4f;
                break;
            case 0.3f:
            case 0.4f:
                probability = 0.5f;
                break;
            case 0.5f:
            case 0.6f:
                probability = 0.6f;
                break;

            case 0.7f:
                probability = 0.7f;
                break;
            case 0.8f:
            case 0.9f:
                probability = 0.8f;
                break;
            case 1f:
                probability = 0.9f;
                break;
        }
    }

}
