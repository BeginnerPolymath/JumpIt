using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float amplitude = 0.5f; // Высота амплитуды колебания
    public float frequency = 1f; // Частота колебания

    public SpriteRenderer SpriteRenderer;

    private Vector2 startPosition;

    public float AnimTime = 0;
    public float AnimHeight = 0.3f;

    public bool PickUp;


    void Start()
    {
        // Запоминаем начальную позицию монетки
        startPosition = transform.position;
    }

    void Update()
    {
        AmplitudeAnim ();
    }

    void AmplitudeAnim ()
    {
        if(!PickUp)
        {
            Vector3 newPosition = startPosition;
            newPosition.y += Mathf.Sin(Time.time * frequency) * amplitude;

            transform.position = newPosition;
        }
        else
        {
            DestroyAnimation ();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !PickUp)
        {
            other.GetComponent<PlayerScript>().Main.AddCoins(1);
            PickUp = true;
        }
    }

    void DestroyAnimation ()
    {
        AnimTime += Time.deltaTime * 2;
        transform.position = new Vector2(startPosition.x, Mathf.Lerp(startPosition.y, startPosition.y + AnimHeight, AnimTime));
        SpriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, AnimTime));

        if(AnimTime >= 1)
        {
            Destroy(gameObject);
        }
    }

}
