using UnityEngine;
using System;
public class PlayerMovement : MonoBehaviour
{
    private kinematics Movement;
    private int[] exactMove;    // move tile-by-tile
    private float[] acceleration;
    private Vector2 transformSprite;
    public GameObject wind; // for summoning a gust of wind
    private GameObject[] solids; // shit player walks on lol
    
    // ASSUMPTION:
    // 1. Amount of Solids in a given scene is constant.
    void Awake()    
    {
        acceleration = new float[2];
        exactMove = new int[2];
        Movement = GetComponent<kinematics>();
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    void Update()
    {
        solids = checkCollision.getObjects(this.gameObject, "solids");
        controlMovementX();
        controlMovementY();

        float[] Move = { 0, 0 };
        Movement.velocity = worldSettings.diff(Movement.velocity, acceleration);
        Move = worldSettings.diff(Move, Movement.velocity);
        exactMove[0] = Mathf.RoundToInt(Move[0]);
        exactMove[1] = Mathf.RoundToInt(Move[1]);

        transformSprite = moveSprite(exactMove);

        transform.position = (Vector2)transform.position + transformSprite;
    }

    public void controlMovementX()
    {
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            if (Input.GetKey("a"))
            {
                if(Movement.velocity[0] > -4f)  // max velocity
                    Movement.velocity[0] += -1f;  // acceleration
            }
            if (Input.GetKey("d"))
            {
                if(Movement.velocity[0] < 4f)
                    Movement.velocity[0] += 1f;
            }
        }
        else
        {
            if (Movement.velocity[0] > 0 && Movement.velocity[0] + Mathf.Sign(Movement.velocity[0]) * worldSettings.mu_f * -1f < 0)
            {
                Movement.velocity[0] = 0;
            }
            else if(Movement.velocity[0] < 0 && Movement.velocity[0] +  Mathf.Sign(Movement.velocity[0]) * worldSettings.mu_f * -1f > 0)
            {
                Movement.velocity[0] = 0;
            }
            else
            {
                Movement.velocity[0] += Mathf.Sign(Movement.velocity[0]) * worldSettings.mu_f * -1f; // deceleration
            }
        }
    }

    public void controlMovementY()
    {
        float fallMultiplier = 1f;
        float lowJumpMultiplier = 2f;
        if(checkCollision.isColliding(new Vector2(0f, -1f), solids) && Input.GetKeyDown("w"))
        {
            acceleration[1] = 0f;
            Movement.velocity[1] = 20f;
        }
        else if(checkCollision.isColliding(new Vector2(0f, -1f), solids))
        {
            acceleration[1] = 0f;
            Movement.velocity[1] = 0f;
        }
        else if (!checkCollision.isColliding(new Vector2(0f, -1f), solids))
        {
            acceleration[1] = -1f * worldSettings.g_a + -1f * Movement.velocity[1] * worldSettings.mu_d;
            if (Movement.velocity[1] < 0)
            {
                Movement.velocity[1] += acceleration[1] * (fallMultiplier - 1);
            }
            else if (Movement.velocity[1] > 0 && !Input.GetKey("w"))
            {
                Movement.velocity[1] += acceleration[1] * (lowJumpMultiplier - 1);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direct.z = 0;
            GameObject.Find("telekenesis").transform.position = direct;
            Instantiate(wind, direct, Quaternion.identity);
            Movement.velocity[0] += GetComponent<TelekenesisMovement>().tryForceJump()[0];
            Movement.velocity[1] += GetComponent<TelekenesisMovement>().tryForceJump()[1];
        }
    }

    public Vector2 moveSprite(int[] exactMove)
    {
        Vector2 mySprite = new Vector2(0f, 0f);
        while (worldSettings.MaxAbs(exactMove) != 0)
        {
            mySprite = (Vector2)mySprite + new Vector2(Math.Sign(exactMove[0]), Math.Sign(exactMove[1]));
            if (!checkCollision.isColliding(mySprite, solids))
            {
                exactMove[0] += Math.Sign(exactMove[0]) * -1;
                exactMove[1] += Math.Sign(exactMove[1]) * -1;
            }
            else if(!checkCollision.isColliding(((Vector2)mySprite - new Vector2(Math.Sign(exactMove[0]), 0)), solids))   // if it doesnt collide regardless of the x component
            {
                mySprite = (Vector2)mySprite - new Vector2(Math.Sign(exactMove[0]), 0);
                exactMove[0] = 0;
            }
            else if(!checkCollision.isColliding(((Vector2)mySprite - new Vector2(0f, Math.Sign(exactMove[1]))), solids))   // if it still collides regardless of the x component 
            { 
                mySprite = (Vector2)mySprite - new Vector2(0, Math.Sign(exactMove[1]));
                exactMove[1] = 0;
            }
            else
            {
                mySprite = (Vector2)mySprite - new Vector2(Math.Sign(exactMove[0]), Math.Sign(exactMove[1]));
                break;
            }
        }
        return mySprite;
    }
}
