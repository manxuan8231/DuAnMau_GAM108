using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _item;
    private bool _isPlayer = false;
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (_isPlayer == false && other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _isPlayer = true;
            _particleSystem.Play();
            //hiện ra vật phẩm
            Instantiate(_item, transform.position, Quaternion.identity);
            //biến mất khối
            Destroy(gameObject, 1f);
        }
    }
}
