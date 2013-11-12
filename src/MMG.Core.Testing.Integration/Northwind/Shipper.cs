// *************************************************
// MMG.Core.Testing.Integration.Shipper.cs
// Last Modified: 11/12/2013 7:34 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel.DataAnnotations;
using MMG.Core.Persistence;

namespace MMG.Core.Testing.Integration.Northwind
{
    [DisplayColumn("Name")]
    public class Shipper : IDbEntity, IEquatable<Shipper>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public virtual PriorityTypeADT PriorityLevel { get; set; }

        public bool Equals(Shipper other)
        {
            return Id.Equals(other.Id);
        }
    }


}