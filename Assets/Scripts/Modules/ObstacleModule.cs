using Delegates;
using Tools;
using UnityEngine;

namespace Modules
{
    public class ObstacleModule : MonoBehaviour, IObstacleModule
    {
        public event DoubleStringValue EvnObstacleDestroy;
        
        [SerializeField] private int maxResistance = 1;

        private Rigidbody _rigidbody;
        private float     _speed;
        private int       _curResistance;

        public bool IsAvailable => !gameObject.activeSelf;

        public Transform Parent
        {
            get => transform.parent;
            set
            {
                transform.SetParent(value);
                transform.localPosition = Vector3.zero;
            }
        }

        
    #region Unity Methods

        private void Update()
        {
            _rigidbody.velocity = Vector3.down * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {

            switch (other.tag)
            {
                case Tags.NormalBullet:
                    
                    _curResistance -= other.GetComponent<IBulletModule>().DamageDone();

                    if (_curResistance < 1)
                    {
                        EvnObstacleDestroy?.Invoke(tag, Tags.NormalBullet);
                        gameObject.SetActive(false);
                    }
                    
                    break;
                
                case Tags.Respawn:
                    
                    EvnObstacleDestroy?.Invoke(tag, Tags.Respawn);
                    gameObject.SetActive(false);
                    
                    break;
            }
            
        }

    #endregion
        
    #region Interface Methods

        public void Init()
        {
            _rigidbody     = GetComponent<Rigidbody>();
            _curResistance = maxResistance;
            gameObject.SetActive(false);
        }
        
        public void Drop(Vector3 pos, float speed)
        {
            transform.position = pos;
            _speed             = speed;
            _curResistance     = maxResistance;
            gameObject.SetActive(true);
        }

    #endregion
        
        
    }
    
    public interface IObstacleModule : IModuleProperty
    {
        event DoubleStringValue EvnObstacleDestroy;
        void Drop(Vector3 pos, float speed);
    }
}


