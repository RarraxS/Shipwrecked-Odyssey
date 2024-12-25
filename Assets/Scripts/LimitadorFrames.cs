using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    [SerializeField] private int maxFps;
    private void Awake()
    {
        Application.targetFrameRate = maxFps;
    }
}
