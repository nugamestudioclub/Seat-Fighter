using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _fill;

    [SerializeField]
    private SpriteRenderer _stroke;

    [SerializeField]
    private TMPro.TMP_Text _text;

    private float remainingPlayDuration = 0;
    private bool isEnabled;

    private void Awake()
    {
        SetEnabled(false);
    }
    private void Update()
    {
        if (remainingPlayDuration > 0)
        {
            remainingPlayDuration -= Time.deltaTime;
        }
        else if (isEnabled)
        {
            SetEnabled(false);
        }
    }

    public void SetEnabled(bool value)
    {
        isEnabled = value;
        var transforms = gameObject.GetComponentsInChildren<Transform>(includeInactive:true)[1..];
        foreach(Transform t in transforms)
        {
            Debug.Log($"Setting {t.gameObject.name} to {(value ? "enabled" : "disabled")}");
            t.gameObject.SetActive(value);
        }
    }

    public void SetFillColor(Color color)
    {
        _fill.color = color;
    }

    public void SetFillSprite(Sprite sprite)
    {
        _fill.sprite = sprite;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Vector3 rotation)
    {
        transform.Rotate(rotation);
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetShapeScale(Vector3 scale)
    {
        _fill.transform.localScale = scale;
    }

    public void SetStrokeColor(Color color)
    {
        _stroke.color = color;
    }

    public void SetStrokeSprite(Sprite sprite)
    {
        _stroke.sprite = sprite;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetTextColor(Color color)
    {
        _text.color = color;
    }

    public void Set(BubbleConfig bubbleConfig)
    {
        SetFillColor(bubbleConfig.FillColor);
        SetFillSprite(bubbleConfig.FillSprite);
        SetStrokeColor(bubbleConfig.StrokeColor);
        SetStrokeSprite(bubbleConfig.StrokeSprite);
        SetScale(bubbleConfig.ShapeScale);
        SetShapeScale(bubbleConfig.ShapeScale);
        SetText(bubbleConfig.Text);
        SetTextColor(bubbleConfig.TextColor);
    }

    public void Play(float duration)
    {
        Debug.Log($"playing {name} bubble for {duration} seconds");
        remainingPlayDuration = duration;
        SetEnabled(true);
    }
}