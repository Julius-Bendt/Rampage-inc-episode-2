using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class LOS : MonoBehaviour
{
    public float distance = 4, fov = 4, offset = 0.1f;
    public int lines;

    public LayerMask mask;
    List<Ray2D> rays = new List<Ray2D>();

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnValidate()
    {
        if ((lines % 2) != 0)
        {
            lines += 1;
        }

        if (fov < 0)
            fov = 0.1f;

        if (distance < 0)
            distance = 0.1f;
    }

    private void Update()
    {
        Transform player = null; ;
        rays.Clear();
        for (int i = -lines / 2; i < lines / 2 + 1; i++)
        {
            Ray2D r = new Ray2D(transform.position + (transform.right * offset), transform.right + transform.up * i * fov / lines);
            rays.Add(r);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction,distance, mask);

            if(hit)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if(!hit.collider.gameObject.GetComponent<Player>().ghost)
                        player = hit.collider.gameObject.transform;
                }
            }
        }

        enemy.target = player;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, transform.right * distance);

        Gizmos.color = Color.green;
        foreach (Ray2D ray in rays)
        {
            Gizmos.DrawRay(ray.origin,ray.direction * distance);
        }
    }
}
