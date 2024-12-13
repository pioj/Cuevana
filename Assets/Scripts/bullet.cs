using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class bullet : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;
    
    private Collider2D _col;
    private SpriteRenderer _rend;
    private bool _isMoving = true;
    private Vector2 _dir;
    private Bullet_Pool _sender;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_isMoving)
        {
            //transform.Translate(dir * (speed * Time.deltaTime));
            transform.localPosition += (Vector3)_dir * (speed * Time.deltaTime);
        }
    }

    public void Reset()
    {
        _col.enabled = false;
        _rend.enabled = false;
        _col.transform.position = Vector3.zero;
        _dir = Vector2.zero;
        _isMoving = false;
    }

    public void Fire(Bullet_Pool author, Vector3 pos = new Vector3(), Vector2 direc = new Vector2() )
    {
        if (author.Equals(null)) return;

        _sender = author;
        //_col.enabled = true;
        _rend.enabled = true;
        _col.transform.position = pos;
        _dir = direc;
        _isMoving = true;

        StartCoroutine(nameof(EnableCol), 0.5f);
        StartCoroutine(nameof(RecycleAtSecs), 10f);
    }

    public void SetDir(Vector2 newDir)
    {
        _dir = newDir;
    }

    public Vector2 GetDir()
    {
        return _dir;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("suelo")) // por ejemplo
        {
            RecycleToPool();
        }
    }

    public void RecycleToPool()
    {
        if (!_isMoving) return;
        if (_sender) _sender.Recycle(this.gameObject);
    }

    IEnumerator EnableCol(float secs = 0.5f)
    {
        yield return new WaitForSeconds(secs);
        _col.enabled = true;
    }

    IEnumerator RecycleAtSecs(float secs = 10f)
    {
        yield return new WaitForSeconds(secs);
        RecycleToPool();
    }
}
