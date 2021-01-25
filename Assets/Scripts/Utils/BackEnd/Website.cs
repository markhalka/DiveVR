using PlayFab.Internal;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayFab.Internal
{
    public class CustomCertificateHandler : CertificateHandler
    {
        // Encoded RSAPublicKey
        private static readonly string PUB_KEY = "";

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}

public class Website : MonoBehaviour
{
    public void Start()
    {

    }

    public void Update()
    {

    }
    //ok, so here just make a second method that uses unityweberequest instead of http webrequest 

    public void initCertificates()
    {

        ServicePointManager
        .ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;
    }

    public IEnumerator GetRequest(string url, Action<string> operation)
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();


        UnityWebRequest uwr = UnityWebRequest.Get(url);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);

        }
        else
        {
            operation(uwr.downloadHandler.text);
        }
    }

    public IEnumerator PutRequest(string url, string inputData, Action<string> operation)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(inputData);
        UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);

        }

        operation(uwr.downloadHandler.text);
    }
}
