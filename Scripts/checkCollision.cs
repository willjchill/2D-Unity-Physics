using UnityEngine;

public static class checkCollision 
{

    // ASSUMPTIONS ABOUT isColliding:
    // 1. The 0'th index of the list is the object that is being checked
    // 2. All other objects in the array are SOLIDS (isActor = false)
    // 3. All GameObjects in the array have a SpriteRenderer component 
    // 4. The SpriteRenderer list is sorted in the exact same form as the GameObjects list 
    public static GameObject collidingObj(Vector2 possibleTransform, GameObject[] objects)    
    {
        SpriteRenderer[] sprite = new SpriteRenderer[objects.Length];
        int j = 0;
        foreach(GameObject go in objects)
        {
            sprite[j] = go.GetComponent<SpriteRenderer>();
            j++;
        }

        // Detection via Axis-Aligned Bounding Box
        GameObject collidingObj = null;
        float aboveOfObject, belowOfObject, leftOfObject, rightOfObject;

        for (int i = 1; i < sprite.Length; i++)
        {
            aboveOfObject = (objects[i].transform.position.y - sprite[i].bounds.size.y / 2) - (objects[0].transform.position.y + possibleTransform.y + sprite[0].bounds.size.y / 2);
            belowOfObject = (objects[0].transform.position.y + possibleTransform.y  - sprite[0].bounds.size.y / 2) - (objects[i].transform.position.y + sprite[i].bounds.size.y / 2);
            leftOfObject = (objects[i].transform.position.x - sprite[i].bounds.size.x / 2) - (objects[0].transform.position.x + possibleTransform.x + sprite[0].bounds.size.x / 2);
            rightOfObject = (objects[0].transform.position.x + possibleTransform.x - sprite[0].bounds.size.x / 2) - (objects[i].transform.position.x + sprite[i].bounds.size.x / 2);
            // OBJECTIVE: If the boxes are NOT far away from each other's edges, they must be colliding.
            if (!(aboveOfObject >= 0 || belowOfObject >= 0) && !(leftOfObject >= 0 || rightOfObject >= 0))
            {
                collidingObj = objects[i];
                break;
            }
        }

        return collidingObj;    // if no collidingobj is found, return null
    }

    public static GameObject[] getObjects(GameObject check, string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject[] sorted = new GameObject[objects.Length + 1];
        sorted[0] = check;
        int i = 1;
        foreach (GameObject go in objects)
        {
            sorted[i] = go;
            i++;
        }
        return sorted; 
    }

    public static bool isColliding(Vector2 possibleTransform, GameObject[] objects)
    {
        return (collidingObj(possibleTransform, objects) != null);
    }
}
