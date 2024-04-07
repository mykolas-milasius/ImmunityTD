using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreenService
{
    bool IsFullScreen { get; set; }
}

public class ScreenService : IScreenService
{
    public bool IsFullScreen
    {
        get => Screen.fullScreen;
        set => Screen.fullScreen = value;
    }
}

public class FullScreenToggle : MonoBehaviour
{
    public IScreenService ScreenService { get; set; } = new ScreenService();

    public void ToggleFullScreen()
    {
        ScreenService.IsFullScreen = !ScreenService.IsFullScreen;
    }
}