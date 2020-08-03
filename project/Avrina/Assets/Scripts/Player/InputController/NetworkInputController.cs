public class NetworkInputController : InputController
{
    /// <summary>
    ///  The network controller does not use any mapping type
    /// </summary>
    public override MappingType type { get; } = MappingType.None;

    /// <summary>
    ///  The network controller does not handle any inputs
    /// </summary>
    public override void HandleKeyInputs() {}
}
