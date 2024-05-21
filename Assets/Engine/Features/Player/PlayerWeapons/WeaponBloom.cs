using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerWeapons
{
    internal sealed class WeaponBloom : MonoBehaviour
    {
        [SerializeField] float defaultBloomAngle = 3;


        private float _currentBloom;


        public Vector3 BloomAngle(Transform barrelPosition)
        {
            _currentBloom = defaultBloomAngle;

            float randX = Random.Range(-_currentBloom, _currentBloom);
            float randY = Random.Range(-_currentBloom, _currentBloom);
            float randZ = Random.Range(-_currentBloom, _currentBloom);

            Vector3 randomRotaion = new Vector3(randX, randY, randZ);
            return barrelPosition.localEulerAngles + randomRotaion;
        }
    }
}
