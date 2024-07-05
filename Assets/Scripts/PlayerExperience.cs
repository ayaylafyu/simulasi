using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create a PlayerExperience script that handles the player's experience points
public class PlayerExperience : MonoBehaviour
{
    private int currentExperience = 0;

    // Accessor for the current experience
    public int CurrentExperience => currentExperience;

    // Method to add experience points
    public void AddExperience(int experienceToAdd)
    {
        currentExperience += experienceToAdd;

        // Add any additional logic for level-up or other events
    }
}

