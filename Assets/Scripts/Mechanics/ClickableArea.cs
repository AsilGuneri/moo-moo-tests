using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClickableArea : MonoBehaviour
{
    // This will be the collider that is clicked by the user.
    public Collider ClickableCollider { get; private set; }

    private void Awake()
    {
        // Get the Collider attached to this GameObject.
        ClickableCollider = GetComponent<Collider>();
    }
}
