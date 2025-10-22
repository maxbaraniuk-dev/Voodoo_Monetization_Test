using System;

namespace Infrastructure
{
    public interface ISystem : IDisposable
    {
        void Initialize();
    }
}