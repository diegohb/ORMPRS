// *************************************************
// MMG.Core.Testing.UnitTests.Class1.cs
// Last Modified: 11/07/2013 6:44 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;
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
    public class ADTTests
    {
        class TestIntegerADT : ADTBase<int>
        {
            public TestIntegerADT(int pValue) : base(pValue) { }

            public override int Value { get; set; }
        }

        [Test]
        public void ADTConverter_ShouldConvertToUnderlyingType()
        {
            var adt = new TestIntegerADT(5);
            var typeConverter = TypeDescriptor.GetConverter(typeof(TestIntegerADT));
            Assert.IsNotNull(typeConverter);
            var actualValue = Convert.ToInt32(adt);
            //var actualValue = typeConverter.ConvertTo(adt, typeof(int)) as int?;
            //Assert.True(actualValue);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.AreEqual(5, actualValue);
        }

    }
}