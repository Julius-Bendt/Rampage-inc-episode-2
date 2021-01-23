using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juto.Audio;
public class Player : Character
{
    Camera cam;

    Vector2 input, mousePos;

    public bool ghost = true;

    [Header("Possess")]
    public BoxCollider2D col;
    public Animator anim;
    public SpriteRenderer rendere;
    public GameObject ghost_sprite_obj;
    private SpriteRenderer ghost_sprite;

    private GameObject enemyInRange;

    private const float GHOSTMAXTIME = 10;
    private float ghostTimer;
    

    public override void Start()
    {
        if (LoadScene.w != null)
            weapon = LoadScene.w;

        base.Start();
        cam = Camera.main;

        CameraScript.target = gameObject.transform;

        ghost_sprite = ghost_sprite_obj.GetComponent<SpriteRenderer>();

        col = GetComponent<BoxCollider2D>();
    }

    public override void OnDie()
    {
        base.OnDie();
        dead = false;

        App.Instance.death++;

        ghost = true;
        ghost_sprite_obj.SetActive(true);
        dead = false;
        ThrowGun();

        //Release the sprite
        if(ghost)
        {
            GameObject g = new GameObject("Dead sprite");
            g.transform.localScale = new Vector3(5, 5, 5);
            SpriteRenderer r = g.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            Animator a = g.AddComponent(typeof(Animator)) as Animator;

            g.transform.position = rendere.transform.position;
            g.transform.rotation = rendere.transform.rotation;

            a.runtimeAnimatorController = anim.runtimeAnimatorController;
            a.SetTrigger("dead");
            a.SetInteger("die", 1);
            r.sprite = rendere.sprite;

            anim.runtimeAnimatorController = null;
            rendere.sprite = null;

            col.isTrigger = true;
            col.enabled = true;
        }
    }



    // Update is called once per frame
    public override void Update()
    {
        if (!App.Instance.isPlaying)
            return;

        base.Update();

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);


        if(weapon != null && !ghost)
        {
            if (weapon.autoFire)
                fire = Input.GetButton("Fire");
            else
                fire = Input.GetButtonDown("Fire");

            if (Input.GetMouseButton(1))
                ThrowGun();
        }

        if(ghost)
        {
            ghostTimer += Time.deltaTime;

            float a = 1 - (ghostTimer/GHOSTMAXTIME) / 2;
            ghost_sprite.color = new Color(1, 1, 1,a);

            if(ghostTimer >= GHOSTMAXTIME)
            {
                App.Instance.respawn.PlayerDied();
            }
        }

        if(ghost && enemyInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            Possess(enemyInRange);
        }

    }

    public void FixedUpdate()
    {
        Move(input.normalized);
        LookAt(mousePos);
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        if (!App.Instance.isPlaying)
            return;

        if (o.GetComponent<Enemy>() && ghost)
        {
            InteractText.ChangeText("Press 'E' to possess", 99);
            enemyInRange = o.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D o)
    {
        if (o.GetComponent<Enemy>())
        {
            enemyInRange = o.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D o)
    {
        if (!App.Instance.isPlaying)
            return;

        if (o.GetComponent<Enemy>())
        {
            if(InteractText.id == 99)
                InteractText.ChangeText("", 99);

            enemyInRange = null;
        }
    }

    public void Possess(GameObject e)
    {
        if (!App.Instance.isPlaying)
            return;

        ghost_sprite_obj.SetActive(false);
        GameObject s = e.transform.Find("sprite").gameObject;
        rendere.sprite = s.GetComponent<SpriteRenderer>().sprite;
        anim.runtimeAnimatorController = s.GetComponent<Animator>().runtimeAnimatorController;
        gunPos.localPosition = e.transform.Find("gunpos").localPosition;
        e.GetComponent<Enemy>().ThrowGun();
        e.GetComponent<Enemy>().fire = false;


        col.offset = e.GetComponent<BoxCollider2D>().offset;
        col.size= e.GetComponent<BoxCollider2D>().size;
        col.isTrigger = false;

        Debug.Log(e.gameObject.name);
        Destroy(e);

        gunPos = transform.Find("gunpos");

        AudioController.PlaySound(App.Instance.audioDB.FindClip("possess"));

        ghost = false;
    }
}
