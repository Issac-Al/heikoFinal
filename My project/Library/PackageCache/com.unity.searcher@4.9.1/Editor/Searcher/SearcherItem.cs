﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityEditor.Searcher
{
    [PublicAPI]
    [Serializable]
    public class SearcherItem
    {
        [SerializeField] int m_Id;
        [SerializeField] List<int> m_ChildrenIds;
        [SerializeField] string m_Name;
        [SerializeField] string m_Help;
        [SerializeField] string[] m_Synonyms;
        [SerializeField] Texture2D m_Icon;
        [SerializeField] bool m_CollapseEmptyIcon = true;

        public int Id => m_Id;

        public virtual string Name => m_Name;

        public string Path { get; private set; }

        public string Help
        {
            get => m_Help;
            set => m_Help = value;
        }

        public string[] Synonyms { get { return m_Synonyms; } set { m_Synonyms = value; } }

        public int Depth => Parent?.Depth + 1 ?? 0;

        public object UserData { get; set; }

        public Texture2D Icon { get => m_Icon; set { m_Icon = value; } }

        public bool CollapseEmptyIcon { get => m_CollapseEmptyIcon; set { m_CollapseEmptyIcon = value; } }

        public SearcherItem Parent { get; private set; }
        public SearcherDatabaseBase Database { get; private set; }

        // the backing field gets serialized otherwise and triggers a "Serialization depth limit 7 exceeded" warning
        [field:NonSerialized]
        public List<SearcherItem> Children { get; private set; }
        public bool HasChildren => Children.Count > 0;

        public SearcherItem(string name, string help = "", List<SearcherItem> children = null, object userData = null, Texture2D icon = null, bool collapseEmptyIcon = true)
        {
            m_Id = -1;
            Parent = null;
            Database = null;

            m_Name = name;
            m_Help = help;
            m_Icon = icon;
            UserData = userData;
            m_CollapseEmptyIcon = collapseEmptyIcon;

            Children = new List<SearcherItem>();
            if (children == null)
                return;

            Children = children;
            foreach (var child in children)
                child.OverwriteParent(this);
        }

        public void AddChild(SearcherItem child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (Database != null)
                throw new InvalidOperationException(
                    "Cannot add more children to an item that was already used in a database.");

            if (Children == null)
                Children = new List<SearcherItem>();

            Children.Add(child);
            child.OverwriteParent(this);
        }

        internal void OverwriteId(int newId)
        {
            m_Id = newId;
        }

        void OverwriteParent(SearcherItem newParent)
        {
            Parent = newParent;
        }

        internal void OverwriteDatabase(SearcherDatabaseBase newDatabase)
        {
            Database = newDatabase;
        }

        internal void OverwriteChildrenIds(List<int> childrenIds)
        {
            m_ChildrenIds = childrenIds;
        }

        internal void GeneratePath()
        {
            if (Parent != null)
                Path = Parent.Path + " ";
            else
                Path = string.Empty;
            Path += Name;
        }

        internal void ReInitAfterLoadFromFile()
        {
            if (Children == null)
                Children = new List<SearcherItem>();

            foreach (var id in m_ChildrenIds)
            {
                var child = Database.ItemList[id];
                Children.Add(child);
                child.OverwriteParent(this);
            }

            GeneratePath();
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Depth)}: {Depth}";
        }
    }
}
