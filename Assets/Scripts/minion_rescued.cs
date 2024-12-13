using System.Timers;
using UnityEngine;

public class minion_rescued : MonoBehaviour
{
    private Vector2 _dir;
    private Animator _anim;
    private SpriteRenderer _spr;
    public float speed = 1f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("safezone"))
        {
            var centro = other.bounds.center;
            var dir = centro.x - transform.position.x;
            
            //Debug.DrawLine(transform.position,centro,Color.green);
            var dist = Vector2.Distance(transform.position, centro);
            
            if  (dist<0.1)
            {
                GameMode.PrisonersRescued++;
                Destroy(gameObject);
            }
            else
            {
                _anim.SetBool("isMoving", true);

                if (dir < 0)
                {
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                    _spr.flipX = false;
                }
                else if (dir > 0)
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                    _spr.flipX = true;
                }
            }
        }
    }
}
