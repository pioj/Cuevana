using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class gravitable : MonoBehaviour
{
    //privadas
    private Rigidbody2D _rb;
    private Collider2D _col;

    [SerializeField] private float giroFlotando = 10f;
    private float _defaultGravity;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _defaultGravity = _rb.gravityScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_rb || !_col) return;
        //if (col is BoxCollider2D boxCollider2D) col = boxCollider2D;
        
        if (other.CompareTag("0gravzone"))
        {
            var puntos = new ContactPoint2D[0];
            _col.GetContacts(puntos);
            foreach (var punto in puntos)
            {
                if (!other.OverlapPoint(punto.point)) return;
            }
            
            if (_rb.gravityScale !=0f) _rb.gravityScale = 0f; //por defecto, cada control tiene su propio peso...
            if (_rb.freezeRotation) _rb.freezeRotation = false;
            _rb.angularVelocity = giroFlotando;  //gira por ahí...
            return;
        }

        if (other.CompareTag("invertgravzone"))
        {
            var puntos = new ContactPoint2D[0];
            _col.GetContacts(puntos);
            foreach (var punto in puntos)
            {
                if (!other.OverlapPoint(punto.point)) return;
            }
            
            if (_rb.gravityScale != (-1f * _defaultGravity)) _rb.gravityScale = (-1f * _defaultGravity); //por defecto la nave tiene 0.1

            if (_rb.rotation!=180f) _rb.rotation = Mathf.LerpAngle(_rb.rotation, 180f, 5f * Time.deltaTime);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_rb || !_col) return;
        
        if (other.CompareTag("0gravzone"))
        {
            _rb.gravityScale = _defaultGravity;
            if (!_rb.freezeRotation) _rb.freezeRotation = true;
            _rb.angularVelocity = 0f;
            _rb.rotation = 0f;
            return;
        }

        if (other.CompareTag("invertgravzone"))
        {
            _rb.gravityScale = _defaultGravity;

            if (_rb.rotation != 0f) _rb.rotation = 0f;
        }
    }
}
