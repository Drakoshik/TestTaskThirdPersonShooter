using UnityEngine;

namespace TPS.Engine.Features.Enemies
{
    internal class EnemyBullet : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        private Vector3 _startPoint;
        private Vector3 _endPoint;

        public void SetBullet(Vector3 startPoint, Vector3 endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        private void Update()
        {
            // transform.position = Vector3.MoveTowards(transform.position, _endPoint * 2f, 0.1f);
            // if (Vector3.Distance(transform.position, _endPoint * 2f) < 0.1f)
            //     Destroy(gameObject);
        }

    }
}
