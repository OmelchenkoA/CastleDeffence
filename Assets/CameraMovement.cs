using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{

    public CinemachineVirtualCamera currentCamera;
    public CinemachineBrain myBrain;
    public float moveSpeed;
    public float pathPosition = 0.0f;

    private float pathLength;
    private CinemachineTrackedDolly cinemachineTrackedDolly;

    private InputControls inputActions;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
	{
        inputActions = new InputControls();
    }


	private void OnEnable()
	{
        inputActions.Enable();

        myBrain.enabled = true;

        cinemachineTrackedDolly = currentCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        pathLength = cinemachineTrackedDolly.m_Path.MaxPos;
        pathPosition = cinemachineTrackedDolly.m_PathPosition;
    }

	private void OnDisable()
	{
        inputActions.Disable();

        myBrain.enabled = false;
    }


	// Update is called once per frame
	void Update()
    {
        Vector2 movementInput = inputActions.Camera.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(movementInput.x, 0, movementInput.y);
        pathPosition = Mathf.Clamp(pathPosition + movementInput.x * moveSpeed * Time.deltaTime,0, pathLength);
        cinemachineTrackedDolly.m_PathPosition = pathPosition;
    }
}
