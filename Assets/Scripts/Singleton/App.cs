using UnityEngine;
using System.IO;
using Juto;
using Juto.Audio;
using UnityEngine.SceneManagement;
using Juto.Sceneloader;

public class App : Singleton<App>
{
    // (Optional) Prevent non-singleton constructor use.
    protected App() { }



    public bool isPlaying = false;
    public bool isNewGame = false;

    public int death, killed;

    public Settings settings;
    public CameraScript cameraController;
    public string CurrentLevel;

    public RespawnManager respawn;
    public AudioDB audioDB;

    private static bool doneDontDestroy = false;

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int loadedScene = SceneManager.GetActiveScene().buildIndex;
        App.Instance.CurrentLevel = scene.name;

        if (loadedScene >= 2)
        {
            isPlaying = true;

            if (cameraController == null)
                cameraController = FindObjectOfType<CameraScript>();

            cameraController.Start();
        }

    }

    private void Start()
    {
        if(!doneDontDestroy)
        {
            DontDestroyOnLoad(gameObject);
            doneDontDestroy = true;
        }

        if(cameraController == null)
            cameraController = FindObjectOfType<CameraScript>();

        if(respawn == null)
            respawn = FindObjectOfType<RespawnManager>();
    }


    public void Respawn()
    {
        Debug.Log("kldskjlsd");
        SceneLoader.LoadScene(CurrentLevel);
    }


}
