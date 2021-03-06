﻿// *************************************************
// MMG.Core.Testing.Integration.Contact.cs
// Last Modified: 08/31/2013 4:04 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    public class Contact : IDbEntity
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public virtual Address Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }
}