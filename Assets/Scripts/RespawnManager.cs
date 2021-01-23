using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Juto.Sceneloader;

public class RespawnManager : MonoBehaviour
{
    TextMeshProUGUI t, d,k;
    public string[] hints;

    private void Start()
    {
        if (t == null)
            t = GameObject.Find("RespawnText").GetComponent<TextMeshProUGUI>();

        if(d == null)
            d = GameObject.Find("RespawnDeathText").GetComponent<TextMeshProUGUI>();

        if (k == null)
            k = GameObject.Find("RespawnKey").GetComponent<TextMeshProUGUI>();
    }

    public void PlayerDied()
    {
        InteractText.ChangeText("", 0);
        string randomHint = hints[Random.Range(0, hints.Length - 1)];
        t.text = "<color=red><b>You died!</b><color=white><size=20%>\n\n" + randomHint;
        d.text = "Death count: " + App.Instance.death;
        k.text = "Press [Space] to respawn";

        App.Instance.isPlaying = false;
    }

    public void Clear()
    {
        t.text = "";
        d.text = "";
        k.text = "";
    }

    private void Update()
    {
        if(!App.Instance.isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Clear();
                App.Instance.Respawn();
            }
        }
    }
}
