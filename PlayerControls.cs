using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast the spacecraft moves up and down based on player input")]
    [SerializeField] float speed = 20f;

    [Tooltip("How far the spacecraft moves horizontally")][SerializeField] float xRange = 5f;
    [Tooltip("How far the spacecraft moves vertically")] [SerializeField] float yRange = 3.5f;

    [Header("Screen position based on tuning ")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float postionYawFactor = 2f; 

    [Header("Player input based on tuning")]
    [SerializeField] float controlPitchFactor = -15f;

    [SerializeField] float controlRollFactor = -20f;

    [Header("Spacecraft's Lasers Array")]
    [Tooltip("Add all lasers here")]
    [SerializeField] GameObject[] lasers; 
    float xThrow, yThrow;
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring ();

    }

    void ProcessRotation ()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor ; 
        float pitchDueToControl =  yThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControl; //rotate up, down
        float yaw = transform.localPosition.x * postionYawFactor; //rotate left, right
        float roll = xThrow * controlRollFactor; //roll
        transform.localRotation = Quaternion.Euler (pitch, yaw, roll);
    }
    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * speed;
        float rawXPosition = transform.localPosition.x + xOffset;
        float clampedXPositon = Mathf.Clamp(rawXPosition, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * speed;
        float rawYPosition = transform.localPosition.y + yOffset;
        float clampedYPosition = Mathf.Clamp(rawYPosition, -yRange, yRange);

        float zPosition = transform.localPosition.z;

        transform.localPosition = new Vector3(rawXPosition, rawYPosition, zPosition);
    }

    void ProcessFiring ()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive (bool isActived)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActived;
        }
    }
}
