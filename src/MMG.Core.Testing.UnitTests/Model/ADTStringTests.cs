// *************************************************
// MMG.Core.Testing.UnitTests.ADTStringTests.cs
// Last Modified: 11/08/2013 10:02 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    [TestFixture]
    public class ADTStringTests
    {
        private class StringADT : ADT<string>
        {
            public StringADT(string pValue) : base(pValue) {}

            public override string Value { get; set; }

            public static implicit operator StringADT(string pValue)
            {
                return new StringADT(pValue);
            }
        }

        [Test]
        public void ShouldCreateToProperType()
        {
            const string expectedValue = "test_me_well";
            var adt = new StringADT(expectedValue);
            Assert.IsInstanceOf<string>(adt.Value);
            Assert.AreEqual(expectedValue, adt.Value);
        }

        [Test]
        public void ShouldConvertToUnderlyingType()
        {
            const string expectedValue = "test_me_well";
            var adt = new StringADT(expectedValue);
            var actualValue = Convert.ToString(adt);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingType()
        {
            const string underlyingValue = "test_me_well";
            StringADT actualValue = underlyingValue;
            ADT<string> actualValueFromBase = underlyingValue;
            Assert.IsInstanceOf<string>(underlyingValue);
            Assert.IsAssignableFrom<StringADT>(actualValueFromBase);
            var expectedValue = new StringADT(underlyingValue);
            Assert.AreEqual(expectedValue.Value, actualValue.Value);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(expectedValue.Equals(actualValueFromBase));
        }

        [Test]
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const string expectedValue = "test_me_well";
            var adt = new StringADT(expectedValue);
            Assert.AreEqual(expectedValue, adt);
        }
    }
}