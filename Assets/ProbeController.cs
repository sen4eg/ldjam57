using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class ProbeController : MonoBehaviour
{
    private InputSystem_Actions _inputActions = null;
    public Vector2 MoveInput {get; private set; }

    public float Vertical{get; private set;}=0;
    
    [SerializeField]
    private float horizontalSpeed = 15f;
    
    [SerializeField]
    private float verticalSpeed = 3f;

    [SerializeField] 
    private float downWardsFactor = 6f;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _initialPosition = Vector2.zero;
        
    }
    void OnEnable() {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        _inputActions.Player.Vertical.performed += ctx => Vertical = ctx.ReadValue<float>();
        _inputActions.Player.Vertical.canceled += ctx => Vertical = 0;

    }

    void OnDisable() {
        _inputActions.Player.Disable();
    }

    
    
    private Vector2 _initialPosition;
    [SerializeField] private float displacementLimit = 15f;

    void Update()
    {
        #region PlanarMovement
        Vector3 currentPosition = transform.position;
 
        Vector3 delta = new Vector3(MoveInput.x, 0f, MoveInput.y) * (horizontalSpeed * Time.deltaTime);

        Vector3 newPosition = currentPosition + delta;

        Vector2 flatPos = new Vector2(newPosition.x, newPosition.z);
        if (flatPos.magnitude > displacementLimit)
        {
            flatPos = flatPos.normalized * displacementLimit;
            newPosition.x = flatPos.x;
            newPosition.z = flatPos.y;
        }
        #endregion

        #region VerticalMovement
        float verticalDisp = Vertical * verticalSpeed * Time.deltaTime;
        if (Vertical > 0)
        {
            verticalDisp *= downWardsFactor;
        }
        
        newPosition.y -= verticalDisp;
        #endregion
        
        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        DrawCircleXZ(transform.position, displacementLimit, 64);
    }
    void DrawCircleXZ(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * angleStep);
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }
}
