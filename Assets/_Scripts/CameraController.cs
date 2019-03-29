using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distanceFromTarget = 5;
    public bool follow = true;
    public float displacementY = 1.5f;
    public float easing = 0.1f;

    void Update()
    {
        // if allowed to follow the target
        if (follow)
        {
            Vector3 newPos = target.position - (target.forward * distanceFromTarget);
            
            newPos.y = target.position.y + displacementY;
            transform.position += (newPos - transform.position) * easing;
            transform.LookAt(target);
        }
        else
        {
            transform.LookAt(target);
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}