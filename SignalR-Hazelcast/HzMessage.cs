using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelcast.IO.Serialization;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Hazelcast
{
    public class HzMessage : IPortable
    {
        public ulong Id { get; set; }
        public ScaleoutMessage ScaleoutMessage { get; set; }

        public static byte[] ToBytes(IList<Message> messages)
        {
            using (var ms = new MemoryStream())
            {
                var binaryWriter = new BinaryWriter(ms);

                var scaleoutMessage = new ScaleoutMessage(messages);
                var buffer = scaleoutMessage.ToBytes();

                binaryWriter.Write(buffer.Length);
                binaryWriter.Write(buffer);

                return ms.ToArray();
            }
        }

        public static ScaleoutMessage FromBytes(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var message = new HzMessage();

                // read message id from memory stream until SPACE character
                var messageIdBuilder = new StringBuilder(20);
                do
                {
                    // it is safe to read digits as bytes because they encoded by single byte in UTF-8
                    int charCode = stream.ReadByte();
                    if (charCode == -1)
                    {
                        Console.Out.WriteLine("Received Message could not be parsed.");
                        throw new EndOfStreamException();
                    }
                    char c = (char) charCode;
                    if (c == ' ')
                    {
                        message.Id = ulong.Parse(messageIdBuilder.ToString(), CultureInfo.InvariantCulture);
                        messageIdBuilder = null;
                    }
                    else
                    {
                        messageIdBuilder.Append(c);
                    }
                } while (messageIdBuilder != null);

                var binaryReader = new BinaryReader(stream);
                int count = binaryReader.ReadInt32();
                byte[] buffer = binaryReader.ReadBytes(count);

               return ScaleoutMessage.FromBytes(buffer);
            }
        }

        public int GetClassId()
        {
            return HzConfiguration.ClassId;
        }

        public int GetFactoryId()
        {
            return HzConfiguration.FactoryId;
        }

        public void ReadPortable(IPortableReader reader)
        {
            Id = (ulong)reader.ReadLong("id");
            ScaleoutMessage = ScaleoutMessage.FromBytes(reader.ReadByteArray("m"));
        }

        public void WritePortable(IPortableWriter writer)
        {
            writer.WriteLong("id", (long)Id);
            writer.WriteByteArray("m", ScaleoutMessage.ToBytes());
        }
    }
}
