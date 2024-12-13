using UnityEngine;

public class lifesystems : MonoBehaviour
{
    public GameObject prefabLifebar;
    public float health;
    private float _currentHealth; 

    private GameObject _bar;
    private info_lifebar _infoComp;
    
    private void Start()
    {
        if (!prefabLifebar) return;
        
        _bar = Instantiate(prefabLifebar, transform);
        _infoComp = _bar.GetComponent<info_lifebar>();

        _currentHealth = health;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!prefabLifebar) return;
        
        if (other.collider.CompareTag("bullet"))
        {
            var bull = other.transform.GetComponent<bullet>();
            var dmg = bull.damage;
            _currentHealth -= dmg;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, health);
            var healthLeft = _currentHealth / health;
            _infoComp.Show(healthLeft, Vector2.up);
            bull.RecycleToPool();

            if (_currentHealth == 0f)
            {
                //si tiene escombros y spawn de prisioneros, lo activamos
                var _escombros = GetComponent<minion_spawnpoint>();
                if (_escombros) _escombros.Spawn();
                Destroy(gameObject);
            }
        }
    }
}
