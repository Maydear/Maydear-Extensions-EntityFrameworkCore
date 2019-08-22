/*****************************************************************************************
 * Copyright (c) 2008-2019 kelvin(kelvin@onloch.com)
 * 梁乔峰版权所有2008-2019。
 * 本软件文档资料是梁乔峰的资产,任何人士阅读和使用本资料必
 * 须获得相应的书面授权,承担保密责任和接受相应的法律束.
 ****************************************************************************************
 * FileName: ServiceCollectionExtension.cs
 * Author:梁乔峰(5768534@qq.com)
 * CreateDate:2019/07/16 15:52:22
 * Description:
 *     依赖注入扩展实体
 * <version>|<author>            |<time>                                    |<desc>
 * 1        |梁乔峰              |2019/07/16 15:52:22                       |创建依赖注入扩展实体
*****************************************************************************************/
using Maydear.Extensions.EntityFrameworkCore.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {

        /// <summary>
        /// 注册服务层的依赖注入服务
        /// </summary>
        /// <param name="services">DI容器服务集合</param>
        /// <param name="configuration">配置对象</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddRepositories(options =>
            {
                options.UseInMemoryDatabase("tests");
            }, 256);
        }

        /// <summary>
        /// 注册服务层的依赖注入服务
        /// </summary>
        /// <param name="services">DI容器服务集合</param>
        /// <param name="configuration">配置对象</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services, Action<DbContextOptionsBuilder> setupAction, int poolSize)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            services.AddOptions();
            services.AddDbContextPool<TestContext>(setupAction, poolSize);
            services.AddTransient<TestRepository>();
            return services;
        }
    }
}
