namespace AntiDuplWPF.DragDrop
{
    interface IDropable
    {
        /// <summary>
        /// Drop data into the collection.
        /// </summary>
        /// <param name="data">The data to be dropped</param>
        void Drop(object dropObject);
    }
}
