using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	// Start is called before the first frame update

	public GameObject doorClosed;
	public GameObject doorOpen;
	public SpriteRenderer spriteClosed;
	public SpriteRenderer spriteOpen;
	public Collider2D doorColliderClosed;
	public Collider2D doorColliderOpen;
	public bool open;

    void Start()
    {
		open = true;
		spriteOpen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "BulletLarge")
		{
			if (open)
			{
				open = false;
				spriteOpen.enabled = true;
				doorColliderOpen.enabled = true;

				spriteClosed.enabled = false;
				doorColliderClosed.enabled = false;

			}
			

		}
	}

}
