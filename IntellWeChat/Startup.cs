﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AuthentValitor.AuthentModel;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using AutoMapper;
using Dtol;
using FluentValidation.AspNetCore;
using IntellWeChat.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using ViewModel.WeChatViewModel.MiddleModel;

namespace IntellWeChat
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //register config file 
            Configuration = setConfig(env, "appsettings.json");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //EFBaseClass.connection = Configuration.GetConnectionString("SqlServerConnection");
            var Service = Assembly.Load("Dto.Service");
            var IService = Assembly.Load("Dto.IService");
            var IRepository = Assembly.Load("Dto.IRepository");
            var Repository = Assembly.Load("Dto.Repository");
            var valitorAssembly = Assembly.Load("ViewModel");

            #region 验证
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            services.AddAuthentication(x =>
            {
                
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,//参数配置在下边
                        ValidateIssuer = true,
                        ValidIssuer = audienceConfig["Issuer"],//发行人
                        ValidateAudience = true,
                        ValidAudience = audienceConfig["Audience"],//订阅人
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                    };

                });
            #endregion
            #region HttpClientFactory
            
            services.AddHttpClient("WeChatToken", client =>
            {
                client.BaseAddress = new Uri("https://api.weixin.qq.com/cgi-bin/token");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
            });

            #endregion
            #region 配置文件
            services.AddOptions();
            services.Configure<WeChartTokenMiddles>(Configuration.GetSection("WeChatToken"));
            #endregion

            #region EFCore
            var connection = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<DtolContext>(options =>
            options.UseSqlServer(connection));
            services.AddDbContext<DtolContext>
                (options =>
                {
                    //sqlServerOptions数据库提供程序级别的可选行为选择器
                    //UseQueryTrackingBehavior 为通用EF Core行为选择器
                    options.UseSqlServer(connection, sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure();
                        sqlServerOptions.CommandTimeout(60);
                    })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });


            #endregion
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "东疆智慧后勤用户管理接口文档",
                    Description = "移动端模块接口",
                    Contact = new Contact
                    {
                        Name = "张祎荻",
                        Email = "479663032@qq.com",
                    },
                    Version = "v1"
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录
                var xmlPath = Path.Combine(basePath, "IntellWeChat.xml");
                var xmlPathModel = Path.Combine(basePath, "ViewModel.xml");
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlPathModel);

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "IntellWeChat", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("IntellWeChat", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion

            });
            #endregion
            #region AutoMapper
            services.AddAutoMapper(Service);
            #endregion
            #region mvc服务

            services.AddMvc()
                .AddFluentValidation(config => {
                    config.RegisterValidatorsFromAssembly(valitorAssembly);//程序集注入
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion
            #region AutoFac

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();

            //   builder


            //根据名称约定（数据访问层的接口和实现均以Repository结尾），实现数据访问接口和数据访问实现的依赖


            builder.RegisterAssemblyTypes(IRepository, Repository)
              .Where(t => t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();
            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(IService, Service)
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces();

            //将services填充到Autofac容器生成器中
            builder.Populate(services);
            //日志依赖注入
            builder.RegisterLogger(autowireProperties: true);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();
            #endregion
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();

            //}



            app.UseAuthentication();//启用jwt验证
            #region 日志
            // app.UseSerilogRequestLogging();
            #endregion

            //允许所有的域
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            // app.UseHttpsRedirection();

            //，启用中间件为生成的 JSON 文档和 Swagger UI 提供服务
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "用户管理文档 V1");
                c.RoutePrefix = string.Empty;
            });





            app.UseHttpsRedirection();
            app.UseMvc();
        }




        public IConfiguration setConfig(IHostingEnvironment env, String config)
        {
  
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile(config, optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
