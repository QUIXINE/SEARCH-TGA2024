using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public Transform PlayerTransform;
    private void Update()
    {
        Vector3 dirPlayerToThis = (transform.position - PlayerTransform.position);
        Vector3 dir = (PlayerTransform.position - transform.position);
        float angle = Vector3.Angle(transform.right, dirPlayerToThis);
        // Debug.Log("angle: " + angle);
        Debug.DrawLine(transform.position, PlayerTransform.position, Color.green);
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

    }
}
