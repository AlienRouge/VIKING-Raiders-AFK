using UnityEngine;

public class InventorySceneController : MonoBehaviour
{
    private User _userData;
    
    [SerializeField] private InventoryPanelController _panelController;
    private SceneLoader _sceneLoader;
    private void Start()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        _userData = Resources.Load<User>("Player");
        
        FillInventoryPanel();
    }

    public void BackToMainMenuScene()
    {
        _sceneLoader.Load("MainMenuScene");
    }

    private void FillInventoryPanel()
    {
        _panelController.Init(_userData._heroList);
    }
}
