﻿using Lm.Streamthis.Catalog.Domain.Exceptions;
using Lm.Streamthis.Catalog.Domain.SeedWork;

namespace Lm.Streamthis.Catalog.Domain.Entities;

public class Category : AggregateRoot
{
    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be null or empty.");
        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null.");
        switch (Name.Length)
        {
            case < 3:
                throw new EntityValidationException($"{nameof(Name)} should not have less than 3 characters.");
            case > 255:
                throw new EntityValidationException($"{nameof(Name)} should not have more than 255 characters.");
        }
        if (Description.Length > 10000)
            throw new EntityValidationException($"{nameof(Description)} should not have more than 10.000 characters.");
    }
}