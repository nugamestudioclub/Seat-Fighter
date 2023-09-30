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

    public EnvironmentConfig environmentConfig;

}
