using UnityEngine;
using UnityEngine.UI;
public class buttonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(delegate { click(); });
    }

    void click()
    {
        Information.currentBox = gameObject;
    }

}
