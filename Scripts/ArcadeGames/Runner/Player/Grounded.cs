using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] private LayerMask _groundedLayer;
    [SerializeField] private Transform _jumpPivot;
    [SerializeField] private float _rayLength;
    [SerializeField] private float _coyoteTime;

    private float _coyoteCounter;

    void Start()
    {
        _coyoteCounter = _coyoteTime;
    }
    void Update()
    {
        if (GetGrounded() != null)
        {
            _coyoteCounter = _coyoteTime;
        }
        else
            _coyoteCounter -= Time.deltaTime;
    }
    /// <summary>
    /// Is raycast touching the Jumable Platform Layer;
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        //mean true
        return _coyoteCounter > 0;
    }
    public GameObject GetGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_jumpPivot.position, Vector2.down, _rayLength, _groundedLayer);
        if (hit.collider != null)
            return hit.collider.gameObject;
        return null;
    }
}