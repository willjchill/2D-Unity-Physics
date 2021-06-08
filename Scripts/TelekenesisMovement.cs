using UnityEngine;

public class TelekenesisMovement : MonoBehaviour
{
    private GameObject tele;
    private float hiddenCharge;

    void Awake()
    {
        tele = GameObject.Find("telekenesis");
        hiddenCharge = 100000f; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))    // ATTRACTION
        {
            Vector3 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direct.z = 0;
            tele.transform.position = direct;
            tele.GetComponent<kinematics>().charge = -1f * hiddenCharge;
        }
        else
        {
            tele.GetComponent<kinematics>().charge = 0f;
            tele.transform.position = transform.position;
        }
        hiddenCharge += Input.mouseScrollDelta.y * 100000f;
    }

    public float[] tryForceJump()
    {
        float[] boost = new float[2];
        float[] normal = new float[2];
        float[] tangential = new float[2];
        float[] v1 = { 0f, 0f };
        float magOfV2IMPACT = 10f;
        float v1_n, v1_t, v2_n, m1, m2;
        float v1_prime_n;

        m1 = 1f;
        m2 = getHiddenCharge() * 0.0001f;

        Vector2 r1 = (Vector2)transform.position;
        Vector2 r2 = (Vector2)tele.transform.position;
        float dist = Vector2.Distance(r2, r1);
        // ASSIGNING NEW VELOCITIES IF COLLISION IS DETECTED BASED ON PERFECTLY ELASTIC COLLISION **
        // CHANGING VELOCITY VECTOR FOR THIS OBJECT
        // STEP 1: FINDING NORMAL VECTOR AND TANGENTIAL VECTOR
        normal[0] = transform.position.x - tele.transform.position.x;
        normal[1] = transform.position.y - tele.transform.position.y;
        normal = worldSettings.unitize(normal);
        tangential[0] = -1f * normal[1];
        tangential[1] = normal[0];
        // STEP 2: COMPUTING DOT PRODUCT OF TANGENTIAL AND NORMAL VECTORS
        v1_n = normal[0] * v1[0] + normal[1] * v1[1];
        v1_t = tangential[0] * v1[0] + tangential[1] * v1[1];
        v2_n = normal[0] * Mathf.Sign(normal[0]) * magOfV2IMPACT + normal[1] * magOfV2IMPACT * Mathf.Sign(normal[1]);
        // STEP 3: TANGENTIAL VELOCITIES ARE EQUAL TO FINAL TANGENTIAL VELOCITIES
        // STEP 4: FIND FINAL NORMAL VELOCITIES
        v1_prime_n = (v1_n * (m1 - m2) + 2f * m2 * v2_n) / (m1 + m2);
        // STEP 5: CONVERT SCALAR AND TANGENTIAL VELOCITIES INTO VECTORS
        // STEP 6: GETTING THE FINAL VECTORS!!! (fucking finally)
        v1_prime_n = v1_prime_n * Mathf.Exp(-1f * dist / (worldSettings.worldScale * worldSettings.worldScale));
        boost[0] = (v1_prime_n * normal[0] + v1_t * tangential[0]);
        boost[1] = (v1_prime_n * normal[1] + v1_t * tangential[1]);
        boost[0] = Mathf.Round(boost[0]);
        boost[1] = Mathf.Round(boost[1]);

        return boost;
    }

    public float getHiddenCharge()
    {
        return hiddenCharge;
    }
}
