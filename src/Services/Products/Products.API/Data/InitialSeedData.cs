﻿using Marten.Schema;

namespace Products.API.Data;

public class InitialSeedData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if(await session.Query<Product>().AnyAsync())
        {
            return;
        }

        session.Store<Product>(GetPreConfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product>() 
    {
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone X",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-1.png",
            Price = 950.00M,
            Category = new List<string> { "Smart Phone" },
            Stock = 12
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 10",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-2.png",
            Price = 840.00M,
            Category = new List<string> { "Smart Phone" },
            Stock = 2
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Huawei Plus",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-3.png",
            Price = 650.00M,
            Category = new List<string> { "White Appliances" },
            Stock = 5
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Xiaomi Mi 9",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-4.png",
            Price = 470.00M,
            Category = new List<string> { "White Appliances" },
            Stock = 2
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "HTC U11+ Plus",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-5.png",
            Price = 380.00M,
            Category = new List<string> { "Smart Phone" },
            Stock = 10
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "LG G7 ThinQ",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-6.png",
            Price = 240.00M,
            Category = new List<string> { "Home Kitchen" },
            Stock = 50
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Panasonic Lumix",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-6.png",
            Price = 240.00M,
            Category = new List<string> { "Camera" },
            Stock = 30
        }
    };
}
