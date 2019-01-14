using System;
using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitMovement : MonoBehaviour
    {
        public float Mass = 15;
        public float MaxVelocity = 3;
        public float MaxForce = 15;
        public float DefaultClosingDistance = 1;


        private Vector3 _destination;
        private Vector3 _velocity;
        private float _closingDistance;

        private bool IsMoving { get; set; }

        public void MoveTo(Vector3 location)
        {
            _destination = location;
            _closingDistance = 0;
            IsMoving = true;
        }

        public void MoveToWithin(Vector3 position, float range)
        {
            _destination = position;
            _closingDistance = range;
            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }

        private void Update()
        {
            var desiredVelocity = _destination - transform.position;
            var approachRadius = Math.Max(_closingDistance, DefaultClosingDistance);
            var distance = desiredVelocity.magnitude;
            if (distance < approachRadius)
            {
                desiredVelocity = desiredVelocity.normalized * MaxVelocity * (distance / approachRadius);
            }
            else
            {
                desiredVelocity = desiredVelocity.normalized * MaxVelocity;
            }

            var steering = desiredVelocity - _velocity;
            steering = Vector3.ClampMagnitude(steering, MaxForce);
            steering /= Mass;

            _velocity = Vector3.ClampMagnitude(_velocity + steering, MaxVelocity);
            transform.position += _velocity * Time.deltaTime;
            if (distance > 0.1f)
            {
                transform.forward = _velocity.normalized;
            }
        }
    }
}