using Modules;
using Tools;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;

        [Header("World")]
        [SerializeField] private Vector2 playerFirstPos;

        [Header("Properties")]
        [SerializeField] private float playerSpeed = 1.0f;

        [SerializeField] private float bulletSpeed = 1.0f;
        [SerializeField] private float bulletLife  = 3.0f;
        [SerializeField] private float shootDelay  = 0.1f;

        private Transform                 _player;
        private ObjectPool<IBulletModule> _bulletPool;

        private float _shootTime;

    #region Interface Methods

        public void Init()
        {
            _shootTime = 0.0f;

            _player = Instantiate(playerPrefab, playerFirstPos, Quaternion.identity).transform;

            _bulletPool = ObjectPool<IBulletModule>.InstantiatePool(bulletPrefab, 30, true, InitBullet);
        }

        public void LocomotionUpdate()
        {
            _player.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis(ButtonKeyWord.Horizontal),
                                                                     Input.GetAxis(ButtonKeyWord.Vertical),
                                                                     0.0f) * playerSpeed;

            if (Input.GetButton(ButtonKeyWord.Fire1))
            {
                _shootTime -= Time.deltaTime;

                if (_shootTime < 0)
                {
                    _bulletPool.GetObject(InitBullet).Shoot(_player.position);
                    _shootTime = shootDelay;
                }
            }
            else if (Input.GetButtonUp(ButtonKeyWord.Fire1))
            {
                _shootTime = 0;
            }
        }

    #endregion

    #region Class Methods

        private void InitBullet(GameObject bullet)
        {
            bullet.GetComponent<IBulletModule>().SetProperties(bulletSpeed, bulletLife);
        }

    #endregion
    }

    public interface IPlayerController
    {
        void Init();
        void LocomotionUpdate();
    }
}