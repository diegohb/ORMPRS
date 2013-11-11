// *************************************************
// MMG.Core.Testing.UnitTests.ADTStringEnumTests.cs
// Last Modified: 11/11/2013 9:51 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using MMG.Common.Extensions;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    [TestFixture]
    public class ADTStringEnumTests
    {
        private enum MyEnum
        {
            [System.ComponentModel.Description("Choice 1")] Choice1,
            [System.ComponentModel.Description("Choice 2")] Choice2,
            [System.ComponentModel.Description("Choice 3")] Choice3
        }

        private class StringEnumADT : ADTStringEnum<MyEnum>
        {
            #region Constructors

            public StringEnumADT(MyEnum pValue) : base(pValue) {}

            public StringEnumADT(string pValue) : base(pValue) {}

            #endregion

            public override string Value
            {
                get { return _underlyingValue; }
                set { setValue(value); }
            }

            #region Operators

            public static implicit operator StringEnumADT(MyEnum pValue)
            {
                return new StringEnumADT(pValue);
            }

            public static implicit operator MyEnum(StringEnumADT pEnumTypeADT)
            {
                return pEnumTypeADT.EnumValue;
            }

            public static implicit operator string(StringEnumADT pEnumTypeADT)
            {
                return pEnumTypeADT.Value;
            }

            public static implicit operator StringEnumADT(string pEnumDescription)
            {
                return new StringEnumADT(pEnumDescription);
            }

            #endregion

            protected override string convertEnumValueToUnderlyingValue(MyEnum pEnumValue)
            {
                return pEnumValue.ToDescriptionString();
            }

            protected override MyEnum convertUnderlyingValueToEnumValue(string pValue)
            {
                return pValue.ToEnum<MyEnum>();
            }
        }

        [Test]
        public void ShouldCreateToProperTypeFromDescriptionString()
        {
            const string expectedValueString = "Choice 1";
            const MyEnum expectedValueEnum = MyEnum.Choice1;
            var adt = new StringEnumADT(expectedValueString);
            Assert.IsInstanceOf<string>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedValueString, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void DefaultImplementationShouldCreateToProperTypeFromMemberNameString()
        {
            const string memberName = "Choice3";
            const string expectedValueString = "Choice3";
            const MyEnum expectedValueEnum = MyEnum.Choice3;
            var adt = new ADTStringEnum<MyEnum>(memberName);
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
            var adt = new StringEnumADT(memberName);
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
            var adt = new StringEnumADT(expectedValueEnum);
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
            var adt = new StringEnumADT(expectedInitialValueEnum);
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
            var adt = new StringEnumADT(expectedInitialValueEnum);
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
            var adt = new StringEnumADT(expectedValue);
            var actualValue = Convert.ToString(adt);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingTypeString()
        {
            const string underlyingValue = "Choice 3";
            StringEnumADT actualValue = underlyingValue;
            ADT<string> actualValueFromBase = underlyingValue;
            Assert.IsInstanceOf<string>(underlyingValue);
            Assert.IsAssignableFrom<StringEnumADT>(actualValueFromBase);
            var expectedValue = new StringEnumADT(underlyingValue);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(expectedValue.Equals(actualValueFromBase));
            Assert.True(MyEnum.Choice3 == actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingTypeEnum()
        {
            const MyEnum underlyingValue = MyEnum.Choice3;
            StringEnumADT actualValue = underlyingValue;
            Assert.IsInstanceOf<MyEnum>(underlyingValue);
            var expectedValue = new StringEnumADT(underlyingValue);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(MyEnum.Choice3 == actualValue);
        }

        [Test]
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const string expectedValue = "Choice 3";
            var adt = new StringEnumADT(expectedValue);
            Assert.AreEqual(expectedValue, adt);
            Assert.True(MyEnum.Choice3 == adt);
        }
    }
}