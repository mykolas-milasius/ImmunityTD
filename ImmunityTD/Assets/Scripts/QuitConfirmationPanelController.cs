using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject MasterSlider;
    public GameObject MusicSlider;

    private void OnEnable()
    {
        ShowOrHideSliders(true);
    }

    private void OnDisable()
    {
        ShowOrHideSliders(false);
    }

    private void ShowOrHideSliders(bool show)
    {
        MasterSlider.SetActive(!show);
        MusicSlider.SetActive(!show);
    }
}
