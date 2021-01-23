using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juto.Sceneloader;

public class LoadScene : MonoBehaviour
{

    public string SceneToLoad;
    public static Weapon w;
    private void OnTriggerEnter2D(Collider2D o)
    {
        if(o.gameObject.CompareTag("Player"))
        {
            w = o.GetComponent<Character>().weapon;
            SceneLoader.LoadScene(SceneToLoad);
        }
    }
}
