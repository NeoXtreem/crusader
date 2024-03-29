﻿using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Xtreem.Crusader.ML.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Abstractions
{
    internal abstract class ModelService : IModelService
    {
        protected readonly ModelOptions Options;

        protected ModelService(IOptionsFactory<ModelOptions> optionsFactory)
        {
            Options = optionsFactory.Create(Microsoft.Extensions.Options.Options.DefaultName);
        }

        public bool CanUse() => GetType().Name.StartsWith(Options.Type);

        public ITransformer Train<TInput>(IEnumerable<TInput> items) where TInput : class => Train(new MLContext(0), items);

        protected abstract ITransformer Train<TInput>(MLContext mlContext, IEnumerable<TInput> items) where TInput : class;
    }
}
