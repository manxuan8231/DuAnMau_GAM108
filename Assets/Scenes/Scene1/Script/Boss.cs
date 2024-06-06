using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    //giả sử quái dg di chuyển sang phải
    private bool isRight = true;
    [SerializeField] private Slider _healthSlider;
    private int _health;
    [SerializeField] private ParticleSystem _hitEffect;
    void Start()
    {
        _health = 100;
        _healthSlider.maxValue = _health;
    }
    void Update()
    {
        diChuyenNgang();
        hienTai();
    }
    private void diChuyenNgang()
    {
        //di chuyển ngang
        //(1, 0, 0) * 1 * 0.02 = (0.02, 0, 0)

        //var direction = isRight ? Vector3.right : Vector3.left; cách 1
        var direction = Vector3.right; //cách 2
        if ((isRight == false))
        {
            direction = Vector3.left;
        }
        {

        }
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    private void hienTai()
    {
        //currentPosition: vi tri hien tai
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            isRight = false;
        }
        else if (currentPosition.x < leftBoundary)
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            
            //tạo hiệu ứng nổ
            var hitEffect = Instantiate(_hitEffect, transform.position, Quaternion.identity);//tao cai gi, vi tri, goc quay
            Destroy(hitEffect.gameObject, 1f);
            //bien mat vien dan
            Destroy(other.gameObject);

            _health -= 10;
            _healthSlider.value = _health;
            if (_health <= 0)
            {

                //het mau thi chet
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        var tag = other.gameObject.tag;
        Bekilled(tag);

    }
    private void Bekilled(string tag)
    {
        if (tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
