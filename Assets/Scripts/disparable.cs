using UnityEngine;

public class disparable: MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameSystems.bulletPool.Shoot2(transform,transform.up);
        }
    }
}
