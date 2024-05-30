using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveJump = 5f;
    [SerializeField] private float _moveClimb = 5f;//van toc leo thang

    //Khai bao nam ngang and flip
    private float Horizontal;
    private bool right=true;

    //khai báo jump true false
    [SerializeField] private bool _okJump;

    //tham chiếu đạn
    public GameObject bulletPrefab;
    //tham Chiếu tới vị trí súng
    public Transform gunTransform;

    //Khai báo rigidbody
    private Rigidbody2D rb;
    Vector2 moveInput;
    //khai báo animator
    private Animator at;

    //khai bao capsu
    CapsuleCollider2D capsule2D;
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      at = GetComponent<Animator>();
      capsule2D = GetComponent<CapsuleCollider2D>();
    }

    
    void Update()
    {
        Move();
        Flip();
        Animator();
        moveClimb();
        File();
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
    public void File()
    {
        //nếu nhấn f thì bắn 
        if (Input.GetKeyDown(KeyCode.F))
        {
            //animator
            at.SetTrigger("isFile");
            //tạo ra viên đạn tại vị trí súng
            var oneBullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity);
            //cho đạn bay theo huong nhân vật
            var velocity = new Vector2(50f, 0);
            if (right == false)
            {
                velocity.x = -50;
            }
            oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(right ? 50 : -50, 0);
            //huy viên đạn sau 2s
            Destroy(oneBullet, 0.5f);
        }

    }
    //xử lý va chạm 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Dat")
        {
            _okJump = true;
        }else if(other.gameObject.tag == "Trap"|| other.gameObject.tag == "Quai")//đạp trap die
        {
            Destroy(gameObject);
        }else if(other.gameObject.tag == "Coin")//an coin
        {
            Destroy(other.gameObject);
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
   
    void moveClimb()
    {
        var isChamThang = capsule2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if(!isChamThang)
        {
            return; 
        }
        var climbVelocity = new Vector2(rb.velocity.x, moveInput.y * _moveClimb);
        rb.velocity = climbVelocity;

        //điều khiển animation leo thang
        var playerHasVerticalSpeed = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        at.SetBool("isClimping", playerHasVerticalSpeed);
    }
}
