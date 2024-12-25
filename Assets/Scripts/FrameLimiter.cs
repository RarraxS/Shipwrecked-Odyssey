using UnityEngine;

public class LimitadorFrames : MonoBehaviour
{
    [SerializeField] private int maxFps;
    private void Awake()
    {
        Application.targetFrameRate = maxFps;
    }
}
