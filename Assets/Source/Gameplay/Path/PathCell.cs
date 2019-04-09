using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Gameplay.Path
{
    [ExecuteInEditMode]
    public class PathCell : MonoBehaviour
    {
        public bool DebugNewBranch = false;
        public bool DebugClear = false;
        public bool ValidForMesh = true;
        public PathCell GizmoOrigin = null;
        public PathMaker targetToDebugNew;
        private List<PathMaker> branches = new List<PathMaker>();

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

        public List<PathMaker> getBranches()
        {
            return branches;
        }

        public void addBranch(PathMaker branch)
        {
            branches.Add(branch);
        }

        public void createBranch(PathMaker newBranch)
        {
            // checks if branch already exists
            for(int i = 0; i < branches.Count; i++)
            {
                if(branches[i] == newBranch)
                {
                    return;
                }
            }

            ValidForMesh = false;
            branches.Add(newBranch);
            _parentPath.SetBranchGizmos(this, newBranch);
            
        }

        public void deleteBranch(PathMaker targetBranch)
        {
            branches.Clear();
            //Debug.LogError("not implemented");
        }

        private void Update()
        {
            if(DebugNewBranch)
            {
                deleteBranch(branches[0]);
                createBranch(targetToDebugNew);
                DebugNewBranch = false;
            }

            if(DebugClear)
            {
                deleteBranch(branches[0]);
                DebugClear = false;
            }
        }

    }
}
