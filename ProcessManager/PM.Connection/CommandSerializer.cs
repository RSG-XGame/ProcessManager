using PM.Connection.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PM.Connection
{
    public static class CommandSerializer
    {
        public static byte[] SerializeCommand(this CommandBase command)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, command);
                return stream.ToArray();
            }
        }

        public static CommandBase DeserializeCommand(this byte[] data)
        {
            CommandBase result;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                try
                {
                    result = (CommandBase)binaryFormatter.Deserialize(stream);
                }
                catch
                {
                    throw new InvalidCastException("не удалось десериализовать массив байт в команду!");
                }
            }
            return result;
        }
    }
}
