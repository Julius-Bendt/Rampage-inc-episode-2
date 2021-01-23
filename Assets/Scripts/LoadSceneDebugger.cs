using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juto.Sceneloader;

public class LoadSceneDebugger : MonoBehaviour
{
    public string scene;
    public Weapon startwith;
    void Start()
    {

        LoadScene.w = startwith;
        SceneLoader.LoadScene(scene);
    }


}
