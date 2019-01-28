using System;

namespace AntiDuplWPF.UndoRedo
{
    public interface IUCommand : IDisposable
    {
        bool Execute();
        bool UnExecute();
        string Description { get; }
    }
}
