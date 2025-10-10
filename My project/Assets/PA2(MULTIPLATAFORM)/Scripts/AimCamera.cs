using UnityEngine;
using Unity.Cinemachine;

public class AimCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform debugTransform;

    private Vector3 _mouseWorldPosition, _aimDirection;
    
    public void AimChangeMode(bool aim)
    {
        if (aim == true)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            Vector3 worldAimTarget = _mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            _aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, _aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask))
        {
            _mouseWorldPosition = hit.point;
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        return _mouseWorldPosition;
    }
}