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
/// Class Guidance provides game logic for moving torpedoes 
/// (projectiles) in a 2D game world. The guidance system
/// selectively acquires targets and initiates an explosion
/// upon impact. Torpedoes have a designer-specified time
/// to live (TTL), after which they will cease to exist.
///
/// Hint: You will need to upgrade this system.
/// 
/// </summary>

public class Guidance : MonoBehaviour
{
    [Tooltip("Speed of the torpedo")]
    public float velocity = 10.0f;

    [Tooltip("Direction of the torpedo")]
    public Vector3 direction = Vector3.zero;

	[Tooltip("Maximum torpedo life in seconds")]
	public float timeToLive = 6.0f;

	[Tooltip("Time for torpedo to fade out in seconds")]
	public float fadeTime = 0.5f;

	GameObject[] potentialTargets = null;
	GameObject targetObject = null;
	Vector3    targetPoint  = Vector3.zero;

	bool targetImpacted = false;
	bool targetAcquired = false;

	Vector3 startPos;
	float t = 0.0f;

	float tMax = 0.0f;

    // Acquire a new target point
    bool Acquire(out Vector3 target, ref GameObject targObj)
    {
        bool found = false;
        target = Vector3.zero;

        bool candFound = false;
        Vector3 candTarget = Vector3.zero;

        // finds the first applicable target in the list.
        for (int i = 0; i < potentialTargets.Length && !found; i++)
        {
            //Debug.Log("Checking target " + potentialTargets[i].name);
            candFound = potentialTargets[i].GetComponent<Targetable>().Intersect(out candTarget, transform.position, direction);

            if (candFound)
            {
                found    = candFound;
                target   = candTarget;
				targObj  = potentialTargets[i];
            }
        }
        return found;
    }

	// Start is called before the first frame update
	void Start()
    {
		// assemble a list of all targets this torpedo may acquire
		potentialTargets = GameObject.FindGameObjectsWithTag("Targetable");

		startPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		// decrement the lifetime counter for this tick
		timeToLive -= Time.deltaTime;
		//Debug.Log("Time to live: " + timeToLive);

		// move until contact with target or out of gas
		if (!targetImpacted && timeToLive > 0.001f)
		{
			if (!targetAcquired)
			{
				targetAcquired = Acquire(out targetPoint, ref targetObject);

				//Debug.Log("Target acquired: " + targetAcquired + " at position " + targetPoint);
			}


			// Note: The targeting logic below is imperfect and can cause the
			// torpodo to "miss" its target under certain circumstances. Why?
			//
			// Upgrade this code by using LERP to move to torpedo to its target
			// and detect collisions.

			float currentDist = Mathf.Abs((transform.position - targetPoint).sqrMagnitude);

			// Either there's no target, or we haven't yet impacted target. 
			if (!targetAcquired || currentDist > 0.002f)
			{
				// just move along direction of travel indefinitely
				transform.Translate(direction * velocity * Time.deltaTime);
			}
			else // we've hit an acquired target: reflect
			{
				targetImpacted = true;

				// contacted with targetable object; play ricochset sound
				AudioSource ricochet = GetComponents<AudioSource>()[1];
				ricochet.Play();

				timeToLive = 0.5f;
			}
		}

		if (timeToLive <= fadeTime)
		{
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();

			// fade the torpedo out of existence
			renderer.color = new Color (1.0f, 1.0f, 1.0f, (renderer.color.a - (Time.deltaTime / fadeTime)));

			if (timeToLive <= 0.001f)
				// remove the object from the game
				GameObject.Destroy(gameObject);
		}
    }
}
