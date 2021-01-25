using UnityEngine;

public class MouseClickPos : MonoBehaviour
{

    public GameObject canvas;
    public Vector2 position = new Vector2(0, 0);

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Information.isSelect)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out position);
        }
    }
}