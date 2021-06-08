using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject[] objects;
    public kinematics[] health;

    // Start is called before the first frame update
    void Awake()
    {
        objects = getEnemies(this.gameObject);
        health = new kinematics[health.Length];
        int i = 0;
        foreach (GameObject go in objects)
        {
            health[i] = go.GetComponent<kinematics>();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // OBJECTIVES:
        // 1. make particles deal damage at certain velocity
        // 2. make repulse jump deal damage certain velocity
    }

    public static GameObject[] getEnemies(GameObject check)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] sorted = new GameObject[enemies.Length + 1];
        sorted[0] = check;
        int i = 1;
        foreach (GameObject go in sorted)
        {
            sorted[i] = go;
            i++;
        }
        return sorted;
    }
}
