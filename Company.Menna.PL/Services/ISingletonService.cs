﻿namespace Company.Menna.PL.Services
{
    public interface ISingletonService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
