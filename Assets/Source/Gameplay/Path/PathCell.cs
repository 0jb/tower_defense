using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Gameplay.Path
{
    public class PathCell : MonoBehaviour
    {
        private PathMaker _parentPath;
        public PathMaker parentPath
        {
            get
            {
                return _parentPath;
            }
            set
            {
                _parentPath = value;
            }
        }
        
    }
}
