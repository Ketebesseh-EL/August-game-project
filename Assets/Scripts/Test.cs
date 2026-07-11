using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField, Min(1)] private int framerate = 60;
    void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = framerate;
    }
}
