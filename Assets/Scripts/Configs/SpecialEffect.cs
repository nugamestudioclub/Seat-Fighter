using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpecialEffect {
	public List<AudioClip> sfx;
	public List<BubbleConfig> vfx;
	public float vfxDuration;
}

[Serializable]
public struct ActionSpecialEffect {
	public Action action;
	public SpecialEffect specialEffect;
}

[Serializable]
public struct InteractionSpecialEffect {
	public RefereeEventType interaction;
	public SpecialEffect specialEffect;
}

[Serializable]
public struct PlayerStateSpecialEffect
{
    public PlayerState state;
    public SpecialEffect specialEffect;
}