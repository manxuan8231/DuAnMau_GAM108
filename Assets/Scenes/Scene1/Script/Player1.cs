using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Player1 : MonoBehaviour
{
   
    [SerializeField] private float _moveSpeed = 1f;//cho vận tốc di chuyển 
    [SerializeField] private float _moveJump = 5f;//vận tốc nhảy
    [SerializeField] private float _moveTop = 7f;//vận tốc đạp đầu quái nhảy
    [SerializeField] private float _moveSlime = 15f;//vận tốc đạp slime nhảy
    [SerializeField] private float _moveClimb = 1f; //vận tốc leo thang

    //Khai bao nằm ngang và flip(xoay)
    private float Horizontal;
    //đạn bay theo hướng nhân vật
    private bool right=true;
    //nhảy bool
    [SerializeField] private bool _okJump;
    //thang bool
    [SerializeField] private bool _okClimbing;
    //tham chiếu đạn
    public GameObject bulletPrefab;
    //tham Chiếu tới vị trí súng
    public Transform gunTransform;

    //Khai báo rigidbody
    private Rigidbody2D rb;
    
    //khai báo animator
    private Animator at;

    //khai báo capsu
    CapsuleCollider2D capsule2D;

    //tham chiếu audioSource
    private AudioSource AudioSource;//trình phát nhạc
    [SerializeField] private AudioClip coinCollectSXF;//file nhạc coin
    [SerializeField] private AudioClip enemySXF;//Đạp Quái 
    
    //tham chiếu đến điểm số
    [SerializeField] private TextMeshProUGUI ScoreText;
    private static int score = 0;

    ////tham chiếu đến thời gian
    [SerializeField] private TextMeshProUGUI _timeText;
    private static float _time = 0;

    void Start()
    {
    rb = GetComponent<Rigidbody2D>();
    at = GetComponent<Animator>();
    capsule2D = GetComponent<CapsuleCollider2D>();
    AudioSource = GetComponent<AudioSource>();
    ScoreText.text = score.ToString();

    }


    void Update()
    {
        Move();
        Flip();
        Animator();       
        File();
        Times();
    }
    public void Times()//thời gian chơi
    {
        //time
        _time += Time.deltaTime;
        _timeText.text = $"Time: {_time:0.00}";

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
        if(other.gameObject.tag == "Dat")//chạm đất thì dc phép nhảy
        {
            _okJump = true;
            
        }
        else if(other.gameObject.tag == "Trap"|| other.gameObject.tag == "Quai")//đạp trap và quái chết
        {
                      
            Destroy(gameObject);

        }else if(other.gameObject.tag == "Coin")
        {
            //Làm mất coin
            Destroy(other.gameObject);
            //Phát nhạc
            AudioSource.PlayOneShot(coinCollectSXF);
            //tăng điểm 
            score += 10;
            ScoreText.text = score.ToString();
        }
        else if (other.gameObject.CompareTag("EnemyTopSide"))//dap dau
        {
            //lm mat oc
            Destroy(other.gameObject.transform.parent.gameObject);
            AudioSource.PlayOneShot(enemySXF);//Phát nhạc
            rb.AddForce(Vector2.up * _moveTop, ForceMode2D.Impulse);//đạp quái r nhảy
            at.SetTrigger("isJump");//đạp quái animator 
        }
        else if (other.gameObject.CompareTag("slimeJump"))
        {
            rb.AddForce(Vector2.up * _moveSlime, ForceMode2D.Impulse);//đạp slime r nhảy

        }else if(other.gameObject.tag == "Climbing")//chạm thang true
        {
            _okClimbing = true;            
        }

    }

    private void FixedUpdate()//thang
    {
        if (_okClimbing)
        {
            var climninput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climninput * _moveClimb);
        }
    }
    //xử lý va chạm exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Dat")
        {
            _okJump = false;
        }else
        if (other.gameObject.tag == "Climbing")//chạm thang false
        {           
            _okClimbing = false;
            
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
