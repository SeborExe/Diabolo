using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        private void LateUpdate()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            transform.position = target.position;
        }
    }
}
