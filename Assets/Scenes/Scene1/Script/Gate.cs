using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class Gate : MonoBehaviour
{
    [SerializeField] GameObject _loadingCanvas;
    [SerializeField] Slider _loadingSlider;
    [SerializeField] TextMeshProUGUI _loadingText;
    private bool _isTouchingPlayer = false;
    private void Start()
    {
        _loadingCanvas.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isTouchingPlayer = true;
            //hiện ra màn hình loading....
            _loadingCanvas.SetActive(true);
            StartCoroutine(Loading());

        }
    }
    IEnumerator Loading()
    {
        var value = 0;
        _loadingSlider.value = value;
        _loadingText.text = value + "%";
        while (true)
        {
            value++;
            _loadingSlider.value = value;
            _loadingText.text = value + "%";
            yield return new WaitForSeconds(0.03f);
            if (value >= 100)
            {
                break;
            }
        }
        //chuyển sang scene2 
        SceneManager.LoadScene(2);
    }
}
