using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Input Keys Data")]
public class InputKeysData : ScriptableObject
{
    public KeyCode[] SkillKeys;
    public KeyCode AttackKey;
    public KeyCode StopKey;
    public KeyCode MoveKey;
    public KeyCode SelectKey;
    public KeyCode CameraLockKey;
    public KeyCode CenterCameraKey;

    [MyBox.Separator("Development Keys")]
    public KeyCode SpawnWaveKey;
}
