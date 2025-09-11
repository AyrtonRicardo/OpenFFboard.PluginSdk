using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using User.PluginSdkDemo.DTO;

namespace User.PluginSdkDemo.Utils
{
    internal class ProfileToSerialConverter
    {
        internal OpenFFBoard.Commands.FX fx = null;
        internal OpenFFBoard.Commands.FX axis = null;

        internal string profileJson = "{\r\n            \"name\": \"acc\",\r\n            \"data\": [\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"filterCfFreq\",\r\n                    \"value\": 500\r\n                },\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"filterCfQ\",\r\n                    \"value\": 50\r\n                },\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"spring\",\r\n                    \"value\": 0\r\n                },\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"friction\",\r\n                    \"value\": 0\r\n                },\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"damper\",\r\n                    \"value\": 0\r\n                },\r\n                {\r\n                    \"fullname\": \"Effects\",\r\n                    \"cls\": \"fx\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"inertia\",\r\n                    \"value\": 0\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"power\",\r\n                    \"value\": 32767\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"degrees\",\r\n                    \"value\": 900\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"fxratio\",\r\n                    \"value\": 161\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"esgain\",\r\n                    \"value\": 200\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"idlespring\",\r\n                    \"value\": 127\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"axisdamper\",\r\n                    \"value\": 25\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"axisfriction\",\r\n                    \"value\": 12\r\n                },\r\n                {\r\n                    \"fullname\": \"Axis\",\r\n                    \"cls\": \"axis\",\r\n                    \"instance\": 0,\r\n                    \"cmd\": \"axisinertia\",\r\n                    \"value\": 5\r\n                }\r\n            ]\r\n        }";

        /**
         * Let's map the JSON "Cls" key to the actual command class types.
         */
        private static Func<bool> MapCommand(ProfileData data, OpenFFBoard.Board board)
        {
            if (data.Cls == "fx")
            {
                switch (data.Cmd)
                {
                    case "filterCfFreq":
                        return () => board.FX.SetFiltercffreq((ushort)data.Value);
                    case "filterCfQ":
                        return () => board.FX.SetFiltercfq((byte)data.Value);
                    case "spring":
                        return () => board.FX.SetSpring((byte)data.Value);
                    case "friction":
                        return () => board.FX.SetFriction((byte)data.Value);
                    case "damper":
                        return () => board.FX.SetDamper((byte)data.Value);
                    case "inertia":
                        return () => board.FX.SetInertia((byte)data.Value);
                    default:
                        SimHub.Logging.Current.Error($"Unknown FX instance: {data.Instance}");
                        break;
                }
            }
            
            if (data.Cls == "axis")
            {
                switch (data.Cmd)
                {
                    case "power":
                        return () => board.Axis.SetPower((ushort)data.Value);
                    case "degrees":
                        return () => board.Axis.SetDegrees((ushort)data.Value);
                    case "fxratio":
                        return () => board.Axis.SetFxratio((byte)data.Value);
                    case "esgain":
                        return () => board.Axis.SetEsgain((byte)data.Value);
                    case "idlespring":
                        return () => board.Axis.SetIdlespring((byte)data.Value);
                    case "axisdamper":
                        return () => board.Axis.SetAxisdamper((byte)data.Value);
                    /* case "axisfriction":
                        return () => board.Axis.SetAxisfriction((byte)data.Value);
                    case "axisinertia":
                        return () => board.Axis.SetAxisinertia((byte)data.Value); */
                    default:
                        SimHub.Logging.Current.Error($"Unknown AXIS instance: {data.Instance}");
                        break;
                }
            }

            return null;
        }

        /**
         * Returns a list of commands to execute based on the provided profile.
         * The sequence is respecting the order saved in the profile.
         */
        public static List<Func<bool>> ConvertProfileToCommands(Profile profile, OpenFFBoard.Board board)
        {
            List<Func<bool>> cmdsToReturn = new List<Func<bool>>();
            profile.Data.ForEach(data =>
            {
                Func<bool> cmd = MapCommand(data, board);
                if (cmd == null) {
                    SimHub.Logging.Current.Error($"Failed to map command for {data.Fullname} - {data.Cls} - {data.Cmd}");
                    return;
                }

                cmdsToReturn.Add(cmd);
            });

            return cmdsToReturn;
        }
    }
}
