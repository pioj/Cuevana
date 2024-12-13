using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class desactiveitor : MonoBehaviour
{
    public void Quita()
    {
        var rb = GetComponent<Rigidbody2D>();
        var col = GetComponent<Collider2D>();

        if (!rb || !col) return;

        rb.simulated = !rb.simulated;
        col.isTrigger = !col.isTrigger;
    }
}
