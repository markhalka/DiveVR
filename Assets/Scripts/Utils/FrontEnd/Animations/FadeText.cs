using UnityEngine;
public class FadeText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Text outputText;
    void Start()
    {

    }
    public int length = 200;
    private void OnEnable()
    {
        //  outputText = transform.GetComponent<TMPro.TMP_Text>();
        Color fixedColor = outputText.color;
        fixedColor.a = 1;
        outputText.color = fixedColor;
        outputText.CrossFadeAlpha(0f, 0f, true);
        outputText.CrossFadeAlpha(1, 1, false);
        showingText = length;

    }

    private void OnGUI()
    {

    }

    // Update is called once per frame
    int showingText = 0;
    void Update()
    {
        if (showingText > 0)
            showingText--;
        if (showingText > length / 2)
        {

        }
        else if (showingText == length / 2)
        {


            outputText.CrossFadeAlpha(0, 1, false);

        }
        else if (showingText == 1)
        {
            gameObject.SetActive(false);
            showingText = 0;

        }
    }
}
