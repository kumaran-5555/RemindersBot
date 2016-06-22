using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RemindersBot
{
    public abstract class AbstractCommand
    {
        public static Commands CommandsObj
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public static Object CommandsEnum
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        //Commands CommandsObj { get; set; }
    }

    public abstract class AbstractPromptCommand
    {
        public static PaginationCommands CommandsObj
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public class SamplePrompt: AbstractPromptCommand
    {
        public static new PaginationCommands CommandsObj
        {
            get; private set;
        }
        private static List<Tuple<string, Object>> Members;

        static SamplePrompt()
        {
            Members = new List<Tuple<string, object>>();

            Members.Add(new Tuple<string, object>("credit card", 1));
            Members.Add(new Tuple<string, object>("electricity bill", 3));
            Members.Add(new Tuple<string, object>("insurance ", 3));

            CommandsObj = new PaginationCommands(Members, 1);
        }
    }

    
    public class TopLevelCommands: AbstractCommand
    {

        
        public static new Commands CommandsObj
        {
            get; private set;
        }

        private static List<Tuple<char, string, string, Object>> Members;
        static TopLevelCommands()
        {
            Members = new List<Tuple<char, string, string, Object>>();

            Members.Add(new Tuple<char, string, string, Object>('a', "add", "Add new alarm", ConstantCommands.ConstantCommandsList.AddAlaram));
            Members.Add(new Tuple<char, string, string, Object>('v', "view", "View all alarms", ConstantCommands.ConstantCommandsList.ViewAlarms));
            Members.Add(new Tuple<char, string, string, Object>('d', "delete", "Delete all alarms", ConstantCommands.ConstantCommandsList.DeleteAlarams));

            CommandsObj = new Commands(Members);
        }
       

    }


    public class ConstantCommands
    {
        public enum ConstantCommandsList { InvalidCommand, Exit, Previous, Next, AddAlaram, ViewAlarms, DeleteAlarams};

        
    }

    public class Commands
    {
        

        

        Dictionary<int, Object> IntShortcut;
        Dictionary<char, Object> CharShortcut;
        Dictionary<string, Object> StringShortcut;
        List<Tuple<char, string, string, Object>> CommandsList;


        
        public Commands(List<Tuple<char, string, string, Object>> commands)
        {

            IntShortcut = new Dictionary<int, Object>();
            CharShortcut = new Dictionary<char, Object>();
            StringShortcut = new Dictionary<string, Object>();

            int i = 1;
            foreach (var t in commands)
            {
                IntShortcut.Add(i, t.Item4);
                CharShortcut.Add(t.Item1, t.Item4);
                StringShortcut.Add(t.Item2, t.Item4);
                i++;
            }
            
            
            CommandsList = commands;
        }
        public string GeneratePrompt()
        {
            StringBuilder output = new StringBuilder();

            int i = 1;
            foreach(var t in CommandsList)
            {
                output.Append(string.Format("[**{0}|{1}|{2}**]\t{3}\r\n\r\n", i, t.Item1, t.Item2, t.Item3));
                i++;
            }

            output.Append("[**e**] Exit [**p**] Previous [**n**] Next\n\n");

            return output.ToString();
        }

        public bool Validate(string response, out Object outputCommand)
        {

            int iVal;
            char cVal;

            if(int.TryParse(response, out iVal))
            {
                if(IntShortcut.ContainsKey(iVal))
                {
                    outputCommand = IntShortcut[iVal];
                    return true;
                }

                outputCommand = ConstantCommands.ConstantCommandsList.InvalidCommand;
                return false;
            }

            if(char.TryParse(response, out cVal))
            {
                if(cVal == 'e')
                {
                    outputCommand = ConstantCommands.ConstantCommandsList.Exit;
                    return true;
                }
                else if(cVal == 'p')
                {
                    outputCommand = ConstantCommands.ConstantCommandsList.Previous;
                    return true;
                }
                else if(CharShortcut.ContainsKey(cVal))
                {
                    outputCommand = CharShortcut[cVal];
                    return true;
                }
                outputCommand = ConstantCommands.ConstantCommandsList.InvalidCommand;
                return false;
            }


            if(StringShortcut.ContainsKey(response))
            {
                outputCommand = StringShortcut[response];
                return true;
            }

            outputCommand = ConstantCommands.ConstantCommandsList.InvalidCommand;
              
            return false;
        }

    }

    public class PaginationCommands
    {
        int PageOffset;

        

        List<Tuple<string, Object>> Values;

        Dictionary<int, Object> IntShortcut;

        public PaginationCommands(List<Tuple<string, Object>> values, int pageOffset)
        {

            IntShortcut = new Dictionary<int, object>();

            int i = 0;
            foreach(var t in values)
            {
                IntShortcut.Add(i, t.Item2);
                i++;    
            }

            PageOffset = pageOffset;

            Values = values;
        }

        public bool Validate(string response, Object output)
        {
            int ival;
            char cVal;
            if(int.TryParse(response, out ival))
            {
                if(IntShortcut.ContainsKey(ival))
                {
                    output = IntShortcut[ival];
                    return true;
                }

                output = ConstantCommands.ConstantCommandsList.InvalidCommand;
                return false;
            }

            
            if(char.TryParse(response, out cVal))
            {
                if(cVal == 'e')
                {
                    output = ConstantCommands.ConstantCommandsList.Exit;
                    return true;
                }
                else if(cVal == 'p')
                {
                    output = ConstantCommands.ConstantCommandsList.Previous;
                    return true;
                }
                else if(cVal == 'n')
                {
                    output = ConstantCommands.ConstantCommandsList.Next;
                    return true;
                }
                else
                {
                    output = ConstantCommands.ConstantCommandsList.InvalidCommand;
                    return false;
                }
            }
            else
            {
                output = ConstantCommands.ConstantCommandsList.InvalidCommand;
                return false;
            }

            

        }

        public string GeneratePrompt()
        {
            StringBuilder output = new StringBuilder();

            output.Append(string.Format("**page#{0}**\n\n", PageOffset.ToString()));
            int i = 1;
            
            foreach(var t in Values)

            {
                output.Append(string.Format("{0}. {1}\n\n", i, t.Item1));
                i++;

            }

            output.Append("[**e**] Exit [**p**] Previous [**n**] Next\n\n");

            return output.ToString();
        }
    }
}