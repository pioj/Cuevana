using System.Collections;
using UnityEngine;

public class control_minion : controllable
{
    enum onSurface
    {
        onGround,
        onAir,
        onWater,
    }
    [SerializeField] private onSurface _on;

    public float speed = 1f;
    public float jumpforce = 5f;

    private Vector2 _slopedir = Vector2.zero;
    private Vector3 _lastMov; 
    
    private Rigidbody2D _rb;
    private Animator _anim;
    
    [SerializeField] private bool _canMove;
    
    private float _timeFalling;
    private bool _isFalling;
    
    private Vector2 _distFell;
    
    private float _deathFall = 3.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _on = onSurface.onGround;

        //reseteo distancia de caída
        _distFell.x = transform.position.y;
        _distFell.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   
        //vamos a probar a hacer el Raycast AQUI FUERA, que lo haga siempre...
        
        switch (_on)
        {
            case onSurface.onGround:
                //
                
                //RAYCAST DE AGUA POR EL CUELLO
                var watermask = LayerMask.GetMask("Water");
                var cogote = transform.TransformPoint(Vector2.up * 0.25f); 
                var hitwater = Physics2D.Raycast(cogote, Vector2.up, 0.1f, watermask);
                Debug.DrawRay(cogote,Vector2.up * 0.1f,Color.cyan);
                //SI TOCO CONTRA EL AGUA..?
                if (hitwater.collider != null)
                {
                    //limpio timeFalling, por si acaso..
                    _timeFalling = 0f;
                    _anim.SetFloat("timeFalling",0f);
                    _isFalling = false;
                    _anim.SetBool("isFalling",false);
                    
                    if (_rb.bodyType != RigidbodyType2D.Dynamic) _rb.bodyType = RigidbodyType2D.Dynamic;
                    _anim.SetBool("isSwimming",true);
                    _canMove = true;
                    //cambio a OnWater
                    _on = onSurface.onWater;
                    return;
                }
                
                //RAYCAST DE SUELOS/SLOPES
                var laymask = LayerMask.GetMask("suelos");
                var rayh = transform.TransformDirection(Vector2.down);
                //en lugar del centro, tiro el raycast desde la punta de detras...
                var puntita = transform.TransformPoint(Vector2.zero); // ó transform.position , para su centro
                //
                var hitd = Physics2D.Raycast(puntita, rayh, 0.1f, laymask);
                Debug.DrawRay(puntita, rayh * 0.1f, Color.green);
                Debug.DrawRay(puntita, Vector3.left * 0.2f, Color.red);
                Debug.DrawRay(puntita, Vector3.right * 0.2f, Color.red);
                //SI TOCO CONTRA EL SUELO...?
                if (hitd.collider != null)
                {
                    //limpio timeFalling, por si acaso..
                    _timeFalling = 0f;
                    _anim.SetFloat("timeFalling",0f);
                    _isFalling = false;
                    _anim.SetBool("isFalling",false);

                    //saco el angle del contacto
                    Debug.DrawRay(transform.position, -hitd.normal * 0.2f, Color.magenta);
                    if (hitd.normal.x != 0f)
                    {
                        if (_rb.bodyType != RigidbodyType2D.Static) _rb.bodyType = RigidbodyType2D.Static;
                    }
                    else
                    {
                        if (_rb.bodyType != RigidbodyType2D.Dynamic) _rb.bodyType = RigidbodyType2D.Dynamic;
                    }

                    _slopedir = hitd.normal;
                    _canMove = true;

                    //desactivo nadar, por si acaso...
                    _anim.SetBool("isSwimming", false);
                }
                else //SI NO TOCO CONTRA EL SUELO, VUELVO A MODO AIRE-CAER..
                {
                    if (_rb.bodyType != RigidbodyType2D.Dynamic) _rb.bodyType = RigidbodyType2D.Dynamic;
                    _canMove = true;
                    _isFalling = true;

                    _timeFalling = 0f;
                    //informo al Animator que cae
                    _anim.SetFloat("timeFalling",0f);
                    _anim.SetBool("isFalling", true);

                    //cambio al modo aire (por defecto)
                    _on = onSurface.onAir;
                    return;
                }
                
                //CAMINAR
                if (_canMove) Mechanics_MOVE();
                
                //SALTAR
                if (_canMove) Mechanics_JUMP();


                //
                break;
            
            case onSurface.onWater:
                //
                
                //NADAR
                Mechanics_SWIM();

                //
                break;
            
            case onSurface.onAir:
                //
                
                //pase lo que pase, me aseguro de que el rigidbody funcione de nuevo
                if (_rb.bodyType != RigidbodyType2D.Dynamic) _rb.bodyType = RigidbodyType2D.Dynamic;
                
                //CAER
                _isFalling = (Vector2.Dot(_rb.velocity, Vector2.up * _rb.gravityScale) < 0f);

                if (_isFalling)
                {
                    //voy reduciendo el momentum hasta caer recto
                    _lastMov *= 0.99f;
                    transform.localPosition += _lastMov * Time.deltaTime; //Also, FIX del "super-salto"... 
                    
                    if (_timeFalling > 0.75f) 
                    {
                        _canMove = false;
                        
                        //marco la istancia inicial de caída
                        if (_distFell == Vector2.zero) _distFell.x = transform.position.y;
                    }
                    _timeFalling += 1f * Time.deltaTime;
                    //Anims de Caer
                    _anim.SetBool("isFalling", _isFalling);
                    //blendtree de caida...
                    _anim.SetFloat("timeFalling", _timeFalling);

                    //EL RAYCAST DEL SUELO
                    var layermask = LayerMask.GetMask("suelos");
                    var ray = transform.TransformDirection(Vector2.down);
                    var hit = Physics2D.Raycast(transform.position, ray, 0.1f, layermask);
                    Debug.DrawRay(transform.position, ray * 0.1f, Color.green);
                    //si choco contra el suelo...?
                    if (hit.collider != null)
                    {
                        _isFalling = false;
                        _anim.SetBool("isFalling", false);
                        
                        //actualizo la distancia final de la caida
                        _distFell.y = transform.position.y;
                        
                        _timeFalling = 0f;
                        
                        //prueba, cambio a onGround
                        _canMove = false;
                        //_on = onSurface.onGround;
                        return;
                    }
                }
                else if (!_isFalling)
                {
                    _slopedir = Vector2.zero;
                    transform.localPosition += _lastMov * Time.deltaTime; //Also, FIX del "super-salto", aplico también el time.deltaTime
                    _canMove = false;
                }

                //
                    break;
        }
    }
    
    //Sólo podemos entrar en vehículos si estamos caminando/nadando y podemos movernos...
    public bool CanEnterVehicles()
    {
        return (_on == onSurface.onGround || _on == onSurface.onWater) && _canMove;
    }

    private void Mechanics_JUMP()
    {
        if ((Input.GetButtonDown("Thrust")) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_rb.bodyType != RigidbodyType2D.Dynamic) _rb.bodyType = RigidbodyType2D.Dynamic;

            _rb.velocity = Vector2.zero;
            _rb.AddForce(transform.up * jumpforce, ForceMode2D.Force);
            _anim.SetTrigger("jump");
            _canMove = false;
            //cambio a modo aire
            _on = onSurface.onAir;
        }
    }

    private void Mechanics_SWIM()
    {
        var swAmount = Input.GetAxisRaw("Horizontal");
        var isSwimming = Input.GetAxisRaw("Horizontal") != 0f;
        if (isSwimming && swAmount != 0f)
        {
            transform.localScale = swAmount > 0 ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            if (swAmount < 0f) swAmount = -1f;
            if (swAmount > 0f) swAmount = +1f;
        }

        //-Blendtree de Nadar
        _anim.SetFloat("SwimDir", swAmount);

        
        //Fix del "super-salto" -> aplicar Time.deltaTime SOLO en la asignación del transform.localPosition
        transform.localPosition += Vector3.right *(swAmount * speed* Time.deltaTime);
        _lastMov = Vector3.zero;
    }

    private void Mechanics_MOVE()
    {
        var movAmount = Input.GetAxisRaw("Horizontal");
        var isMoving = Input.GetAxisRaw("Horizontal") != 0f;
        if (isMoving && movAmount != 0f)
        {
            transform.localScale = movAmount > 0 ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            if (movAmount < 0f) movAmount = -1f;
            if (movAmount > 0f) movAmount = +1f;
        }

        //-Blendtree de Moverse
        _anim.SetFloat("MovDir", movAmount);

        var tempdir = (_slopedir.x != 0f) ? -Vector2.Perpendicular(_slopedir) : Vector2.right;

        
        
        //fix para el "super-salto" -> aplicar el Time.DeltaTime SOLO en la asignación del transform.localPosition;
        transform.localPosition += (Vector3)tempdir * (movAmount * speed * Time.deltaTime);
        _lastMov = (Vector3) tempdir * (movAmount * speed);
    }


    private void DelayStandup(float secs)
    {
        _slopedir = Vector2.zero;
        _lastMov = Vector3.zero;

        _canMove = false;
        
        _timeFalling = 0f;
        _anim.SetBool("isFalling",false);
        _anim.SetFloat("timeFalling",0f);

        
        //Esta línea asegura que sólo aplicamos delay si chocamos contra el Suelo,etc...
        if (_on == onSurface.onWater)
        {
            //reseteo la distancia de caída
            _distFell.x = transform.position.y;
            _distFell.y = _distFell.x;
            
            _canMove = true;
            return;
        }

        var hostiazo = Vector2.Distance(new Vector2(0f, _distFell.x), new Vector2(0f, _distFell.y));
        //Debug.Log(hostiazo);
        //Debug.DrawLine(new Vector2(0f,_distFell.x),new Vector2(0f,_distFell.y),Color.yellow);
        
        //si cae desde más alto de lo permitido, lo mato.
        if (hostiazo >= _deathFall)
        {
            KillMinion();
        }
        else
        {
            StartCoroutine(nameof(MakeDelay), secs);
        }
    }

    IEnumerator MakeDelay(float secs)
    {
        yield return new WaitForSeconds(secs);
        
        //reseteo la distancia de caída
        _distFell.x = transform.position.y;
        _distFell.y = _distFell.x;
        
        _canMove = true;
        //cambio a modo Ground
        _on = onSurface.onGround;
    }

    private void KillMinion()
    {
        _canMove = false;
        GameMode.Lives--;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {  
            _anim.SetBool("isSwimming",true);           
            _isFalling = false;
            _anim.SetBool("isFalling",false);
            _timeFalling = 0f;
            _anim.SetFloat("timeFalling",0f);
            
            //reseteo la distancia de caída
            _distFell.x = transform.position.y;
            _distFell.y = _distFell.x;
            
            _canMove = true;
            //cambio a onWater
            _on = onSurface.onWater;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            _anim.SetBool("isSwimming",false);
            _isFalling = true;
            _anim.SetBool("isFalling",true);
            _timeFalling = 0f;
            _anim.SetFloat("timeFalling",0);
            //cambio a onAir
            _on = onSurface.onAir;
        }
    }
}
