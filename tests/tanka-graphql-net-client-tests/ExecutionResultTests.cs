using System;
using System.Collections.Generic;
using System.Linq;
using Tanka.GraphQL.Net.Client.Tests.Data.StarWars;
using Tanka.GraphQL.Net.Client.Tests.Helpers;
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
            var errors = new List<ExecutionError>()
                {
                    new ExecutionError("Error message")
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
                Errors = new List<ExecutionError>()
                    {
                        new ExecutionError("Error message")
                    }
            };
            var errors = new List<ExecutionError>();

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
                Errors = new List<ExecutionError>()
                    {
                        new ExecutionError("Error message")
                    }
            };

            // Act
            result.Errors = null;

            // Assert
            Assert.Null(result.Errors);
        }

        [Fact]
        public void Create_ExecutionResult_From_ResponseJson_Should_Contain_Data()
        {
            // Arrange
            var resultJson = @"{
                   ""data"": {
                    ""character"": {
                      ""id"": ""humans/luke"",
                      ""name"": ""Luke"",
                      ""appearsIn"": [
                        ""JEDI"",
                        ""EMPIRE"",
                        ""NEWHOPE""
                        ]
                     }
                    }
                 }";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            var data = result.Data;
          
            // Assert
            Assert.NotNull(data);
            Assert.Equal(1, data.Count);
            Assert.Equal("character", data.First().Key);
            Assert.NotNull(data.First().Value);
        }

        [Fact]
        public void Create_ExecutionResult_From_ResponseJson_With_Only_Data_Shouldnt_Contain_Errors_or_Extensions()
        {
            // Arrange
            var resultJson = @"{
                   ""data"": {
                    ""character"": {
                      ""id"": ""humans/luke"",
                      ""name"": ""Luke"",
                      ""appearsIn"": [
                        ""JEDI"",
                        ""EMPIRE"",
                        ""NEWHOPE""
                        ]
                     }
                    }
                 }";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            var errors = result.Errors;
            var extensions = result.Extensions;

            // Assert
            Assert.Null(errors);
            Assert.Null(extensions);
        }

        [Fact]
        public void GetDataFieldAs_Should_Throw_InvlidOperationException_When_Data_Is_Null()
        {
            // Arrange
            var result = new ExecutionResult();

            // Act
#pragma warning disable IDE0039 // Use local function
            Action act = () => result.GetDataFieldAs<Human>();
#pragma warning restore IDE0039 // Use local function

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetExtensionFieldAs_Should_Throw_InvlidOperationException_When_Data_Is_Null()
        {
            // Arrange
            var result = new ExecutionResult();

            // Act
#pragma warning disable IDE0039 // Use local function
            Action act = () => result.GetExtensionFieldAs<Human>();
#pragma warning restore IDE0039 // Use local function

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void GetDataFieldAs_Should_Return_RequestedObject_When_It_Exists()
        {
            // Arrange
            var resultJson = @"{
                   ""data"": {
                    ""character"": {
                      ""id"": ""humans/luke"",
                      ""name"": ""Luke"",
                      ""generalPerformanceRaiting"": 6.7,
                      ""appearsIn"": [
                        ""JEDI"",
                        ""EMPIRE"",
                        ""NEWHOPE""
                        ]
                     }
                    }
                 }";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            Human human = result.GetDataFieldAs<Human>();

            // Assert
            Assert.Equal("Luke", human.Name);
            Assert.Equal("humans/luke", human.Id);
            Assert.Equal(6.7, human.GeneralPerformanceRaiting);
            Assert.Equal(3, human.AppearsIn.Count);
        }

        [Fact]
        public void GetDataFieldAs_Should_Return_RequestedObject_With_Nested_Types()
        {
            // Arrange
            var resultJson = @"{
                  ""data"": {
                    ""character"":
                      {
                        ""id"": ""humans/han"",
                        ""name"": ""Han"",
                        ""friends"": [
                          {
                            ""id"": ""humans/luke"",
                            ""name"": ""Luke""
                          }
                        ]
                      }
                  }}";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            Human human = result.GetDataFieldAs<Human>();

            // Assert
            Assert.Equal("Han", human.Name);
            Assert.Equal("humans/han", human.Id);
            Assert.Equal("Luke", human.Friends.First().Name);
            Assert.Equal("humans/luke", human.Friends.First().Id);
        }

        [Fact]
        public void GetDataFieldAs_Should_Return_MultipleObjects_With_Correct_Values()
        {
            // Arrange
            var resultJson = @"{
                  ""data"": {
                    ""characters"": [
                      {
                        ""id"": ""humans/han"",
                        ""name"": ""Han"",
                        ""friends"": [
                          {
                            ""id"": ""humans/luke"",
                            ""name"": ""Luke""
                          }
                        ]
                      },
                      {
                        ""id"": ""humans/luke"",
                        ""name"": ""Luke"",
                        ""friends"": [
                          {
                            ""id"": ""humans/han"",
                            ""name"": ""Han""
                          }
                        ]
                      }
                    ]
                  }}";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            List<Human> characters = result.GetDataFieldAs<List<Human>>();
            Human Han = characters[0];
            Human Luke = characters[1];

            // Assert
            Assert.Equal(2, characters.Count);
            Assert.Equal("Han", Han.Name);
            Assert.Equal("humans/han", Han.Id);
            Assert.Equal("Luke", Han.Friends.First().Name);
            Assert.Equal("humans/luke", Han.Friends.First().Id);
            Assert.Equal("Luke", Luke.Name);
            Assert.Equal("humans/luke", Luke.Id);
            Assert.Equal("Han", Luke.Friends.First().Name);
            Assert.Equal("humans/han", Luke.Friends.First().Id);
        }

        [Fact]
        public void GetDataFieldAs_Should_Return_RequestedObject_With_Key_When_It_Exists()
        {
            // Arrange
            var resultJson = @"{
                   ""data"": {
                    ""character"": {
                      ""id"": ""humans/luke"",
                      ""name"": ""Luke"",
                      ""appearsIn"": [
                        ""JEDI"",
                        ""EMPIRE"",
                        ""NEWHOPE""
                        ]
                     }
                    }
                 }";
            var result = ExecutionResultHelper.CreateFromJson(resultJson);

            // Act
            Human human = result.GetDataFieldAs<Human>("character");

            // Assert
            Assert.Equal("Luke", human.Name);
            Assert.Equal("humans/luke", human.Id);
            Assert.Equal(3, human.AppearsIn.Count);
        }

        [Fact]
        public void GetDataFieldAs_Should_Throw_KeyNotFoundException_When_FieldName_Doesnt_Exists()
        {
            //Arrange 
            var result = new ExecutionResult()
            {
                Data = new Dictionary<string, object>() {
                    { "key", "value" }
                }
            };

            // Act
#pragma warning disable IDE0039 // Use local function
            Action act = () => result.GetDataFieldAs<Human>("key_that_doesn't_exists");
#pragma warning restore IDE0039 // Use local function

            // Assert
            Assert.Throws<KeyNotFoundException>(act);
        }

        [Fact]
        public void GetExtensionFieldAs_Should_Throw_KeyNotFoundException_When_FieldName_Doesnt_Exists()
        {
            //Arrange 
            var result = new ExecutionResult()
            {
                Extensions = new Dictionary<string, object>() {
                    { "key", "value" }
                }
            };

            // Act
#pragma warning disable IDE0039 // Use local function
            Action act = () => result.GetExtensionFieldAs<Human>("key_that_doesn't_exists");
#pragma warning restore IDE0039 // Use local function

            // Assert
            Assert.Throws<KeyNotFoundException>(act);
        }
    }
}
