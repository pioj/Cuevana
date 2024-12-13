using System;
using UnityEngine;

public class exitable : MonoBehaviour
{
    public GameObject prefabPlayable;
    public GameObject prefabDummy;
    
    private bool _canLeave;

    private Game _gameComp;

    private void Awake()
    {
        if (!prefabDummy || !prefabPlayable) throw new Exception("Error! Falta asignar los prefabs de Dummy!!");
        
        _gameComp = FindObjectOfType<Game>();
    }

    void Update()
    {
        _canLeave = GameMode.PlayerLanded;

        if (_canLeave)
        {
            if (Input.GetAxisRaw("Enter/Exit Vehicle") <= -1)
            {
                if (!prefabPlayable || !prefabDummy) return;

                Mechanic_EXITVEHICLE();
            }
        }
    }

    //Ahora incluyo un flag opcional de dejar o no el dummy en su sitio...
    public void Mechanic_EXITVEHICLE(bool ponDummy = true)
    {
        var navePos = transform.position;
        if (ponDummy) Instantiate(prefabDummy, navePos, Quaternion.identity);
        Destroy(gameObject);
                
        var miniYo = Instantiate(prefabPlayable, navePos, Quaternion.identity);
        _gameComp.FocusOn(miniYo.transform);
                
        //Debug.Log("Sale del vehiculo!");
        //Debug.Break(); 
    }
}
