using Delegates;
using Modules;
using Tools;
using UnityEngine;
namespace Controllers
{
    public class EnemiesController : MonoBehaviour, IEnemiesController
    {
        public event ObstacleTerminatedValue EvnAsteroidTerminated;

        [Header("Prefabs")]
        [SerializeField] private GameObject obstaclePrefab;

        [Header("Properties")]
        [SerializeField] private Vector2 generateDelay;
        [SerializeField] private Vector2 obstacleSpeed;

        private ObjectPool<IObstacleModule> _obstaclePool;
        private Vector2                     _generateArea;
        private float                       _generateTimer;
        
    #region Interface Methods

        public void Init()
        {
            _obstaclePool = ObjectPool<IObstacleModule>.InstantiatePool(obstaclePrefab, 30, true, InitObstacle);

            _generateTimer = Random.Range(generateDelay.x, generateDelay.y);
        }

        public void GeneratorUpdate()
        {
            _generateTimer -= Time.deltaTime;

            if (_generateTimer < 0)
            {
                Vector3 obstaclePos = Vector3.zero;

                obstaclePos.x = Random.Range(_generateArea.x * -1, _generateArea.x);
                obstaclePos.y = _generateArea.y;

                _obstaclePool.GetObject(InitObstacle).Drop(obstaclePos, Random.Range(obstacleSpeed.x, obstacleSpeed.y));
                
                _generateTimer = Random.Range(generateDelay.x, generateDelay.y);
            }
        }

        public void SetGeneratorArea(Vector2 area)
        {
            _generateArea = area;
            _generateArea.x -= 1;
        }

    #endregion
        
    #region Class Methods

        private void InitObstacle(GameObject obstacle)
        {
            obstacle.GetComponent<IObstacleModule>().EvnObstacleDestroy += ObstacleTerminated;
        }

    #endregion

    #region Event Handler

        private void ObstacleTerminated(string obstacleType, string terminatorType)
        {
            if (obstacleType == Tags.Asteroid)
            {
                switch (terminatorType)
                {
                    case Tags.NormalBullet:
                        EvnAsteroidTerminated?.Invoke(ObstacleTerminator.NormalBullet);
                        break;
                    
                    case Tags.Respawn:
                        EvnAsteroidTerminated?.Invoke(ObstacleTerminator.Dismiss);
                        break;
                }
            }
        }

    #endregion
        
    }
    
    public interface IEnemiesController
    {
        void Init();
        void GeneratorUpdate();

        void SetGeneratorArea(Vector2 area);
    }
}
