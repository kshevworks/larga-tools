namespace LargaTools.DI
{
    public class ExampleComponent : InjectedMonoBehaviour
    {
        protected override bool UpdateWhenDisabled => false;
        private AnotherExampleComponent _anotherExampleComponent;

        protected override void OnAwakeInternal()
        {
            Container.Instance.MakeDependency(true, out _anotherExampleComponent);
            _anotherExampleComponent.SaySomething();
        }
    }
}