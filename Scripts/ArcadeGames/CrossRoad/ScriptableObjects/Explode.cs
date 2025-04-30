using UnityEngine;

[CreateAssetMenu(fileName = "Explode Action", menuName = "ScriptableObjects/CrossRoad/Explode Action", order = 2)]
public class Explode : EnemyEventTypeSO
{
    [SerializeField] private GameObject _explodeObject;
    public override void ExecuteAction(GameObject @object)
    {
        // Define the size and position of the box area
        Vector2 boxSize = new Vector2(10f, 7f); // Adjust size as needed
        Vector2 boxCenter = @object.transform.position;
        Debug.Log("sa");
        Debug.Log("Position: " + boxCenter);
        // Perform an overlap box check to find the player
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, LayerMask.NameToLayer("Player"));
        Instantiate(_explodeObject, boxCenter, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Player player))
            {
                Debug.Log("I got the player");
                Debug.Log("Explodeee");
                player.TakeDamage(999);
            }
        }
    }
}
