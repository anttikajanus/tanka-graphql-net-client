using System.Collections.Generic;
using Xunit;

namespace Tanka.GraphQL.Net.Client.Tests
{
    public class ExecutionResultTests
    {
        [Fact]
        public void Create_ExecutionReusult_Should_Have_Null_State()
        {
            // Arrange
            var result = new ExecutionResult();
            // Act
            // Assert
            Assert.Null(result.Data);
            Assert.Null(result.Extensions);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void Create_ExecutionReusult_And_Set_Data_Should_Return_Same_Data()
        {
            // Arrange
            var result = new ExecutionResult();
            var data = new Dictionary<string, object>() {
                {"key", "value"}
            };

            // Act
            result.Data = data;

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void ExecutionReusult_Setting_Data_Without_Any_Items_Should_Change_Data_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Data = new Dictionary<string, object>() {
                    {"key", "value" }
                }
            };
            var data = new Dictionary<string, object>();

            // Act
            result.Data = data;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void ExecutionReusult_Setting_Data_To_Null_Should_Change_Data_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Data = new Dictionary<string, object>() {
                    {"key", "value" }
                }
            };
            
            // Act
            result.Data = null;

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void Create_ExecutionReusult_And_Set_Extensions_Should_Return_Same_Extensions()
        {
            // Arrange
            var result = new ExecutionResult();
            var extensions = new Dictionary<string, object>() {
                {"key", "value"}
            };

            // Act
            result.Extensions = extensions;

            // Assert
            Assert.NotNull(result.Extensions);
            Assert.Equal(extensions, result.Extensions);
        }

        [Fact]
        public void ExecutionReusult_Setting_Extensions_Without_Any_Items_Should_Change_Extensions_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Extensions = new Dictionary<string, object>() {
                    {"key", "value" }
                }
            };
            var extensions = new Dictionary<string, object>();

            // Act
            result.Extensions= extensions;

            // Assert
            Assert.Null(result.Extensions);
        }

        [Fact]
        public void ExecutionReusult_Setting_Extensions_To_Null_Should_Change_Extensions_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Extensions = new Dictionary<string, object>() {
                    {"key", "value" }
                }
            };

            // Act
            result.Extensions = null;

            // Assert
            Assert.Null(result.Extensions);
        }

        [Fact]
        public void Create_ExecutionReusult_And_Set_Errors_Should_Return_Same_Errors()
        {
            // Arrange
            var result = new ExecutionResult();
            var errors = new List<GraphQLError>()
                {
                    new GraphQLError("Error message")
                };

            // Act
            result.Errors = errors;

            // Assert
            Assert.NotNull(result.Errors);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void ExecutionReusult_Setting_Errors_Without_Any_Items_Should_Change_Errors_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Errors = new List<GraphQLError>()
                    {
                        new GraphQLError("Error message")
                    }
            };
            var errors = new List<GraphQLError>();

            // Act
            result.Errors = errors;

            // Assert
            Assert.Null(result.Errors);
        }

        [Fact]
        public void ExecutionReusult_Setting_Errors_To_Null_Should_Change_Errors_To_Null()
        {
            // Arrange
            var result = new ExecutionResult()
            {
                Errors = new List<GraphQLError>()
                    {
                        new GraphQLError("Error message")
                    }
            };

            // Act
            result.Errors = null;

            // Assert
            Assert.Null(result.Errors);
        }
    }
}
