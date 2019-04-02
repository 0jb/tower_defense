using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Gameplay.Path;


namespace TD.Gameplay.Units
{
    public class FollowPath : MonoBehaviour
    {
        [SerializeField]
        private float Speed = 1.0f;

        private PathCell _pathA;
        private PathCell _pathB;
        private bool _canMove = false;
        private float _distanceCloseToTarget = 0.5f;
        private float _speedToRotate = 4.0f;

        public void OnInstantiate(PathCell pathToStart)
        {
            _pathA = pathToStart;
            transform.position = _pathA.transform.position;            
            _pathB = pathToStart.parentPath.GetNextPathCell(_pathA);
            _canMove = true;
        }

        private void ChangeTarget()
        {
            _pathA = _pathB;
            _pathB = _pathA.parentPath.GetNextPathCell(_pathA);
            if(_pathB == null)
            {
                GotToFinishLine();
            }
        }

        private void GotToFinishLine()
        {
            _canMove = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if(_canMove)
            {
                Quaternion targetRotation = Quaternion.LookRotation(transform.position - _pathB.transform.position, 
                                                                    Vector3.up);
                float rotateSpeed = Time.deltaTime * _speedToRotate * 100;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                                targetRotation,
                                                                rotateSpeed);
                Vector3 newPos = transform.position;

                Vector3 speed = new Vector3(Time.deltaTime * Speed, 
                                            Time.deltaTime * Speed, 
                                            Time.deltaTime * Speed);

                newPos += Vector3.Scale( transform.forward, -speed );
                transform.position = newPos;

                if ( Vector3.Distance( transform.position, _pathB.transform.position ) < _distanceCloseToTarget)
                {
                    ChangeTarget();
                }
            }            
        }
    }
}

