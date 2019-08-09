using Tools;
using UnityEngine;

namespace Modules
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletModule : MonoBehaviour, IBulletModule
    {
        [SerializeField] private int damage = 1;
        
        private Rigidbody _rigidbody;
        private float     _speed;
        private float     _lifeTime;

        private float _lifeTimer;
        public  bool  IsAvailable => !gameObject.activeSelf;

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
            _rigidbody.velocity = Vector3.up * _speed;

            _lifeTimer -= Time.deltaTime;

            if (_lifeTimer < 0)
            {
                gameObject.SetActive(false);
            }
        }

    #endregion

    #region Interface Methods

        public void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        public void SetProperties(float speed, float lifeTime)
        {
            _speed    = speed;
            _lifeTime = lifeTime;
        }

        public void Shoot(Vector3 pos)
        {
            _lifeTimer         = _lifeTime;
            transform.position = pos;
            gameObject.SetActive(true);
        }

        public void Shoot(Vector3 pos, float speed, float lifeTime)
        {
            transform.position = pos;
            _speed             = speed;
            _lifeTime          = lifeTime;
            _lifeTimer         = _lifeTime;
            gameObject.SetActive(true);
        }

        public int DamageDone()
        {
            gameObject.SetActive(false);
            return damage;
        }

    #endregion
    }

    public interface IBulletModule : IModuleProperty
    {
        void SetProperties(float speed, float lifeTime);
        void Shoot(Vector3 pos);
        void Shoot(Vector3 pos, float speed, float lifeTime);
        int DamageDone();
    }
}