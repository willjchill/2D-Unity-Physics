using UnityEngine;
using UnityEngine.UI;

public class DisplayCharge : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("playerModel");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "MAGNITUDE OF CHARGE: " + player.GetComponent<TelekenesisMovement>().getHiddenCharge();
    }
}