using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        t = Mathf.Clamp01(t); 
        return a + (b - a) * t;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        t = Mathf.Clamp01(t);
        return a + (b - a) * t;
    }
    
    [Header("The setting of guidence")]

    [Tooltip("The speed of torpedo(per second).")]
    public float speed = 1f; 
    
    [Tooltip("The max range of torpedos")]
    public float maxRange = 1000f;
    
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float journeyTraveled = 0f;
    void Start()
    {
        startPosition = transform.position;

        endPosition = (Vector2) startPosition + new Vector2(0,1) * maxRange;
        
    }

    // Update is called once per frame
    void Update()
    {
        journeyTraveled += speed * Time.deltaTime;
        
        float t = journeyTraveled / maxRange;

        // new position
        transform.position = Lerp(startPosition, endPosition, t);
        
        if (t >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}