// *************************************************
// MMG.Core.Testing.UnitTests.ADTEnumTests.cs
// Last Modified: 11/08/2013 3:02 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.ComponentModel;
using MMG.Common.Extensions;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    [TestFixture]
    public class ADTEnumTests
    {
        private enum MyEnum
        {
            [System.ComponentModel.Description("Choice 1")] Choice1,
            [System.ComponentModel.Description("Choice 2")] Choice2,
            [System.ComponentModel.Description("Choice 3")] Choice3
        }

        [TypeConverter(typeof(EnumConverter))]
        private class EnumADT : ADTEnumBase<MyEnum>
        {
            #region Constructors

            public EnumADT(MyEnum pValue) : base(pValue) {}

            public EnumADT(string pValue) : base(pValue) {}

            #endregion

            public override string Value 
            {
                get { return _stringValue; }
                set { setStringValue(value); }
            }

            #region Operators

            public static implicit operator EnumADT(MyEnum pValue)
            {
                return new EnumADT(pValue);
            }

            public static implicit operator MyEnum(EnumADT pEnumTypeADT)
            {
                return pEnumTypeADT.EnumValue;
            }

            public static implicit operator string(EnumADT pEnumTypeADT)
            {
                return pEnumTypeADT.Value;
            }

            public static implicit operator EnumADT(string pEnumDescription)
            {
                return new EnumADT(pEnumDescription);
            }

            #endregion
            
            protected override string convertEnumMemberToStringValue(MyEnum pEnumValue)
            {
                return pEnumValue.ToDescriptionString();
            }

            protected override MyEnum convertStringToEnumValue(string pValue)
            {
                return pValue.ToEnum<MyEnum>();
            }
        }

        [Test]
        public void ShouldCreateToProperTypeFromDescriptionString()
        {
            const string expectedValueString = "Choice 1";
            const MyEnum expectedValueEnum = MyEnum.Choice1;
            var adt = new EnumADT(expectedValueString);
            Assert.IsInstanceOf<string>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedValueString, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void ShouldCreateToProperTypeFromMemberNameString()
        {
            const string memberName = "Choice1";
            const string expectedValueString = "Choice 1";
            const MyEnum expectedValueEnum = MyEnum.Choice1;
            var adt = new EnumADT(memberName);
            Assert.IsInstanceOf<string>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedValueString, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void ShouldCreateToProperTypeFromEnum()
        {
            const string expectedValueString = "Choice 1";
            const MyEnum expectedValueEnum = MyEnum.Choice1;
            var adt = new EnumADT(expectedValueEnum);
            Assert.IsInstanceOf<string>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedValueString, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void SetValueDescriptionStringShouldChangeEnumValue()
        {
            const string changeValueString = "Choice 2";
            const MyEnum expectedChangedValueEnum = MyEnum.Choice2;
            const MyEnum expectedInitialValueEnum = MyEnum.Choice1;
            var adt = new EnumADT(expectedInitialValueEnum);
            Assert.AreEqual(expectedInitialValueEnum, adt.EnumValue);
            adt.Value = changeValueString;
            Assert.AreEqual
                (expectedChangedValueEnum, adt.EnumValue,
                    "If Value is overriden at derived type, you shoudl call into setStringValue or implement setting _enumValue yourself.");
        }

        [Test]
        public void SetValueMemberNameStringShouldChangeEnumValue()
        {
            const string changeValueString = "Choice2";
            const MyEnum expectedChangedValueEnum = MyEnum.Choice2;
            const MyEnum expectedInitialValueEnum = MyEnum.Choice1;
            var adt = new EnumADT(expectedInitialValueEnum);
            Assert.AreEqual(expectedInitialValueEnum, adt.EnumValue);
            adt.Value = changeValueString;
            Assert.AreEqual
                (expectedChangedValueEnum, adt.EnumValue,
                    "If Value is overriden at derived type, you shoudl call into setStringValue or implement setting _enumValue yourself.");
        }

        [Test]
        public void ShouldConvertToUnderlyingType()
        {
            const string expectedValue = "Choice 3";
            var adt = new EnumADT(expectedValue);
            var actualValue = Convert.ToString(adt);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingType()
        {
            const string underlyingValue = "Choice 3";
            EnumADT actualValue = underlyingValue;
            ADT<string> actualValueFromBase = underlyingValue;
            Assert.IsInstanceOf<string>(underlyingValue);
            Assert.IsAssignableFrom<EnumADT>(actualValueFromBase);
            var expectedValue = new EnumADT(underlyingValue);
            Assert.AreEqual(expectedValue.Value, actualValue.Value);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(expectedValue.Equals(actualValueFromBase));
        }

        [Test]
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const string expectedValue = "Choice 3";
            var adt = new EnumADT(expectedValue);
            Assert.AreEqual(expectedValue, adt);
        }
    }
}