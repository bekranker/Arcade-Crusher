using System;
using UnityEngine;

public class BlackHoles : SpaceFightEnvironment
{
    [SerializeField] private float _gravitationalForce = 10f;
    [SerializeField] private float _centerDistance;
    [SerializeField] private float _gravitationalForceArea;
    [SerializeField] private LayerMask _targetLayer;
    private Player _player;

    public override event Action OnCollect;
    public override event Action OnReturnAction;
    public override event Action OnGetAction;

    public override string PoolKey { get => "BlackHole"; set => throw new NotImplementedException(); }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _gravitationalForceArea, _targetLayer);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.attachedRigidbody;
            if (rb != null)
            {
                if (Vector2.Distance(transform.position, collider.transform.position) > _centerDistance)
                {
                    Vector2 direction = (transform.position - rb.transform.position).normalized;
                    rb.AddForce(direction * _gravitationalForce, ForceMode2D.Force);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _gravitationalForceArea);
    }

    public override void CollectMe(MonoBehaviour collectable)
    {

    }

    public override void OnInit(PoolManager poolManager)
    {
    }

    public override void OnReturn()
    {
    }

    public override void OnGet()
    {
    }

    public override void InitSpaceFightEnvironment(PoolManager poolManager, Transform parent, Player player = null)
    {
        transform.localScale = Vector3.one;
        Vector3 velocityDirectionOfPlayer = player.GetComponent<Rigidbody2D>().linearVelocity;
        transform.position = player.transform.position + velocityDirectionOfPlayer.normalized * 10f;
        _poolManager = poolManager;
        _player = player;
    }
}