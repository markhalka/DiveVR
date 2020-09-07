using PlayFab.Internal;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Website
{

    //ok, so here just make a second method that uses unityweberequest instead of http webrequest 

    public static void initCertificates()
    {

        ServicePointManager
        .ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;
    }





    /*
            //have the get function here 
            public static string GET(string url)
        {
           // System.Net.ServicePointManager.CertificatePolicy request = new TrustAllCertificatePolicy();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Debug.Log("error: " + ex);
                WebResponse errorResponse = ex.Response;
                if(errorResponse == null)
                {
                    return "";
                }
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    return errorText;
                }
                throw;
            }
        }

        public static void PUT(string url, string postData)
        {
            WebRequest request = WebRequest.Create(url);

            request.Method = "PUT"; //was post

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

            }

            response.Close();
        }

        */



    IEnumerator GetRequest(string url)
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();


        UnityWebRequest uwr = UnityWebRequest.Get(Information.loadDocUrl);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received");
        }
    }

    IEnumerator PutRequest(string url)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes("Hello, This is a test");
        UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }


}
