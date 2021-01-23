using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    private Transform target;
    public Weapon weapon;

    private Player p;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        p = target.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(App.Instance.isPlaying && !p.ghost)
        {
            if (weapon == null)
                return;

            if (Vector2.Distance(transform.position, target.position) < 2)
            {
                if (p.weapon != null)
                {
                    InteractText.ChangeText("Press 'e' to pick up " + weapon.name, gameObject.GetInstanceID());

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        p.PickUpWeapon(weapon);

                        if (InteractText.id == gameObject.GetInstanceID())
                        {
                            InteractText.ChangeText("", 0);
                        }

                        Destroy(gameObject);
                    }
                }
                else
                {
                    p.PickUpWeapon(weapon);
                    Destroy(gameObject);
                }

            }
            else
            {
                if (InteractText.id == gameObject.GetInstanceID())
                {
                    InteractText.ChangeText("", 0);
                }
            }
        }

    }
}
