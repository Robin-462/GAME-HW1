using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [Tooltip("Line of sight in local (object) space")]
    public Vector3 lineOfSight = Vector3.zero;
    
    GameObject gc;
    GameObject hero;

    // direction to active target (in world space)
    Vector3 targetDir  = Vector3.zero;

    // A vector defining the surface normal in world space.
    public Vector3 getLineOfSight()
    {
        return transform.localToWorldMatrix * lineOfSight;
    }

    // Start is called before the first frame update
    void Start()
    {
        gc   = GameObject.FindGameObjectWithTag("GameController");
        hero = gc.GetComponent<GameManager> ().playerShip;
    }

    // Update is called once per frame
    void Update()
    {
    }	
}
