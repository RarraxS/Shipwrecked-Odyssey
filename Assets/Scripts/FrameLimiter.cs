using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    [SerializeField] private int maxFrames;

    void Awake()
    {
        Application.targetFrameRate = maxFrames;
    }
}
