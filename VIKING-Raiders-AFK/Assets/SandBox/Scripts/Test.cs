using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private bool _isWorking = true;
    private IEnumerator firstCorutine;
    private async Task Async1()
    {
        int counter = 0;
        while (_isWorking)
        {
            counter += 1;
            Debug.Log(counter);
            await Task.Delay(100);
        }
    }
    private async Task Async2()
    {
        await Task.Delay(2000);
        Debug.Log("Second done");
    }

    private IEnumerator Corutine()
    {
        int counter = 0;
        while (_isWorking)
        {
            counter += 1;
            Debug.Log(counter);
            yield return new WaitForSeconds(1);
        }
    }

    public async void StartAsync()
    {
        firstCorutine =  Corutine();
        StartCoroutine(firstCorutine);
        Debug.Log("Done!");
    }
    
    public async void StopAsync()
    {
        StopCoroutine(firstCorutine);
        Debug.Log("Done!");
    }
}
