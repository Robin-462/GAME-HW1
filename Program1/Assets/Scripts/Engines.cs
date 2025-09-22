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
/// Class Engines provides simple logic for moving and 
/// rotating a GameObject in world space based on its 
/// current position. Provides a listener that responds 
/// to user's invocation of arrow keys (up, down, left, 
/// and right).
/// 
/// </summary>

public class Engines : MonoBehaviour
{
    [Tooltip("Frame-rate independent movement rate in screen units")]
    public float MovementRate = 2.0f;

    [Tooltip("Frame-rate independent rotation rate in degrees")]
    public float RotationRate = 60.0f;

    [Tooltip("Time to fade engine sound after shutoff, in seconds")]
    public float fadeTime = 0.5f;

    float startVolume;

    // Start is called before the first frame update
    void Start()
    {
        // the engine's user-specified sound preset
        startVolume = GetComponents<AudioSource>()[0].volume;
    }

    // Update is called once per frame
    void Update()
    {
        // input from up/down, left/right keys
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        AudioSource impulse = GetComponents<AudioSource>()[0];
        if ((Mathf.Abs (x) > 0.0f || Mathf.Abs (y) > 0.0f))
        {
            // ship is moving; engage the engine (sounds)
            impulse.volume = startVolume;
            if (!impulse.isPlaying)
            {
                impulse.Play();
            }
        }
        else
        {
            // ship has come to a stop, cease engine (sounds)
            if (impulse.isPlaying)
            {
                if (fadeTime > 0.01)
                    // a linear fade on the sound itself, simple but not very elegant
                    impulse.volume -= (Time.deltaTime / fadeTime);
                else
                    impulse.Stop();
            }
        }

        transform.Rotate(0.0f, 0.0f, -x * RotationRate * Time.deltaTime);

        // a framerate independent translation along the vector defined by user directional inputs
        transform.Translate((new Vector3 (0,y,0)).normalized * MovementRate * Time.deltaTime);
    }
}
