using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{

    public GameObject points;

    public Vector3 pointsStart = new Vector3(0, 0, 0);
    public Vector3 pointsEnd = new Vector3(-2, -2, 0);

    public Button ok;


    void Start()
    {
        ok.onClick.AddListener(delegate { takeOk(); });
    }

    void takeOk()
    {

        ok.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        points.transform.localPosition = pointsStart;
        StartCoroutine(pointsAnimation());
    }



    public float journeyTime = 1.5f;
    public float speed = 0.1f;

    float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;

    float fraction = 0;


    IEnumerator pointsAnimation()
    {

        points.gameObject.SetActive(true);
        int count = 0;
        while (count < 2)
        {
            count++;
            yield return new WaitForSeconds(1);
        }

        while (fraction < 100)
        {
            GetCenter(Vector3.up);
            float fracComplete = fraction / 100;
            points.transform.localPosition = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            points.transform.localPosition += centerPoint;
            fraction += 1;
            yield return new WaitForSeconds(0.01f);

        }

        //   Website.GET(Information.setPointsUrl + Information.username + "/100"); 
        Information.totalEarnedPoints += 100;

        yield break;
    }


    public void GetCenter(Vector3 direction)
    {
        centerPoint = (pointsStart + pointsEnd) * .5f;
        centerPoint -= direction;
        startRelCenter = pointsStart - centerPoint;
        endRelCenter = pointsEnd - centerPoint;
    }
}
