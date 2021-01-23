using Juto.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rig;
    public Weapon weapon;
    public float moveSpeed, runSpeed;
    public Transform gunPos;
    public bool running = false;
    private float speed;

    public GameObject blood;
    public AudioSource footSource;

    [HideInInspector]
    public bool fire;
    private int ammo;
    private Transform[] shootPos;
    private GameObject[] muzzles;
    private GameObject gun;

    [HideInInspector]
    public Animator charAnim;
    private Animator gunAnim;

    public bool dead;

    public virtual void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        charAnim = GetComponentInChildren<Animator>();

        if (weapon != null)
            PickUpWeapon(weapon);


        StartCoroutine(Shoot());
    }

    public virtual void Update()
    {
        if (gunAnim != null)
        {
            if(weapon != null)
                if(!weapon.melee)
                    gunAnim.SetBool("shooting", fire);
        }

        speed = (running) ? runSpeed : moveSpeed;

    }

    public virtual void OnDie()
    {
        if(!dead)
        {
            //gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            Instantiate(blood, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = -3;
            ThrowGun();
            rig.freezeRotation = true;

            if (charAnim != null)
            {
                charAnim.SetInteger("die", 1);

                charAnim.SetTrigger("dead");
            }


            dead = true;

            if(gameObject.tag != "Player")
                StopAllCoroutines();
        }

    }



    public void OnCollisionEnter2D(Collision2D o)
    {

        Bullet b = o.gameObject.GetComponent<Bullet>();

        if (b != null)
        {
            if (b.senderTag != gameObject.tag)
            {
                Destroy(b.gameObject);

                OnDie();
            }
        }
        else
        {
            if(o.gameObject != gameObject)
            {


                MeleeWeapon mw = gameObject.GetComponentInChildren<MeleeWeapon>();

                if (mw != null)
                {
                    if (mw.senderTag != o.gameObject.tag)
                    {
                        Character c = o.gameObject.GetComponent<Character>();
                        if (c)
                            c.OnDie();
                    }
                }
            }
           
        }
    }

    public void Move(Vector2 moveTo)
    {
        Debug.Log("Moving (dead:" + dead);
        if (dead)
            return;



        rig.MovePosition(rig.position + moveTo * speed * Time.fixedDeltaTime);
        Debug.Log(moveTo);
        if (charAnim != null)
        {
            charAnim.SetBool("moving", moveTo.magnitude != 0);

            if (charAnim.GetBool("moving"))
            {
                if (!footSource.isPlaying)
                {
                    footSource.Play();
                }

            }
            else
            {
                footSource.Stop();
            }
        }




    }

    public void LookAt(Vector2 lookAt)
    {
        if (dead)
            return;

        Vector2 lookDir = lookAt - rig.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rig.rotation = angle;
    }

    public void PickUpWeapon(Weapon w)
    {
        if (dead)
            return;

        if (w == null)
            return;

        if(weapon != null)
        {
            ThrowGun();
        }

        gun = Instantiate(w.gun, gunPos.position, gunPos.rotation, transform);
        gun.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;

        gunAnim = gun.GetComponentInChildren<Animator>();

        List<Transform> _sp = new List<Transform>();
        List<GameObject> _m = new List<GameObject>();

        for (int i = 0; i < gun.transform.childCount; i++)
        {
            if (gun.transform.GetChild(i).name == "shootpos")
                _sp.Add(gun.transform.GetChild(i));
            else if (gun.transform.GetChild(i).name == "muzzle")
            {
                _m.Add(gun.transform.GetChild(i).gameObject);
                gun.transform.GetChild(i).gameObject.SetActive(false);
            }
               

        }

        muzzles = _m.ToArray();
        shootPos = _sp.ToArray();

        
        ammo = w.maxAmmo;

        if (gameObject.CompareTag("Enemy"))
            ammo = int.MaxValue;

            if (charAnim != null)
            charAnim.SetInteger("weapon", 1);

        gunAnim = gun.GetComponent<Animator>();

        if (w.melee)
            gun.GetComponent<MeleeWeapon>().senderTag = gameObject.tag;

        weapon = w;
    }

    IEnumerator Shoot()
    {
        while (App.Instance.isPlaying)
        {
            if (weapon != null && !dead && fire)
            {
                if(weapon.melee)
                {
                    float elapsedTime = 0;
                    if (weapon.swingRotate.magnitude != 0)
                    {
                       
                        Quaternion startRot = gun.transform.rotation;

                        while (elapsedTime <= weapon.swingTime)
                        {
                            elapsedTime += Time.deltaTime;
                            gun.transform.rotation = Quaternion.Lerp(startRot, Quaternion.Euler(weapon.swingRotate), elapsedTime / weapon.swingTime);
                            yield return null;
                        }

                        elapsedTime = 0;
                        while (elapsedTime <= weapon.swingTime * 1.5f)
                        {
                            elapsedTime += Time.deltaTime;

                            gun.transform.rotation = Quaternion.Lerp(Quaternion.Euler(weapon.swingRotate), startRot, elapsedTime / (weapon.swingTime * 1.5f));
                            yield return null;
                        }

                        if (GetComponentInChildren<MeleeWeapon>())
                            GetComponentInChildren<MeleeWeapon>().senderTag = gameObject.tag;
                    }
                    else
                    {
                        Vector3 startPos = gunPos.position;
                        Vector3 moveTo = startPos + (gun.transform.right);

                        while (elapsedTime <= weapon.swingTime/2)
                        {
                            elapsedTime += Time.deltaTime;
                            gun.transform.position = Vector3.Lerp(startPos, moveTo, elapsedTime / (weapon.swingTime/2));
                            yield return null;
                        }

                        
                        elapsedTime = 0;
                        while (elapsedTime <= weapon.swingTime / 2)
                        {
                            elapsedTime += Time.deltaTime;
                            gun.transform.position = Vector3.Lerp(moveTo, gunPos.position, elapsedTime / (weapon.swingTime / 2));
                            yield return null;
                        }

                    }

                    weapon.durability--;

                    if(weapon.durability <= 0)
                    {
                        Destroy(gun);
                        weapon = null;
                    }

                    yield return null;
                }
                else  if (ammo > 0)
                {
                    //create bullet
                    foreach (Transform t in shootPos)
                    {
                        Vector3 pos = new Vector3(t.position.x, t.position.y, -1);
                        Bullet b = Instantiate(weapon.bullet,pos, t.rotation).GetComponent<Bullet>();
                        b.speed += rig.velocity.normalized.magnitude;
                        b.senderTag = gameObject.tag;
                    }

                    StartCoroutine(MuzzleFlash());
                    NoiseManager.MakeNoise(transform.position, 0.2f, 12.5f, "gun");

                    if (gameObject.CompareTag("Player"))
                        App.Instance.cameraController.Shake(weapon.shakeTime);

                    // Instantiate(weapon.muzzle, shootPos.position, shootPos.rotation);

                    AudioController.PlaySound(weapon.GetRandomClip());
                    ammo--;

                    if (ammo <= 0)
                    {
                        if (weapon.destroyAfterThrow)
                        {
                            Destroy(gun);

                        }
                        else
                        {
                            ThrowGun();
                        }

                        if (charAnim != null)
                            charAnim.SetInteger("weapon",0);
                        weapon = null;
                    }

                    if(weapon != null)
                        yield return new WaitForSeconds(weapon.fireRate);
                }
            }
            

            yield return null;
        }

        yield return null;
    }

    IEnumerator MuzzleFlash()
    {
        foreach (GameObject g in muzzles)
        {
            g.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject g in muzzles)
        {
            g.SetActive(false);
        }
    }


    public void ThrowGun()
    {
        if (dead)
            return;



        if (gun != null)
        {
            
            gun.transform.parent = null;

            foreach (GameObject g in muzzles)
            {
                g.SetActive(false);
            }

            if (gunAnim != null)
                gunAnim.SetBool("shooting", false);

            if (ammo > 0)
                gun.AddComponent<PickUpWeapon>().weapon = weapon;


            gun.GetComponentInChildren<SpriteRenderer>().sortingOrder = 99;

            if(!gun.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D g_rig = gun.AddComponent<Rigidbody2D>();
                Vector3 pos = (transform.up + transform.right) / 2;

                g_rig.gravityScale = 0;
                g_rig.velocity = pos * 10 * Random.Range(0.5f, 1.5f);
                g_rig.drag = 5;
                g_rig.angularDrag = 1;
            }   
        }
    }
}
