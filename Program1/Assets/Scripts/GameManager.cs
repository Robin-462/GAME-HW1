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
/// Class GameManager provides state management for 
/// global game state, such as user options and exiting 
/// the game. This is not meant to be a "god" class; do 
/// not add logic specific to other types of GameObjects 
/// to this class.
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    [Tooltip("Reference to player's ship")]
    public GameObject playerShip = null;

    // Start is called before the first frame update
    void Start()
    {
        if (playerShip == null)
            playerShip = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // exit game on escape (ESC) key (game build only)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
