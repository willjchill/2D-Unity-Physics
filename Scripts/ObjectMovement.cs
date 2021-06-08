using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private kinematics Movement;
    private GameObject[] particles;
    public float[] netForce;

    // ASSUMPTION:
    // 1. Amount of Solids in a given scene is constant.
    void Awake()
    {
        netForce = new float[2];
        Movement = GetComponent<kinematics>();
    }

    // Update is called once per frame
    void Update()
    {
        float[] F, Move = new float[2];
        netForce[0] = 0f;
        netForce[1] = 0f;
        particles = GameObject.FindGameObjectsWithTag("charge");

        foreach (GameObject go in particles)
        {
            F = worldSettings.determineForce(this.gameObject, go, "charge");    // CALCULATING FOR ELECTRICAL FORCE
            netForce[0] += F[0];
            netForce[1] += F[1];
            F = worldSettings.determineForce(this.gameObject, go, "mass");  // CALCULATING FOR MASS
            netForce[0] += F[0];
            netForce[1] += F[1];
        }
        Movement.velocity = worldSettings.diff(Movement.velocity, netForce);
        Move = worldSettings.diff(Move, Movement.velocity);
        transform.position = (Vector2)transform.position + new Vector2(Move[0], Move[1]);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
