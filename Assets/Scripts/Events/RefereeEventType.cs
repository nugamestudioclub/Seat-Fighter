using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RefereeEventType
{
    ShoveIdle,
    ShoveShove,
    ShoveBlock,
    ShovePush,
    ShoveDodge,

    PushIdle,
    PushBlock,
    PushShove,
    PushPush,
    PushDodge,

    BlockBlock,
    BlockDodge,
    BlockIdle,

    DodgeDodge,
    DodgeBlock,
    DodgeIdle,

    IdleIdle,

    StunStun,
    StunIdle,
    StunBlock,
    StunPush,
    StunShove,
    StunDodge,

    StaminaRefresh,
    OutOfBounds,
    Win,
}
