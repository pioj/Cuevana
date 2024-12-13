using UnityEngine;

public class IA_disparable : MonoBehaviour
{
    private void Start()
    { }

    
     //version principal del disparo, sólo pasamos el origen      
    public void Shoot(Transform from)
    {
        GameSystems.bulletPool.Shoot(from);
    }
    
    
    //variedad que le pasa también el Direction
    public void Shoot(Transform from, Vector3 direction)
    {
        GameSystems.bulletPool.Shoot(from, direction);
    }
    
    //variedad 2
    public void Shoot2(Transform from, Vector3 direction)
    {
        GameSystems.bulletPool.Shoot2(from, direction);
    }
}
