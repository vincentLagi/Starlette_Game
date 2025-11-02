using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    public Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        // SetActive Timeline Or Chat
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}
