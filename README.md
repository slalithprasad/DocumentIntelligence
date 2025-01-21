# Document Intelligence

## Overview
This project, Document Intelligence, aims to develop intelligent solutions for document processing and analysis. By leveraging OpenAI's advanced machine learning models, we automate and enhance the extraction, classification, and interpretation of information from various types of documents. This approach improves efficiency by reducing manual processing time, enhances accuracy through sophisticated AI algorithms, and ensures scalability to handle large volumes of documents across different domains and industries.

## Functions

### Classify Function

The `Classify` function processes a POST request containing a base64 encoded image of a document. It validates the input, classifies the document type using the OpenAI service, and returns the classification result in JSON format.

#### Endpoint
```
POST /api/classify
```

#### Request Body
```json
{
  "base64Content": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
}
```

#### Response
```json
{
  "isSuccess": true,
  "result": {
    "documentType": "Aadhar",
    "issuingAuthority": "Government of India"
  },
  "error": null
}
```

### Extract Function

The `Extract` function processes a POST request containing a base64 encoded image of a document and a language. It validates the input, extracts key information from the document using the OpenAI service, and returns the extracted data in JSON format. The `language` key in the POST request body specifies the language in which the data should be extracted. If the specified language is not found, the extracted data is translated to the provided language. If no language is specified, the data will be extracted in English.

#### Endpoint
```
POST /api/extract
```

#### Request Body
```json
{
  "language": "Telugu",
  "base64Content": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
}
```

#### Response
```json
{
  "isSuccess": true,
  "result": {
    "documentType": "ఆధార్",
    "firstName": "జాన్",
    "lastName": "డో",
    "dateOfBirth": "01-01-1990",
    "address": "123 మెయిన్ స్ట్రీట్",
    "documentNumber": "123456789012",
    "fatherName": "రిచర్డ్ డో",
    "motherName": "జేన్ డో",
    "gender": "పురుషుడు",
    "nationality": "భారతీయుడు",
    "issuer": "భారత ప్రభుత్వం",
    "issueDate": "01-01-2010"
  },
  "error": null
}
```

## Services

### OpenAI Service

The `OpenAIService` class provides methods to interact with the OPENAI API for document classification and extraction.

#### Methods

- `Task<ClassificationResponse> ClassifyAsync(ClassifyRequest? request)`: Asynchronously classifies the given request and returns a classification response.
- `Task<ExtractionResponse> ExtractAsync(ExtractRequest? request)`: Asynchronously extracts information from the given request and returns an extraction response.

## Getting Started

To get started with the Document Intelligence project, follow these steps:

1. **Clone the repository**:
    ```sh
    git clone https://github.com/lalith-kaara/DocumentIntelligence.git
    ```

2. **Navigate to the project directory**:
    ```sh
    cd DocumentIntelligence
    ```

3. **Install .NET dependencies**:
    ```sh
    dotnet restore
    ```

4. **Add OpenAI configuration**:
    In the `local.settings.json` file, add the following keys:
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "OpenAIKey": "<key>",
        "OpenAIUri": "<uri>",
        "OpenAIModel": "<model>"
      }
    }
    ```

5. **Run the application**:
    ```sh
    dotnet run
    ```

## Contributing

We welcome contributions to the Document Intelligence project. Please read our [contributing guidelines](CONTRIBUTING.md) for more information.

## Contact

For any questions or feedback, please reach out to us at [lalith.s@kaaratech.com](mailto:lalith.s@kaaratech.com).