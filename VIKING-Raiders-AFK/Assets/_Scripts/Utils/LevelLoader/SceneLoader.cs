using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _transitionTime;
    private static readonly int StartTransition = Animator.StringToHash("StartTrans");


    public void Load(int ind)
    {
        StartCoroutine(LoadScene(ind));
    }
    private IEnumerator LoadScene(int sceneIndex)
    {
        _animator.SetTrigger(StartTransition);
        
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
