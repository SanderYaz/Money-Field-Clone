using UnityEngine;

namespace Util
{
    public static class Constants
    {
   
        public static readonly Vector3[] VectorAxes = new Vector3[] {
            Vector3.back,
            Vector3.down,
            Vector3.forward,
            Vector3.left,
            Vector3.right,
            Vector3.up,
            Vector3.zero
        };
    
        public enum Direction { Back, Down, Forward, Left, Right, Up, Zero };
        
        public enum AltCodes
        {
            Smiley = 9786,
            NextSmiley = 9787,
            RightArrow = 16,
        }
    }
}
