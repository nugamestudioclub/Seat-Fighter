using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(EnvironmentConfig),
    menuName = "ScriptableObjects/" + nameof(EnvironmentConfig))
]
public class EnvironmentConfig : ScriptableObject
{
    public int startingPositon;
    public int edgeDistance;
    public int armrestWidth;
}
