using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    static public ScreenManager SM { get; set; }
    private void Awake()
    {
        SM = this;
    }

    public float getScreenHeight()
    {
        return Camera.main.orthographicSize * 2.0f;

    }
    public float getScreenWidth()
    {
        return Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
    }
}
