using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public enum PlatformTypes
    {
        Standart,
        Disable,
        Moving
    }

    public PlatformTypes PlatformType;

    public float HideTime = 3;
    public float UnhideTime = 1.5f;

    public float CurrentDisableTime = 3;

    public float MoveSpeed = 1;
    public float StopTime = 0.4f;

    public AnimationCurve Curve;

    public Rigidbody2D Rigidbody;

    Vector2 RemPosition;
    public Vector2 MovePosition;


    public SpriteRenderer Sprite;
    public Collider2D Collider;

    public bool Disable;



    void Start()
    {
        if(PlatformType == PlatformTypes.Moving)
        {
            RemPosition = transform.position;
        }
    }

    void Update()
    {
        if(PlatformType == PlatformTypes.Disable)
        {
            DisablePlatform ();
        }
        else if(PlatformType == PlatformTypes.Moving)
        {
            MovingPlatform ();
        }
    }

    float timez = 0;
    bool _switch = true;

    void MovingPlatform ()
    {
        timez += Time.deltaTime * MoveSpeed;

        if(_switch)
        {
            Rigidbody.MovePosition(Vector2.Lerp(RemPosition, RemPosition + MovePosition, Curve.Evaluate(timez)));
        }
        else
        {
            Rigidbody.MovePosition(Vector2.Lerp(RemPosition, RemPosition + MovePosition, 1 - Curve.Evaluate(timez)));
        }

        if(timez >= 1 + StopTime)
        {
            _switch = !_switch;
            timez = 0;
        }

        
        
        

        
    }

    void DisablePlatform ()
    {
        CurrentDisableTime -= Time.deltaTime;

        if(CurrentDisableTime <= 0)
        {
            Disable = !Disable;

            if(Disable)
            {
                CurrentDisableTime = UnhideTime;

                Collider.enabled = false;
                Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 0.5f);
            }
            else
            {
                CurrentDisableTime = HideTime;


                Collider.enabled = true;
                Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, 1f);
            }
        }
    }

}
