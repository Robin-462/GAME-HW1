using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// SPACE STATION DEFENSE! (Starter Code)
/// School of Information, University of Arizona
/// A simple 2D game demonstration by Leonard D. Brown
///
/// This code may modified freely for ISTA 425 and 
/// INFO 525 (Algorithms for Games) students in their
/// assignments. Other uses covered by the terms of 
/// the GNU Lesser General Public License (LGPL).
/// 
/// Class Launcher provides logic for launch a spread
/// of torpedo projectiles toward a target. Torpedoes 
/// are launched from a designer-specified offset position 
/// relative to the GameObject for which the Launcher is 
/// a component. The Launcher maintains a queue of active 
/// torpedoes.
/// 
/// </summary>

public class Launcher : MonoBehaviour
{
    // maximum torpedos that may be in play at a time
    private GameObject[] TorpedoArray;
    int index = 0;

    [Tooltip("Type of torpedo prefab fired by launcher")]
    public GameObject TorpedoType;

    [Tooltip("Max number of torpedoes that may be in play at any time")]
    public int MaxTorpedoes = 8;

    [Tooltip("Max number of torpedoes that may be fired per second")]
    public float RateOfFire = 4.0f;

    public GameObject GetLastTorpedo()
    {
        return TorpedoArray[index]; 
    }

    // Start is called before the first frame update
    void Start()
    {
        TorpedoArray = new GameObject[MaxTorpedoes];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Guidance guidance;

            index++;
            if (index >= MaxTorpedoes)
                index = 0;

            if (TorpedoArray[index] != null)
                GameObject.Destroy(TorpedoArray[index]);

            Vector4 position = transform.localToWorldMatrix * new Vector4 (0.0f, 1.0f, 0.0f, 1.0f);
            Quaternion rotation = Quaternion.identity;

            TorpedoArray[index] = Instantiate(TorpedoType, position, rotation) as GameObject;
            
            // init guidance system to direction ship is facing
            guidance = TorpedoArray[index].GetComponent<Guidance>();
            guidance.direction = (new Vector3 (position.x, position.y, position.z) - transform.position).normalized;

            //Debug.Log("Torpedo direction is " + guidance.direction);
        }
    }
}
