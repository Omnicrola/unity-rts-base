using UnityEngine;

namespace DefaultNamespace
{
    public class RtsCameraControl : MonoBehaviour
    {
        public float Speed = 1;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                var delta = transform.forward * Time.deltaTime * Speed;
                ApplyTransform(delta);
            }

            if (Input.GetKey(KeyCode.S))
            {
                var delta = transform.forward * -1f * Time.deltaTime * Speed;
                ApplyTransform(delta);
            }

            if (Input.GetKey(KeyCode.A))
            {
                var delta = transform.right * -1f * Time.deltaTime * Speed;
                ApplyTransform(delta);
            }

            if (Input.GetKey(KeyCode.D))
            {
                var delta = transform.right * Time.deltaTime * Speed;
                ApplyTransform(delta);
            }
        }

        private void ApplyTransform(Vector3 delta)
        {
            var position = transform.position;
            position.x += delta.x;
            position.z += delta.z;
            transform.position = position;
        }
    }
}