using UnityEngine;

[CreateAssetMenu(fileName = "Customer Type", menuName = "Scriptable Objects/Work/CustomerType")]
public class CustomerType : ScriptableObject
{
    public Sprite Sprite;
    public Animation IdleClip;
    public Animation MoveClip;

}