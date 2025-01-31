using UnityEngine;

[CreateAssetMenu(fileName = "Customer Type", menuName = "Scriptable Objects/Work/CustomerType")]
public class CustomerType : ScriptableObject
{
    public Animation IdleClip;
    public Animation MoveClip;
    public Sprite PlaceHolder;
    public string CustomerName;

}