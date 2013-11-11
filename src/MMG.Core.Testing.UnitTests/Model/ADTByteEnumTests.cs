// *************************************************
// MMG.Core.Testing.UnitTests.ADTByteEnumTests.cs
// Last Modified: 11/11/2013 10:48 AM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using MMG.Core.Model;
using NUnit.Framework;

namespace MMG.Core.Testing.UnitTests.Model
{
    [TestFixture]
    public class ADTByteEnumTests
    {
        private enum MyEnum : byte
        {
            Choice10 = 10,
            Choice20 = 20,
            Choice30 = 30
        }


        [Test]
        public void ShouldCreateToProperTypeFromUnderlyingValue()
        {
            const byte expectedByteValue = 20;
            const MyEnum expectedValueEnum = MyEnum.Choice20;
            var adt = new ByteEnumADT<MyEnum>(expectedByteValue);
            Assert.IsInstanceOf<byte>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedByteValue, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }
        [Test]
        public void ShouldCreateToProperTypeFromMemberNameString()
        {
            const string memberName = "Choice30";
            const byte expectedValueString = 30;
            const MyEnum expectedValueEnum = MyEnum.Choice30;
            var adt = new ByteEnumADT<MyEnum>(memberName);
            Assert.IsInstanceOf<byte>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedValueString, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void ShouldCreateToProperTypeFromEnum()
        {
            const byte expectedByteValue = 10;
            const MyEnum expectedValueEnum = MyEnum.Choice10;
            var adt = new ByteEnumADT<MyEnum>(expectedValueEnum);
            Assert.IsInstanceOf<byte>(adt.Value);
            Assert.IsInstanceOf<MyEnum>(adt.EnumValue);
            Assert.AreEqual(expectedByteValue, adt.Value);
            Assert.AreEqual(expectedValueEnum, adt.EnumValue);
        }

        [Test]
        public void SetValueDescriptionStringShouldChangeEnumValue()
        {
            const byte expectedByteValue = 20;
            const MyEnum expectedChangedValueEnum = MyEnum.Choice20;
            const MyEnum expectedInitialValueEnum = MyEnum.Choice10;
            var adt = new ByteEnumADT<MyEnum>(expectedInitialValueEnum);
            Assert.AreEqual(expectedInitialValueEnum, adt.EnumValue);
            adt.Value = expectedByteValue;
            Assert.AreEqual
                (expectedChangedValueEnum, adt.EnumValue,
                    "If Value is overriden at derived type, you shoudl call into setStringValue or implement setting _enumValue yourself.");
        }

        [Test]
        public void SetValueMemberNameStringShouldChangeEnumValue()
        {
            const byte changedByteValue = 20;
            const MyEnum expectedChangedValueEnum = MyEnum.Choice20;
            const MyEnum expectedInitialValueEnum = MyEnum.Choice10;
            var adt = new ByteEnumADT<MyEnum>(expectedInitialValueEnum);
            Assert.AreEqual(expectedInitialValueEnum, adt.EnumValue);
            adt.Value = changedByteValue;
            Assert.AreEqual
                (expectedChangedValueEnum, adt.EnumValue,
                    "If Value is overriden at derived type, you shoudl call into setStringValue or implement setting _enumValue yourself.");
        }

        [Test]
        public void ShouldConvertToUnderlyingType()
        {
            const byte expectedValue = 30;
            var adt = new ByteEnumADT<MyEnum>(expectedValue);
            var actualValue = Convert.ToByte(adt);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingTypeString()
        {
            const byte underlyingValue = 30;
            ByteEnumADT<MyEnum> actualValue = underlyingValue;
            ADT<byte> actualValueFromBase = underlyingValue;
            Assert.IsInstanceOf<byte>(underlyingValue);
            Assert.IsAssignableFrom<ByteEnumADT<MyEnum>>(actualValueFromBase);
            var expectedValue = new ByteEnumADT<MyEnum>(underlyingValue);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(expectedValue.Equals(actualValueFromBase));
            Assert.True(MyEnum.Choice30 == actualValue);
        }

        [Test]
        public void ShouldConvertFromUnderlyingTypeEnum()
        {
            const MyEnum underlyingValue = MyEnum.Choice30;
            ByteEnumADT<MyEnum> actualValue = underlyingValue;
            Assert.IsInstanceOf<MyEnum>(underlyingValue);
            var expectedValue = new ByteEnumADT<MyEnum>(underlyingValue);
            Assert.True(expectedValue.Equals(actualValue));
            Assert.True(MyEnum.Choice30 == actualValue);
        }

        [Test]
        public void ShouldConvertImplicitlyToUnderlyingType()
        {
            const byte expectedValue = 30;
            var adt = new ByteEnumADT<MyEnum>(expectedValue);
            Assert.AreEqual(expectedValue, adt);
            Assert.True(MyEnum.Choice30 == adt);
        }
    }
}