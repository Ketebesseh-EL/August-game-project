using UnityEngine;

public class Test : MonoBehaviour
{
    void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 20;
    }
}
