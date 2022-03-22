using System;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private bool _isWorking = true;
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

    public async void StartAsync()
    {
        var a= Async1();
        await Async2();
        Debug.Log("Done!");
    }
}
