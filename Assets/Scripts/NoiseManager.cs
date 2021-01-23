using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{

    public static void MakeNoise(Vector3 pos, float travelSpeed, float volume, string soundID)
    {
        MakeNoise(new NoiseItem(pos,travelSpeed,volume,soundID));
    }

    public static void MakeNoise(NoiseItem noise)
    {
        GameObject n = new GameObject("Noise");
        n.AddComponent(typeof(Noise));
        n.GetComponent<Noise>().Init(noise);
    }

    [System.Serializable]
    public struct NoiseItem
    {
        public Vector3 pos;
        public float travelTime;
        public float volume;
        public string soundID;

        public NoiseItem(Vector3 _pos, float _travelTime, float _volume, string _soundID)
        {
            pos = _pos;
            travelTime = _travelTime;
            volume = _volume;
            soundID = _soundID;
        }
    }

}
