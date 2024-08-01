using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioManager.instance.PlaySFX(0, transform);
            GameManager.Instance.AddScore(coinValue);
            Destroy(gameObject);
        }
    }
}
