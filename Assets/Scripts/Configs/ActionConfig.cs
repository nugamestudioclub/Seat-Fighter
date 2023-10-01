using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(
    fileName = nameof(ActionConfig),
    menuName = "ScriptableObjects/" + nameof(ActionConfig))
]
public class ActionConfig : ScriptableObject
{
    [field: SerializeField]
    public List<ActionDuration> ActionDurations { get; private set; }

    [field: SerializeField]
    public List<SpriteDuration> SpriteDurations { get; private set; }

    [field: SerializeField]
    public int StaminaModifier { get; private set; }

    public List<ActionFrameData> GetFrameData()
    {
        List<ActionFrameData> allFrameData = new();

        List<ActionState> allActionFrames = new();
        ActionDurations.ForEach(actionDuration =>
        {
            for (int i = 0; i < actionDuration.duration; i++)
            {
                allActionFrames.Add(actionDuration.state);
            }

        });
        List<Sprite> allSpriteFrames = new();
        SpriteDurations.ForEach(spriteDuration =>
        {
            for (int i = 0; i < spriteDuration.duration; i++)
            {
                allSpriteFrames.Add(spriteDuration.sprite);
            }

        });

        int totalActionFrames = allActionFrames.Count;
        int totalSpriteFrames = allSpriteFrames.Count;

        if (totalActionFrames != allActionFrames.Count)
        {
            Debug.Log($"Total action frames {totalActionFrames} does not equal total sprite frames {totalSpriteFrames}");
        }
        int totalFrames = Math.Min(totalActionFrames, totalSpriteFrames);
        for (int i =0; i< totalFrames;i++)
        {
            allFrameData.Add(new ActionFrameData(allActionFrames[i], allSpriteFrames[i]));
        }
        return allFrameData;
    }

    public ActionConfig(List<ActionDuration> states, int staminaCost)
    {
        ActionDurations = states;
        StaminaModifier = staminaCost;
    }
}
