using UnityEngine;

namespace Script
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _targetPlane;
        [SerializeField] private Camera _mainCamera;
        private const float MaxRaycastDistance = 100;
        private Vector3 _rayDirection;
        [SerializeField] private float _coolingTime = 0.1f;
        private float _accumulateTime;
        [SerializeField] private AudioSource _audioSource;
    
        private void Start()
        {
            if (_mainCamera == null)
            { 
                _mainCamera = Camera.main;
            }

            _accumulateTime = 0f;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _accumulateTime > _coolingTime)
            {
                Vector3 targetPoint = DrawTargetPoint();
                _rayDirection = (targetPoint - _centerPoint.position).normalized;
                InitializedPrefab(_centerPoint.position, _rayDirection);
                _audioSource.Play();
                _accumulateTime = 0;
            }
            _accumulateTime += Time.deltaTime;
        }

        private Vector3 DrawTargetPoint()
        {
            Vector3 hitPoint = new Vector3();
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, MaxRaycastDistance);
            if (hits != null)
            {
                foreach (var onehit in hits)
                {
                    // 检测是否与远处平面相交
                    if (onehit.collider.transform == _targetPlane)
                    {
                        // 获取相交点的位置
                        hitPoint = onehit.point;
                        hitPoint.z = 1f;
                        // 在这里处理你的逻辑，使用 targetPosition 作为点击在远处平面上的位置
                        Debug.Log("点击位置在远处平面上的坐标：" + hitPoint);
                    }
                    else
                    {
                        Debug.Log(onehit);
                    }
                }
            }
            else
            {
                Debug.Log("No Collide Target" + ray);
            }
            return hitPoint;
        }

        private void InitializedPrefab(Vector3 initialPosition,Vector3 initialRotation)
        {
            GameObject instantiatedObject = Instantiate(_bulletPrefab, initialPosition, Quaternion.Euler(initialRotation));
            instantiatedObject.GetComponent<BulletController>().MoveDirection = initialRotation;
            instantiatedObject.GetComponent<BulletController>().StartPoint = initialPosition;
            instantiatedObject.GetComponent<BulletController>().LookAtMoveDirection();
        }
    }
}
