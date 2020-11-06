using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public GameObject weaponSpriteRndrObj;
    private SpriteRenderer weaponSpriteRndr;
    // Start is called before the first frame update
    void Start()
    {
        this.weaponSpriteRndr = weaponSpriteRndrObj.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate () {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        if (horizontal < 0 && weaponSpriteRndr.flipX == false)
        {
            weaponSpriteRndr.flipX = true;
        } else if (horizontal >= 0 && weaponSpriteRndr.flipX == true)
        {
            weaponSpriteRndr.flipX = false;
        }

        GetComponent<Rigidbody2D> ().velocity = new Vector2 (horizontal * speed, vertical * speed);
    }


}
