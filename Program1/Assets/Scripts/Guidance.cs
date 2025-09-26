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

    [Tooltip("Maximum number of reflections before torpedo expires")]
    public int numberReflections = 2;

    [Tooltip("Layers that trigger reflections")]
    public LayerMask reflectiveLayers;

    GameObject[] _potentialTargets;
    Vector3 _targetPoint;
    bool _targetImpacted;
    bool _targetAcquired;
    AudioSource _ricochetSound;
    int _currentReflections = 0;

    float MyDot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    Vector2 MyReflect(Vector2 incident, Vector2 normal)
    {
        float dot = MyDot(incident, normal);
        return new Vector2(
            incident.x - 2f * dot * normal.x,
            incident.y - 2f * dot * normal.y
        );
    }

    void Start()
    {
        _potentialTargets = GameObject.FindGameObjectsWithTag("Targetable");

        AudioSource[] sounds = GetComponents<AudioSource>();
        if (sounds.Length > 1) _ricochetSound = sounds[1];
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (!_targetImpacted && timeToLive > 0.001f)
        {
            if (!_targetAcquired)
            {
                _targetAcquired = AcquireTarget();
            }

            CheckReflections();

            if (!_targetAcquired || Mathf.Abs((transform.position - _targetPoint).sqrMagnitude) > 0.002f)
            {
                transform.Translate(direction * velocity * Time.deltaTime);
            }
            else
            {
                _targetImpacted = true;
                if (_ricochetSound != null) _ricochetSound.Play();
                timeToLive = 0.5f;
            }
        }

        if (timeToLive <= fadeTime)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            float newAlpha = spriteRenderer.color.a - (Time.deltaTime / fadeTime);
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, newAlpha);

            if (timeToLive <= 0.001f)
                Destroy(gameObject);
        }
    }

    void CheckReflections()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, reflectiveLayers);
        if (hit.collider != null)
        {
            Reflect(hit);
        }
        else
        {
            CheckStarfieldBounds();
        }
    }

    void Reflect(RaycastHit2D hit)
    {
        if (_currentReflections >= numberReflections)
        {
            _targetImpacted = true;
            timeToLive = 0.1f;
            return;
        }

        _currentReflections++;

        Vector2 incident = new Vector2(direction.x, direction.y);
        Vector2 reflection = MyReflect(incident, hit.normal);
        direction = new Vector3(reflection.x, reflection.y, 0f);

        if (_ricochetSound != null) _ricochetSound.Play();
    }

    void CheckStarfieldBounds()
    {
        if (Camera.main == null) return;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 normal = Vector2.zero;
        bool hitBoundary = false;

        if (viewportPos.x <= 0.02f) { normal = Vector2.right; hitBoundary = true; }
        else if (viewportPos.x >= 0.98f) { normal = Vector2.left; hitBoundary = true; }
        else if (viewportPos.y <= 0.02f) { normal = Vector2.up; hitBoundary = true; }
        else if (viewportPos.y >= 0.98f) { normal = Vector2.down; hitBoundary = true; }

        if (hitBoundary)
        {
            RaycastHit2D fakeHit = new RaycastHit2D();
            fakeHit.normal = normal;
            Reflect(fakeHit);
        }
    }

    bool AcquireTarget()
    {
        Vector3 candTarget; 

        for (int i = 0; i < _potentialTargets.Length; i++)
        {
            bool candFound = _potentialTargets[i].GetComponent<Targetable>().Intersect(out candTarget, transform.position, direction);
            if (candFound)
            {
                _targetPoint = candTarget;
                return true;
            }
        }

        return false;
    }
}