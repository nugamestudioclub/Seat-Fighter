using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AnimationSO")]
public class AnimationSO : ScriptableObject
{
    public Sprite idle;
    public List<Sprite> dodge;
    public List<Sprite> block;
    public List<Sprite> shove;
    public List<Sprite> push;
}