﻿using Lm.Streamthis.Catalog.Domain.Exceptions;
using Lm.Streamthis.Catalog.Domain.SeedWork;
using Lm.Streamthis.Catalog.Domain.Validation;

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
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MinLength(Name, 3, nameof(Name));
        DomainValidation.MaxLength(Name, 255, nameof(Name));
        DomainValidation.MaxLength(Description, 10000, nameof(Description));
    }
}