using UnityEngine;

public class GameSystems : MonoBehaviour
{
    [Header("Pools")]
    public static Bullet_Pool bulletPool;

    private void Awake()
    {
        bulletPool = FindObjectOfType<Bullet_Pool>();
    }

    private void Start() {}
}
