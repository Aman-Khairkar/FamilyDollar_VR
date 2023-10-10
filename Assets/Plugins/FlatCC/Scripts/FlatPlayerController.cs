using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class FlatPlayerController : MonoBehaviour
{
    public const string VIEWER_HEIGHT_PLAYERPREF_KEY = "ViewerHeight";
    public const string WALK_SPEED_PLAYERPREF_KEY = "WalkSpeed";

    /// <summary>
    /// Called event for when we set Fly Mode
    /// </summary>
    /// <param name="flyMode">Whether the character is now in fly mode</param>
    public delegate void FlyModeSetEvent(bool flyMode);
    /// <summary>
    /// Attach here to be alerted when fly mode is set for this character
    /// </summary>
    public FlyModeSetEvent OnFlyModeSet;
    public BookmarkIndicator ActiveIndicator;

    public float WalkSpeed = 4;

    public float gravity = 20;

    bool isGrounded = false;

    static bool isAnimating = false;
    public static bool IsAnimating
    {
        get
        {
            return isAnimating;
        }
    }
   internal static bool interruptCurrentAnimation = false;

    bool flyMode;
    public bool FlyMode
    {
        get
        {
            return flyMode;
        }
    }
    public float divideFlySpeed = 40;

    public float ViewerHeight
    {
        get
        {
            return transform.localScale.y;
        }
    }

    Rigidbody ourRigidbody;
    Collider headCollider;
    Collider bodyCollider;

    private static GameObject flatCharacterControllerInstance;
    public static GameObject FlatCharacterControllerInstance
    {
        get
        {
            return flatCharacterControllerInstance;
        }
    }

    public GameObject CameraParent;
    public GameObject Camera;

    public FlatMouseLook YMouseLookScript;

    void Awake()
    {
        ourRigidbody = GetComponent<Rigidbody>();
        headCollider = Camera.GetComponent<Collider>();
        bodyCollider = GetComponent<Collider>();
        ourRigidbody.freezeRotation = true;
    }

    private void OnEnable()
    {
        //if we haven't seen a FlatCharacterController before this one
        if (flatCharacterControllerInstance == null)
        {
            //make this the instance
            flatCharacterControllerInstance = gameObject;
            //make it do not destroy on load
            DontDestroyOnLoad(gameObject);
        }
        else if (flatCharacterControllerInstance != this.gameObject)
        {
            //if we already had an instance (and it wasn't this one), then destroy this FlatCharacterController
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += ChangedScene;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= ChangedScene;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlyMode();
        }

    }

    void FixedUpdate()
    {
        if (!flyMode)
        {
            //calculate how fast we should be moving      
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * WalkSpeed, 0, Input.GetAxis("Vertical") * WalkSpeed);
            targetVelocity = transform.TransformDirection(targetVelocity);

            //zero movement if control is held down
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                targetVelocity = Vector3.zero;
            }

            //apply a force that attempts to reach our target velocity
            Vector3 velocity = ourRigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.y = 0f;
            ourRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else //Fly mode active so base movement off of the camera direction
        {

            Vector3 directionOfMovement = Vector3.zero;
            Vector3 directionOfMovementSide = Vector3.zero;

            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                //do not check for movement input
            }
            else
            {
                //forward & backward
                if (Input.GetKey("up") || Input.GetKey(KeyCode.W))
                {
                    directionOfMovement = Camera.transform.forward;
                }
                else if (Input.GetKey("down") || Input.GetKey(KeyCode.S))
                {
                    directionOfMovement = -Camera.transform.forward;
                }

                //left & right
                if (Input.GetKey("left") || Input.GetKey(KeyCode.A))
                {
                    directionOfMovementSide = -Camera.transform.right;
                }
                else if (Input.GetKey("right") || Input.GetKey(KeyCode.D))
                {
                    directionOfMovementSide = Camera.transform.right;
                }
            }            

            //move the char controller accordingly
            transform.position += ((directionOfMovement + directionOfMovementSide) * WalkSpeed / divideFlySpeed);

            //ensures that no forces should be applied during fly mode since gravity is turned off 
            ourRigidbody.velocity = Vector3.zero;
            ourRigidbody.angularVelocity = Vector3.zero;
        }
    }    	

    public void SetViewerHeight(float newValue, bool saveHeight = true)
    {
        float previousHeight = transform.localScale.y;

        //apply the height number
        transform.localScale = new Vector3(transform.localScale.x, newValue, transform.localScale.z);

        //change the position so it matches with the new scale
        float newPosition = (newValue - previousHeight) + transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, newPosition, transform.localPosition.z);

        if (saveHeight)
        {
            // Update this value in the player prefs
            PlayerPrefs.SetFloat(VIEWER_HEIGHT_PLAYERPREF_KEY, newValue);
            PlayerPrefs.Save();
        }
    }

    public void SetWalkSpeed(float newValue)
    {
        // Apply the new walk speed value
        WalkSpeed = newValue;

        // Update this value in the player prefs
        PlayerPrefs.SetFloat(WALK_SPEED_PLAYERPREF_KEY, newValue);
        PlayerPrefs.Save();
    }

    private void ChangedScene(Scene newScene, LoadSceneMode loadingMode)
    {
        if (gameObject.activeInHierarchy)
        {
            //Freeze position (as well as rotation) to prevent jumping/shifting (often caused by momentary clipping through a scene's original character)
            ourRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(UnfreezePlayerPosition());
        }
    }

    private IEnumerator UnfreezePlayerPosition()
    {
        yield return new WaitForEndOfFrame();
        //Unfreezes position by leaving it absent while also making sure rotation maintains its freeze
        ourRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// Move the flat character controller to the target position and rotation within the specified amount of time.
    /// </summary>
    /// <param name="targetPosition">The global position where the character should end up.</param>
    /// <param name="targetRotation">Euler angles of the rotation the character should end up being.</param>
    /// <param name="speed">How long it should take from the moment the character starts moving until when it reaches the target, in ms.</param>
    /// <param name="targetViewerHeight">How tall the player should be after the movement.</param>
    public void MoveToTarget(Vector3 targetPosition, Vector3 targetRotation, float speed, float targetViewerHeight = 0)
    {
        StartCoroutine(AnimateTransitionToBookmark(targetPosition, targetRotation, speed, targetViewerHeight));
    }

    private IEnumerator AnimateTransitionToBookmark(Vector3 targetPosition, Vector3 targetRotation, float speed, float targetViewerHeight = 0)
    {
        // Tell any current animations to stop
        if (IsAnimating)
        {
            interruptCurrentAnimation = true;
        }
        // Wait until any existing animations are done
        yield return new WaitUntil(() => !IsAnimating);

        // Mark that we are now animating
        isAnimating = true;

        // Turn off gravity so the player doesn't fall into the ether
        ourRigidbody.useGravity = false;

        // Turn off the player's collider(s) so shelves don't explode if the player goes through them
        bodyCollider.enabled = false;
        headCollider.enabled = false;

        // Get the player's current position and rotation
        Vector3 startingPosition = transform.position + new Vector3(0, -transform.localScale.y, 0);
        float startingHeight = ViewerHeight;
        Vector3 startingRotation = GetCombinedEulerAngles();

        // Make sure that the player isn't going to turn 360 degrees to reach the desired angle in any direction
        startingRotation = FindEfficientStartingRotation(startingRotation, targetRotation);

        // How long (in seconds) the transition should take based on the distance and speed of the journey (0 if speed is 0 or negative)
        float transitionLength = speed <= 0 ? 0 : Vector3.Distance(startingPosition, targetPosition) / speed;

        // If we do actually need to move in order to get to the destination point
        if (transitionLength > 0)
        {
            float startTime = Time.time;

            // The progress made between startingPosition and targetPosition (a percentage clamped between 0 and 1)
            float t = 0;

            // While we still haven't finished the transition
            while (t < 1 && !interruptCurrentAnimation)
            {
                // Update t based on the current time
                t = Mathf.Clamp((Time.time - startTime) / transitionLength, 0, 1);

                // If a height was specified
                if (targetViewerHeight > 0)
                {
                    // Update the player height
                    SetViewerHeight(Mathf.Lerp(startingHeight, targetViewerHeight, t), false);
                }

                // Update the position of the player based on t
                SetPlayerPositionAndRotation(
                        Vector3.Lerp(startingPosition, targetPosition, t),
                        Vector3.Lerp(startingRotation, targetRotation, t)
                    );

                yield return new WaitForEndOfFrame();
            }
        }

        // If the animation wasn't interrupted
        if (!interruptCurrentAnimation)
        {
            // Make sure we're at the target position and rotation exactly
            SetPlayerPositionAndRotation(targetPosition, targetRotation);

            // If a height was specified
            if (targetViewerHeight > 0)
            {
                // Set the height
                SetViewerHeight(targetViewerHeight, false);
            }

            UpdateCollidersForCurrentFlyMode();
        }

        if (!interruptCurrentAnimation)
        {
            ActiveIndicator.IsTransitionComplete = true;
        }
        // Mark that we no longer want to interrupt the current animation
        interruptCurrentAnimation = false;

        // Mark that we are done with our animation
        isAnimating = false;
        yield return null;
    }

    /// <summary>
    /// Set the player to the specified position and rotation.
    /// </summary>
    /// <param name="newPosition">The position the player's "feet" should be (the bottom of the collider, acting as a player-height-neutral anchor point).</param>
    /// <param name="newRotation">The euler angles of the direction the player should look in, in degrees.</param>
    internal void SetPlayerPositionAndRotation(Vector3 newPosition, Vector3 newRotation)
    {
        // Apply the position
        transform.position = newPosition + new Vector3(0, transform.localScale.y, 0);

        // Apply the rotation
        transform.localRotation = Quaternion.Euler(0, newRotation.y, 0);
        CameraParent.transform.localRotation = Quaternion.Euler(newRotation.x, 0, newRotation.z);
        YMouseLookScript.UpdateRotationTracker();
    }

    public Vector3 GetCombinedEulerAngles()
    {
        // The y comes from the character controller GO, while the x and z come from its child, the CameraParent GO
        Vector3 combinedEulerAngles = CameraParent.transform.localEulerAngles;
        combinedEulerAngles.y = transform.localEulerAngles.y;

        return combinedEulerAngles;
    }

    public void ToggleFlyMode()
    {
        SetFlyMode(!flyMode);
    }

    public void SetFlyMode(bool enable, bool updateColliders = true)
    {
        if (flyMode != enable)
        {
            flyMode = enable;

            if (updateColliders)
            {
                UpdateCollidersForCurrentFlyMode();
            }
        }

        if (OnFlyModeSet != null)
        {
            OnFlyModeSet(flyMode);
        }
    }

    public void UpdateCollidersForCurrentFlyMode()
    {
        ourRigidbody.useGravity = !flyMode;

        ourRigidbody.velocity = Vector3.zero;
        ourRigidbody.angularVelocity = Vector3.zero;

        //When in Fly Mode, colliders should be off to allow maximum maneuverability and camera placement
        //Turn off the "body" collider for fly mode
        bodyCollider.enabled = !flyMode;

        //Turn off the "head" collider for fly mode
        headCollider.enabled = !flyMode;
    }

    /// <summary>
    /// Given a proposed starting rotation and target rotation, find the version of the starting 
    /// rotation that is closest to the target rotation.
    /// For example, if given (20, -2.0, 181) as the proposed starting rotation and (60, 359, 0) 
    /// as the target rotation, this method would return (20, 358, 179).
    /// </summary>
    /// <param name="proposedStartingRotation">The starting rotation of the character, not yet optimized based on the target rotation.</param>
    /// <param name="targetRotation">The rotation that the character is about to rotate to.</param>
    /// <returns>Returns the version of <paramref name="proposedStartingRotation"/> that is closest to <paramref name="targetRotation"/>.</returns>
    private static Vector3 FindEfficientStartingRotation(Vector3 proposedStartingRotation, Vector3 targetRotation)
    {
        return new Vector3(FindEfficientStartingAngle(proposedStartingRotation.x, targetRotation.x),
                           FindEfficientStartingAngle(proposedStartingRotation.y, targetRotation.y),
                           FindEfficientStartingAngle(proposedStartingRotation.z, targetRotation.z));
    }

    /// <summary>
    /// Finds the version of <paramref name="proposedStartingAngle"/> closest to <paramref name="targetAngle"/>
    /// by adding/subtracting 360 degrees if necessary.
    /// </summary>
    /// <param name="proposedStartingAngle">The starting angle, not yet optimized based on the target angle.</param>
    /// <param name="targetAngle">The angle we want the starting angle to be close to.</param>
    /// <returns>Returns the version of <paramref name="proposedStartingAngle"/> closest to <paramref name="targetAngle"/>.</returns>
    private static float FindEfficientStartingAngle(float proposedStartingAngle, float targetAngle)
    {
        float efficientStartingAngle = proposedStartingAngle;

        // Flags to make sure we don't somehow get into an infinite loop
        bool added360 = false;
        bool subtracted360 = false;

        // If there is more than a 180 difference between the starting angle and the target angle
        // (i.e., turning the opposite direction would be faster)
        while (Mathf.Abs(targetAngle - efficientStartingAngle) > 180
            && !(added360 && subtracted360))
        {
            // Add or subtract 360 to the starting rotation so that we turn in the opposite direction
            if (efficientStartingAngle < targetAngle)
            {
                efficientStartingAngle += 360;
                added360 = true;
            }
            else
            {
                efficientStartingAngle -= 360;
                subtracted360 = true;
            }
        }

        return efficientStartingAngle;
    }
}