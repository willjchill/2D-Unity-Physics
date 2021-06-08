using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        GetComponent<Animator>().Play("wind_effect_Y");
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("HERE! Y");
    }
}
