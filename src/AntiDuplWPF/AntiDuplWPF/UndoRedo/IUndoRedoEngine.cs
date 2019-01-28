using System.ComponentModel;

namespace AntiDuplWPF.UndoRedo
{
    public interface IUndoRedoEngine : INotifyPropertyChanged
    {
        void ExecuteCommand(IUCommand command);
        bool Redo();
        bool RedoEnable { get; }
        bool Undo();
        bool UndoEnable { get; }

        void Clear();

        string UndoTooltip { get; }
    }
}
