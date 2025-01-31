using UnityEngine;

[CreateAssetMenu(fileName = "Customer Type", menuName = "Scriptable Objects/Work/CustomerType")]
public class CustomerType : ScriptableObject
{
    public AnimationClip IdleClip;
    public AnimationClip MoveClip;
    public AnimationClip EatingClip;
    public Sprite PlaceHolder;
    public string CustomerName;

}