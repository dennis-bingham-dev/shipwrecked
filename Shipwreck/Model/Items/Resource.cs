﻿namespace Shipwreck.Model.Items
{
    public class Resource : IItem
    {
        public string Name { get; }
        public string Description { get; }
        public bool Droppable { get; }
        public Type ResourceType { get; }
        public Resource(string name, string description, bool droppable = true)
        {
            Name = name;
            Description = description;
            Droppable = droppable;
        }

        public enum Type
        {
            Branch,
            Match,
            SharpStone,
            Stone,
            Vine,
        }
    }
}
