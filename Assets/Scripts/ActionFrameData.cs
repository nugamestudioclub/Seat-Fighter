using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActionFrameData
{
    public ActionState state;
    public Sprite sprite;

    public ActionFrameData(ActionState state, Sprite sprite)
    {
        this.state = state;
        this.sprite = sprite;
    }
}
