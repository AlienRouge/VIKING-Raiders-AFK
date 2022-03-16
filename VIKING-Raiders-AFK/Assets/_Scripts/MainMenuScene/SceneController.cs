using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private User _userData;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _userlvl;
    
    private void Start()
    {
        _userName.text = _userData._userName;
        _userlvl.text = $"{_userData._accountLevel} lvl";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
