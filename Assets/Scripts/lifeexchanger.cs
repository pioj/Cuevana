using UnityEngine;

public class lifeexchanger : MonoBehaviour
{
   private bool _naveLanded;
   private bool _canswap;
   private float _swapfreq = 2f;
   private float _swaptimer;

   private void Update()
   {

      if (_naveLanded)
      {
         _canswap = (GameMode.PrisonersAboard > 0);
         if (!_canswap) return;
         
         if (_swaptimer > _swapfreq)
         {
            _swaptimer = 0;
            GameMode.Lives++;
            GameMode.PrisonersAboard--;
            GameMode.PrisonersKilled++;
         }
         else
         {
            _swaptimer += 1f * Time.deltaTime;
         }
      }
   }
   
   private void OnTriggerStay2D(Collider2D other)
   {
      if (other.CompareTag("nave"))
      {
         _naveLanded = GameMode.PlayerLanded;
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("nave")) _naveLanded = false;
   }
}
