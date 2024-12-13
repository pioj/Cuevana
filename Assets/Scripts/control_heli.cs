using System.Collections;
using UnityEngine;

public class control_heli : controllable
{
    private Rigidbody2D _rb;
    private BoxCollider2D _col;

    public float vel;
    public float thrust = 2f;
    private Vector2 dir;
    public float maxsteer = 20f;
    public bool landed;
    private float caida;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        vel = _rb.velocity.magnitude;
        
        //aterrizar 
        if (vel > 0.1f)
        {
            landed = false;
        }
        else
        {
            var bottom = transform.position - new Vector3(0, 0.4f, 0);
            var hit = Physics2D.Raycast(bottom, -transform.up, 0.2f, LayerMask.GetMask("suelos"));
            Debug.DrawLine(bottom, bottom - new Vector3(0, 0.2f, 0), Color.magenta);
            landed = (hit.collider != null && vel < 0.1f);
            
        }
        //
        
        //metricas del modo de juego
        GameMode.PlayerLanded = landed;
        //
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetButton("Thrust"))
        {
            _rb.velocity+= Vector2.up * (thrust * Time.deltaTime);
        }

        var _steer = Input.GetAxis("Horizontal");
        //dir.x = _steer * -maxsteer;
        dir.x = Mathf.Lerp(dir.x, (_steer * -maxsteer),  Time.deltaTime);
        _rb.rotation = dir.x;
        
        Bounds boxBounds = _col.bounds;
        Vector2 topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        caida = transform.TransformPoint(topRight).magnitude;

        //if (_steer != 0) _rb.gravityScale = caida;
        
        _rb.velocity = new Vector2(-dir.x * 0.25f, _rb.velocity.y);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var tago = other.transform.tag;
        switch (tago)
        {
            case "base" : case "suelo":
                //
                if (vel > 1f)
                {
                    //se destruye la nave
                    GameMode.Lives--;
                    GameMode.PrisonersKilled += GameMode.PrisonersAboard;
                    GameMode.PrisonersAboard = 0;
                    Destroy(gameObject);
                } else if (vel > 0.5f)
                {
                    var choque = other.GetContact(0).normal; 
                    var rebote = Vector2.Reflect(_rb.velocity,choque);
                    _rb.velocity = rebote;

                    StartCoroutine(nameof(resetRebote));
                }
                //
                break;
        }
    }
    
    IEnumerator resetRebote()
    {
        yield return new WaitForSeconds(0.5f);
        _rb.angularVelocity = 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var points = new ContactPoint2D[0];
        var puntos = _col.GetContacts(points);
        if (points.Length < 1) return;
        
        
        if (other.CompareTag("0gravzone"))
        {
            foreach (var point in points)
            {
                var po = transform.TransformPoint(point.point);
                if (!other.OverlapPoint(po)) return;
            }
            
            if (_rb.gravityScale !=0f) _rb.gravityScale = 0f; //por defecto la nave tiene 0.1
        }

        if (other.CompareTag("invertgravzone"))
        {
            
            foreach (var point in points)
            {
                var po = transform.TransformPoint(point.point);
                if (!other.OverlapPoint(po)) return;
            }
            
            if (_rb.gravityScale != -0.1f) _rb.gravityScale = -0.1f; //por defecto la nave tiene 0.1
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("0gravzone"))
        {
            _rb.gravityScale = 0.1f; //por defecto la nave tiene 0.1
        }

        if (other.CompareTag("invertgravzone"))
        {
            _rb.gravityScale = 0.1f; //por defecto la nave tiene 0.1 y hacia abajo..
        }
    }
    
}
