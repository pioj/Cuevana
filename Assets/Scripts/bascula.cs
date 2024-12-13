using UnityEngine;

public class bascula : MonoBehaviour
{
    public float pesoRequerido = 110f;

    private bool IsPassed => _peso >= pesoRequerido;
    public bool passed;

    private float _peso;
    private Rigidbody2D rb;
    private SpriteRenderer led;
    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        led = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!passed.Equals(IsPassed)) passed = IsPassed;
        var col1 = Color.red;
        var col2 = Color.green;

        led.color = passed ? col2 : col1;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.rigidbody) _peso = rb.mass + other.rigidbody.mass;
    }
}
