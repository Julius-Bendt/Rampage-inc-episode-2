using UnityEngine;
using UnityEngine.SceneManagement;

/*
Author: Julius Bendt
Created: 11/16/2018 7:40:31 PM
Project name: Juto Standard Asset
Company: Juto Studio
Unity Version: 2018.1.0f2


This script is under the CC0 license.
https://creativecommons.org/publicdomain/zero/1.0/

Credit isn't needed, but I would greatly appreciate it.
Give me credit as following:

"Using assets by Julius bendt,
https://www.juto.dk"

*/

namespace Juto.Sceneloader
{
    public class SceneLoader : MonoBehaviour
    {


        private static string SceneToLoad;
        private static LoadSceneMode LoadMode;

        // Use this for initialization
        void Start()
        {
            if (!string.IsNullOrEmpty(SceneToLoad))
            {
                SceneManager.LoadSceneAsync(SceneToLoad,LoadMode);
                SceneToLoad = null;
            }
            else
            {
                Debug.LogWarning("Scene loader was enabled, but couldn't find a scene to load!");
            }
        }


        /// <summary>
        /// Loads a scene with 
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName,LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if(Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneToLoad = sceneName;
                LoadMode = loadMode;

                SceneManager.LoadSceneAsync("Scene loader",loadMode);
            }
            else
            {
                Debug.LogException(new System.Exception("Can't find a scene with the name " + sceneName + "in the build settings! be sure to add it there before trying to load it."));
            }
            

        }
    }
}


