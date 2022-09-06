using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityData
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;
        float timeToCast;

        public AbilityData(GameObject user)
        {
            this.user = user;
        }

        public IEnumerable<GameObject> GetTargets()
        {
            return targets;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this.targets = targets;
        }

        public GameObject GetUser()
        {
            return user;
        }

        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            this.targetedPoint = targetedPoint;
        }

        public Vector3 GetTargetedPoint()
        {
            return targetedPoint;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void SetTimeToCast(float timeToCast)
        {
            this.timeToCast = timeToCast;
        }

        public float GetTimeToCast()
        {
            return timeToCast;
        }
    }
}
