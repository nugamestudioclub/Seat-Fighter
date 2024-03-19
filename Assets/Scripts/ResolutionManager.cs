using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    [field: SerializeField]
    public int StartingWidth { get; private set; } = 1920;
    [field: SerializeField]
    public int StartingHeight { get; private set; } = 1080;

    [field: SerializeField]
    public bool StartingFullscreen { get; private set; } = false;

    void Awake()
    {
        Screen.SetResolution(StartingWidth, StartingHeight, StartingFullscreen);
    }
}
