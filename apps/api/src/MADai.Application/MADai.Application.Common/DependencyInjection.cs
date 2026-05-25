using System;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MADai.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MADai.Application.Common;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		services.AddAutoMapper((Action<IMapperConfigurationExpression>)delegate
		{
		}, new Assembly[1] { assembly });
		services.AddValidatorsFromAssembly(assembly);
		services.AddMediatR(delegate(MediatRServiceConfiguration cfg)
		{
			cfg.RegisterServicesFromAssembly(assembly);
			cfg.AddOpenBehavior(typeof(ValidationBehavior<, >));
		});
		return services;
	}
}
