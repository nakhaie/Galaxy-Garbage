using Delegates;
using Modules;
using Tools;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event PlayerDefeatValue EvnPlayerDefeated;
        public event PlayerHpValue EvnPlayerHpChange;

        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;

        [Header("World")]
        [SerializeField] private Vector2 playerFirstPos;

        [Header("Properties")]
        [SerializeField] private int   playerHp    = 1;
        [SerializeField] private float playerSpeed = 1.0f;
        [SerializeField] private float bulletSpeed = 1.0f;
        [SerializeField] private float bulletLife  = 3.0f;
        [SerializeField] private float shootDelay  = 0.1f;

        private IPlayerModule             _player;
        private ObjectPool<IBulletModule> _bulletPool;

        private int CurHp
        {
            get => _curHp;
            set
            {
                _curHp = Mathf.Max(0 , value);
                EvnPlayerHpChange?.Invoke(GetHp());
            }
        }
        
        private int   _curHp;
        private float _shootTime;
        
        
    #region Interface Methods

        public void Init()
        {
            _shootTime = 0.0f;
            CurHp = playerHp;

            _player = Instantiate(playerPrefab, playerFirstPos, Quaternion.identity).GetComponent<IPlayerModule>();

            _bulletPool = ObjectPool<IBulletModule>.InstantiatePool(bulletPrefab, 30, true, InitBullet);
            
            _player.Init();
            
            _player.EvnPlayerDamaged += OnPlayerDamaged;
        }

        public void LocomotionUpdate()
        {
            _player.SetVelocity(new Vector3(Input.GetAxis(ButtonKeyWord.Horizontal),
                                            Input.GetAxis(ButtonKeyWord.Vertical),
                                            0.0f) * playerSpeed);
            
            if (Input.GetButton(ButtonKeyWord.Fire1))
            {
                _shootTime -= Time.deltaTime;

                if (_shootTime < 0)
                {
                    _bulletPool.GetObject(InitBullet).Shoot(_player.GetPosition());
                    _shootTime = shootDelay;
                }
            }
            else if (Input.GetButtonUp(ButtonKeyWord.Fire1))
            {
                _shootTime = 0;
            }
        }

        public void Respawn()
        {
            _shootTime = 0.0f;
            CurHp      = playerHp;
            _player.SpawnIn(playerFirstPos);
        }

        public float GetHp()
        {
            return CurHp / (float)playerHp;
        }
        
    #endregion

    #region Class Methods

        private void InitBullet(GameObject bullet)
        {
            bullet.GetComponent<IBulletModule>().SetProperties(bulletSpeed, bulletLife);
        }

    #endregion

    #region Event Handler

        private void OnPlayerDamaged(EPlayerDefeat enemyType, int amount)
        {
            CurHp -= amount;

            if (CurHp < 1)
            {
                EvnPlayerDefeated?.Invoke(enemyType);
            }
        }

    #endregion
    }

    public interface IPlayerController
    {
        event PlayerDefeatValue EvnPlayerDefeated;
        event PlayerHpValue EvnPlayerHpChange;
        void Init();
        void LocomotionUpdate();
        void Respawn();
        float GetHp();

    }
}