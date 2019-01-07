using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitMovement : MonoBehaviour
    {
        public float MaxSpeed = 5;
        public float Accelleration = 1f;
        public float TurnAccelleration = 1;
        public float MaxTurnSpeed = 2;

        [ReadOnly] public float AngleToTarget;
        [ReadOnly] public float CurrentVelocity;
        [ReadOnly] public float CurrentTurnSpeed;

        private Vector3 _destination;
        private float _closingRange;
        private bool IsMoving { get; set; }

        public void MoveTo(Vector3 location)
        {
            _destination = location;
            _closingRange = 0;
            IsMoving = true;
        }

        public void MoveToWithin(Vector3 position, float range)
        {
            _destination = position;
            _closingRange = range;
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }

        private void Update()
        {
            if (IsMoving)
            {
                var positionDelta = _destination - transform.position;
                var movement = positionDelta.normalized * CurrentVelocity * Time.deltaTime;
                var isCloseEnough = positionDelta.magnitude <= movement.magnitude + _closingRange;
                if (isCloseEnough)
                {
                    CurrentVelocity = 0;
                    CurrentTurnSpeed = 0;
                    IsMoving = false;
                }
                else
                {
                    if (TurnToFace(_destination))
                    {
                        if (CurrentVelocity <= MaxSpeed)
                        {
                            CurrentVelocity += Accelleration * Time.deltaTime;
                        }

                        transform.position += movement;
                    }
                }
            }
        }

        private bool TurnToFace(Vector3 target)
        {
            if (CurrentTurnSpeed <= MaxTurnSpeed)
            {
                CurrentTurnSpeed += TurnAccelleration * Time.deltaTime;
            }

            var vectorToTarget = target.SetY(0) - transform.position.SetY(0);
            var facingDirection = transform.forward;
            var angleInDegrees = Vector3.SignedAngle(facingDirection, vectorToTarget, Vector3.up);

            AngleToTarget = angleInDegrees;
            if (Mathf.Abs(angleInDegrees) >= 5f)
            {
                var rotation = CurrentTurnSpeed * Time.deltaTime;
                rotation = angleInDegrees >= 0 ? rotation : rotation * -1f;
                transform.Rotate(Vector3.up, rotation);
                return false;
            }

            return true;
        }
    }
}