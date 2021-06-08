using UnityEngine;

public class SummonParticle : MonoBehaviour
{
    public GameObject particle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Vector2 randPosition = new Vector2(Random.Range(1f, 2f) * worldSettings.worldScale, Random.Range(1f, 2f) * worldSettings.worldScale);
            Instantiate(particle);
            particle.transform.position = (Vector2)randPosition + (Vector2)GameObject.Find("telekenesis").transform.position;
        }
    }
}

