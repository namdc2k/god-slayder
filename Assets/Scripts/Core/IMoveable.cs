using UnityEngine;

namespace Core{
    public interface IMoveable{
        void Move(Vector3 dir, float duration);
        void Rotate(Vector3 dir, float duration);
    }
}