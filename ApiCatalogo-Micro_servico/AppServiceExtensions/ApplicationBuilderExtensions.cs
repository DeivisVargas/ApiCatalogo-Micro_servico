namespace ApiCatalogo_Micro_servico.AppServiceExtensions
{
    public static class ApplicationBuilderExtensions
    {
        //metodos de extenção para IApplicationBuilder
        //criado para configurar o pipiline de um aplicativo 
        //IWebHostEnvironment para saber o ambiente da aplicaçaõ 
        public static IApplicationBuilder UseExeptionHandling(this IApplicationBuilder app ,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            return app; 
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            //definindo politicas de cors

            app.UseCors( p =>
            {
                p.AllowAnyOrigin();
                p.WithMethods("GET");
                p.AllowAnyHeader();
            });

            return app;
        }


        //setando as configurações do swagger 
        public static IApplicationBuilder UseSwaggerBuilder(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
