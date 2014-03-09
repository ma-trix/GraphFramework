using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertex : Vertex
    {
        public ABVertex(VertexType type)
        {
            Type = type;
            inL = false;
            D = new LinkedList<ABVertex>();
        }

        public ABVertex(VertexType type, string name) : this(type)
        {
            _name = name;
        }

        public ABVertex Twin { get; private set; }
        public bool IsPushed { get; private set; }
        public VertexType Type { get; private set; }
        public ABVertex L { get; set; }
        public Tuple<Arc, StackVertex> P { get; set; }
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

        public void AddToE(Tuple<Arc, StackVertex> connection)
        {
            E.AddLast(connection);
        }

        public void AddToR(Tuple<Arc, StackVertex> connection)
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

        public LinkedList<Tuple<Arc,StackVertex>> E = new LinkedList<Tuple<Arc, StackVertex>>();
        public LinkedList<Tuple<Arc, StackVertex>> R = new LinkedList<Tuple<Arc, StackVertex>>();

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
    }
}