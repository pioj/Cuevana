using System.Collections;
using UnityEngine;

public class control_moto : controllable
{
    [Header("Motocicleta")] 
    private Vector2 dir;
    private float lado,lastlado;
    private bool stopped;
    private bool canMove = false;
    
    public float thrust = 3f;
    public float vel;
    
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
        GameMode.PlayerLanded = (stopped && canMove);
        
        Raycast_Ground();
        
        Mechanics_ACCEL();
        
        Mechanics_FLIP();

        Mechanics_MOVE();
    }
    
    private void Raycast_Ground()
    {
        var layerSuelo = LayerMask.GetMask("suelos");
        var hit = -1f * transform.up;
        var ray = Physics2D.Raycast(transform.position, hit, 0.1f, layerSuelo);
        
        Debug.DrawRay(transform.position,hit * 0.1f,Color.red);
        canMove = (ray.collider != null);
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
        if (!canMove) return;
        
        if ((Input.GetKey(KeyCode.UpArrow)) || Input.GetButton("Thrust"))
        {
            _rb.velocity += dir * (thrust * Time.deltaTime);
        }

        vel = _rb.velocity.magnitude;

        stopped = (Mathf.Approximately(vel,0f));
    }

    private void Mechanics_FLIP()
    {
        if (canMove) return;
        
        if ((Input.GetKey(KeyCode.Q)) || Input.GetAxisRaw("RCS") <= -1)
        {
            _rb.angularVelocity = 0f;
            _rb.rotation += 30f * (thrust * Time.deltaTime);
        }
        
        if ((Input.GetKey(KeyCode.E)) || Input.GetAxisRaw("RCS") >= 1)
        {
            _rb.angularVelocity = 0f;
            _rb.rotation -= 30f * (thrust * Time.deltaTime);
        }
        
        //si la moto da casi la vuelta , me caigo de ella
        var maxrot = Mathf.Abs(_rb.rotation);
        if (maxrot > 160f)
        {
            var exitComp = GetComponent<exitable>();
            if (exitComp) exitComp.Mechanic_EXITVEHICLE(false);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("base")) // o choca contra otro tipo de edificios y cosas duras...
        {
            if (vel > 1f)
            {
                Destroy(gameObject);
            } 
            else if (vel > 0.5f)
            {
                var choque = other.GetContact(0).normal; 
                var rebote = Vector2.Reflect(_rb.velocity,choque);
                _rb.velocity = rebote;

                StartCoroutine(nameof(resetRebote));
            }
        }
    }

    IEnumerator resetRebote()
    {
        yield return new WaitForSeconds(0.5f);
        _rb.angularVelocity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            //explota e instancia un Minion Playable.
            if (prefabMinion) _miniYo = Instantiate(prefabMinion, transform.position, Quaternion.identity);
            _follow.SetTarget(_miniYo.transform);
            Destroy(gameObject);
        }
    }
}
