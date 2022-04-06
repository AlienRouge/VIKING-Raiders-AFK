using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _quitConfirmationUI;
    [SerializeField] private SceneLoader _sceneLoader;
    
    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        _sceneLoader.Load("MainMenuScene");
    }

    public void Restart()
    {
        _sceneLoader.ReloadScene();
    }

    public void ShowQuitConfirmation()
    {
        _quitConfirmationUI.SetActive(true);
    }
    public void HideQuitConfirmation()
    {
        _quitConfirmationUI.SetActive(false);
    }
}
