using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertex : Vertex, IStackVertex
    {
        public ABVertex(VertexType type)
        {
            Type = type;
            inL = false;
            D = new LinkedList<ABVertex>();
            Descendants = new LinkedList<IStackVertex>();
        }

        public ABVertex(VertexType type, string name) : this(type)
        {
            _name = name;
        }

        public ABVertex Twin { get; private set; }
        public bool IsPushed { get; private set; }
        public VertexType Type { get; private set; }
        public ABVertex L { get; set; }
        public Connection P { get; set; }
        public LinkedList<ABVertex> D { get; set; }
        private bool inL;
        public override String Name { get { return _name + "." + Type.ToString(); } }

        public void Pushed()
        {
            IsPushed = true;
        }

        public void SetTwin(ABVertex twin)
        {
            Twin = twin;
        }

        public void AddToE(Connection connection)
        {
            E.AddLast(connection);
        }

        public void AddToR(Connection connection)
        {
            R.AddLast(connection);
        }

        public void Expand(Arc arc)
        {
            throw new System.NotImplementedException();
        }

        public bool IsInL()
        {
            return inL;
        }

        public void EmptyD()
        {
            D.Clear();
        }

        public LinkedList<Connection> E = new LinkedList<Connection>();
        public LinkedList<Connection> R = new LinkedList<Connection>();

        public void AddToD(ABVertex v)
        {
            D.AddLast(v);
        }

        public void AddAnotherDToD(LinkedList<ABVertex> AnotherD)
        {
            foreach (var v in AnotherD)
            {
                D.AddLast(v);
            }
        }

        public void AddedToL()
        {
            inL = true;
        }

        public ABVertex Value { get { return this; } }
        public IStackVertex Ancestor { get; private set; }
        public LinkedList<IStackVertex> Descendants { get; private set; }
        public bool IsExpanded { get; private set; }
        public ExpandedArc ExpandedArc { get; private set; }
        public void AddDescendant(IStackVertex vertex)
        {
            Descendants.AddLast(vertex);
        }
        public void Pushed(IStackVertex ancestor)
        {
            Ancestor = ancestor;
        }
    }
}