using UnityEngine;

public class StartManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LinkButton (string link)
    {
        Application.OpenURL(link);
    }
}
