// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Resolvers
{
    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(13)
            {
                { typeof(global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerAchievementData>), 0 },
                { typeof(global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerTaskData>), 1 },
                { typeof(global::System.Collections.Generic.Dictionary<int, int>), 2 },
                { typeof(global::System.Collections.Generic.Dictionary<uint, global::Game.Data.BuildingData>), 3 },
                { typeof(global::System.Collections.Generic.HashSet<global::Game.Data.FeatureOpen.FeatureType>), 4 },
                { typeof(global::System.Collections.Generic.HashSet<int>), 5 },
                { typeof(global::Game.Data.FeatureOpen.FeatureType), 6 },
                { typeof(global::Game.Data.TaskState), 7 },
                { typeof(global::Game.Data.BuildingData), 8 },
                { typeof(global::Game.Data.PlayerAchievementData), 9 },
                { typeof(global::Game.Data.PlayerData), 10 },
                { typeof(global::Game.Data.PlayerTaskData), 11 },
                { typeof(global::Game.Data.SettingData), 12 },
            };
        }

        internal static object GetFormatter(global::System.Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.DictionaryFormatter<int, global::Game.Data.PlayerAchievementData>();
                case 1: return new global::MessagePack.Formatters.DictionaryFormatter<int, global::Game.Data.PlayerTaskData>();
                case 2: return new global::MessagePack.Formatters.DictionaryFormatter<int, int>();
                case 3: return new global::MessagePack.Formatters.DictionaryFormatter<uint, global::Game.Data.BuildingData>();
                case 4: return new global::MessagePack.Formatters.HashSetFormatter<global::Game.Data.FeatureOpen.FeatureType>();
                case 5: return new global::MessagePack.Formatters.HashSetFormatter<int>();
                case 6: return new MessagePack.Formatters.Game.Data.FeatureOpen.FeatureTypeFormatter();
                case 7: return new MessagePack.Formatters.Game.Data.TaskStateFormatter();
                case 8: return new MessagePack.Formatters.Game.Data.BuildingDataFormatter();
                case 9: return new MessagePack.Formatters.Game.Data.PlayerAchievementDataFormatter();
                case 10: return new MessagePack.Formatters.Game.Data.PlayerDataFormatter();
                case 11: return new MessagePack.Formatters.Game.Data.PlayerTaskDataFormatter();
                case 12: return new MessagePack.Formatters.Game.Data.SettingDataFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1649 // File name should match first type name


// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Game.Data.FeatureOpen
{

    public sealed class FeatureTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.FeatureOpen.FeatureType>
    {
        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.FeatureOpen.FeatureType value, global::MessagePack.MessagePackSerializerOptions options)
        {
            writer.Write((global::System.Int32)value);
        }

        public global::Game.Data.FeatureOpen.FeatureType Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            return (global::Game.Data.FeatureOpen.FeatureType)reader.ReadInt32();
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Game.Data
{

    public sealed class TaskStateFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.TaskState>
    {
        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.TaskState value, global::MessagePack.MessagePackSerializerOptions options)
        {
            writer.Write((global::System.Int32)value);
        }

        public global::Game.Data.TaskState Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            return (global::Game.Data.TaskState)reader.ReadInt32();
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name



// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Game.Data
{
    public sealed class BuildingDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.BuildingData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.BuildingData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(4);
            writer.Write(value.type);
            writer.Write(value.level);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::UnityEngine.Vector2>(formatterResolver).Serialize(ref writer, value.position, options);
            writer.Write(value.rotation);
        }

        public global::Game.Data.BuildingData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::Game.Data.BuildingData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.type = reader.ReadInt32();
                        break;
                    case 1:
                        ____result.level = reader.ReadInt32();
                        break;
                    case 2:
                        ____result.position = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::UnityEngine.Vector2>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 3:
                        ____result.rotation = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class PlayerAchievementDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.PlayerAchievementData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.PlayerAchievementData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(2);
            writer.Write(value.complete);
            writer.Write(value.progress);
        }

        public global::Game.Data.PlayerAchievementData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::Game.Data.PlayerAchievementData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.complete = reader.ReadBoolean();
                        break;
                    case 1:
                        ____result.progress = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class PlayerDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.PlayerData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.PlayerData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(10);
            writer.WriteNil();
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<uint, global::Game.Data.BuildingData>>(formatterResolver).Serialize(ref writer, value.buildings, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerTaskData>>(formatterResolver).Serialize(ref writer, value.tasks, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, int>>(formatterResolver).Serialize(ref writer, value.backpack, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, int>>(formatterResolver).Serialize(ref writer, value.currency, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Game.Data.SettingData>(formatterResolver).Serialize(ref writer, value.settingData, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.HashSet<int>>(formatterResolver).Serialize(ref writer, value.unlockedBuildings, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.DateTime>(formatterResolver).Serialize(ref writer, value.lastLoginTime, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.HashSet<global::Game.Data.FeatureOpen.FeatureType>>(formatterResolver).Serialize(ref writer, value.unlockedFeatures, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerAchievementData>>(formatterResolver).Serialize(ref writer, value.achievementData, options);
        }

        public global::Game.Data.PlayerData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::Game.Data.PlayerData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 1:
                        ____result.buildings = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<uint, global::Game.Data.BuildingData>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 2:
                        ____result.tasks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerTaskData>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 3:
                        ____result.backpack = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, int>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 4:
                        ____result.currency = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, int>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 5:
                        ____result.settingData = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Game.Data.SettingData>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 6:
                        ____result.unlockedBuildings = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.HashSet<int>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 7:
                        ____result.lastLoginTime = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.DateTime>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 8:
                        ____result.unlockedFeatures = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.HashSet<global::Game.Data.FeatureOpen.FeatureType>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 9:
                        ____result.achievementData = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<int, global::Game.Data.PlayerAchievementData>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class PlayerTaskDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.PlayerTaskData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.PlayerTaskData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Game.Data.TaskState>(formatterResolver).Serialize(ref writer, value.state, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<int[]>(formatterResolver).Serialize(ref writer, value.currentNum, options);
        }

        public global::Game.Data.PlayerTaskData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::Game.Data.PlayerTaskData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.state = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Game.Data.TaskState>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 1:
                        ____result.currentNum = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<int[]>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class SettingDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Game.Data.SettingData>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Game.Data.SettingData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(2);
            writer.Write(value.bgmVolume);
            writer.Write(value.soundVolume);
        }

        public global::Game.Data.SettingData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::Game.Data.SettingData();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.bgmVolume = reader.ReadSingle();
                        break;
                    case 1:
                        ____result.soundVolume = reader.ReadSingle();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

