using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Broccoli.Model;

namespace Broccoli.Pipe {
    /// <summary>
    /// Variation group class.
    /// </summary>
    [System.Serializable]
    public class VariationGroup {
        #region Vars
        public int id = 0;
        public string name = "";
        public int seed = 0;
        public int minFrequency = 1;
        public int maxFrequency = 4;
        public Vector3 center = Vector3.zero;
        public float radius = 0f;
        public enum OrientationMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public OrientationMode orientation = OrientationMode.CenterToPeriphery;
        public float orientationRandomness = 0f;
        public float minScaleAtCenter = 1f;
        public float maxScaleAtCenter = 1f;
        public float minScaleAtPeriphery = 1f;
        public float maxScaleAtPeriphery = 1f;
        public enum BendMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public BendMode bendMode = BendMode.CenterToPeriphery;
        public float minBendAtCenter = 0f;
        public float maxBendAtCenter = 0f;
        public float minBendAtPeriphery = 0f;
        public float maxBendAtPeriphery = 0f;
        #endregion

        #region Constructor
        public VariationGroup () {}
        #endregion

        #region Clone
        /// <summary>
        /// Clone this instance.
        /// </summary>
        public VariationGroup Clone () {
            VariationGroup clone = new VariationGroup ();
            clone.id = id;
            clone.name = name;
            clone.seed = seed;
            clone.minFrequency = minFrequency;
            clone.maxFrequency = maxFrequency;
            clone.center = center;
            clone.radius = radius;
            clone.orientation = orientation;
            clone.orientationRandomness = orientationRandomness;
            clone.minScaleAtCenter = minScaleAtCenter;
            clone.maxScaleAtCenter = maxScaleAtCenter;
            clone.minScaleAtPeriphery = minScaleAtPeriphery;
            clone.maxScaleAtPeriphery = maxScaleAtPeriphery;
            clone.bendMode = bendMode;
            clone.minBendAtCenter = minBendAtCenter;
            clone.maxBendAtCenter = maxBendAtCenter;
            clone.minBendAtPeriphery = minBendAtPeriphery;
            clone.maxBendAtPeriphery = maxBendAtPeriphery;
            return clone;
        }
        #endregion
    }
}