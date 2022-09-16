using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        public Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
            {
                if (!dis.Check(evaluators))
                {
                    return false;
                }
            }

            return true;
        }

        [System.Serializable]
        public class Disjunction
        {
            public Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [System.Serializable]
        public class Predicate
        {
            public EPredicate predicate;
            public string[] parameters;
            public bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);

                    if (result == null)
                    {
                        continue;
                    }

                    if (result == negate) return false;
                }

                return true;
            }
        }
    }
}
