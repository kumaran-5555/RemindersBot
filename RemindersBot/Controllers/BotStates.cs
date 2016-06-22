using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemindersBot
{
    public class BotStates
    {
        public enum BotStateTopLevel{ GetCommand, NewUser, InAdd, InDelete, InView};



        public enum BotStateSecondLevel { Add_GetName, Add_GetType, Add_GetTime, Add_GetReminedFrequence, Add_GetConfirmation, Delete_GetName, Delete_GetConfirmation, View_GetPagination, None};

        public BotStateTopLevel CurrentTopLevelState;
        public BotStateSecondLevel CurrentSecondLevelState;

        public BotStates(BotStateTopLevel topLevelState, BotStateSecondLevel subState)
        {
            CurrentTopLevelState = topLevelState;
            CurrentSecondLevelState = subState;
        }        

    }

    public class Alarm
    {
        public enum TypeEnum { Once, Daily, Monthly, Weekly, Hourly, Yearly};

        private TypeEnum Type { get; set; }
        private string Name { get; set; }
        private int Id { get; set; }
        private int IntervalMins { get; set; }
        private int Count { get; set; }
        private DateTime Time { get; set; }

        public Alarm()
        {

        }

        public static bool ValidateName(string name)
        {
            return true;
        }

    }
}