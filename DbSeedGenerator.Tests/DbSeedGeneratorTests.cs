using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DbSeedGenerator.Tests
{
    public sealed class DbSeedGeneratorTests
    {
        internal sealed class TypeWithoutKey
        {
            public string Name { get; set; }
        }
        
        [Fact]
        public void ShouldBeThrowKeyNotFoundExeptionIfKeyPropertyIsntExists()
        {
            var ex = Record.Exception(new Assert.ThrowsDelegate(() => DbSeedGenerator.GenCode(new TypeWithoutKey { Name = "Test" })));
            Assert.IsType<KeyNotFoundException>(ex);
            Assert.Equal("Cannot find key for entity TypeWithoutKey", ex.Message);
        }

        internal struct UnknownStruct
        {
            public int Id { get; set; }
        }

        internal class TypeWithUnknownValueType
        {
            [Key]
            public int Id { get; set; }
            public UnknownStruct Value { get; set; }
        }

        [Fact]
        public void ShouldBeThrowInvalidOperationExeptionForUnknownValueType()
        {
            var ex = Record.Exception(new Assert.ThrowsDelegate(() => DbSeedGenerator.GenCode(new TypeWithUnknownValueType { Value = new UnknownStruct() })));
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(@"Unknown primitive or value type ""UnknownStruct"".", ex.Message);
        }

        internal sealed class TypeWithDiscardAttribute
        {
            [Key]
            public int Id { get; set; }
            [Discard]
            public string DiscardValue { get; set; }
        }

        [Fact]
        public void ShouldAvoidCodeGenerationForPropertiesWithDiscardAttribute()
        {
            var result = DbSeedGenerator.GenCode(new TypeWithDiscardAttribute { Id = 1});
            Assert.Equal(1, result.Count);
            Assert.Equal("var typewithdiscardattribute_1 = new TypeWithDiscardAttribute {Id = 1};", result[0]);

        }

        internal sealed class IntContainer
        {
            [Key]
            public int Id { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForInt()
        {
            var result = DbSeedGenerator.GenCode(new IntContainer { Id = 1 });
            Assert.Equal(1, result.Count);
            Assert.Equal("var intcontainer_1 = new IntContainer {Id = 1};", result[0]);
        }

        internal sealed class BoolContainer
        {
            [Key]
            public int Id { get; set; }
            public bool Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForBool()
        {
            var result = DbSeedGenerator.GenCode(new BoolContainer { Id = 1, Value = true });
            Assert.Equal(1, result.Count);
            Assert.Equal("var boolcontainer_1 = new BoolContainer {Id = 1, Value = true};", result[0]);
        }

        internal sealed class GuidContainer
        {
            [Key]
            public int Id { get; set; }
            public Guid Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForGuid()
        {
            var result = DbSeedGenerator.GenCode(new GuidContainer { Id = 1, Value = new Guid("e2f0b4a8-4b5b-4d42-bff6-86b5a5df1ff6") });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var guidcontainer_1 = new GuidContainer {Id = 1, Value = new Guid(""e2f0b4a8-4b5b-4d42-bff6-86b5a5df1ff6"")};", result[0]);
        }

        internal sealed class StringContainer
        {
            [Key]
            public int Id { get; set; }
            public String Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForString()
        {
            var result = DbSeedGenerator.GenCode(new StringContainer { Id = 1, Value = "string" });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var stringcontainer_1 = new StringContainer {Id = 1, Value = @""string""};", result[0]);
        }

        [Fact]
        public void ShouldBeCorrectEscapeStringWithQutationMark()
        {
            var result = DbSeedGenerator.GenCode(new StringContainer { Id = 1, Value = @"s""tring" });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var stringcontainer_1 = new StringContainer {Id = 1, Value = @""s""""tring""};", result[0]);
        }
        
        internal sealed class DateTimeContainer
        {
            [Key]
            public int Id { get; set; }
            public DateTime Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForDateTime()
        {
            var result = DbSeedGenerator.GenCode(new DateTimeContainer { Id = 1, Value = new DateTime(2014, 1, 2, 3, 4, 5) });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var datetimecontainer_1 = new DateTimeContainer {Id = 1, Value = new DateTime(2014, 1, 2, 3, 4, 5)};", result[0]);
        }

        internal sealed class DecimalContainer
        {
            [Key]
            public int Id { get; set; }
            public decimal Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForDecimal()
        {
            var result = DbSeedGenerator.GenCode(new DecimalContainer { Id = 1, Value = 10.12m });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var decimalcontainer_1 = new DecimalContainer {Id = 1, Value = 10.12m};", result[0]);
        }

        internal sealed class DoubleContainer
        {
            [Key]
            public int Id { get; set; }
            public double Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForDouble()
        {
            var result = DbSeedGenerator.GenCode(new DoubleContainer { Id = 1, Value = 10.12 });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var doublecontainer_1 = new DoubleContainer {Id = 1, Value = 10.12};", result[0]);
        }

        internal sealed class FloatContainer
        {
            [Key]
            public int Id { get; set; }
            public float Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForFloat()
        {
            var result = DbSeedGenerator.GenCode(new FloatContainer { Id = 1, Value = 10.12f });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var floatcontainer_1 = new FloatContainer {Id = 1, Value = 10.12f};", result[0]);
        }

        internal sealed class NullableBoolContainer
        {
            [Key]
            public int Id { get; set; }
            public bool? Value { get; set; }
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForNullableTypeWhenPropertyHasNotValue()
        {
            var result = DbSeedGenerator.GenCode(new NullableBoolContainer { Id = 1, Value = null });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var nullableboolcontainer_1 = new NullableBoolContainer {Id = 1, Value = null};", result[0]);
        }

        [Fact]
        public void ShouldBeCorrectGenerateCodeForNullableTypeWhenPropertyHasValue()
        {
            var result = DbSeedGenerator.GenCode(new NullableBoolContainer { Id = 1, Value = true });
            Assert.Equal(1, result.Count);
            Assert.Equal(@"var nullableboolcontainer_1 = new NullableBoolContainer {Id = 1, Value = true};", result[0]);
        }

        [Fact]
        public void ShouldBeGenerateValidCodeForSingleObject()
        {
            var topItem = new TopItem { Id = 1, Name = "I'm TopItem" };

            var subItem1 = new SubItem1 { Id = 2, Name = "I'm SubItem1", TopItem = topItem };

            var subItem2Type = new SubItem2Type { Id = 3, Name = @"This is ""SubItem2""" };
            var subItem2 = new SubItem2
            {
                Id = 4,
                ParentSubItem = subItem1,
                Name = "I'm Parent",
                Type = subItem2Type
            };

            var subItem3Type = new SubItem3Type { Id = 5, Name = @"This is ""SubItem3""" };
            var subItem3 = new SubItem3 { Id = 6, Name = "I'm SubItem3", Type = subItem3Type, Parent = subItem2 };

            var subItem4Type = new SubItem4Type { Id = 7, Name = @"This is ""SubItem4""" };
            var subItem4 = new SubItem4 { Id = 8, SubItem3 = subItem3, SubItem4Type = subItem4Type, Name = "I'm SubItem4" };

            var result = DbSeedGenerator.GenCode(subItem4);
            Assert.Equal(8, result.Count);
            Assert.Equal(@"var subitem2type_3 = new SubItem2Type {Id = 3, Name = @""This is """"SubItem2""""""};", result[0]);
            Assert.Equal(@"var topitem_1 = new TopItem {Id = 1, Name = @""I'm TopItem""};", result[1]);
            Assert.Equal(@"var subitem1_2 = new SubItem1 {Id = 2, Name = @""I'm SubItem1"", TopItem = topitem_1};", result[2]);
            Assert.Equal(@"var subitem2_4 = new SubItem2 {Id = 4, Name = @""I'm Parent"", ParentSubItem = subitem1_2, Type = subitem2type_3};", result[3]);
            Assert.Equal(@"var subitem3type_5 = new SubItem3Type {Id = 5, Name = @""This is """"SubItem3""""""};", result[4]);
            Assert.Equal(@"var subitem3_6 = new SubItem3 {Id = 6, Name = @""I'm SubItem3"", Type = subitem3type_5, Parent = subitem2_4};", result[5]);
            Assert.Equal(@"var subitem4type_7 = new SubItem4Type {Id = 7, Name = @""This is """"SubItem4""""""};", result[6]);
            Assert.Equal(@"var subitem4_8 = new SubItem4 {Id = 8, Name = @""I'm SubItem4"", SubItem4Type = subitem4type_7, SubItem3 = subitem3_6};", result[7]);
        }
        
        [Fact]
        public void ShouldBeGenerateValidCodeForMultipleObjects()
        {
            var topItem = new TopItem { Id = 1, Name = "I'm TopItem" };

            var subItem1 = new SubItem1 { Id = 2, Name = "I'm SubItem1", TopItem = topItem };

            var subItem2Type = new SubItem2Type { Id = 3, Name = @"This is ""SubItem2""" };
            var subItem2 = new SubItem2
            {
                Id = 4,
                ParentSubItem = subItem1,
                Name = "I'm Parent",
                Type = subItem2Type
            };

            var subItem3Type = new SubItem3Type { Id = 5, Name = @"This is ""SubItem3""" };
            var subItem3 = new SubItem3 { Id = 6, Name = "I'm SubItem3", Type = subItem3Type, Parent = subItem2 };

            var subItem4Type = new SubItem4Type { Id = 7, Name = @"This is ""SubItem4""" };
            var objectList = new List<object>
            {
                new SubItem4
                {
                    Id = 8,
                    SubItem3 = subItem3,
                    SubItem4Type = subItem4Type,
                    Name = "I'm SubItem4"
                },
                new SubItem4
                {
                    Id = 9,
                    SubItem3 = subItem3,
                    SubItem4Type = subItem4Type,
                    Name = "I'm SubItem4"
                },
                new SubItem4
                {
                    Id = 10,
                    SubItem3 = subItem3,
                    SubItem4Type = subItem4Type,
                    Name = "I'm SubItem4"
                }
            };


            var result = DbSeedGenerator.GenCode(objectList);

            Assert.Equal(10, result.Count);
            Assert.Equal(@"var subitem2type_3 = new SubItem2Type {Id = 3, Name = @""This is """"SubItem2""""""};", result[0]);
            Assert.Equal(@"var topitem_1 = new TopItem {Id = 1, Name = @""I'm TopItem""};", result[1]);
            Assert.Equal(@"var subitem1_2 = new SubItem1 {Id = 2, Name = @""I'm SubItem1"", TopItem = topitem_1};", result[2]);
            Assert.Equal(@"var subitem2_4 = new SubItem2 {Id = 4, Name = @""I'm Parent"", ParentSubItem = subitem1_2, Type = subitem2type_3};", result[3]);
            Assert.Equal(@"var subitem3type_5 = new SubItem3Type {Id = 5, Name = @""This is """"SubItem3""""""};", result[4]);
            Assert.Equal(@"var subitem3_6 = new SubItem3 {Id = 6, Name = @""I'm SubItem3"", Type = subitem3type_5, Parent = subitem2_4};", result[5]);
            Assert.Equal(@"var subitem4type_7 = new SubItem4Type {Id = 7, Name = @""This is """"SubItem4""""""};", result[6]);
            Assert.Equal(@"var subitem4_8 = new SubItem4 {Id = 8, Name = @""I'm SubItem4"", SubItem4Type = subitem4type_7, SubItem3 = subitem3_6};", result[7]);
            Assert.Equal(@"var subitem4_9 = new SubItem4 {Id = 9, Name = @""I'm SubItem4"", SubItem4Type = subitem4type_7, SubItem3 = subitem3_6};", result[8]);
            Assert.Equal(@"var subitem4_10 = new SubItem4 {Id = 10, Name = @""I'm SubItem4"", SubItem4Type = subitem4type_7, SubItem3 = subitem3_6};", result[9]);
        }
    }
}
