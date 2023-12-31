using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioHandler : MonoBehaviour {
	[SerializeField]
	private AudioSource leftAudioSource;

	[SerializeField]
	private AudioSource rightAudioSource;

	[SerializeField]
	private AudioSource centerAudioSource;

	private readonly System.Random random = new();

	private Dictionary<Action, List<AudioClip>> actionSounds;

	private Dictionary<RefereeEventType, List<AudioClip>> interactionSounds;

	private Dictionary<PlayerState, List<AudioClip>> stateSounds;

	public void PlayRandomFromList(EventSource source, List<AudioClip> list)
	{
        var audioSource = source switch
        {
            EventSource.LEFT => leftAudioSource,
            EventSource.RIGHT => rightAudioSource,
            _ => centerAudioSource
		};
        audioSource.PlayOneShot(list[random.Next(list.Count)]);
    }

    public void Bind(Referee referee, Player leftPlayer, Player rightPlayer, Environment environment, SpecialEffectConfig specialEffects) {
		referee.RefereeEvent += Referee_OnInteraction;
		leftPlayer.PlayerEvent += Player_OnChange;
		leftPlayer.PlayerStateEvent += Player_OnState;

		rightPlayer.PlayerEvent += Player_OnChange;
		rightPlayer.PlayerStateEvent += Player_OnState;

		environment.EnvironmentChangeEvent += Environment_OnChange;

		actionSounds = new(specialEffects.actions.Select(x =>
			new KeyValuePair<Action, List<AudioClip>>(x.action, new(x.specialEffect.sfx))
		));
		interactionSounds = new(specialEffects.interactions.Select(x =>
			new KeyValuePair<RefereeEventType, List<AudioClip>>(x.interaction, x.specialEffect.sfx)
		));

		stateSounds = new(specialEffects.states.Select(x =>
			new KeyValuePair<PlayerState, List<AudioClip>>(x.state, x.specialEffect.sfx)
		));
	}

	private void Player_OnChange(object sender, PlayerEventArgs e) {
		var audioSource = e.sender switch
		{
			EventSource.LEFT => leftAudioSource,
			EventSource.RIGHT => rightAudioSource,
			_ => centerAudioSource
		};
		if( actionSounds.TryGetValue(e.action, out var sounds) && sounds.Count > 0 )
			audioSource.PlayOneShot(sounds[random.Next(sounds.Count)]);
	}

	private void Player_OnState(object sender, PlayerStateEventArgs e) {
		var audioSource = e.sender switch
		{
			EventSource.LEFT => leftAudioSource,
			EventSource.RIGHT => rightAudioSource,
			_ => centerAudioSource
		};
		if( stateSounds.TryGetValue(e.state, out var sounds) && sounds.Count > 0 ) {
			audioSource.PlayOneShot(sounds[random.Next(sounds.Count)]);
		}
	}

	private void Environment_OnChange(object sender, EnvironmentEventArgs e) {

	}

	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		var audioSource = e.sender switch
		{
			EventSource.LEFT => leftAudioSource,
			EventSource.RIGHT => rightAudioSource,
			_ => centerAudioSource
		};
		var audioClip = GetAudioClip(e);
		if( audioClip != null )
			audioSource.PlayOneShot(audioClip);
	}

	private AudioClip GetAudioClip(RefereeEventArgs e) {
		if( e.type == RefereeEventType.Win )
			return GetWinAudioClip(e);
		else if( interactionSounds.TryGetValue(e.type, out var sounds) && sounds.Count > 0 )
			return sounds[random.Next(sounds.Count)];
		else
			return null;
	}

	private AudioClip GetWinAudioClip(RefereeEventArgs e) {
		if( interactionSounds.TryGetValue(e.type, out var sounds) ) {
			if( sounds.Count > 2 ) {
				if( GameInProgress.Instance.PlayerCount > 1 )
					return sounds[0];
				else
					return e.sender switch {
						EventSource.LEFT => sounds[0],
						EventSource.RIGHT => sounds[1],
						_ => null
					};
			}
			else if( sounds.Count == 1 ) {
				return sounds[0];
			}
			else {
				return null;
			}
		}
		else {
			return null;
		}
	}
}