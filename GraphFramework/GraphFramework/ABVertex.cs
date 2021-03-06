﻿using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ABVertex : Vertex, IStackableVertex
    {
        public ABVertex(VertexType type, TwinGraph twinGraph)
        {
            Type = type;
            _inL = false;
            D = new LinkedList<ABVertex>();
            Descendants = new LinkedList<IStackableVertex>();
            IsExpanded = false;
            TwinGraph = twinGraph;
        }

        public ABVertex(VertexType type, string name, TwinGraph twinGraph) : this(type, twinGraph)
        {
            _name = name;
        }

        public TwinGraph TwinGraph { get; private set; }

        public ABVertex Twin { get; private set; }
        public bool IsPushed { get; private set; }
        public VertexType Type { get; private set; }
        public ABVertex L { get; set; }
        public Connection P { get; set; }
        public LinkedList<ABVertex> D { get; set; }
        private bool _inL;
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

        public void Expand(ExpandedArc arc)
        {
            IsExpanded = true;
            ExpandedArc = arc;
        }

        public bool IsInL()
        {
            return _inL;
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

        public void AddAnotherDtoD(LinkedList<ABVertex> anotherD)
        {
            foreach (var v in anotherD)
            {
                D.AddLast(v);
            }
        }

        public void AddedToL()
        {
            _inL = true;
        }

        public ABVertex Value { get { return this; } }
        public IStackableVertex Ancestor { get; private set; }
        public Arc ArcFromAncestor { get; private set; }
        public LinkedList<IStackableVertex> Descendants { get; private set; }
        public bool IsExpanded { get; private set; }
        public ExpandedArc ExpandedArc { get; private set; }

        public void AddDescendant(IStackableVertex vertex)
        {
            Descendants.AddLast(vertex);
        }

        public void Pushed(IStackableVertex ancestor, Arc arcFromAncestor)
        {
            Ancestor = ancestor;
            ArcFromAncestor = arcFromAncestor;
        }

        public override void ArcReverted(Arc arc)
        {
            if (Type == VertexType.A)
            {
                OutboundArcs.RemoveAll(a => a.End == TwinGraph.EndVertex);
                TwinGraph.EndVertex.InboundArcs.RemoveAll(a => a.Start == this);
                TwinGraph.Arcs.RemoveAll(a => a.Start == this && a.End == TwinGraph.EndVertex);
            }
            else
            {
                InboundArcs.RemoveAll(a => a.Start == TwinGraph.StartVertex);
                TwinGraph.StartVertex.OutboundArcs.RemoveAll(a => a.End == this);
                TwinGraph.Arcs.RemoveAll(a => a.Start == TwinGraph.StartVertex && a.End == this);
            }
            base.ArcReverted(arc);
        }

        //Weighted Vertex additions

        public double DualWeight
        {
            get; private set;
        }
    }
}