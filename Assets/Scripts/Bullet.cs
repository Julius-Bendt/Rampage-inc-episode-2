using Juto.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public int damage;
    public bool piercing = false;
    Rigidbody2D rig;

    public string senderTag;

    const float ALIVETIME = 2.5f;

    public LayerMask destroy;

    public float explosiveRadius;
    public GameObject explosion;
    public AudioClip explosionSound;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, ALIVETIME);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.MovePosition(rig.position + new Vector2(transform.right.x, transform.right.y) * speed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        if (explosiveRadius <= 0)
            return;


        Enemy[] e = FindObjectsOfType<Enemy>();
        Player p = FindObjectOfType<Player>();

        if(Vector2.Distance(p.transform.position,transform.position) < explosiveRadius)
        {
            p.OnDie();
        }

        foreach (Enemy enemy in e)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) < explosiveRadius)
            {
                enemy.OnDie();
            }
        }

        GameObject g = Instantiate(explosion, transform.position, transform.rotation);
        g.transform.localScale = Vector3.one * explosiveRadius;
        AudioController.PlaySound(explosionSound);
        Destroy(g, 2);

    }

    private void OnCollisionEnter2D(Collision2D o)
    {
        if (((1 << o.gameObject.layer) & destroy) != 0)
        {
            Destroy(gameObject);
        }
    }

}
