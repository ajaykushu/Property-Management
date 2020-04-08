using Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.StateMachine
{
    public static class WorkOrderState
    {
        private readonly static Dictionary<FilterEnumWOStage, FilterEnumWOStage> TrackOut = null;
        private readonly static Dictionary<FilterEnumWOStage, FilterEnumWOStage> Trackin = null;

        static WorkOrderState()
        {
            Trackin = new Dictionary<FilterEnumWOStage, FilterEnumWOStage>
            {
                {FilterEnumWOStage.INITWO,FilterEnumWOStage.WOPROG },
                { FilterEnumWOStage.WOPROG,FilterEnumWOStage.WOCOMP}
            };

            TrackOut = Trackin.ToDictionary(x => x.Value, x => x.Key);
        }

        public static FilterEnumWOStage GetNextState(FilterEnumWOStage current, ProcessEnumWOStage command)
        {
            if (command == ProcessEnumWOStage.TrackOut && TrackOut.TryGetValue(current, out var prevstate))
            {
                return prevstate;
            }
            else if (command == ProcessEnumWOStage.TrackIn && Trackin.TryGetValue(current, out var nextstate))
            {
                return nextstate;
            }
            return current;
        }
    }
}