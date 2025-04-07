using UnityEngine;

public class FollowY : MonoBehaviour
{
    [SerializeField] Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float difference = 10;
    void Start()
    {
        difference = target.position.y - transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, target.position.y + difference, transform.position.z);
    }
}
