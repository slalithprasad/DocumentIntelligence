using System;
using System.Text.Json.Serialization;

namespace DocumentIntelligence.Core.Models.Response;

public record ExtractionResponse(
    [property: JsonPropertyName("documentType")] string? DocumentType,
    [property: JsonPropertyName("fullName")] string? FullName,
    [property: JsonPropertyName("firstName")] string? FirstName,
    [property: JsonPropertyName("middleName")] string? MiddleName,
    [property: JsonPropertyName("lastName")] string? LastName,
    [property: JsonPropertyName("dateOfBirth")] string? DateOfBirth,
    [property: JsonPropertyName("address")] string? Address,
    [property: JsonPropertyName("documentNumber")] string? DocumentNumber,
    [property: JsonPropertyName("fatherName")] string? FatherName,
    [property: JsonPropertyName("motherName")] string? MotherName,
    [property: JsonPropertyName("gender")] string? Gender,
    [property: JsonPropertyName("nationality")] string? Nationality,
    [property: JsonPropertyName("issuer")] string? Issuer,
    [property: JsonPropertyName("issueDate")] string? IssueDate
);