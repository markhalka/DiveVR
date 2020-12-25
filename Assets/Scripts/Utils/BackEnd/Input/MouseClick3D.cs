using UnityEngine;

public class MouseClick3D : MonoBehaviour
{
    public GameObject entityContainer;
    GameObject[] entities;

    void Start()
    {
        if (entityContainer != null)
        {
            entities = new GameObject[entityContainer.transform.childCount];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = entityContainer.transform.GetChild(i).gameObject;
            }
        }
    }

    void updateNewEntities()
    {
        if (Information.updateEntities != null && Information.updateEntities.Length > 0)
        {
            entities = Information.updateEntities;
            Information.updateEntities = null;
        }


    }

    void Update()
    {
        updateNewEntities();

        if (Information.click2d)
        {
            click2D();
        }
        else
        {
            click3D();
        }

    }

    void click3D()
    {
        if (Input.GetMouseButtonDown(0) || Information.isSelect || Information.isVrMode)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out hit))
            {

                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i].name == hit.transform.gameObject.name)
                    {

                        Information.currentBox = entities[i];
                        break;
                    }
                }

            }
        }
    }

    void dragObject(GameObject curr)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (UnityEngine.Physics.Raycast(ray, out hit))
        {
            float oldY = curr.transform.localPosition.y;
            curr.transform.localPosition.Set(hit.point.x, oldY, hit.point.z);
        }
    }


    void click2D()
    {
        if (Input.GetMouseButtonDown(0) || Information.isSelect || Information.isVrMode)
        {
            RaycastHit2D hitInfo;

            hitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (hitInfo)
            {
                if (entities == null)
                {
                    Debug.LogError("entities is null");
                    return;
                }

                for (int i = 0; i < entities.Length; i++)
                {

                    if (entities[i].transform == hitInfo.transform)
                    {
                        Information.currentBox = entities[i];
                        break;
                    }
                }
            }
        }
    }
}
