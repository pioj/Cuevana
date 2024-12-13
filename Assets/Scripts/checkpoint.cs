using UnityEngine;

public class checkpoint : MonoBehaviour
{
   [SerializeField] private Transform flag;
   [SerializeField] private bool isCurrent;
   private Vector2 _downPos;
   private Vector2 _upPos;
   private Vector2 _nextPos;
   private Vector2 _navePos;
   private bool _naveLanded;

   private void Start()
   {
      if (flag)
      {
         _downPos = flag.position;
         _upPos = _downPos + new Vector2(0, 0.85f);
         _nextPos = _downPos;
      }
   }
   
   private void Update()
   {
      if (_naveLanded && !isCurrent)
      {
         isCurrent = true;
         GameMode.CurrentCheckPoint = _navePos;
      }

      if (!isCurrent) return;
      if (flag) flag.position = Vector2.MoveTowards(flag.position, _nextPos, Time.deltaTime);
   }
   
   private void OnTriggerStay2D(Collider2D other)
   {
      if (other.CompareTag("nave"))
      {
         _naveLanded = GameMode.PlayerLanded;
         if (flag) _nextPos = _upPos;
         _navePos = other.transform.position;
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("nave")) _naveLanded = false;
   }

   public void TurnOff()
   {
      isCurrent = false;
      if (flag) _nextPos = _downPos;
   }
}
