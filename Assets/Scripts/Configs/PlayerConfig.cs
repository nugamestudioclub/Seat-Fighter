using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(PlayerConfig),
    menuName = "ScriptableObjects/" + nameof(PlayerConfig))
]
public class PlayerConfig : ScriptableObject
{
    public int pushDamage;
    public int shoveDamage;
    public int stunTime;
    public int maxStamina;
    public int health;

    public ActionConfig push;
    public ActionConfig shove;
    public ActionConfig dodge;
    public ActionConfig block;

}
