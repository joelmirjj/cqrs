using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Cqrs.Commands;
using Core.Cqrs.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Cqrs
{
    public static class HandlerRegistration
    {
        public static void AddCqrsHandlers(this IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes =>
                    classes.AssignableTo(typeof(ICommandHandlerAsync<,>))
                        .Where(x => x.Name.EndsWith("Handler"))).AsImplementedInterfaces()
                .WithScopedLifetime().AddClasses(classes =>
                    classes.AssignableTo(typeof(IQueryHandlerAsync<,>))
                        .Where(x => x.Name.EndsWith("Handler"))).AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        public static void AddCqrsGlobalCommandDecorator(this IServiceCollection services,
            Type decorator, Assembly assembly, params Type[] exclude)
        {
            var handlerInterface = typeof(ICommandHandlerAsync<,>);
            services.AddCqrsGlobalDecorator(handlerInterface, decorator, assembly, exclude);
        }

        public static void AddCqrsGlobalQueryDecorator(this IServiceCollection services,
            Type decorator, Assembly assembly, params Type[] exclude)
        {
            var handlerInterface = typeof(IQueryHandlerAsync<,>);
            services.AddCqrsGlobalDecorator(handlerInterface, decorator, assembly, exclude);
        }

        private static void AddCqrsGlobalDecorator(this IServiceCollection services,
            Type handlerInterface, Type decorator, Assembly assembly,
            IEnumerable<Type> exclude = null)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface))
                .Where(x => x.Name.EndsWith("Handler")).ToList();
            if (exclude != null)
            {
                handlerTypes = handlerTypes.Except(exclude).ToList();
            }

            foreach (var handlerType in handlerTypes)
            {
                services.AddCqrsDecorator(handlerInterface, handlerType, decorator);
            }
        }

        public static void AddCqrsCommandDecorator(this IServiceCollection services,
            Type handlerType, Type decorator)
        {
            var handlerInterface = typeof(ICommandHandlerAsync<,>);
            services.AddCqrsDecorator(handlerInterface, handlerType, decorator);
        }

        public static void AddCqrsQueryDecorator(this IServiceCollection services, Type handlerType,
            Type decorator)
        {
            var handlerInterface = typeof(IQueryHandlerAsync<,>);
            services.AddCqrsDecorator(handlerInterface, handlerType, decorator);
        }

        private static void AddCqrsDecorator(this IServiceCollection services,
            Type handlerInterface, Type handlerType, Type decorator)
        {
            var commandHandlerInterface = handlerType.GetInterfaces().Single(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == handlerInterface);
            var arguments = commandHandlerInterface.GetGenericArguments();
            var genericHandler = handlerInterface.MakeGenericType(arguments);
            if (decorator.IsGenericType)
            {
                services.Decorate(genericHandler, decorator.MakeGenericType(arguments));
            }
            else
            {
                services.Decorate(genericHandler, decorator);
            }
        }
    }
}