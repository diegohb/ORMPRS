// *************************************************
// MMG.Core.Testing.UnitTests.ADTIntegerTests.cs
// Last Modified: 11/08/2013 10:02 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    /// <summary>
    /// Tests for adt types.
    /// </summary>
    /// <remarks>See more:
    /// http://msdn.microsoft.com/en-us/library/system.componentmodel.typeconverter(v=vs.110).aspx
    /// http://msdn.microsoft.com/en-us/library/98bbex99(v=vs.100).aspx
    /// </remarks>
    [TestFixture]
    public class ADTIntegerTests
    {
        private class IntegerADT : ADT<int>
        {
            public IntegerADT(int pValue) : base(pValue) {}

            public override int Value { get; set; }

            public static implicit operator IntegerADT(int pValue)
            {
                return new IntegerADT(pValue);
            }
        }

        [Test]
        public void ShouldCreateToProperType()
        {
            const int expectedValue = 5;
            var adt = new IntegerADT(expectedValue);
            Assert.IsInstanceOf<int>(adt.Value);
            Assert.AreEqual(expectedValue, adt.Value);
        }

        [Test]
        public void ShouldConvertToUnderlyingType()
        {
            const int expectedValue = 5;
            var adt = new IntegerADT(expectedValue);
            var actualValue = Convert.ToInt32(adt);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingType()
        {
            const int underlyingValue = 10;
            ADT<int> actualValue = underlyingValue;
            ADT<int> actualValueFromBase = underlyingValue;
            Assert.IsInstanceOf<int>(underlyingValue);
            Assert.IsAssignableFrom<IntegerADT>(actualValueFromBase);
            var expectedValue = new IntegerADT(underlyingValue);
            Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(expectedValue, actualValueFromBase);
        }

        [Test]
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const int expectedValue = 5;
            var adt = new IntegerADT(expectedValue);
            Assert.AreEqual(expectedValue, adt);
        }
    }
}