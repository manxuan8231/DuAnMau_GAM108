using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quai1 : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _leftBoundary;
    [SerializeField] private float _rightBoundary;

    //giả sử quái dg di chuyển sang phải
    private bool isRight = true;

    void Start()
    {
        
    }

    
    void Update()
    {
        diChuyenNgang();
        hienTai();
    }

    private void diChuyenNgang()
    {
        //di chuyển ngang
        var direction = Vector3.right; //cách 2
        if ((isRight == false))
        {
            direction = Vector3.left;
        }
        {

        }
        transform.Translate(direction * _moveSpeed * Time.deltaTime);

    }
    private void hienTai()
    {
        //currentPosition: vi tri hien tai
        var currentPosition = transform.localPosition;
        if (currentPosition.x > _rightBoundary)
        {
            isRight = false;
        }
        else if (currentPosition.x < _leftBoundary)
        {
            isRight = true;
        }
        //scale hiện tai
        var currentScale = transform.localScale;
        if (isRight == true && currentScale.x > 0 || isRight == false && currentScale.x < 0)
        {
            currentScale.x *= -1;
        }
        transform.localScale = currentScale;
    }

    public void OnTriggerEnter2D(Collider2D other)//xử lý va chạm
    {
        var name = other.gameObject.name;
        var tag = other.gameObject.tag;
        if (tag == "Bullet")//đụng bullet chết
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        var tag = other.gameObject.tag;
        Bekilled(tag);

    }

    private void Bekilled(string tag)
    {
        if (tag == "dan")
        {
            Destroy(gameObject);
        }
    }

}
