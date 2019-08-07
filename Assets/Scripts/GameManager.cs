using Controllers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject wallPrefab;

    [Header("Components")]
    [SerializeField] private Camera cameraController;

    private IPlayerController _playerController;
    private IUiController _uiController;
    private IEnemiesController _enemiesController;
    
#region Unity Methods

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _playerController  = FindObjectOfType<PlayerController>();
        _uiController      = FindObjectOfType<UiController>();
        _enemiesController = FindObjectOfType<EnemiesController>();

        _enemiesController.Init();
        
        MakeBlockOnBorders();

        _playerController.Init();
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

#endregion
    
#region Event Handler
    
    
    
#endregion
}