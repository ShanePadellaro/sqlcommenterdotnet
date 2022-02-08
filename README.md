# sqlcommenterdotnet


Dotnet implementation of the SqlCommenter spec created by Google for SQL query analysis. https://google.github.io/sqlcommenter/

# Usage

Add nuget package to csproj
`<PackageReference Include="SqlCommenterDotnet" Version="0.1.1" />`

Or install using CLI
`dotnet add package SqlCommenterDotnet`

Add package to services
`services.AddSqlCommenter();`

When registering your EF context for DI use the two parameter action instead, passing the service provider to UseSqlCommenter().


            services.AddDbContext<TestContext>((p, o) =>
                o.UseNpgsql("Host=localhost;Database=sqlcommenter;Username=postgres")
                    .UseSqlCommenter(p));





