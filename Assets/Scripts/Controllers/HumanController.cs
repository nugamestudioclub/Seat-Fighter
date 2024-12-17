using FMODUnity;

public class HumanController : IActionProvider {
	private readonly InputController _controller;

	public HumanController(InputController controller) {
		_controller = controller;
	}

	public Action GetNextAction() {
		if( IsBlocking() )
			return Action.Block;
		else if( IsDodging() )
			return Action.Dodge;
		else if( IsPushing() )
			return Action.Push;
		else if( IsShoving() )
			return Action.Shove;
		else
			return Action.None;
	}

	private bool IsBlocking() {
		var blockState = _controller.InputData.GetButtonState(Button.Block);
		var pushState = _controller.InputData.GetButtonState(Button.Push);
		return blockState.IsPressed && (!pushState.IsPressed || pushState.DownFrame >= blockState.DownFrame);

	}

	public bool IsDodging() {
		var dodge = _controller.InputData.GetButtonState(Button.Dodge);

		return _controller.InputData.GetButtonState(Button.Dodge).IsDown;
	}

	private bool IsPushing() {
		var pushState = _controller.InputData.GetButtonState(Button.Push);
		var blockState = _controller.InputData.GetButtonState(Button.Block);
		return pushState.IsPressed && (!blockState || blockState.DownFrame > pushState.DownFrame);
	}

	public bool IsShoving() {
		return _controller.InputData.GetButtonState(Button.Shove).IsDown;
	}
}