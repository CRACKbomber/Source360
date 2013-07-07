using System;
using System.IO;
namespace Source360.IO
{
    public interface ISerializable : IDisposable
    {
        byte[] Serialize();
        bool UnSerialize(IOReader reader);
    }
}
