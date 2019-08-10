using Controllers;
using Scriptable;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject wallPrefab;

    [Header("Components")]
    [SerializeField] private Camera cameraController;

    private IPlayerController  _playerController;
    private IUiController      _uiController;
    private IEnemiesController _enemiesController;

    private Configurable _config;
    
#region Unity Methods

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _config = Configurable.Instance;
        
        _playerController  = FindObjectOfType<PlayerController>();
        _uiController      = FindObjectOfType<UiController>();
        _enemiesController = FindObjectOfType<EnemiesController>();

        _enemiesController.Init();
        
        MakeBlockOnBorders();

        _playerController.Init();
        
        _uiController.Init(_playerController.GetHp());

        for (int i = 0; i < 3; i++)
        {
            _uiController.SetCurrency((ECurrencyType)i, _config.GetCurrency((ECurrencyType)i));
        }

        SetupAchievementItems();

        _playerController.EvnPlayerHpChange      += OnPlayerHpChange;
        _playerController.EvnPlayerDefeated      += OnPlayerDefeated;
        _enemiesController.EvnAsteroidTerminated += OnAsteroidTerminated;
        _uiController.EvnAchievementDone         += OnAchievementDone;
    }


    private void Update()
    {
        _playerController.LocomotionUpdate();
        _enemiesController.GeneratorUpdate();
    }

#endregion

#region Class Methods

    private void MakeBlockOnBorders()
    {
        Transform[] walls = new Transform[4];

        Vector2 screenSize = Vector3.zero;
        screenSize.y = cameraController.orthographicSize;
        screenSize.x = (screenSize.y * Screen.width) / Screen.height;

        for (int i = 0; i < walls.Length; i++)
        {
            walls[i]            = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity).transform;
            walls[i].localScale = Vector3.one / 10.0f;
        }

        walls[0].position = Vector3.up   * screenSize.y;
        walls[1].position = Vector3.down * screenSize.y;

        walls[0].localScale += screenSize.x * 2 * Vector3.right;
        walls[1].localScale =  walls[0].localScale;

        walls[2].position = Vector3.right * screenSize.x;
        walls[3].position = Vector3.left  * screenSize.x;

        walls[2].position -= screenSize.x * 2 * _uiController.GetHudHorizontalOffset() * Vector3.right;
        walls[3].position -= screenSize.x * 2 * _uiController.GetHudHorizontalOffset() * Vector3.left;
        
        walls[2].localScale += screenSize.y * 2 * Vector3.up;
        walls[3].localScale =  walls[2].localScale;

        Vector2 upSideArea = Vector2.zero;

        upSideArea.x = screenSize.x * 2 * _uiController.GetHudHorizontalOffset();
        upSideArea.y = screenSize.y;
        
        _enemiesController.SetGeneratorArea(upSideArea);
    }

    private void SetupAchievementItems()
    {
        AchievementProperty[] achievementProperties =
            Resources.Load<AchievementData>(AchievementData.ResourcePath).achievementProperties;

        for (int index = 0; index < achievementProperties.Length; index++)
        {
            AchievementProperty property = achievementProperties[index];
            
            int achieveLevel = _config.GetAchievementLevel(property.achieveName);
            
            _uiController.SetAchievementItem(index, property.achieveName, property.description, property.statusType,
                                             property.points[achieveLevel].rewardType.ToString(),
                                             property.points[achieveLevel].reward, achieveLevel,
                                             property.points[achieveLevel].point,
                                             _config.GetStatusPoint(property.statusType.ToString()));
        }
    }
#endregion
    
#region Event Handler

    private void OnAsteroidTerminated(EStatusType state)
    {
        _uiController.SetAchievementPoint(state, _config.AddStatusCount(state.ToString(), 1));
    }

    private void OnPlayerDefeated(EStatusType state)
    {
        _uiController.SetAchievementPoint(state,_config.AddStatusCount(state.ToString(), 1));
        _enemiesController.ClearArea();
        _playerController.Respawn();
    }
    
    private void OnPlayerHpChange(float amount)
    {
        _uiController.SetPlayerHealth(amount);
    }
    
    private void OnAchievementDone(int index)
    {
        AchievementProperty achievementProperty =
            Resources.Load<AchievementData>(AchievementData.ResourcePath).achievementProperties[index];

        int achieveLevel = _config.GetAchievementLevel(achievementProperty.achieveName);

        _config.TakeStatusPoints(achievementProperty.statusType.ToString(),
                                 achievementProperty.points[achieveLevel].point);
        
        _config.AddAchievementLevel(achievementProperty.achieveName);

        _uiController.SetCurrency(achievementProperty.points[achieveLevel].rewardType,
                                  _config.GiveCurrency(achievementProperty.points[achieveLevel].rewardType,
                                                              achievementProperty.points[achieveLevel].reward));
        
        achieveLevel = _config.GetAchievementLevel(achievementProperty.achieveName);
        
        _uiController.SetAchievementItem(index, achievementProperty.achieveName, achievementProperty.description,
                                         achievementProperty.statusType,
                                         achievementProperty.points[achieveLevel].rewardType.ToString(),
                                         achievementProperty.points[achieveLevel].reward,
                                         achieveLevel,
                                         achievementProperty.points[achieveLevel].point,
                                         _config.GetStatusPoint(achievementProperty.statusType.ToString()));
        
        _uiController.SetAchievementPoint(achievementProperty.statusType,
                                          _config.GetStatusPoint(achievementProperty.statusType.ToString()));
    }

#endregion
}