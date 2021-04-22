namespace Shipwreck.Model.Items
{
    public interface IItem
    {
        protected ItemType ItemType { get; set; }
        public string StringItemType { get; set; }
    }
}