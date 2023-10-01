using System.Collections.Generic;
using System.Linq;
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

	public void Bind(Referee referee, Player leftPlayer, Player rightPlayer, Environment environment, SpecialEffectConfig specialEffects) {
		referee.RefereeEvent += Referee_OnInteraction;
		leftPlayer.PlayerEvent += Player_OnChange;
		rightPlayer.PlayerEvent += Player_OnChange;
		environment.EnvironmentChangeEvent += Environment_OnChange;

		actionSounds = new(specialEffects.Actions.Select(x =>
			new KeyValuePair<Action, List<AudioClip>>(x.action, new(x.specialEffect.sfx))
		));
		interactionSounds = new(specialEffects.Interactions.Select(x =>
			new KeyValuePair<RefereeEventType, List<AudioClip>>(x.interaction, x.specialEffect.sfx)
		)); ;
	}

	private void Player_OnChange(object sender, PlayerEventArgs e) {
		var audioSource = e.sender switch {
			EventSource.LEFT => leftAudioSource,
			EventSource.RIGHT => rightAudioSource,
			_ => centerAudioSource
		};
		if( actionSounds.TryGetValue(e.action, out var sounds) && sounds.Count > 0 )
			audioSource.PlayOneShot(sounds[random.Next(sounds.Count)]);
	}

	private void Environment_OnChange(object sender, EnvironmentEventArgs e) {

	}
	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		switch( e.type ) {
		case RefereeEventType.Dodge:

			break;
		case RefereeEventType.ShovePush:
		case RefereeEventType.ShoveContact:

			break;
		case RefereeEventType.StartBlock:

			break;
		case RefereeEventType.StartShove:

			break;
		case RefereeEventType.StartPush:

			break;
		case RefereeEventType.ShoveBlock:

			break;
		}

		var audioSource = e.sender switch {
			EventSource.LEFT => leftAudioSource,
			EventSource.RIGHT => rightAudioSource,
			_ => centerAudioSource
		};
		if( interactionSounds.TryGetValue(e.type, out var sounds) && sounds.Count > 0 )
			audioSource.PlayOneShot(sounds[random.Next(sounds.Count)]);
	}
}
