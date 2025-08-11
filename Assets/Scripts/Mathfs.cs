using UnityEngine;
using static UnityEngine.Mathf;

namespace Funnelling
{
    public static class Mathfs
    {
        public static float TAU => PI * 2;
        
        public static Vector3 GetVectorByAngle(float angleRadius, float yOffset = 0f)
        {
            return new Vector3(Cos(angleRadius), yOffset, Sin(angleRadius));
        }

        public static Vector3 GetOffsetYPosition(Vector3 position, float yOffset = 0)
        {
            return new Vector3(position.x, position.y + yOffset, position.z);
        }
    }
}
