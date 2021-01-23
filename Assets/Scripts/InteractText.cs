using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractText : MonoBehaviour
{
    private static TextMeshProUGUI t;
    public static int id;
    // Start is called before the first frame update

    private void Start()
    {
        t = GetComponent<TextMeshProUGUI>();
    }

    public static void ChangeText(string text,int objId)
    {
        t.text = text;
        id = objId;
    }
}
