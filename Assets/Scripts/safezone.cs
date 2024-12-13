using UnityEngine;

public class safezone : MonoBehaviour
{
    public GameObject prefabRescued;
    private float _freqspawn=1f;
    private float _timer;
    private bool _naveLanded;
    private Vector2 _posnave;


    private void Start()
    {
        _timer = 0f;
        _naveLanded = false;
    }

    private void Update()
    {
        if (!_naveLanded) return;
        
        if (_timer > _freqspawn)
        {
            if (prefabRescued) {
                if (GameMode.PrisonersAboard > 0)
                {
                    Instantiate(prefabRescued, _posnave, Quaternion.identity);
                    GameMode.PrisonersAboard--;
                }
            }
            _timer = 0f;
        }
        else
        {
            _timer += 1f * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("nave"))
        {
            var naveComp = other.transform.GetComponent<control_nave>();
            _naveLanded = GameMode.PlayerLanded;
            _posnave = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _naveLanded = false;
    }
}
