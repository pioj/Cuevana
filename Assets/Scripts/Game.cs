using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject Player;
    private GameObject _player;

    //para que la camara siga siempre a una nave...
    private cameraFollow _camComp;

    private points_of_interest _poiComp;
    
    private void Awake()
    {
        var nao = FindObjectOfType<control_nave>();
        if (nao) _player = nao.gameObject;
        _camComp = FindObjectOfType<cameraFollow>();
        _poiComp = GetComponent<points_of_interest>();
    }

    void Start()
    {
        if (_player) _camComp.SetTarget(_player.transform);
    }

    void Update()
    {
        //PAUSA con la tecla P
        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();
        
        if (!_player && Input.GetKeyDown(KeyCode.F1))
        {
            _poiComp.RefreshPOI();
            
            var playerito = FindObjectOfType<control_minion>();
            if (playerito)
            {
                GameMode.Lives--; 
                Destroy(playerito.gameObject);
            }
            
            _player = Instantiate(Player, GameMode.CurrentCheckPoint, Quaternion.identity);
            _camComp.SetTarget(_player.transform);
        } else if (_player && Input.GetKeyDown(KeyCode.F1))
        {
            var _rb = _player.GetComponent<Rigidbody2D>();
            _rb.velocity = Vector2.zero;
            _rb.rotation = 0f;
            _player.transform.position = Vector3.zero;
        }
    }

    public void FocusOn(Transform target)
    {
        if (!target) return;
        _camComp.SetTarget(target);
    }
}
