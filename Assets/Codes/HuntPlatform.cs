using UnityEngine;

public class HuntPlatform : MonoBehaviour
{
    public Collider2D platformCollider;

    void Awake()
    {
        platformCollider =
            GetComponent<Collider2D>();

        if (platformCollider == null)
        {
            platformCollider =
                GetComponentInChildren<Collider2D>();
        }
    }
}