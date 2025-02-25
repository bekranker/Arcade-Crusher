using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] private LayerMask _groundedLayer;
    [SerializeField] private Transform _jumpPivot;
    [SerializeField] private float _rayLength;


    /// <summary>
    /// Is raycast touching the Jumable Platform Layer;
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        //mean true
        return GetGrounded() != null;
    }
    public GameObject GetGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_jumpPivot.position, Vector2.down, _rayLength, _groundedLayer);
        if (hit.collider != null)
            return hit.collider.gameObject;
        return null;
    }
}