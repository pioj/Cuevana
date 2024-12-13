using System;
using UnityEngine;

public class garage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _open;
    
    [Header("Sprites")]
    [SerializeField] private Sprite garageClosed;
    [SerializeField] private Sprite garageOpen;

    [Header("Precios")] 
    [SerializeField] private int precioMoto = 100;
    [SerializeField] private int preciolancha = 1000;

    [Header("Alamacen")] 
    public int motos;
    public int lanchas;
    
    private SpriteRenderer _spr;

    private void Awake()
    {
        if (!garageClosed || !garageOpen) throw new Exception("Error! No has asignado los sprites del Garage!!");
        _spr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        _spr.sprite = _open ? garageOpen : garageClosed;
    }

    public void OpenGarage()
    {
        _open = true;
    }

    public void CloseGarage()
    {
        _open = false;
    }

    public void ToggleGarage()
    {
        _open = !_open;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_open) return;
        
        if (other.CompareTag("moto"))
        {
            var exitComp = other.GetComponent<exitable>();
            if (exitComp) exitComp.Mechanic_EXITVEHICLE(false);
            
            motos++;
        }

        else if (other.CompareTag("lancha"))
        {
            var exitComp = other.GetComponent<exitable>();
            if (exitComp) exitComp.Mechanic_EXITVEHICLE(false);
            
            lanchas++;
        }
    }
}
