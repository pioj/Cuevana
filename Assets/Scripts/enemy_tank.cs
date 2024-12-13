using System;
using UnityEngine;

public class enemy_tank : MonoBehaviour
{
    public float speed = 0.5f;
    private bool _onSight;
    private Transform _target;
    
    [Header("Cannon")]
    [SerializeField] private Transform cannon;

    private float _freqShoot = 2.5f;
    private float _timerShoot;
    private IA_disparable shotComp;

    private void Awake()
    {
        if (!cannon) throw new Exception("Error! No se han asignado los internals al Tanke!");
        shotComp = GetComponent<IA_disparable>();
    }

    // Start is called before the first frame update
    void Start() { }


    private void Update()
    {
        if (_onSight && _target)
        {
            var dir = _target.position - transform.position;
            transform.localScale = (dir.x > 0f) ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            cannon.localScale = (dir.x > 0f) ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            
            var dist = Vector2.Distance(transform.position, _target.position);
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (transform.localScale.x == 1f) ? angle :  180f - angle;
            
            //apunto con el canyon
            if (angle > 10f && angle < 80f)
            {
                cannon.transform.right = _target.position - cannon.transform.position;
                
                //si está lejos me acerco un poco
                if (dist > 4.8f) transform.localPosition += Vector3.right * (transform.localScale.x * speed * Time.deltaTime);

                //disparo si puedo y cuando llega al timer
                if (shotComp)
                {
                    if (_timerShoot > _freqShoot)
                    {
                        _timerShoot = 0f;
                        shotComp.Shoot2(cannon.transform,cannon.transform.right);
                    }
                    else
                    {
                        _timerShoot += 1f * Time.deltaTime;
                    }
                }
            }
            
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("nave") || other.CompareTag("helicoptero"))
        {
            _onSight = true;
            _target = other.transform;
        }

        if (other.CompareTag("water"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("nave") || other.CompareTag("helicoptero"))
        {
            _onSight = false;
            _target = null;
        }
    }
}
