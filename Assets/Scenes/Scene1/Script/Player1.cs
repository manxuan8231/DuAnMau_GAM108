using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player1 : MonoBehaviour
{
   
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveJump = 5f;
    [SerializeField] private float _moveClimb = 5f;    
    //Khai bao nam ngang and flip
    private float Horizontal;
    private bool right=true;

    //khai báo jump true false
    [SerializeField] private bool _okJump;
    [SerializeField] private bool _okClimb;
    //tham chiếu đạn
    public GameObject bulletPrefab;
    //tham Chiếu tới vị trí súng
    public Transform gunTransform;

    //Khai báo rigidbody
    private Rigidbody2D rb;
    
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
        Climbing();
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
            
        }
        else if(other.gameObject.tag == "Trap"|| other.gameObject.tag == "Quai")//đạp trap die
        {
            Destroy(gameObject);

        }else if(other.gameObject.tag == "Coin")//an coin
        {
            Destroy(other.gameObject);

        }else if (other.gameObject.CompareTag("EnemyTopSide"))//dap dau
        {
            //lm mat oc
            Destroy(other.gameObject.transform.parent.gameObject);
            rb.AddForce(Vector2.up * _moveJump, ForceMode2D.Impulse);

        }else if (other.gameObject.CompareTag("Climbing"))
        {
           
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

    public void Climbing()
    {
        if(gameObject.tag == "Dat")
        {
            var vertical = Input.GetAxis("Vertical") * _moveClimb;
            rb.velocity = new Vector2(0, vertical);
        }      
    }
}
