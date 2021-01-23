using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    CircleCollider2D col;
    NoiseManager.NoiseItem noise;
    public void Init(NoiseManager.NoiseItem _noise)
    {
        noise = _noise;
        gameObject.transform.position = noise.pos;
        gameObject.AddComponent(typeof(CircleCollider2D));

        col = gameObject.GetComponent<CircleCollider2D>();
        col.isTrigger = true;

        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float elapsedTime = 0;

        while (elapsedTime < noise.travelTime)
        {
            elapsedTime += Time.deltaTime;
            col.radius = Mathf.Lerp(0, noise.volume, elapsedTime / noise.travelTime);
            yield return null;
        }
        Destroy(gameObject);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        Enemy e = o.GetComponent<Enemy>();

        if(e)
        {
            e.NoticeNoise(noise.pos, noise.soundID);
        }
    }
}
