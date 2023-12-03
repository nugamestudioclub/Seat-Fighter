using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;

public class PointerButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	[SerializeField]
	private ButtonClickedEvent onPointerClick;

	[SerializeField]
	private ButtonClickedEvent onPointerDown;

	[SerializeField]
	private ButtonClickedEvent onPointerUp;

	public void OnPointerClick(PointerEventData eventData) {
		onPointerClick?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData) {
		onPointerDown?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData) {
		onPointerUp?.Invoke();
	}
}