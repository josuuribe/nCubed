
namespace RaraAvis.nCubed.EventSourcing.Core.Mementos
{
    /// <summary>
    /// Defines that the implementor can create memento objects (snapshots), that can be used to recreate the original state.
    /// </summary>
    public interface IMementoOriginator
    {
        /// <summary>
        /// Saves the object's state to an opaque memento object (a snapshot) that can be used to restore the state.
        /// </summary>
        /// <returns>An opaque memento object that can be used to restore the state.</returns>
        IMemento Memento { get; set; }
    }
}
