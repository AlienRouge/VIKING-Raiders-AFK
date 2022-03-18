using UnityEngine;

public class InventorySceneController : MonoBehaviour
{
    [SerializeField] private InventoryPanelController _panelController;

    [SerializeField] private User _userData;
    private void Start()
    {
        _userData = Resources.Load<User>("Player");
        
        FillInventoryPanel();
    }

    private void FillInventoryPanel()
    {
        _panelController.Init(_userData._heroList);
    }

    private void Update()
    {
        
    }
}
