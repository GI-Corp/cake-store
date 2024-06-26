﻿namespace Shared.Domain.Entities.Swagger;

public class SwaggerOption
{
    public bool Enabled { get; set; }
    public bool ReDocEnabled { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string RoutePrefix { get; set; }
    public bool IncludeSecurity { get; set; }
    public string SecurityType { get; set; }
}