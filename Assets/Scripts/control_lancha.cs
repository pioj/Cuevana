using System.Collections;
using UnityEngine;

public class control_lancha : controllable
{
    [Header("Lancha")] 
    private Vector2 dir;
    private float lado,lastlado;
    public float thrust = 4f;
    public float vel;
    private bool canMove = false;

    private bool _idling;

    
    private Rigidbody2D _rb;
    
    [Header("Minion Playable")]
    public GameObject prefabMinion;
    private GameObject _miniYo;
    private cameraFollow _follow;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _follow = Camera.main.GetComponent<cameraFollow>();
    }
    
    private void Start()
    {
        //timer = 0f;

        dir = transform.right; //por defecto mira a la derecha
        _rb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GameMode.PlayerLanded = (_idling || !canMove);
        
        if (!canMove) return;
        
        Mechanics_ACCEL();

        Mechanics_MOVE();
    }

    private void Mechanics_MOVE()
    {
        dir = (transform.localScale == Vector3.one) ? Vector2.right : Vector2.left;
        var isMoving = Input.GetAxisRaw("Horizontal") != 0f;
        if (isMoving)
        {
            lado = Input.GetAxisRaw("Horizontal");

            if (lado != lastlado)
            {
                if (lado >= 1f) transform.localScale = Vector3.one;
                if (lado <= -1f) transform.localScale = Vector3.left + Vector3.up + Vector3.forward;
                lastlado = lado;
            }
        }
    }

    private void Mechanics_ACCEL()
    {
        if ((Input.GetKey(KeyCode.UpArrow)) || Input.GetButton("Thrust"))
        {
            _rb.velocity += dir * (thrust * Time.deltaTime);
        }

        vel = _rb.velocity.magnitude;
        
        _idling = (vel < 0.2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("suelo"))
        {
            var choque = other.GetContact(0).normal;

            if (choque.y > 0)
            {
                canMove = false;
                return;
            }

            var rebote = Vector2.Reflect(_rb.velocity,choque);
            _rb.velocity = rebote;
            _rb.freezeRotation = false;
            StartCoroutine(nameof(resetRebote));
        }
    }
    
    IEnumerator resetRebote()
    {
        yield return new WaitForSeconds(0.5f);
        _rb.angularVelocity = 0f;
        _rb.rotation = 0f;
        _rb.freezeRotation = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            canMove = true;
        }
    }
}
