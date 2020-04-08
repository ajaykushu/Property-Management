using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.StateMachine
{
    public static class WorkOrderState
    {
        private static Dictionary<FilterEnumWOStage, FilterEnumWOStage> TrackOut = null;
        private static Dictionary<FilterEnumWOStage, FilterEnumWOStage> Trackin = null;
        static  WorkOrderState()
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
            else if(command == ProcessEnumWOStage.TrackIn && Trackin.TryGetValue(current,out var nextstate))
            {
                return nextstate;
            }
            return current;
        }
    }
}
