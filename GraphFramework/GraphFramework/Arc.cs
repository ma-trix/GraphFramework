﻿using System.Reflection;
using log4net;

namespace GraphFramework
{
    public class Arc
    {
        private static readonly ILog Log = LogManager.GetLogger
    (MethodBase.GetCurrentMethod().DeclaringType);

        public IVertex Start { get; set; }
        public IVertex End { get; set; }
        public Graph Graph { get; private set; }
        public bool IsInMatching { get; private set; }
        
        public Arc(Graph graph, IVertex start, IVertex end)
        {
            Start = start;
            End = end;
            Graph = graph;
        }

        public Arc(Graph graph, IVertex start, IVertex end, bool inMatching) : this(graph, start, end)
        {
            if (inMatching)
            {
                AddToMatching();    
            }
        }

        public void AddToMatching()
        {
            IsInMatching = true;
        }

        public override string ToString()
        {
            return Start.Name + " -> " + End.Name;
        }

        public void Revert()
        {
            var tmp = Start;
            Start = End;
            End = tmp;
            IsInMatching = !IsInMatching;
            Log.Info("Reverted arc " + this);
            Start.ArcReverted(this);
            End.ArcReverted(this);
        }
    }
}