using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RefereeEventType
{

    ShoveContact,
    ShoveShove,
    ShoveBlock,
    ShovePush,
    ShoveDodge,

    PushContact,
    PushBlock,
    PushShove,
    PushPush,
    PushDodge,

    BlockBlock,
    BlockDodge,
    Block,

    DodgeDodge,
    DodgeBlock,
    Dodge,

    Idle,

    Stun,
    StaminaRefresh,
    OutOfBounds,
    Win,

    // UNIMPLEMENTED
    StartShove,
    StartPush,
    StartBlock
}
