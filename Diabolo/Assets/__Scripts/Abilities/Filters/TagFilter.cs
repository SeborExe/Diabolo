using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Abilities;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "TagFilter_", menuName = "Abilities/Filters/Tag Filter")]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter;

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectToFlter)
        {
            foreach (GameObject gameObject in objectToFlter)
            {
                if(gameObject.CompareTag(tagToFilter))
                {
                    yield return gameObject;
                }
            }
        }
    }
}
