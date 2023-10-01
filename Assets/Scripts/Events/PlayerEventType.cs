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
    PushShove = ShovePush,
    PushPush,
    PushDodge,

    BlockBlock,
    BlockDodge,
    Block,

    DodgeDodge,
    DodgeBlock = BlockDodge,
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
