using UnityEngine;

public class MouseClickPos : MonoBehaviour
{

    public GameObject canvas;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Information.isSelect)
        {
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
            Vector2 tempOut = new Vector2(0, 0);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out tempOut);
            Information.currPosition = tempOut;

        }
    }
}