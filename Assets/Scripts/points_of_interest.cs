using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class points_of_interest : MonoBehaviour
{
    [SerializeField] private List<Interactivo> puntosDeInteres;
    private int _currentPointed=0; 
        
    private void Awake()
    {
        RefreshPOI();
    }

    public Transform GiveNextPOI()
    {
        if (puntosDeInteres==null || puntosDeInteres.Count <1) 
        {
            RefreshPOI();
            return null;
        }
        
        //borra los que estén missing
        puntosDeInteres.RemoveAll(x => !x);
        
        var poi = puntosDeInteres[_currentPointed];
        if (!poi && _currentPointed == puntosDeInteres.Count- 1)
        {
            RefreshPOI();
            return null;
        } else if (!poi)
        {
            _currentPointed = ( _currentPointed + 1 + puntosDeInteres.Count) % puntosDeInteres.Count;
            GiveNextPOI();
        }

        _currentPointed = ( _currentPointed + 1 + puntosDeInteres.Count) % puntosDeInteres.Count;
        
        return poi.transform;
    }

    public void RefreshPOI()
    {
        var lista = FindObjectsOfType<Interactivo>();
        
        if (puntosDeInteres!= null && puntosDeInteres.Count >0) puntosDeInteres.Clear();

        puntosDeInteres = lista.Distinct().ToList();

        OrdenaPreferentes();
        
        _currentPointed = 0;
    }

    public void OrdenaPreferentes()
    {
        puntosDeInteres.OrderBy(x => x is controllable ? 0 : 1);
    }

}
