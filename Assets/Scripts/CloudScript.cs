using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public float amplitude = 0.5f; // Высота амплитуды колебания
    public float frequency = 1f; // Частота колебания

    private Vector2 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AmplitudeAnim ();
    }


    void AmplitudeAnim ()
    {
        Vector3 newPosition = startPosition;
        newPosition.x += Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = newPosition;
    }
}
