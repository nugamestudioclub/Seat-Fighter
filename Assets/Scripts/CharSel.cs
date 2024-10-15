using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharSel : MonoBehaviour {
	private InputController inputController_menu;
	private InputController inputController_left;
	private InputController inputController_right;

	private GameInProgress gameInProgress;

	[SerializeField]
	private AudioHandler audioHandler;

	private int leftPlayerIndex = 0;
	private int LeftPlayerIndex {
		get => leftPlayerIndex;
		set {
			leftPlayerIndex = value;
			UpdateSprites();
		}
	}
	private int rightPlayerIndex = 0;
	private int RightPlayerIndex {
		get => rightPlayerIndex;
		set {
			rightPlayerIndex = value;
			UpdateSprites();
		}
	}
	private IList<PlayerConfig> characters;

	private bool LeftPlayerMoveLeft => leftDesiredDirection.x > 0 || leftDesiredDirection.y > 0;
	private bool LeftPlayerMoveRight => leftDesiredDirection.x < 0 || leftDesiredDirection.y < 0;
	private bool RightPlayerMoveLeft => rightDesiredDirection.x > 0 || rightDesiredDirection.y > 0;
	private bool RightPlayerMoveRight => rightDesiredDirection.x < 0 || rightDesiredDirection.y < 0;

	[SerializeField] private Image leftPlayerSprite;
	[SerializeField] private Image rightPlayerSprite;

	[SerializeField] private TMP_Text leftPlayerNameLabel;
	[SerializeField] private TMP_Text rightPlayerNameLabel;

	[SerializeField] private TMP_Text leftCharacterNameLabel;
	[SerializeField] private TMP_Text rightCharacterNameLabel;

	[SerializeField] private TMP_Text leftCharacterSpecialtyLabel;
	[SerializeField] private TMP_Text rightCharacterSpecialtyLabel;

	[SerializeField] private TMP_Text pressStartLabel;

	[SerializeField] private GameObject[] rightPlayerArrows;
	[SerializeField] private GameObject[] leftPlayerArrows;

	[SerializeField] private GameObject leftCheckmark;
	[SerializeField] private GameObject rightCheckmark;

	[SerializeField]
	private Image readyButtonImage;

	[SerializeField]
	private Sprite readyButtonSprite_disabled;

	[SerializeField]
	private Sprite readyButtonSprite_enabled;

	[SerializeField]
	private int frameInterval = 5;
	private int fixedFrameCount;
	[SerializeField]
	private float timeTillStartIsEnabled = 1.0f;

	private bool isLeftPlayerReady;
	private bool IsLeftPlayerReady {
		get => isLeftPlayerReady;
		set {
			if( value ) {
				audioHandler.PlayRandomFromList(EventSource.LEFT, gameInProgress.LeftPlayer.greetings);
			}
			isLeftPlayerReady = value;
		}
	}

	private bool isRightPlayerReady;
	private bool IsRightPlayerReady {
		get => isRightPlayerReady;
		set {
			if( value ) {
				audioHandler.PlayRandomFromList(EventSource.RIGHT, gameInProgress.RightPlayer.greetings);
			}
			isRightPlayerReady = value;
		}
	}

	private void SetPlayerReadyUIActive(int player, bool active) {
		foreach( GameObject arrow in player == 0 ? leftPlayerArrows : rightPlayerArrows ) {
			arrow.SetActive(active);
		}
	}

	private float lastLeftStart;
	private float lastRightStart;


	private Vector2 leftDesiredDirection;
	private Vector2 rightDesiredDirection;
	private bool isAI;

	private void Awake() {
		fixedFrameCount = 0;
		inputController_menu = new InputController(-1);
		inputController_left = new InputController(0);
		inputController_right = new InputController(1);
		IsLeftPlayerReady = false;
		IsRightPlayerReady = false;
		isAI = false;
		lastLeftStart = 0;
		lastRightStart = 0;
	}

	private void Start() {
		gameInProgress = GameInProgress.Instance;
		characters = gameInProgress.AllPlayers.Options;
		RightPlayerIndex = GetOtherPlayerIndex(0, characters.Count);
		if( gameInProgress.PlayerCount == 1 ) {
			leftPlayerNameLabel.text = "Player";
			rightPlayerNameLabel.text = "Computer";
			isAI = true;
		}
		else {
			leftPlayerNameLabel.text = "Player 1";
			rightPlayerNameLabel.text = "Player 2";
		}
		ShowInstructions(false);
		UpdateSprites();
	}

	void Update() {
		leftDesiredDirection = GetDesiredDirection(inputController_left);
		rightDesiredDirection = GetDesiredDirection(inputController_right);

		if( Time.timeSinceLevelLoad < timeTillStartIsEnabled ) {
			return;
		}

		if( IsLeftPlayerReady && IsRightPlayerReady ) {
			if( GetRightCancel() ) {
				IsRightPlayerReady = false;
			}
			else if( GetLeftCancel() ) {
				IsLeftPlayerReady = false;
			}
			else if( (GetLeftStart() || GetRightStart()) ) {
				GameInProgress.Instance.LoadScene("MainScene");
			}
		}
		else if( IsRightPlayerReady ) {
			if( GetRightCancel() ) {
				IsRightPlayerReady = false;
			}
			else if( GetLeftStart() ) {
				IsLeftPlayerReady = true;
			}
		}
		else if( IsLeftPlayerReady ) {
			if( GetLeftCancel() ) {
				IsLeftPlayerReady = false;
			}
			else if( GetRightStart() ) {
				IsRightPlayerReady = true;
			}
		}
		else {
			if( GetLeftCancel() || GetRightCancel() ) {
				HandleBack();
			}
			else if( GetLeftStart() ) {
				IsLeftPlayerReady = true;
			}
			else if( GetRightStart() ) {
				IsRightPlayerReady= true;
			}
		}
	}

	void FixedUpdate() {
		fixedFrameCount++;
		if( fixedFrameCount % frameInterval == 0 ) {
			if( !IsLeftPlayerReady ) {
				if( LeftPlayerMoveLeft ) {
					LeftPlayerIndex = (LeftPlayerIndex - 1 + characters.Count) % characters.Count;
				}
				else if( LeftPlayerMoveRight ) {
					LeftPlayerIndex = (LeftPlayerIndex + 1) % characters.Count;
				}
			}
			else if( isAI && !IsRightPlayerReady ) {
				if( LeftPlayerMoveLeft ) {
					RightPlayerIndex = (RightPlayerIndex + characters.Count - 1) % characters.Count;
				}
				else if( LeftPlayerMoveRight ) {
					RightPlayerIndex = (RightPlayerIndex + 1) % characters.Count;
				}
			}


			if( !isAI && !IsRightPlayerReady ) {
				if( RightPlayerMoveLeft ) {
					RightPlayerIndex = (RightPlayerIndex + characters.Count - 1) % characters.Count;
				}
				else if( RightPlayerMoveRight ) {
					RightPlayerIndex = (RightPlayerIndex + 1) % characters.Count;
				}
			}
		}
		ShowArrows(IsLeftPlayerReady, IsRightPlayerReady, isAI);
		ShowCheckmarks(IsLeftPlayerReady, IsRightPlayerReady);
		ShowInstructions(IsLeftPlayerReady && IsRightPlayerReady);
	}

	public void HandleBack() {
		GameInProgress.Instance.LoadScene("TitleScreen");
	}

	public void HandleLeftPlayerNext() {
		if( !IsLeftPlayerReady )
			LeftPlayerIndex = (LeftPlayerIndex - 1 + characters.Count) % characters.Count;
	}

	public void HandleLeftPlayerPrevious() {
		if( !IsLeftPlayerReady )
			LeftPlayerIndex = (LeftPlayerIndex + 1) % characters.Count;
	}

	public void HandleLeftPlayerSelect() {
		inputController_left.InputData.SetButtonState(IsLeftPlayerReady ? Button.Cancel : Button.Start, true);
	}

	public void HandleRightPlayerNext() {
		if( !IsRightPlayerReady )
			RightPlayerIndex = (RightPlayerIndex + characters.Count - 1) % characters.Count;
	}

	public void HandleRightPlayerPrevious() {
		if( !IsRightPlayerReady )
			RightPlayerIndex = (RightPlayerIndex + 1) % characters.Count;
	}

	public void HandleRightPlayerSelect() {
		if( isAI ) {
			if( !IsLeftPlayerReady )
				return;
			inputController_right.InputData.SetButtonState(IsRightPlayerReady ? Button.Cancel : Button.Start, true);
		}
		else {
			inputController_right.InputData.SetButtonState(IsRightPlayerReady ? Button.Cancel : Button.Start, true);
		}
	}

	public void HandleReady() {
		if( IsLeftPlayerReady && IsRightPlayerReady )
			inputController_left.InputData.SetButtonState(Button.Start, true);
	}

	private Vector2 GetDesiredDirection(InputController controller) {
		var direction = controller.InputData.Direction;
		return isAI && Mathf.Approximately(direction.magnitude, 0)
			? inputController_menu.InputData.Direction
			: direction;
	}

	private int GetOtherPlayerIndex(int playerIndex, int numPlayers) {
		System.Random random = new();
		int candidate = playerIndex;
		while( candidate == playerIndex ) {
			candidate = random.Next(0, numPlayers);
		}
		return candidate;
	}

	private bool GetLeftCancel() {
		return inputController_left.InputData.GetButtonState(Button.Cancel).IsDown;
	}

	private bool GetLeftStart() {
		bool ableTomove = lastLeftStart + timeTillStartIsEnabled < Time.timeSinceLevelLoad;
		bool willMove = inputController_left.InputData.GetButtonState(Button.Start).IsDown && ableTomove;
		if( willMove ) {
			lastLeftStart = Time.timeSinceLevelLoad;
		}
		return willMove;
	}

	private bool GetRightCancel() {
		return inputController_right.InputData.GetButtonState(Button.Cancel).IsDown;
	}

	private bool GetRightStart() {
		if( isAI && !IsLeftPlayerReady ) {
			return false;
		}
		bool ableTomove = lastRightStart + timeTillStartIsEnabled < Time.timeSinceLevelLoad;
		bool willMove = inputController_right.InputData.GetButtonState(Button.Start).IsDown && ableTomove;
		if( willMove ) {
			lastRightStart = Time.timeSinceLevelLoad;
		}
		return willMove;
	}

	private void ShowArrows(bool isLeftPlayerReady, bool isRightPlayerReady, bool isAI) {
		SetPlayerReadyUIActive(0, !isLeftPlayerReady);
		SetPlayerReadyUIActive(1, !isRightPlayerReady && (!isAI || isLeftPlayerReady));
	}

	private void ShowCheckmarks(bool isLeftPlayerChecked, bool isRightPlayerChecked) {
		leftCheckmark.SetActive(isLeftPlayerChecked);
		rightCheckmark.SetActive(isRightPlayerChecked);
	}

	private void ShowInstructions(bool value) {
		if( value ) {
			readyButtonImage.sprite =readyButtonSprite_enabled;
			pressStartLabel.text =
#if UNITY_ANDROID || UNITY_IOS || DEBUG_MOBILE
				"Tap READY to begin!";
#else
				"Click READY to begin!";
#endif
		}
		else {
			readyButtonImage.sprite =readyButtonSprite_disabled;
			pressStartLabel.text =
#if UNITY_ANDROID || UNITY_IOS || DEBUG_MOBILE
				"Tap a character to select!";
#else
				"Click READY to begin!";
#endif
		}
	}

	private void UpdateSprites() {
		leftPlayerSprite.sprite = characters[LeftPlayerIndex].portrait;
		rightPlayerSprite.sprite = characters[RightPlayerIndex].portrait;
		leftCharacterNameLabel.text = characters[LeftPlayerIndex].characterName;
		rightCharacterNameLabel.text = characters[RightPlayerIndex].characterName;
		leftCharacterSpecialtyLabel.text = characters[LeftPlayerIndex].characterSpecialty;
		rightCharacterSpecialtyLabel.text = characters[RightPlayerIndex].characterSpecialty;

		gameInProgress.LeftPlayer = characters[LeftPlayerIndex];
		gameInProgress.RightPlayer = characters[RightPlayerIndex];
	}
}