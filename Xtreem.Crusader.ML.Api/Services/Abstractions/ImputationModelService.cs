using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Utilities.Extensions;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class ImputationModelService : ModelService
    {
        private readonly Dictionary<Type, (PropertyInfo property, IEnumerable<Attribute> attributes)[]> _propertiesCache = new Dictionary<Type, (PropertyInfo property, IEnumerable<Attribute> attributes)[]>();
        
        protected ImputationModelService(IOptionsFactory<ModelOptions> optionsFactory)
            : base(optionsFactory)
        {
        }

        protected override ITransformer Train<TInput>(MLContext mlContext, IEnumerable<TInput> items)
        {
            var predictColumnProperties = GetPropertiesWithAttributes<TInput, PredictColumnAttribute>().ToArray();
            IEstimator<ITransformer> pipeline = new EstimatorChain<ITransformer>();
            var itemsArray = items.ToArrayOrCast();

            if (predictColumnProperties.Any())
            {
                // Build a model for each specified predict column to achieve imputation.
                foreach (var (property, attribute) in predictColumnProperties)
                {
                    pipeline = AddColumnToPipeline(pipeline, mlContext, property, attribute, itemsArray);
                }
            }
            else
            {
                // Build a model on the assumption that expected columns are present.
                pipeline = CreateSinglePipeline<TInput>(pipeline, mlContext);
            }

            return Evaluate(mlContext, pipeline, itemsArray);
        }

        protected abstract IEstimator<ITransformer> AddColumnToPipeline<TInput>(IEstimator<ITransformer> pipeline, MLContext mlContext, PropertyInfo property, PredictColumnAttribute attribute, IEnumerable<TInput> items) where TInput : class;

        protected virtual IEstimator<ITransformer> CreateSinglePipeline<TInput>(IEstimator<ITransformer> pipeline, MLContext mlContext) where TInput : class
        {
            throw new NotSupportedException();
        }

        protected abstract ITransformer Evaluate<TInput>(MLContext mlContext, IEstimator<ITransformer> pipeline, IEnumerable<TInput> items) where TInput : class;

        protected IEnumerable<(PropertyInfo property, TAttribute attribute)> GetPropertiesWithAttributes<TInput, TAttribute>() where TInput : class where TAttribute : Attribute
        {
            return GetPropertiesWithAttributes<TInput>().Select(p => (p.property, attribute: p.attributes.OfType<TAttribute>().SingleOrDefault())).Where(p => p.attribute != null);
        }

        protected IEnumerable<(PropertyInfo property, IEnumerable<Attribute> attributes)> GetPropertiesWithAttributes<TInput>() where TInput : class
        {
            var inputType = typeof(TInput);

            if (!_propertiesCache.TryGetValue(inputType, out var properties))
            {
                properties = inputType.GetProperties().Select(p => (property: p, attributes: p.GetCustomAttributes())).Where(p => !p.attributes.OfType<ColumnNameAttribute>().Any()).ToArray();
                _propertiesCache.Add(inputType, properties);
            }

            return properties;
        }
    }
}
