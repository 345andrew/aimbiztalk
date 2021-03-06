// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AzureIntegrationMigration.ApplicationModel;
using Microsoft.AzureIntegrationMigration.ApplicationModel.Source;
using Microsoft.AzureIntegrationMigration.BizTalk.Types;
using Microsoft.AzureIntegrationMigration.BizTalk.Types.Entities;
using Microsoft.AzureIntegrationMigration.BizTalk.Types.Entities.Orchestrations;
using Microsoft.AzureIntegrationMigration.Runner.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Xbehave;
using Xunit;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Microsoft.AzureIntegrationMigration.BizTalk.Parse.Tests
{
    /// <summary>
    /// Tests for the <see cref="OrchestrationMultiPartMessageTypeParser"/> class.
    /// </summary>
    public class OrchestrationMultiPartMessageTypeParserFeature
    {
        #region Constructor Tests

        /// <summary>
        /// Scenario tests that the object construction throws an exception when null model is passed.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <param name="e">The runner exception, if any.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ConstructWithNullModel(IBizTalkParser parser, ILogger logger, IApplicationModel model, MigrationContext context, Exception e)
        {
            "Given a parser"
                .x(() => parser.Should().BeNull());

            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And null model"
                .x(() => model.Should().BeNull());

            "And a context"
                .x(() => context = new MigrationContext());

            "When constructing with null model"
                .x(() => e = Record.Exception(() => new OrchestrationMultiPartMessageTypeParser(model, context, logger)));

            "Then the parser constructor should throw an exception"
                .x(() => e.Should().NotBeNull().And.Subject.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("model"));
        }

        /// <summary>
        /// Scenario tests that the object construction throws an exception when null context is passed.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <param name="e">The runner exception, if any.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ConstructWithNullContext(IBizTalkParser parser, ILogger logger, IApplicationModel model, MigrationContext context, Exception e)
        {
            "Given a parser"
                .x(() => parser.Should().BeNull());


            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And a model"
                .x(() => model = new AzureIntegrationServicesModel());

            "And a null context"
                .x(() => context.Should().BeNull());

            "When constructing with null context"
                .x(() => e = Record.Exception(() => new OrchestrationMultiPartMessageTypeParser(model, context, logger)));

            "Then the parser constructor should throw an exception"
                .x(() => e.Should().NotBeNull().And.Subject.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("context"));
        }

        /// <summary>
        /// Scenario tests that the object construction throws an exception when null logger is passed.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <param name="e">The runner exception, if any.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ConstructWithNullLogger(IBizTalkParser parser, ILogger logger, IApplicationModel model, MigrationContext context, Exception e)
        {
            "Given a parser"
                .x(() => parser.Should().BeNull());

            "And null logger"
                .x(() => logger.Should().BeNull());

            "And a model"
                .x(() => model = new AzureIntegrationServicesModel());

            "And a context"
                .x(() => context = new MigrationContext());

            "When constructing with null logger"
                .x(() => e = Record.Exception(() => new OrchestrationMultiPartMessageTypeParser(model, context, logger)));

            "Then the parser constructor should throw an exception"
                .x(() => e.Should().NotBeNull().And.Subject.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("logger"));
        }

        /// <summary>
        /// Scenario tests that the object construction succeeds.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <param name="e">The runner exception, if any.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ConstructWithSuccess(IBizTalkParser parser, ILogger logger, IApplicationModel model, MigrationContext context, Exception e)
        {
            "Given a parser"
                .x(() => parser.Should().BeNull());

            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And a model"
                .x(() => model = new AzureIntegrationServicesModel());

            "And a context"
                .x(() => context = new MigrationContext());

            "When constructing"
                .x(() => e = Record.Exception(() => parser = new OrchestrationMultiPartMessageTypeParser(model, context, logger)));

            "Then the parser constructor should succeed"
                .x(() =>
                {
                    e.Should().BeNull();
                    parser.Should().NotBeNull();
                });
        }

        #endregion

        #region Parse Tests

        /// <summary>
        /// Scenario tests parser skips if the source model is missing.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The migration context.</param>
        /// <param name="model">The model.</param>
        /// <param name="e">The exception that is generated, if appropriate.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ParseIsSkippedIfModelIsMissing(OrchestrationMultiPartMessageTypeParser parser, ILogger logger, MigrationContext context, AzureIntegrationServicesModel model, Exception e)
        {
            "Given a model"
                .x(() =>
                {
                    model = new AzureIntegrationServicesModel();
                });

            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And a context"
                .x(() => context = new MigrationContext());

            "And a parser"
                .x(() => parser = new OrchestrationMultiPartMessageTypeParser(model, context, logger));

            "When parsing"
                .x(() => e = Record.Exception(() => parser.Parse()));

            "Then the code should not throw an exception"
                .x(() => e.Should().BeNull());
        }

        /// <summary>
        /// Scenario tests the happy path when a multi-part message type is parsed.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The migration context.</param>
        /// <param name="model">The model.</param>
        /// <param name="e">The exception that is generated, if appropriate.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ParseMultiPartMessageTypeWithSuccess(OrchestrationMultiPartMessageTypeParser parser, ILogger logger, MigrationContext context, AzureIntegrationServicesModel model, Exception e)
        {
            var orchestrationDefinitionName = "orchestrationDefinitionName";
            var orchestrationDefinitionKey = "orchestrationDefinitionKey";
            var multiPartMessageTypeName = "multiPartMessageTypeName";
            var asmContainerKey = "asmContainerKey";

            "Given a source model with an orchestration and a multi part message type"
               .x(() =>
               {
                   var odxModel = new MetaModel
                   {
                       Element = new Element[]
                       {
                            new Element
                            {
                                Type = "Module",
                                Element1 = new Element[]
                                {
                                    new Element
                                    {
                                        Type = "MultipartMessageType",
                                        Property = new ElementProperty[]
                                        {
                                            new ElementProperty { Name = "Name", Value = multiPartMessageTypeName }
                                        }
                                    }
                                }
                            }
                         }
                   };

                   var orchestration = new Orchestration
                   {
                       Name = orchestrationDefinitionName,
                       ResourceContainerKey = asmContainerKey,
                       ResourceDefinitionKey = orchestrationDefinitionKey,
                       Model = odxModel
                   };

                   var parsedApplication = new ParsedBizTalkApplication
                   {
                       Application = new BizTalkApplication()
                   };

                   parsedApplication.Application.Orchestrations.Add(orchestration);

                   model = new AzureIntegrationServicesModel();
                   var group = new ParsedBizTalkApplicationGroup();
                   model.MigrationSource.MigrationSourceModel = group;
                   group.Applications.Add(parsedApplication);
               });

            "And one orchestration in the source report model"
                .x(() =>
                {
                    var msiContainer = new ResourceContainer() { Key = "TestMsi.Key", Name = "TestMsi", Type = ModelConstants.ResourceContainerMsi, ContainerLocation = @"C:\Test\Test.msi" };
                    model.MigrationSource.ResourceContainers.Add(msiContainer);

                    var cabContainer = new ResourceContainer() { Key = "TestCab.Key", Name = "TestCab", Type = ModelConstants.ResourceContainerCab, ContainerLocation = @"C:\Test\Test.CAB" };
                    msiContainer.ResourceContainers.Add(cabContainer);

                    var asmContainer = new ResourceContainer() { Key = asmContainerKey, Name = "TestAssembly", Type = ModelConstants.ResourceContainerAssembly, ContainerLocation = @"C:\Test\Test.dll" };
                    cabContainer.ResourceContainers.Add(asmContainer);

                    var orchestrationDefinition = new ResourceDefinition()
                    {
                        Key = orchestrationDefinitionKey,
                        Name = orchestrationDefinitionName,
                        Type = ModelConstants.ResourceDefinitionOrchestration
                    };
                    asmContainer.ResourceDefinitions.Add(orchestrationDefinition);

                    var metaModel = ((ParsedBizTalkApplicationGroup)model.MigrationSource.MigrationSourceModel).Applications[0].Application.Orchestrations[0].Model;
                    var metaModelResource = new ResourceItem()
                    {
                        Key = string.Concat(orchestrationDefinitionKey, ":", MetaModelConstants.MetaModelRootElement),
                        Name = MetaModelConstants.MetaModelRootElement,
                        Type = ModelConstants.ResourceMetaModel
                    };
                    metaModel.Resource = metaModelResource;
                    metaModelResource.SourceObject = metaModel;
                    orchestrationDefinition.Resources.Add(metaModelResource);

                    var moduleResource = new ResourceItem()
                    {
                        Key = string.Concat(metaModelResource.Key, ":", MetaModelConstants.ElementTypeModule),
                        Name = MetaModelConstants.ElementTypeModule,
                        Type = ModelConstants.ResourceModule
                    };
                    metaModelResource.Resources.Add(moduleResource);
                });

            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And a context"
                .x(() => context = new MigrationContext());

            "And a parser"
                .x(() => parser = new OrchestrationMultiPartMessageTypeParser(model, context, logger));

            "When parsing"
                .x(() => e = Record.Exception(() => parser.Parse()));

            "Then the code should not throw an exception"
                .x(() => e.Should().BeNull());

            "And there should be no errors logged"
                 .x(() => context.Errors.Should().BeNullOrEmpty());

            "And the resources should be set."
                .x(() =>
                {
                    // Check the multi part message type resource has been created.
                    var moduleResource = model.FindResourcesByType(ModelConstants.ResourceModule).SingleOrDefault();
                    moduleResource.Should().NotBeNull();
                    var multiPartMessageTypeResource = model.FindResourcesByType(ModelConstants.ResourceMultipartMessageType).SingleOrDefault();
                    multiPartMessageTypeResource.Should().NotBeNull();
                    var multiPartMessageType = ((ParsedBizTalkApplicationGroup)model.MigrationSource.MigrationSourceModel).Applications[0].Application.Orchestrations[0].FindMultiPartMessageTypes().Single();

                    // Validate the multi part message type resource.
                    multiPartMessageTypeResource.Should().NotBeNull();
                    multiPartMessageTypeResource.Key.Should().Be(string.Concat(moduleResource.Key, ":", multiPartMessageTypeName));
                    multiPartMessageTypeResource.Name.Should().Be(multiPartMessageTypeName);
                    multiPartMessageTypeResource.Type.Should().Be(ModelConstants.ResourceMultipartMessageType);

                    multiPartMessageType.Resource.Should().Be(multiPartMessageTypeResource); // The pointer to the resource should be set.
                    multiPartMessageTypeResource.ParentRefId.Should().Be(moduleResource.RefId); // The parent ref ID should be set.
                    multiPartMessageTypeResource.SourceObject.Should().Be(multiPartMessageType); // The resource should have a pointer to the source object. 
                });
        }

        /// <summary>
        /// Scenario tests code fails if the orchestration module is missing.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The migration context.</param>
        /// <param name="model">The model.</param>
        /// <param name="e">The exception that is generated, if appropriate.</param>
        [Scenario]
        [Trait(TestConstants.TraitCategory, TestConstants.CategoryUnitTest)]
        public void ParseMultiPartMessageTypeWithMissingModule(OrchestrationMultiPartMessageTypeParser parser, ILogger logger, MigrationContext context, AzureIntegrationServicesModel model, Exception e)
        {
            var orchestrationDefinitionName = "orchestrationDefinitionName";
            var orchestrationDefinitionKey = "orchestrationDefinitionKey";
            var asmContainerKey = "asmContainerKey";
            var wrongKey = "wrongKey";

            "Given a source model with an orchestration with a missing module"
               .x(() =>
               {
                   var orchestration = new Orchestration
                   {
                       Name = orchestrationDefinitionName,
                       ResourceContainerKey = asmContainerKey,
                       ResourceDefinitionKey = wrongKey
                   };

                   var parsedApplication = new ParsedBizTalkApplication
                   {
                       Application = new BizTalkApplication()
                   };

                   parsedApplication.Application.Orchestrations.Add(orchestration);

                   model = new AzureIntegrationServicesModel();
                   var group = new ParsedBizTalkApplicationGroup();
                   model.MigrationSource.MigrationSourceModel = group;
                   group.Applications.Add(parsedApplication);

               });

            "And one orchestration in the source report model"
                .x(() =>
                {
                    var msiContainer = new ResourceContainer() { Key = "TestMsi.Key", Name = "TestMsi", Type = ModelConstants.ResourceContainerMsi, ContainerLocation = @"C:\Test\Test.msi" };
                    model.MigrationSource.ResourceContainers.Add(msiContainer);

                    var cabContainer = new ResourceContainer() { Key = "TestCab.Key", Name = "TestCab", Type = ModelConstants.ResourceContainerCab, ContainerLocation = @"C:\Test\Test.CAB" };
                    msiContainer.ResourceContainers.Add(cabContainer);

                    var asmContainer = new ResourceContainer() { Key = asmContainerKey, Name = "TestAssembly", Type = ModelConstants.ResourceContainerAssembly, ContainerLocation = @"C:\Test\Test.dll" };
                    cabContainer.ResourceContainers.Add(asmContainer);

                    var orchestrationDefinition = new ResourceDefinition()
                    {
                        Key = orchestrationDefinitionKey,
                        Name = orchestrationDefinitionName,
                        Type = ModelConstants.ResourceDefinitionOrchestration
                    };
                    asmContainer.ResourceDefinitions.Add(orchestrationDefinition);

                });

            "And a logger"
                .x(() => logger = new Mock<ILogger>().Object);

            "And a context"
                .x(() => context = new MigrationContext());

            "And a parser"
                .x(() => parser = new OrchestrationMultiPartMessageTypeParser(model, context, logger));

            "When parsing"
                .x(() => e = Record.Exception(() => parser.Parse()));

            "Then the code should not throw an exception"
                .x(() => e.Should().BeNull());

            "And there should be an error logged"
                 .x(() =>
                 {
                     context.Errors.Should().NotBeNull();
                     context.Errors.Should().HaveCount(1);
                     context.Errors[0].Message.Should().Contain(wrongKey);
                     context.Errors[0].Message.Should().Contain(ModelConstants.ResourceModule);                     
                 });
        }

        #endregion
    }
}
