using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    [Header("Frog Colors")]

    public List<Color> FogColors = new List<Color>();

    [Header("Main Links")]
    public Rigidbody2D Rigidbody;

    public RectTransform ForceVision;

    public Transform Camera;
    
    public MainScript Main;
    public ShopScript Shop;

    public SpriteRenderer SpriteRenderer;
    public Transform Visual;
    

    [Header("Jump Info")]

    public float angle;
    public float force;

    public float size;

    public ForceMode2D ForceMode;

    public float HightJump;



    public Vector2 RememberTouch;

    public Vector2 Angle;

    public bool Pressed;

    public float MaxJumpForce = 7.1f, MinJumpForce = 3.325f;

    public float AngelLeft = 90, AngleRight = -90;

    public AnimationCurve GravityCompensation;


    [Header("Other")]

    public Vector2 SpawnPoint;

    public LineRenderer ForceLine;

    [Header("Cosmetics")]

    public int ClothID;
    public SpriteRenderer ClothRenderer;

    public int ItemID;
    public SpriteRenderer ItemhRenderer;


    [Header("Particle System")]
    public ParticleSystem ParticleSystem;
    public float ParticlePlayForce = 1;
    public float SpaceJumpForce = 3.23f;
    public Color[] ParticleColors;
    public Material ParticleMat;

    [Header("Stun")]
    public float stunDuration = 2.0f; // Длительность оглушения
    public float fallThreshold = -10.0f; // Порог скорости падения для оглушения
    public bool isStunned = false;
    private float stunTimer = 0.0f;
    public SpriteRenderer ReloadSprite;

    [Header("On Ground")]
    public Transform groundCheck;
    public Vector2 groundCheckRadius = new Vector2(1, 1);
    public LayerMask whatIsGround;
    public bool onGround;

    public float JumpTime;
    public float JumpReloadTime = 0.2f;
    public bool JumpCheck = true;

    [Header("Blink")]

    public Vector2 blinkDuration = new Vector2(0.1f, 0.2f); // Длительность одного моргания
    public Vector2 blinkInterval = new Vector2(1, 3); // Интервал между морганиями

    public SpriteRenderer[] spriteRenderer;
    private float nextBlinkTime;
    private bool isBlinking = false;


    void Start()
    {
        Application.targetFrameRate = 120;

        SpawnPoint = transform.position;

        nextBlinkTime = Time.time + UnityEngine.Random.Range(blinkInterval.x, blinkInterval.y);
    }

    void Update()
    {
        Stunted ();
        Blink ();

        OnGroundCheck();

        if(!Shop.ShopMenu.interactable)
            Touch ();

        KeyRestart ();
        AutoRestart ();
        CameraFollow ();

        HightJumpProcess ();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        

        // Проверка вертикального падения и удара о землю
        if (!isStunned && Rigidbody.velocity.y >= fallThreshold && IsGroundCollision(collision))
        {
            print(Rigidbody.velocity.y);
            StunCharacter();

            foreach (var sprite in spriteRenderer)
            {
                sprite.enabled = true;
            }

            SpriteRenderer.color = new Color(0.75f, 0.75f, 0.75f, 1f);
            ReloadSprite.enabled = true;
        }
    }



    void Stunted ()
    {
        if (isStunned)
        {
            ReloadSprite.transform.eulerAngles -= new Vector3(0, 0, 2);
            
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                ReloadSprite.enabled = false;
                isStunned = false;
                SpriteRenderer.color = FogColors[0];
                foreach (var sprite in spriteRenderer)
                {
                    sprite.enabled = false;
                }
            }
        }
    }



    bool IsGroundCollision(Collision2D collision)
    {
        
        // Проверяем, произошло ли столкновение с землей (или ногами)
        foreach (ContactPoint2D contact in collision.contacts)
        {

            // Проверяем направление контакта
            if (contact.normal.y > 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    void StunCharacter()
    {
        isStunned = true;
        stunTimer = stunDuration;
    }


    void OnGroundCheck()
    {
        Collider2D collider = Physics2D.OverlapBox(groundCheck.position, groundCheckRadius, 0, whatIsGround);
        onGround = collider;

        if(onGround)
            if(collider.gameObject.tag == "Ground")
            {
                ParticleMat.color = ParticleColors[1];
            }
            else
            {
                ParticleMat.color = ParticleColors[0];
            }
        

        if(!JumpCheck)
        {
            JumpTime += Time.deltaTime;

            if(JumpTime >= JumpReloadTime)
            {
                JumpCheck = true;
                JumpTime = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckRadius);
    }


    void Blink ()
    {
        if(isStunned)
            return;

        if (Time.time >= nextBlinkTime)
        {
            if (isBlinking)
            {
                foreach (var sprite in spriteRenderer)
                {
                    sprite.enabled = false;
                }
                nextBlinkTime = Time.time + UnityEngine.Random.Range(blinkInterval.x, blinkInterval.y);
                
            }
            else
            {
                foreach (var sprite in spriteRenderer)
                {
                    sprite.enabled = true;
                }
                nextBlinkTime = Time.time + UnityEngine.Random.Range(blinkDuration.x, blinkDuration.y);
            }

            // Переключаем состояние моргания
            isBlinking = !isBlinking;
        }

    }


    void HightJumpProcess ()
    {
        if(HightJump < transform.position.y - SpawnPoint.y)
            HightJump = transform.position.y - SpawnPoint.y;
    }

    void CameraFollow ()
    {
        Camera.position = Vector3.Lerp(Camera.position, transform.position, Time.deltaTime);

        Camera.position = new Vector3(0, Mathf.Clamp(Camera.position.y, 7.14f, float.MaxValue), -10);
    }

    void KeyRestart ()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart ();
        }
    }

    void AutoRestart ()
    {
        if(transform.position.y < -10)
        {
            Restart ();
        }
    }

    public void Restart ()
    {
        transform.position = SpawnPoint;
        Rigidbody.velocity = Vector2.zero;
    }


    void Touch ()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    TouchBegan ();

                break;
            }

            float xPos = touch.position.x;

            

            angle += touch.deltaPosition.x/5;
            force = Vector2.Distance(RememberTouch, touch.position) / 50;

            angle = Mathf.Clamp(angle, AngleRight, AngelLeft);
            force = Mathf.Clamp(force, MinJumpForce, MaxJumpForce);

            size = (force - MinJumpForce) / (MaxJumpForce - MinJumpForce);

            Angle = Vector2.ClampMagnitude(RememberTouch - touch.position, 1);

            float z = Mathf.Atan2(Angle.x, Angle.y);

            float d = (z / Mathf.PI) * 180;
    
            ForceLine.transform.eulerAngles = new Vector3(0, 0, (-d));
            ForceLine.SetPosition(1, new  Vector3(0, Mathf.Lerp(0, 1.2f, size), 0));
            ForceLine.endWidth = Mathf.Lerp(0.02f, 0.3f, size);

            if(onGround && !isStunned)
                Visual.localScale = new Vector3(Mathf.Lerp(1, 1.2f, size), Mathf.Lerp(1, 0.8f, size), 1);
            
            switch (touch.phase)
            {

                case TouchPhase.Ended:

                TouchEnd ();

                break;
            }
           
        }
    }


    void TouchBegan ()
    {
        if(isStunned)
            return;

        angle = 0;
        force = 0.2f;

        RememberTouch = Input.GetTouch(0).position;
        ForceLine.gameObject.SetActive(true);
    }

    void TouchEnd ()
    {
        Pressed = false;

        ForceLine.SetPosition(1, Vector3.zero);
        ForceLine.endWidth = 0;

        if(onGround && force > MinJumpForce && JumpCheck && !isStunned)
            Jump ();

        ForceVision.gameObject.SetActive(false);

        ForceLine.gameObject.SetActive(false);
    }

    void Jump ()
    {
        Rigidbody.AddForce(Angle * (3.325f * GravityCompensation.Evaluate(size)), ForceMode);

        if(force >= ParticlePlayForce)
            ParticleSystem.Play();

        ParticleSystem.emission.SetBurst(0, new ParticleSystem.Burst(){ time = 0, count = (int)force, cycleCount = 1, repeatInterval = 0.01f, probability = 1 });

        JumpCheck = false;

        Visual.localScale = Vector2.one;
    }
}
