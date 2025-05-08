using DataLayer;
using WebBro.DataLayer.EfClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<ILearningPathService, LearningPathService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IStepService, StepService>();
builder.Services.AddScoped<IMarkdownService, MarkdownService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<AppDbContext>();

    if (ctx == null)
    {
        throw new Exception("No ctx was provided.");
    }

    if (!ctx.LearningPaths.Any())
    {
        ctx.LearningPaths.Add(new LearningPath
        {
            LearningPathId = 1,
            Title = "Getting started on Frontend Mentor",
            Description = "These projects will help you find your feet on the Frontend Mentor platform and give you experience working with designs and building small projects. They're all HTML & CSS-only challenges, so they'll help you pick up the basics.",
            ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/LearningPaths/samkit9vyygeuxqi6f4q.jpg"
        });
    }

    if (!ctx.Steps.Any())
    {
        ctx.Steps.AddRange(new List<Step>
        {
            new Step
            {
                StepId = 1,
                LearningPathId = 1,
                Title = "Introduction to Frontend Mentor 1",
                Description = "In this article, we help you get up to speed with the Frontend Mentor platform and what we offer. After reading, you'll be aware of why we exist and will understand our platform's key areas.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 1,
            },
            new Step
            {
                StepId = 2,
                LearningPathId = 1,
                Title = "Introduction to Frontend Mentor 2",
                Description = "In this article, we help you get up to speed with the Frontend Mentor platform and what we offer. After reading, you'll be aware of why we exist and will understand our platform's key areas.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 2,
            },
            new Step
            {
                StepId = 3,
                LearningPathId = 1,
                Title = "Introduction to Frontend Mentor 3",
                Description = "In this article, we help you get up to speed with the Frontend Mentor platform and what we offer. After reading, you'll be aware of why we exist and will understand our platform's key areas.",
                ImageUrl = "https://res.cloudinary.com/dz209s6jk/image/upload/f_auto,q_auto/Admin/rcfhsmc5x11kbdjorjwb.png",
                Order = 3,
            },
        });
    }

    ctx.SaveChanges();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();


app.Run();
