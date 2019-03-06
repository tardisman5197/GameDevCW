using UnityEngine;
public class CameraController : MonoBehaviour
{
    // the target that the camera will follow
    public Transform target;
    // the distance the camera will stay from the target
    public float distanceFromTarget = 5;
    // boolean to toggle following on and off
    public bool follow = true;
    // the displacement of the camera on the Y axis
    public float displacementY = 1.5f;
    // easing variable
    public float easing = 0.1f;
    /**
Update the cameras position to follow the target
*/
    void Update()
    {
        // if allowed to follow the target
        if (follow)
        {
            // move within "distanceFromTarget" metres of the target
            // but retain the same camera rotation at all times
            Vector3 newPos = target.position - (target.forward * distanceFromTarget);
            // apply the vertical displacement
            newPos.y = target.position.y + displacementY;
            transform.position += (newPos - transform.position) * easing;
            transform.LookAt(target);
        }
        else // otherwise just look at the target
        {
            // use LookAt(...) to point towards the target
            transform.LookAt(target);
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}