using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ArcHelper
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool DoesArcExist(IVertex start, IVertex end, LinkedList<Arc> inList)
        {
            var arc = inList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    Log.Info("Arc " + arc.Value.Start.Name + " -> " + arc.Value.End.Name + " exists");
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }

        public static Arc FindArc(Vertex start, Vertex end, LinkedList<Arc> inList)
        {
            var arc = inList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    Log.Info("Arc " + arc.Value.Start.Name + " -> " + arc.Value.End.Name + " exists");
                    return arc.Value;
                }
                arc = nextArc;
            }
            return null;
        }

        public static bool DoesConnectionExist(Vertex start, Vertex end, LinkedList<Tuple<Arc, IStackableVertex>> inList)
        {
            var arc = inList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Item1.Start == start && arc.Value.Item1.End == end)
                {
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }

        public static bool DeleteArc(IVertex start, IVertex end, LinkedList<Arc> fromList)
        {
            var arc = fromList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    fromList.Remove(arc);
                    Log.Info("Removed Arc " + arc.Value.Start.Name + " -> " + arc.Value.End.Name);
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }
    }
}