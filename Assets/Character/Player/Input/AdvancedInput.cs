using UnityEngine;
using Fusion;

public struct AdvancedInput : INetworkInput
{
    public Vector2        MoveDirection;
    public Vector2        LookRotationDelta;
    public NetworkButtons Actions;

    public const int MOUSEBUTTON01   = 0;
    public const int MOUSEBUTTON02   = 1;
}