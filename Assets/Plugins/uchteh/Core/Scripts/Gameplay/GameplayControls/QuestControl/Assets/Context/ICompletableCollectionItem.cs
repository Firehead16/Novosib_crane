using System.Collections.Generic;

namespace Core.Gameplay.Questing
{
    public interface ICompletableCollectionItem
    {

        bool IsCompleted();

        List<ICompletableCollectionItem> GetActiveItems();

        List<ICompletableCollectionItem> GetUncompletedItems();

        void Reset();
    }
}