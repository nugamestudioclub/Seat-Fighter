using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayLabel;
    public string Version { get; private set; }
    public string Platform { get; private set; }

    public string Text { get; private set; }
    void Awake()
    {
        Version = Application.version;
        Platform = Enum.GetName(typeof(RuntimePlatform), Application.platform);
        Text = $"Version: {Platform}.{Version}";
        displayLabel.text = Text;
    }
}
