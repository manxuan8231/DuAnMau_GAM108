using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveJump = 5f;
    //Khai bao nam ngang and flip
    private float Horizontal;
    private bool right=true;

    //khai báo jump true false
    [SerializeField] private bool _okJump;

    //Khai báo rigidbody
    private Rigidbody2D rb;

    //khai báo animator
    private Animator at;
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      at = GetComponent<Animator>();
    }

    
    void Update()
    {
        Move();
        Flip();
        Animator();
    }

    public void Move()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vector3 Direction = new Vector3(Horizontal, 0, 0);
        transform.Translate(Direction*_moveSpeed*Time.deltaTime);
        //nhảy
        if (Input.GetKeyDown(KeyCode.Space) && _okJump)
        {
            rb.AddForce(Vector2.up * _moveJump, ForceMode2D.Impulse);
        }
    }

    //xử lý va chạm 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Dat")
        {
            _okJump = true;
        }
    }

    //xử lý va chạm exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Dat")
        {
            _okJump = false;
        }
    }
   
    public void Animator()
    {
        at.SetFloat("isRun", Mathf.Abs(Horizontal));
    }
    public void Flip()
    {
        if(right && Horizontal < 0 || !right && Horizontal > 0) 
        {
            right=!right;
            Vector3 kichThuoc = transform.localScale;
            kichThuoc.x = kichThuoc.x *-1;
            transform.localScale = kichThuoc;
        }
    }
}
