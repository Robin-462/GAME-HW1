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
/// Class Targetable defines a GameObject that is 
/// targetable or has targetable components which 
/// should react to the game's torpedo (projectile) 
/// system. Note that each targetable GameObject or 
/// Prefab should include both this script, which 
/// defines intersection behaviors, and the Targetable 
/// Tag (which is set via Unity Inspector panel). The 
/// tag is critical to put the GameObject instance onto 
/// the game's intersection test list.
/// 
/// </summary>

public class Targetable : MonoBehaviour
{
	[Tooltip("Line orientation in local (object) space")]
	public Vector3 surface = new Vector3(0.0f, 1.0f, 0.0f);

	[Tooltip("Surface normal in local (object) space")]
	public Vector3 surfaceNormal = new Vector3(-1.0f, 0.0f, 0.0f);

	private float extents;

	// Find point of intersection of targetable with line defined by an initial (starting) 
	// point and a direction vector for that line. Returns true if intersection point
	// found and false otherwise (intersection point is meaningless in latter case. This
	// method should encapsulate intersection (collision detection) logic that is specific
	// and meaningful for this type of targetable GameObject.
	// 
	// Hint: The intersection logic may be overloaded to accommodate other types of inter-
	// section tests beyond line-line intersections.
	public bool Intersect(out Vector3 intersection, Vector3 initial, Vector3 direction)
	{
		bool found = false;
		intersection = Vector3.zero;

		// These variables characterize our targetable. The variable center gives 
		// the midpoint of the targetable GameObject. Variable surface is a 2D line
		// segment in the plane which describes the barrier (e.g. the shield or other 
		// solid Game Object). Variable extent indicates the total length of this line
		// segment (i.e. the line segment runs extent/2 left and right from center
		// (midpoint) along the line characterized by surface.
		Vector3 center  = this.getCenter();
		Vector3 surface = this.getSurface();
		float   extent  = this.getExtent();

		Vector3 lineVec3 = center - initial;
		Vector3 crossVec1and2 = Vector3.Cross(direction, surface);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, surface);

		// if not parallel
		if (crossVec1and2.sqrMagnitude > 0.001f)
		{
			// Intersect the incoming torpedo line of fire with our shield (which
			// is also a line segment). Thus the logic finds the intersection point
			// between two lines in a plane. Note that this logic will not work if
			// the lines are not coplanar, but this is never an issue in a fully 2D
			// game environment. 

			Vector3 candPoint;
			float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
			candPoint = initial + (direction * s);

			// check against the length of the line segment
			if ((candPoint - center).sqrMagnitude <= (extent / 2.0f) * (extent / 2.0f))
			{
				// make sure the target is in front of us, not behind
				if (Vector3.Dot ((candPoint - initial).normalized, direction) > 0.0f)
				{
					found = true;
					intersection = candPoint;
				}
			}
			else
			{
				//Debug.Log("No intersection detected");
			}
		}
		return found;
	}

	// A vector (line) which describes the orientation of a
	// surface in the 2D space of the game world (which is a
	// planar space).
	public Vector3 getSurface ()
    {
		return transform.localToWorldMatrix * surface;
    }

	// A vector defining the surface normal transformed to world space.
	public Vector3 getNormal ()
    {
		return transform.localToWorldMatrix * surfaceNormal;
    }

	// A vector defining the center (midpoint) of this targetable.
	// The midpoint exists on the line that is defined by the
	// getSurface method.
    public Vector3 getCenter ()
    {
		return transform.position;
    }
    
	// A scalar defining the extents of targetable in 2D space
	// along the line defined by getSurface and centered on the
	// midpoint defined by getCenter. Note that the length of
	// the extents are getExtent/2 to each side of the midpoint 
	// along the getSurface line.
    public float getExtent ()
    {
        return extents;
    }

    // Start is called before the first frame update
    void Start()
    {
		// Get the surface length from the vertical dimension of
		// the sprite asset. Note that the sprite image must have
		// a tight bound around the color pixels (no excess transparent
		// pixels) for this method to yield good results.

        BoxCollider2D collider = this.GetComponent<BoxCollider2D> ();
        extents = collider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
