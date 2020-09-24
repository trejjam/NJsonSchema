using NJsonSchema.Generation;
using NJsonSchema.References;
using System.Threading.Tasks;
using Xunit;

namespace NJsonSchema.Tests.Generation
{
    public class EmptyLeafTests
    {
        public class Parent
        {
            public string Name { get; set; }
        }

        public class Child : Parent
        {
        }

        public class ChildOfAChild : Child
        {
        }

        [Fact]
        public async Task When_Object_does_not_have_properties_generate_single_allOf_element()
        {
            //// Arrange
            var settings = new JsonSchemaGeneratorSettings
            {
                FlattenInheritanceHierarchy = false,
            };

            //// Act
            var schema = JsonSchema.FromType(typeof(ChildOfAChild), settings);
            var data = schema.ToJson();

            //// Assert
            var child = (IJsonReference) Assert.Single(schema.AllOf);
            Assert.Equal("#/definitions/Child", child.ReferencePath);
            var parent = (IJsonReference)Assert.Single(schema.Definitions["Child"].AllOf);
            Assert.Equal("#/definitions/Parent", parent.ReferencePath);
        }
    }
}