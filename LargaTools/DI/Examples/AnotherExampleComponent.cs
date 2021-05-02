using UnityEngine;

namespace LargaTools.DI
{
    public class AnotherExampleComponent : InjectedMonoBehaviour
    {
        protected override bool UpdateWhenDisabled => false;

        public void SaySomething()
        {
            Debug.Log("Say");
        }
    }
}