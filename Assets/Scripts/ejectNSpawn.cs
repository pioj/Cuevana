using System.Collections;
using UnityEngine;

public class ejectNSpawn : MonoBehaviour
{
    public GameObject prefabSpawn;
    
    [Header("Spawn Settings")]
    public float speed = 1f;
    public float spawnTime = 0.5f;
    
    private Rigidbody2D _rb;
    private Collider2D _col;
    private Animator _anim;
    
    private float timer;
    private Vector2 dir;

    private GameObject miniYo;
    private cameraFollow _follow;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        _follow = Camera.main.GetComponent<cameraFollow>();
    }

    private void Start()
    {
        timer = 0f;
        dir = Vector2.zero; 
        _rb.velocity = Vector2.up;
        //
        _col.enabled = false;
    }

    public void Init(int lado)
    {
        transform.localScale = (lado > 0f) ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
        if (_anim) _anim.SetFloat("Side",-lado);
        _rb.AddForce(Vector3.right * (-lado * speed), ForceMode2D.Force);
        StartCoroutine(nameof(EnableCol), 1f);
    }

    private void Update()
    {
        //postura del anim acorde a donde mire...
        
        
        if (timer > spawnTime)
        {
            _rb.velocity = Vector2.zero;
            
            //explota e instancia un paracaidista...
            if (prefabSpawn) miniYo = Instantiate(prefabSpawn, transform.position, Quaternion.identity);
            
            _follow.SetTarget(miniYo.transform);
            Destroy(gameObject);
        }
        else
        {
            timer+= 1f* Time.deltaTime;
        }
    }

    private IEnumerator EnableCol(float secs)
    {
        yield return new WaitForSeconds(secs);
        _col.enabled = true;
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        //Si choco contra el agua o contra el suelo, fuerzo el spawn        
        if (other.transform.CompareTag("suelo") || other.transform.CompareTag("water"))
        {
            timer = spawnTime;
        }
    }
}
