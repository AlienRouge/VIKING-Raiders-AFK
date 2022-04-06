using UnityEngine;

public class ModalMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _quitConfirmationUI;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private ModalMenuButton _modalMenuButton;

    public void ShowMenu()
    {
        _modalMenuButton.SetPressedSprite();
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        _modalMenuButton.SetDefaultSprite();
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
