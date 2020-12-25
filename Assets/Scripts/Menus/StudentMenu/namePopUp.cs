using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class namePopUp : MonoBehaviour
{
    public Button nameOk;
    public TMPro.TMP_InputField nameField;
    Website network;
    Panel panel;
    public void Start()
    {
        panel = new Panel();
       
        network = new Website();
        nameOk.onClick.AddListener(delegate { closeName(); });
    }

    void closeName()
    {
        Information.name = nameField.text;
        StartCoroutine(panel.panelAniamtion(false, transform));
        // StartCoroutine(network.GetRequest(Information.saveNameUrl + Information.username + "/" + Information.name, saveName));
    }

    void saveName(string str)
    {
        Debug.LogError("name saved...");
        StartCoroutine(panel.panelAniamtion(false, transform));
    }
}
