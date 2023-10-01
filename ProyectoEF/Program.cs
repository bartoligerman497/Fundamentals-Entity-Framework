using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoEF;
using ProyectoEF.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuro la BD virtual
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));

builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
    // Postman
    // Get: https://localhost:7094/dbconexion
});

app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) =>
{
    //return Results.Ok(dbContext.Tareas);   
    //return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria).Where(p=> p.PrioridadTarea == ProyectoEF.Models.Prioridad.Baja));
    return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria));
    // Postman
    // Get: https://localhost:7094/api/tareas

});

app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    //await dbContext.Tareas.AddAsync(tarea);

    await dbContext.SaveChangesAsync();

    return Results.Ok();

    //Postman
    //{
    //    "categoriaId": "2bbe0713-85a7-4951-ab3b-bf4b5c1fd702",
    //    "titulo": "Visitar a mi tia",
    //    "descripcion": null,
    //    "prioridadTarea": 2
    //}


});

app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if (tareaActual != null)
    {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();

    //Postman
    //{
    //    "categoriaId": "2bbe0713-85a7-4951-ab3b-bf4b5c1fd702",
    //    "titulo": "Visitar a mi tia",
    //    "descripcion": "Debo visitar a mi tia en su casa y recoger los papeles",
    //    "prioridadTarea": 1,
    //    "fechaCreacion": "2023-10-01T16:39:30.1280112"
    //}


});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if (tareaActual != null)
    {
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();

    //Postman
    //{
    //    "categoriaId": "2bbe0713-85a7-4951-ab3b-bf4b5c1fd702",
    //    "titulo": "Visitar a mi tia",
    //    "descripcion": "Debo visitar a mi tia en su casa y recoger los papeles",
    //    "prioridadTarea": 1,
    //    "fechaCreacion": "2023-10-01T16:39:30.1280112"
    //}


});


app.Run();