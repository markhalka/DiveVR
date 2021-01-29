using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    class TableLayout
    {
        public List<int> rows;
        public TableLayout()
        {
            rows = new List<int>();
        }
    }


    public GameObject table;

    TableLayout initTableRows(int n)
    {
        TableLayout output = new TableLayout();
        if (n <= 5)
        {
            for (int i = 0; i < n; i++)
            {
                output.rows.Add(1);
            }
        }
        if (n == 6 || n == 8)
        {
            int rows = n / 2;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(2);
            }
        }
        if (n == 9 || n == 12)
        {
            int rows = n / 3;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(3);
            }
        }
        if (n == 16)
        {
            for (int i = 0; i < 4; i++)
            {
                output.rows.Add(4);
            }
        }
        if (n == 10 || n == 14)
        {
            int largetRow = (n + 2) / 4;
            int smallerRow = largetRow - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 7)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 3; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(smallerRow);
                }
                else
                {
                    output.rows.Add(largetRow);
                }
            }
        }
        if (n == 11)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 2)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 13)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || i == 2)
                {
                    output.rows.Add(largetRow);
                }
                else if (i == 1)
                {
                    output.rows.Add(smallerRow);
                }
                else
                {
                    output.rows.Add(smallerRow - 1);
                }
            }
        }
        if (n == 15)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 1)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }

            }
        }
        return output;
    }

    Vector2 boxOffset = new Vector2(200, 50);
    Vector2 startOffset = new Vector2(0, -20);
    bool inTable = false;


    public void createTable(Action<GameObject> clickAction)
    {
        if(table.transform.parent.childCount > 1)
        {
            // then it was already created (pretest)
            return;
        }
        //   table.transform.parent.GetChild(0).gameObject.SetActive(true); //this is the panel
        TableLayout layout = initTableRows(Information.userModels.Count);

        int rows = layout.rows.Count;

        float yOffset = startOffset.y - (float)(rows - 1) / 2 * boxOffset.y;
        List<GameObject> newButtons = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            float xOffset = startOffset.x - (float)(layout.rows[i] - 1) / 2 * boxOffset.x;
            for (int j = 0; j < layout.rows[i]; j++)
            {
                GameObject curr = Instantiate(table, table.transform, true);
                curr.transform.SetParent(curr.transform.parent.parent);
                curr.transform.localPosition = new Vector2(xOffset + boxOffset.x * j, yOffset + boxOffset.y * i);

                curr.GetComponentInChildren<TMPro.TMP_Text>().text = Information.userModels[index++].simpleInfo[0];
                curr.GetComponent<Button>().onClick.AddListener(delegate { clickAction(curr); });
                curr.gameObject.SetActive(true);
                newButtons.Add(curr);
            }
        }

        Information.updateEntities = newButtons.ToArray();
        inTable = true;

    }


}
