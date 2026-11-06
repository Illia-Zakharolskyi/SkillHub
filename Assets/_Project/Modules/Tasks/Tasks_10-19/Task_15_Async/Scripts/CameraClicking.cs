using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TaskAsync
{
    public class CameraClicking : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private GameObject _interactHint;
        [SerializeField] private LayerMask mask;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, mask))
            {
                Button button = hit.collider.GetComponentInParent<Button>();

                if (button != null && button.interactable)
                {
                    EventSystem.current.SetSelectedGameObject(button.gameObject);

                    if (Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        button.onClick.Invoke();
                    }
                    return;
                }
                else
                {
                    Debug.Log("AA");

                }
            }
            else
            {
                _interactHint.SetActive(false);
            }

            if (EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
