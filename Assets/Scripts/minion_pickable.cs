using UnityEngine;

public class minion_pickable : MonoBehaviour
{
    public GameObject waterPrefab;
    
    private Vector2 _dir;
    private Animator _anim;
    private SpriteRenderer _spr; 

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var maxdist = Vector3.right * 3f;

        Debug.DrawLine(transform.position-maxdist, transform.position+maxdist,Color.green);
        var hit = Physics2D.Linecast(transform.position-maxdist,transform.position+maxdist,LayerMask.GetMask("naves"));

        //Debug.Log(hit.collider);

        if (hit.collider != null && hit.collider.CompareTag("nave"))
        {
            var naveComp = hit.transform.GetComponent<control_nave>();
            if (naveComp.landed)
            {
                var n = transform.InverseTransformPoint(hit.transform.position).x;
                if (n < 0)
                {
                    _dir = Vector2.left;
                }
                else if (n > 0)
                {
                    _dir = Vector2.right;
                }

                _anim.SetBool("isMoving", true);
                _spr.flipX = (_dir.x == 1f);
            }
            else
            {
                _dir = Vector2.zero;
                _anim.SetBool("isMoving", false);
            }

            transform.Translate(_dir.x * Time.deltaTime, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("nave"))
        {
            GameMode.PrisonersAboard++;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("water"))
        {
            if (waterPrefab) Instantiate(waterPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
