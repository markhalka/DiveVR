using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoutubePlayer;

public class Azure : MonoBehaviour
{

    Website website;
    void Start()
    {
        website = new Website();
        
    }

    void Update()
    {
        
    }

    



    // these methods should be called in the student menu, only one time at the beginning 
    public void getLearningType()
    {
        // you need to student id, the surveys and the lessons (3 each)
        string studentLearningInfo = ""; // add this function and the database to wix, and then add this string to information
        StartCoroutine(website.GetRequest(studentLearningInfo, sendAzureLearningType));


    }

    public void sendAzureLearningType(string inputData)
    {
        JArray array = new JArray();

        // int[] input_ints = new int[6];
        string[] spllited = inputData.Split(' ');
        for(int i = 0; i < spllited.Length; i++)
        {
            //input_ints[i] = int.Parse(spllited[i]);
            array.Add(new JValue(int.Parse(spllited[i])));
        }

        //  headers = { 'Content-Type':'application/json' }
        string azureLearningTypeUrl = ""; // find the enpoint and put it in information
        StartCoroutine(website.PutRequest(azureLearningTypeUrl, array.ToString(), recieveAzureLearningType));
        
    } 

    public void recieveAzureLearningType(string inputData)
    {
        string[] splitted = inputData.Split(' ');
        // then from here you can just pick the largest 2, and bam, get the two learning types, and set that in information 
    }



}
