using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(PlayerConfig),
    menuName = "ScriptableObjects/" + nameof(PlayerConfig))
]
public class PlayerConfig : ScriptableObject
{
    public string characterName;
    public string characterSpecialty;
    public int stunTime;
    public int maxStamina;
    public int idleStaminaRegen; //maybe idle should be its own action
    public int health;

    [field: SerializeField]
    public AIConfig AI { get; private set; }

    public ActionConfig push;
    public ActionConfig shove;
    public ActionConfig dodge;
    public ActionConfig block;
    public ActionConfig stunned;
    public ActionConfig idle;

    public Sprite idleSprite;
    public Sprite portrait;

    public List<AudioClip> greetings;
    public List<AudioClip> taunts;

}
