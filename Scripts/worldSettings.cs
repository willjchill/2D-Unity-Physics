using UnityEngine;

public static class worldSettings
{
    public const float g_a = 0.9f; // gravitational acceleration
    public const float mu_f = 0.5f; // friction 
    public const float mu_d = 0.01f; // vertical air drag
    public const float k = 1f; // gravitational constant or coloumb's constant  
    public const float worldScale = 75f; 

    // DETERMINING FORCE VECTOR 
    public static float[] determineForce(GameObject go1, GameObject go2, string property)   // F = c * (p1 * p2) / r^2 * unit vector 
    {
        float[] sumOfF = { 0, 0 };
        float dist, magOfF, q1, q2, sign;
        float[] normal = new float[2];
        if (property == "mass")
        {
            q1 = go1.GetComponent<kinematics>().mass;
            q2 = go2.GetComponent<kinematics>().mass;
            sign = 1f;
        }
        else if(property == "charge")
        {
            q1 = go1.GetComponent<kinematics>().charge;
            q2 = go2.GetComponent<kinematics>().charge;
            sign = -1f;
        }
        else
        {
            Debug.Log("lol wut");
            q1 = 0;
            q2 = 0;
            sign = 0f;
        }
        Vector2 r1 = (Vector2)go1.transform.position;
        Vector2 r2 = (Vector2)go2.transform.position;
        normal[0] = r2.x - r1.x;
        normal[1] = r2.y - r1.y;
        normal = unitize(normal);
        dist = Vector2.Distance(r1, r2);
        if (dist >= worldScale)
        {
            magOfF = sign * k * (q1 * q2) / (dist * dist);
            sumOfF[0] += magOfF * normal[0];
            sumOfF[1] += magOfF * normal[1];
        }
        return sumOfF;
    }

    // FINDING EUCLIDIAN ESTIMATION 
    public static float[] diff(float[] x, float[] x_prime)
    {
        for (int i = 0; i < x.Length; i++)
            x[i] += x_prime[i];
        return x;
    }

    // CREATING UNIT VECTOR
    public static float[] unitize(float[] x)
    {
        float magOfX = Mathf.Sqrt(x[0] * x[0] + x[1] * x[1]);
        x[0] = x[0] / magOfX;
        x[1] = x[1] / magOfX;
        return x;
    }

    // CHECKING MAX ABS VALUE IN ARRAY
    // ASSUMPTION: Array is of int size 2
    public static float MaxAbs(int[] x)
    {
        int[] temp = new int[2];
        temp[0] = Mathf.Abs(x[0]);
        temp[1] = Mathf.Abs(x[1]);
        if (temp[1] > temp[0])
            return temp[1];
        else
            return temp[0];
    }
}
