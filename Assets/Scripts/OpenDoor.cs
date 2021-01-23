using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juto.Audio;

public class OpenDoor : MonoBehaviour
{
    private float startRot;
    private float change = 15;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.eulerAngles.z;
        Debug.Log(startRot - change);
        Debug.Log(startRot + change);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.z < (startRot - change) || transform.eulerAngles.z > (change + startRot))
        {
            AudioController.PlaySound(clip,false,0.7f);
            Destroy(this);
        }
    }
}
