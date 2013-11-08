// *************************************************
// MMG.Core.Testing.UnitTests.StringADTTests.cs
// Last Modified: 11/08/2013 10:02 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    [TestFixture]
    public class StringADTTests
    {
        private class StringADT : ADTBase<string>
        {
            public StringADT(string pValue) : base(pValue) {}

            public override string Value { get; set; }
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
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const string expectedValue = "test_me_well";
            var adt = new StringADT(expectedValue);
            Assert.AreEqual(expectedValue, adt);
        }
    }
}