using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(GameConfig),
    menuName = "ScriptableObjects/" + nameof(GameConfig))
]
public class GameConfig : ScriptableObject
{
    
    public PlayerConfig leftPlayerConfig;
    public PlayerConfig rightPlayerConfig;

    [field:SerializeField]
    public List<ActionConfig> DefaultActionConfigs { get; private set; }

    public EnvironmentConfig environmentConfig;
    public SpecialEffectConfig specialEffectConfig;    
}
