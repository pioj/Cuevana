using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pool : MonoBehaviour
{
    public GameObject prefabBullet;
    public int poolSize = 15;
    
    [SerializeField] private List<GameObject> pool = new List<GameObject>();
    private GameObject _poolFolder;
    private int _bulletsLeft;

    private void Start()
    {
        if (!prefabBullet) throw new Exception("Error! No hay prefab del Bullet asignado a la Pool!");

        _poolFolder = new GameObject("__BULLET_POOL");
        _poolFolder.transform.SetParent(transform);
        
        for (int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(prefabBullet);
            pool.Add(bullet);
            pool[i].transform.SetParent(_poolFolder.transform);
            pool[i].transform.GetComponent<bullet>().Reset();
            _bulletsLeft++;
        }
    }
    
    public void Shoot(Transform origin)
    {
        //ñapa guarra, asume disparar desde el TOP de la nave
        pool[_bulletsLeft-1].transform.SetParent(origin);
        pool[_bulletsLeft-1].transform.position = origin.position;
        pool[_bulletsLeft - 1].transform.rotation = origin.rotation;
        pool[_bulletsLeft-1].transform.Translate(0f,0.6f,0f);
        
        var pos = pool[_bulletsLeft - 1].transform.position;
        var rot = origin.rotation.z;
        var dir = new Vector2(Mathf.Cos((rot+90f) * Mathf.Deg2Rad), Mathf.Sin((rot+90f) * Mathf.Deg2Rad) );
        
        pool[_bulletsLeft-1].GetComponent<bullet>().Fire(this, pos,dir);
        pool[_bulletsLeft-1].transform.SetParent(_poolFolder.transform);
        _bulletsLeft--;
        //
    }
    
    //variedad que dispara pasando un Vector3 Direction
    public void Shoot(Transform origin, Vector3 direction)
    {
        pool[_bulletsLeft-1].transform.SetParent(origin);
        pool[_bulletsLeft-1].transform.position = origin.position;
        pool[_bulletsLeft - 1].transform.rotation = Quaternion.LookRotation(direction);
        pool[_bulletsLeft-1].transform.Translate(0f,0.6f,0f);
        
        var pos = pool[_bulletsLeft - 1].transform.position;
        var rot = pool[_bulletsLeft - 1].transform.rotation.z;
        var dir = new Vector2(Mathf.Cos((rot+90f) * Mathf.Deg2Rad), Mathf.Sin((rot+90f) * Mathf.Deg2Rad) );
        
        pool[_bulletsLeft-1].GetComponent<bullet>().Fire(this, pos,dir);
        pool[_bulletsLeft-1].transform.SetParent(_poolFolder.transform);
        _bulletsLeft--;
    }
    
    //otra variante...
    public void Shoot2(Transform origin, Vector3 dir)
    {
        pool[_bulletsLeft-1].transform.SetParent(origin);
        pool[_bulletsLeft-1].transform.position = origin.position;
        pool[_bulletsLeft - 1].transform.right = dir;
        
        var pos = pool[_bulletsLeft - 1].transform.position;
        
        pool[_bulletsLeft-1].GetComponent<bullet>().Fire(this, pos,dir);
        pool[_bulletsLeft-1].transform.SetParent(_poolFolder.transform);
        _bulletsLeft--;
    }

    
    public void Recycle(GameObject bala)
    {
        pool[pool.IndexOf(bala)].transform.SetParent(_poolFolder.transform);
        pool[pool.IndexOf(bala)].transform.GetComponent<bullet>().Reset();
        _bulletsLeft++;
    }
}
