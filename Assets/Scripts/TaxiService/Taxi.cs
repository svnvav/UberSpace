using System;
using Catlike.ObjectManagement;
using UnityEngine;

namespace Svnvav.UberSpace
{
    public class Taxi : PersistableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _takePassengerRadius;

        public bool IsIdle => _current == null;
        
        private Order _current;
        private Action _onCurrentComplete;
        
        public void ExecuteOrder(Order order, Action onComplete)
        {
            _current = order;
            _onCurrentComplete = onComplete;
        }

        public void GameUpdate(float deltaTime)
        {
            if (IsIdle)
            {
                Idle();
                return;
            }
            switch (_current.Status)
            {
                case OrderStatus.Queued:
                    _current.Take();
                    break;
                case OrderStatus.Taken:
                    MoveTo(_current.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - _current.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        _current.StartExecuting();
                    }

                    break;
                case OrderStatus.Executing:
                    MoveTo(_current.GetCurrentPointToMove(), deltaTime);
                    if (Vector3.SqrMagnitude(transform.position - _current.GetCurrentPointToMove()) <
                        _takePassengerRadius * _takePassengerRadius)
                    {
                        _current.Complete();
                        _onCurrentComplete?.Invoke();
                    }

                    break;
            }
        }

        private void Idle()
        {
        }


        private void MoveTo(Vector3 destination, float deltaTime)
        {
            transform.LookAt(destination, Vector3.forward);
            transform.Translate(_speed * deltaTime * transform.forward, Space.World);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(transform.localPosition);
            writer.Write(transform.localRotation);
        }

        public override void Load(GameDataReader reader)
        {
            _current = null;
            transform.localPosition = reader.ReadVector3();
            transform.localRotation = reader.ReadQuaternion();
        }
    }
}