using Delegates;
using UnityEngine;

namespace Modules
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerModule : MonoBehaviour, IPlayerModule
    {
        public event PlayerDamageValue EvnPlayerDamaged;
        
        private Rigidbody _rigidbody;
        
    #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            
            switch (other.tag)
            {
                case Tags.Asteroid:
                    
                    int damage = other.GetComponent<IObstacleModule>().ObstacleDestroyed();
                    
                    EvnPlayerDamaged?.Invoke(EStatusType.PlayerDefeatedByAsteroid,damage);
                    
                    break;
            }
            
        }

    #endregion
        
    #region Interface Methods

        public void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
        }

        public void Deactivated()
        {
            gameObject.SetActive(false);
        }
        
        public void SpawnIn(Vector3 pos)
        {
            SetPosition(pos);
            gameObject.SetActive(true);
        }

    #endregion
    }
    
    public interface IPlayerModule
    {
        event PlayerDamageValue EvnPlayerDamaged;

        void Init();
        Vector3 GetPosition();
        void SetPosition(Vector3 pos);
        void SetVelocity(Vector3 velocity);
        void Deactivated();
        void SpawnIn(Vector3 pos);
    }
}
