using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class ArcHelper
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool DoesArcExist(Vertex start, Vertex end, LinkedList<Arc> inList)
        {
            var arc = inList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }

        public static bool DoesConnectionExist(Vertex start, Vertex end, LinkedList<Tuple<Arc, StackVertex>> inList)
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

        public static bool DeleteArc(Vertex start, Vertex end, LinkedList<Arc> fromList)
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