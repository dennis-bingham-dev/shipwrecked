using Shipwreck.Model.Items;

namespace Shipwreck.Model.Interfaces
{
    public interface IItemFactory
    {
        public IItem GetItem(ItemType itemType);
    }
}