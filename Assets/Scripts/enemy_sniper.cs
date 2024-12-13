using System;
using UnityEngine;

public class enemy_sniper : MonoBehaviour
{
    private bool _onSight;
    private Transform _target;
    private float _freqShoot = 3.5f;
    private float _timerShoot;
    private IA_disparable shotComp;
    
    [Header("Cannon")]
    [SerializeField] private Transform cannon;
    
    private void Awake()
    {
        if (!cannon) throw new Exception("Error! No se han asignado los internals al Sniper!");
        shotComp = GetComponent<IA_disparable>();
    }

    private void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        if (_onSight && _target)
        {
            //fix, subo un poco la Y del target, que apunte al pecho..
            var fix_targetPos = new Vector3(_target.position.x,_target.position.y + 0.2f,_target.position.z);
            
            var dir = fix_targetPos - transform.position;
            
            transform.localScale = (dir.x > 0f) ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            cannon.localScale = (dir.x > 0f) ? Vector3.one : Vector3.left + Vector3.up + Vector3.forward;
            
            var dist = Vector2.Distance(transform.position, fix_targetPos);
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            //magic number para asegurar que esté siempre en el rango -90,90, da igual donde mire...
            angle = (transform.localScale.x == 1f) ? angle :  180f * Mathf.Sign(angle) - angle;

            //Debug.Log(angle);
            
            //apunto con el canyon
            if (Mathf.Abs(angle) < 80f)
            {
                cannon.transform.right = fix_targetPos - cannon.transform.position;
                //si está más lejos del mínimo, le dispara 
                if (dist > 1f)
                {
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("moto") || other.CompareTag("lancha"))
        {
            _onSight = true;
            _target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("moto") || other.CompareTag("lancha"))
        {
            _onSight = false;
            _target = null;
        }
    }
}
