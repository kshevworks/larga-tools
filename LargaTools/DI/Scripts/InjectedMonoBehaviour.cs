using System;
using UnityEngine;

namespace LargaTools.DI
{
    public abstract class InjectedMonoBehaviour : MonoBehaviour
    {
        protected abstract bool UpdateWhenDisabled { get; }

        private void Awake()
        {
            Container.Register(this);
            OnAwakeInternal();
        }

        protected virtual void OnAwakeInternal()
        {
        }

        private void OnDestroy()
        {
            Container.Unregister(this);
            OnDestroyInternal();
        }

        protected virtual void OnDestroyInternal()
        {
        }

        public void OnInjectedUpdate()
        {
            if (!UpdateWhenDisabled)
                return;

            OnInjectedUpdateInternal();
        }

        /// <summary>
        /// Calls on unity's Update()
        /// </summary>
        protected virtual void OnInjectedUpdateInternal()
        {
        }

        public void OnInjectedLateUpdate()
        {
            if (!UpdateWhenDisabled)
                return;

            OnInjectedLateUpdateInternal();
        }

        /// <summary>
        /// Calls on unity's LateUpdate()
        /// </summary>
        protected virtual void OnInjectedLateUpdateInternal()
        {
        }

        public void OnInjectedFixedUpdate()
        {
            if (!UpdateWhenDisabled)
                return;

            OnInjectedFixedUpdateInternal();
        }

        /// <summary>
        /// Calls on unity's FixedUpdate()
        /// </summary>
        protected virtual void OnInjectedFixedUpdateInternal()
        {
        }
    }
}