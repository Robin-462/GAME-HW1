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
/// Class Thrusters provides simple logic for rotating 
/// a GameObject in world space about its current position. 
/// Provides a listener that responds to the user's invocation 
/// of the bracket ([,]) keys.
/// 
/// </summary>

public class Thrusters : MonoBehaviour
{
    [Tooltip("Frame-rate independent rotation rate in degrees")]
    public float RotationRate = 30.0f;

    [Tooltip("Time to fade thruster sound after shutoff, in seconds")]
    public float fadeTime = 0.1f;

    float startVolume;
   
    // Start is called before the first frame update
    void Start()
    {
        // the thruster's user-specified sound preset
        startVolume = GetComponents<AudioSource>()[0].volume;
    }

    // Update is called once per frame
    void Update()
    {
        float r = 0.0f;
        // continuous input from bracekt ([,]) keys
        if (Input.GetKey(KeyCode.LeftBracket))
            r -= 1.0f;

        if (Input.GetKey(KeyCode.RightBracket))
            r += 1.0f;

        AudioSource thruster = GetComponents<AudioSource>()[0];
        if (Mathf.Abs(r) > 0.0f)
        {
            // ship is rotating; engage the thruster (sounds)
            thruster.volume = startVolume;
            if (!thruster.isPlaying)
            {
                thruster.Play();
            }
        }
        else
        {
            // ship has stopped rotating, cease thruster (sounds)              
            if (thruster.isPlaying)
            {
                // a linear fade on the sound itself, simple but not very elegant
                if (fadeTime > 0.01f)
                    thruster.volume -= (Time.deltaTime / fadeTime);
                else
                    thruster.Stop();
            }
        }
        // a framerate independent rotation based on the user's inputs
        transform.Rotate(0.0f, 0.0f, -r * RotationRate * Time.deltaTime);
    }
}
