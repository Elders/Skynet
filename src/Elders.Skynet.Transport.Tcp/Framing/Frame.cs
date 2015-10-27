using System;
using System.IO;

namespace Elders.Skynet.Transport.Tcp.Framing
{
    public class Frame
    {
        static Guid BeginFrameGuid = Guid.Parse("8ec9c605-5ce9-4c6c-bee8-47fd4098e2f5");

        static byte[] BeginFrame = Guid.Parse("8ec9c605-5ce9-4c6c-bee8-47fd4098e2f5").ToByteArray();

        static int int32size = sizeof(int);

        static int FrameHeadSize = BeginFrame.Length + (2 * sizeof(int));

        private MemoryStream frameStream = new MemoryStream();

        public int FrameLenght { get; private set; }

        public bool IsValid { get; private set; }

        public FrameType Type { get; private set; }

        public bool IsCompleted { get { return frameStream.Length == FrameLenght && FrameLenght > 0 && IsValid == true; } }

        public Frame()
        {
            IsValid = true;
            FrameLenght = 0;
        }

        public Frame(FrameType frameType, byte[] bytes)
        {
            Type = frameType;
            IsValid = true;
            FrameLenght = FrameHeadSize + bytes.Length;

            frameStream.Write(BeginFrame, 0, BeginFrame.Length);

            var typeBytes = BitConverter.GetBytes((int)frameType);
            frameStream.Write(typeBytes, 0, typeBytes.Length);

            var frameLenght = BitConverter.GetBytes(bytes.Length);
            frameStream.Write(frameLenght, 0, frameLenght.Length);

            frameStream.Write(bytes, 0, bytes.Length);
        }

        public void Read(Stream stream)
        {
            long beginPosition = stream.Position;
            if (FrameLenght == 0)
            {
                var leftForHead = FrameHeadSize - (int)frameStream.Position;
                frameStream.TryWrite(stream, leftForHead);
                InterprateHead();
            }
            if (IsValid == false)
            {
                stream.Position = beginPosition;
                return;
            }
            if (FrameLenght > 0 && frameStream.Position < FrameLenght)
            {
                var leftForBody = FrameLenght - frameStream.Position;
                frameStream.TryWrite(stream, (int)leftForBody);
            }
        }

        private void InterprateHead()
        {
            long initStreamPosition = frameStream.Position;
            if (frameStream.Position >= FrameHeadSize && FrameLenght == 0)
            {
                frameStream.Position = 0;
                var start = new byte[BeginFrame.Length];

                frameStream.Read(start, 0, start.Length);
                var gd = new Guid(start);

                if (gd == BeginFrameGuid)
                    IsValid = true;
                else
                    IsValid = false;

                var type = new byte[int32size];
                frameStream.Read(type, 0, type.Length);
                Type = (FrameType)BitConverter.ToInt32(type, 0);

                var bodyLenght = new byte[int32size];
                frameStream.Read(bodyLenght, 0, bodyLenght.Length);
                FrameLenght = FrameHeadSize + BitConverter.ToInt32(bodyLenght, 0);
            }
            else
            {
                var currentBytes = new byte[FrameHeadSize];
                frameStream.Position = 0;
                var toRead = frameStream.Length < FrameHeadSize ? (int)frameStream.Length : FrameHeadSize;
                frameStream.Read(currentBytes, 0, toRead);
                for (int i = 0; i < BeginFrame.Length; i++)
                {
                    if (currentBytes[i] != BeginFrame[i])
                    {
                        IsValid = false;
                        break;
                    }
                }
            }
            frameStream.Position = initStreamPosition;
        }

        public byte[] ToArray()
        {
            if (IsCompleted == false)
                throw new InvalidOperationException("The frame is not completed. Can not convert to byte array.");
            return frameStream.ToArray();
        }

        public byte[] ReadPayload()
        {
            if (IsCompleted == false)
                throw new InvalidOperationException("The frame is not completed. Can not get body.");
            var init = frameStream.Position;
            frameStream.Position = FrameHeadSize;
            var payload = new byte[frameStream.Length - FrameHeadSize];
            frameStream.Read(payload, 0, payload.Length);
            frameStream.Position = init;
            return payload;
        }
    }
}