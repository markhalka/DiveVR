using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streak : MonoBehaviour
{
    int streak = 0;
    Website network;
    public GameObject Certificate;
    public void Start()
    {
        network = new Website();
        StartCoroutine(network.GetRequest(Information.sessionsUrl + Information.username, checkStreak));
    }

    public GameObject streakContainer;
    void checkStreak(string temp)
    {
        Information.wasStreak = true;

        string[] days = temp.Split(',');

        if (days.Length < 2)
        {
            streak = 0;
            return;
        }
        DateTime currentDate = DateTime.Parse(days[days.Length - 1]);
        var diff = DateTime.Today - currentDate;
        if (diff.Days < 1)
        {
            return;
        }
        DateTime pastDate;

        for (int i = days.Length - 2; i >= 0; i--)
        {
            pastDate = DateTime.Parse(days[i]);
            TimeSpan timeDiff = currentDate - pastDate;
            if (timeDiff.Days <= 1)
            {
                streak++;
                currentDate = pastDate;
            }
            else
            {
                break;
            }
        }
        if (streak > 0)
        {
            Information.socialMediaMessage = "I just studied " + streak + " days in a row on Dive!";
            int points = streak * 10;

            Information.name = streak + " day streak!";
            Information.acheivment = "+" + points + " Dive points";

            Certificate.SetActive(true);

            Information.totalEarnedPoints += points;
        }
    }

    public void Update()
    {
       /* if (streakContainer.activeSelf)
        {
            if (!streakContainer.transform.GetChild(1).gameObject.activeSelf)
            {
                Certificate.SetActive(false);
            }

        }*/
    }
}
