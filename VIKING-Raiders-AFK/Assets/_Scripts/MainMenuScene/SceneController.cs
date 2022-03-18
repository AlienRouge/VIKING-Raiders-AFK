using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private User _userData;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _userlvl;
    [SerializeField] private SceneLoader _sceneLoader;
    
    private void Start()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        _userName.text = _userData._userName;
        _userlvl.text = $"{_userData._accountLevel} lvl";
    }

    public void PlayGame()
    {
        _sceneLoader.Load(1);
    }
    public void GoInventory()
    {
        _sceneLoader.Load(5);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
