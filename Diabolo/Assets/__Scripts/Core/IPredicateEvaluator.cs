using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public enum EPredicate
    {
        Select,
        HasQuest,
        CompletedObjective,
        CompletedQuest,
        HasLevel,
        MinimumTrait,
        HasItem,
        HasItems,
        HasItemEquipped
    }

    public interface IPredicateEvaluator
    {
        bool? Evaluate(EPredicate predicate, string[] parameters);
    }
}
