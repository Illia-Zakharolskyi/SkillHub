namespace SkillHub.Task.Cinemachine
{
    using Unity.Cinemachine;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CinemachineCameraAddition : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CinemachineCamera _virtualCamera;
        [SerializeField] private InputActionReference _zoomActionReference;
        [SerializeField] private InputActionReference _toggleCursorReference;

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 0.1f; // Adjust to the scroll scale you see in the log
        [SerializeField] private float smoothTime = 10f;
        [SerializeField] private float minZoom = 20f;    // FOV (perspective) or ortho size (orthographic)
        [SerializeField] private float maxZoom = 60f;

        private float targetZoom;

        private void Start()
        {
            if (_virtualCamera == null)
            {
                _virtualCamera = GetComponent<CinemachineCamera>();
            }

            targetZoom = GetCurrentZoom();
        }

        private void OnEnable()
        {
            if (_zoomActionReference != null && _zoomActionReference.action != null)
            {
                _zoomActionReference.action.Enable();
                _toggleCursorReference.action.Enable();
                _zoomActionReference.action.performed += OnScroll;
                _toggleCursorReference.action.performed += OnCursorToggle;
            }
        }

        private void OnDisable()
        {
            if (_zoomActionReference != null && _zoomActionReference.action != null)
            {
                _zoomActionReference.action.performed -= OnScroll;
                _toggleCursorReference.action.performed -= OnCursorToggle;
                _zoomActionReference.action.Disable();
                _toggleCursorReference.action.Disable();
            }
        }

        private void OnScroll(InputAction.CallbackContext context)
        {
            float scroll = context.ReadValue<float>();
            Debug.Log($"Scroll Value: {scroll}"); // Remove after verifying the value

            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetZoom = Mathf.Clamp(targetZoom - scroll * zoomSpeed, minZoom, maxZoom);
            }
        }

        private void Update()
        {
            float current = GetCurrentZoom();
            float next = Mathf.Lerp(current, targetZoom, Time.deltaTime * smoothTime);
            SetZoom(next);
        }

        private float GetCurrentZoom()
        {
            return _virtualCamera.Lens.Orthographic
                ? _virtualCamera.Lens.OrthographicSize
                : _virtualCamera.Lens.FieldOfView;
        }

        private void SetZoom(float value)
        {
            if (_virtualCamera.Lens.Orthographic)
            {
                _virtualCamera.Lens.OrthographicSize = value;
            }
            else
            {
                _virtualCamera.Lens.FieldOfView = value;
            }
        }

        private void OnCursorToggle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
    }
}