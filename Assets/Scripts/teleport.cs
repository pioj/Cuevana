using System;
using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform destiny;
    private teleport _destComp;
    private Transform _target;
    private bool _locked;
    private bool _teleported;
    
    private void Awake()
    {
        if (!destiny) throw new Exception("ERROR! Falta asignar el destino!");
        _destComp = destiny.GetComponent<teleport>();
    }

    private void Update()
    {
        if (_locked)
        {
            //teleporto y desactivo el teleport de salida, impido loop infinito...
            _destComp.ResetTarget();
            _destComp.enabled = false;
            
            if (!_teleported) 
            {
                if (_target) _target.position = destiny.position;
                _target = null;
                _teleported = true;
            }
            StartCoroutine(nameof(ResetTeleport),3f);
        }
    }

    private void Start() { }

    public void ResetTarget()
    {
        _target = null;
    }

    IEnumerator ResetTeleport(float secs)
    {
        _locked = false;
        yield return new WaitForSeconds(secs);
 
        _destComp.ResetTarget();
        _destComp.enabled = true;
        _teleported = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isMinion = other.GetComponent<control_minion>();
        if (isMinion)
        {
            _target = other.transform;
            _locked = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (destiny)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, destiny.position);
        }
    }
}
